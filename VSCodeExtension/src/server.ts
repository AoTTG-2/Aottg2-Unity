import {
    createConnection,
    TextDocuments,
    ProposedFeatures,
    InitializeParams,
    CompletionItem,
    CompletionItemKind,
    TextDocumentPositionParams,
    TextDocumentSyncKind,
    InitializeResult,
    Hover,
    MarkupKind
} from 'vscode-languageserver/node';

import { TextDocument } from 'vscode-languageserver-textdocument';
import { APIMetadataLoader } from './apiMetadata';
import { SymbolManager } from './symbolManager';
import * as path from 'path';

// Create a connection for the server
const connection = createConnection(ProposedFeatures.all);

// Create a simple text document manager
const documents: TextDocuments<TextDocument> = new TextDocuments(TextDocument);

// API metadata loader
let apiLoader: APIMetadataLoader;

// User symbol manager
const symbolManager = new SymbolManager();

// CL Keywords
const keywords = [
    'if', 'else', 'while', 'for', 'in', 'return', 'break', 'continue',
    'component', 'class', 'function', 'self', 'wait', 'true', 'false', 'null',
    'int', 'float', 'string', 'bool', 'void'
];

connection.onInitialize((params: InitializeParams) => {
    connection.console.log('[CL Server] Initializing...');
    const workspaceFolder = params.rootUri;
    
    // Load API metadata from json directory
    if (workspaceFolder) {
        // Convert file:// URI to local path and decode URL encoding
        let folderPath = workspaceFolder;
        if (folderPath.startsWith('file:///')) {
            folderPath = folderPath.substring(8); // Remove 'file:///'
        } else if (folderPath.startsWith('file://')) {
            folderPath = folderPath.substring(7); // Remove 'file://'
        }
        folderPath = decodeURIComponent(folderPath);
        
        const jsonDir = path.join(folderPath, 'json');
        connection.console.log(`[CL Server] Loading metadata from: ${jsonDir}`);
        apiLoader = new APIMetadataLoader(jsonDir);
        apiLoader.loadAll();
    }

    const result: InitializeResult = {
        capabilities: {
            textDocumentSync: TextDocumentSyncKind.Incremental,
            completionProvider: {
                resolveProvider: false,
                triggerCharacters: ['.']
            },
            hoverProvider: true
        }
    };
    connection.console.log('[CL Server] Initialized successfully');
    return result;
});

connection.onInitialized(() => {
    connection.console.log('Custom Logic Language Server initialized');
});

// Handle document changes
documents.onDidChangeContent(change => {
    connection.console.log(`[CL Server] Document changed: ${change.document.uri}`);
    symbolManager.updateDocument(change.document);
    
    // Debug: log all symbols found
    const allSymbols = symbolManager.getDocumentSymbols(change.document.uri);
    connection.console.log(`[CL Server] Total symbols in document: ${allSymbols.length}`);
    for (const sym of allSymbols) {
        connection.console.log(`[CL Server]   ${sym.name} (${sym.kind}) parent=${sym.parent || 'none'}`);
    }
});

documents.onDidClose(e => {
    connection.console.log(`[CL Server] Document closed: ${e.document.uri}`);
    symbolManager.removeDocument(e.document.uri);
});

// Provide completions
connection.onCompletion(
    (textDocumentPosition: TextDocumentPositionParams): CompletionItem[] => {
        const document = documents.get(textDocumentPosition.textDocument.uri);
        if (!document) {
            return [];
        }

        const text = document.getText();
        const offset = document.offsetAt(textDocumentPosition.position);
        const linePrefix = text.substring(0, offset);
        const lastLine = linePrefix.split('\n').pop() || '';

        const completions: CompletionItem[] = [];

        // Check if we're after a dot (member access)
        const dotMatch = lastLine.match(/(\w+(?:\.\w+)*)\.$/);
        if (dotMatch) {
            let objectName = dotMatch[1];
            let resolvedType: string | undefined = undefined;
            
            connection.console.log(`[CL Server] Member access on: ${objectName}`);
            
            // Handle chained member access like self._tester.
            if (objectName.includes('.')) {
                const parts = objectName.split('.');
                connection.console.log(`[CL Server] Chained access: ${parts.join(' -> ')}`);
                
                // Start with the first part
                let currentType: string | undefined;
                
                if (parts[0] === 'self') {
                    // Get current class
                    const position = textDocumentPosition.position;
                    currentType = symbolManager.getCurrentClass(document, position.line + 1) || undefined;
                    connection.console.log(`[CL Server] self resolves to class: ${currentType}`);
                } else {
                    // Check if first part is a local variable
                    const position = textDocumentPosition.position;
                    const localVarType = symbolManager.findLocalVariableType(document, position.line + 1, parts[0]);
                    if (localVarType) {
                        currentType = localVarType;
                        connection.console.log(`[CL Server] ${parts[0]} is a local variable with type: ${currentType}`);
                    } else {
                        // Assume it's a class name or field
                        currentType = parts[0];
                        connection.console.log(`[CL Server] ${parts[0]} assumed to be type name: ${currentType}`);
                    }
                }
                
                // Resolve each part in the chain
                for (let i = 1; i < parts.length && currentType; i++) {
                    const fieldName = parts[i];
                    connection.console.log(`[CL Server] Looking up field '${fieldName}' in type '${currentType}'`);
                    currentType = symbolManager.findFieldType(currentType, fieldName);
                    connection.console.log(`[CL Server] Field '${fieldName}' has type: ${currentType}`);
                }
                
                resolvedType = currentType;
                connection.console.log(`[CL Server] Final resolved type: ${resolvedType}`);
            } else if (objectName === 'self') {
                // Handle simple self.
                const position = textDocumentPosition.position;
                resolvedType = symbolManager.getCurrentClass(document, position.line + 1) || undefined;
                connection.console.log(`[CL Server] self resolves to class: ${resolvedType}`);
            } else {
                // Check if it's a local variable
                const position = textDocumentPosition.position;
                const localVarType = symbolManager.findLocalVariableType(document, position.line + 1, objectName);
                if (localVarType) {
                    resolvedType = localVarType;
                    connection.console.log(`[CL Server] ${objectName} is a local variable with type: ${resolvedType}`);
                }
            }
            
            if (!resolvedType) {
                connection.console.log(`[CL Server] Could not resolve type for ${objectName}`);
                return completions;
            }
            
            // Check if it's a built-in class
            if (apiLoader) {
                const methods = apiLoader.getMethods(resolvedType);
                const fields = apiLoader.getFields(resolvedType);

                for (const method of methods) {
                    const params = method.parameters.map(p => 
                        `${p.name}${p.isOptional ? '?' : ''}: ${p.type.name}`
                    ).join(', ');

                    completions.push({
                        label: method.label,
                        kind: CompletionItemKind.Method,
                        detail: `(${params}) → ${method.returnType.name}`,
                        documentation: method.info.summary,
                        insertText: `${method.label}($0)`,
                        insertTextFormat: 2 // Snippet
                    });
                }

                for (const field of fields) {
                    completions.push({
                        label: field.label,
                        kind: field.readonly ? CompletionItemKind.Constant : CompletionItemKind.Field,
                        detail: field.type.name,
                        documentation: field.info.summary
                    });
                }
            }

            // Check if it's a user-defined class
            const userMembers = symbolManager.getClassMembers(resolvedType);
            connection.console.log(`[CL Server] Found ${userMembers.length} user-defined members for type ${resolvedType}`);
            for (const member of userMembers) {
                if (member.kind === 'function') {
                    const params = member.parameters?.join(', ') || '';
                    completions.push({
                        label: member.name,
                        kind: CompletionItemKind.Method,
                        detail: `(${params})`,
                        documentation: member.documentation,
                        insertText: `${member.name}($0)`,
                        insertTextFormat: 2
                    });
                } else if (member.kind === 'field') {
                    completions.push({
                        label: member.name,
                        kind: CompletionItemKind.Field,
                        documentation: member.documentation
                    });
                }
            }

            return completions;
        }

        // Get local variables at current position
        const position = textDocumentPosition.position;
        const localVars = symbolManager.getLocalVariables(document, position.line + 1);
        for (const localVar of localVars) {
            completions.push({
                label: localVar.name,
                kind: CompletionItemKind.Variable,
                documentation: localVar.documentation
            });
        }

        // Add user-defined classes
        for (const className of symbolManager.getClassNames()) {
            const cls = symbolManager.findClass(className);
            completions.push({
                label: className,
                kind: CompletionItemKind.Class,
                documentation: cls?.documentation
            });
        }

        // Add user-defined global functions
        for (const symbol of symbolManager.getAllSymbols()) {
            if (symbol.kind === 'function' && !symbol.parent) {
                const params = symbol.parameters?.join(', ') || '';
                completions.push({
                    label: symbol.name,
                    kind: CompletionItemKind.Function,
                    detail: `(${params})`,
                    documentation: symbol.documentation,
                    insertText: `${symbol.name}($0)`,
                    insertTextFormat: 2
                });
            }
        }

        // Add keywords
        for (const keyword of keywords) {
            completions.push({
                label: keyword,
                kind: CompletionItemKind.Keyword
            });
        }

        // Add built-in classes
        if (apiLoader) {
            for (const className of apiLoader.getClassNames()) {
                const cls = apiLoader.getClass(className);
                completions.push({
                    label: className,
                    kind: CompletionItemKind.Class,
                    documentation: cls?.info.summary
                });
            }
        }

        return completions;
    }
);

// Provide hover information
connection.onHover(
    (textDocumentPosition: TextDocumentPositionParams): Hover | null => {
        const document = documents.get(textDocumentPosition.textDocument.uri);
        if (!document || !apiLoader) {
            return null;
        }

        const text = document.getText();
        const offset = document.offsetAt(textDocumentPosition.position);
        
        // Get word at position
        const wordRange = getWordRangeAtPosition(text, offset);
        if (!wordRange) return null;

        const word = text.substring(wordRange.start, wordRange.end);

        // Check if it's a user-defined class
        const userClass = symbolManager.findClass(word);
        if (userClass) {
            return {
                contents: {
                    kind: MarkupKind.Markdown,
                    value: `**${userClass.name}** (user class)\n\n${userClass.documentation || ''}`
                }
            };
        }

        // Check if it's a user-defined function
        const userSymbols = symbolManager.getAllSymbols();
        const userFunc = userSymbols.find(s => s.kind === 'function' && s.name === word);
        if (userFunc) {
            const params = userFunc.parameters?.join(', ') || '';
            return {
                contents: {
                    kind: MarkupKind.Markdown,
                    value: `**function ${userFunc.name}**(${params})\n\n${userFunc.documentation || ''}`
                }
            };
        }

        // Check if it's a built-in class
        const cls = apiLoader.getClass(word);
        if (cls) {
            return {
                contents: {
                    kind: MarkupKind.Markdown,
                    value: `**${cls.name}** (${cls.kind})\n\n${cls.info.summary}`
                }
            };
        }

        // Check if it's a method (look backwards for class name)
        const beforeWord = text.substring(0, wordRange.start);
        const classMatch = beforeWord.match(/(\w+)\.\w*$/);
        if (classMatch) {
            const className = classMatch[1];
            const methods = apiLoader.getMethods(className);
            const method = methods.find(m => m.label === word);
            
            if (method) {
                const params = method.parameters.map(p => 
                    `- \`${p.name}\`: ${p.type.name}${p.isOptional ? ' (optional)' : ''}`
                ).join('\n');

                return {
                    contents: {
                        kind: MarkupKind.Markdown,
                        value: `**${className}.${method.label}**\n\n${method.info.summary}\n\n**Parameters:**\n${params}\n\n**Returns:** ${method.returnType.name}`
                    }
                };
            }
        }

        return null;
    }
);

// Helper function to get word range at position
function getWordRangeAtPosition(text: string, offset: number): { start: number, end: number } | null {
    let start = offset;
    let end = offset;

    // Find start of word
    while (start > 0 && /\w/.test(text[start - 1])) {
        start--;
    }

    // Find end of word
    while (end < text.length && /\w/.test(text[end])) {
        end++;
    }

    if (start === end) return null;
    return { start, end };
}

// Make the text document manager listen on the connection
documents.listen(connection);

// Listen on the connection
connection.listen();
