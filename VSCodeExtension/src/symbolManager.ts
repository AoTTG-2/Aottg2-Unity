import { TextDocument } from 'vscode-languageserver-textdocument';
import { UserSymbol, CLParser } from './parser';

/**
 * Manages user-defined symbols across all open documents
 */
export class SymbolManager {
    private documentSymbols: Map<string, UserSymbol[]> = new Map();
    private parser: CLParser = new CLParser();

    /**
     * Update symbols for a document
     */
    public updateDocument(document: TextDocument): void {
        const symbols = this.parser.parseDocument(document);
        this.documentSymbols.set(document.uri, symbols);
    }

    /**
     * Remove symbols for a document
     */
    public removeDocument(uri: string): void {
        this.documentSymbols.delete(uri);
    }

    /**
     * Get all symbols from all documents
     */
    public getAllSymbols(): UserSymbol[] {
        const allSymbols: UserSymbol[] = [];
        for (const symbols of this.documentSymbols.values()) {
            allSymbols.push(...symbols);
        }
        return allSymbols;
    }

    /**
     * Get symbols for a specific document
     */
    public getDocumentSymbols(uri: string): UserSymbol[] {
        return this.documentSymbols.get(uri) || [];
    }

    /**
     * Get all class names
     */
    public getClassNames(): string[] {
        const names = new Set<string>();
        for (const symbols of this.documentSymbols.values()) {
            for (const symbol of symbols) {
                if (symbol.kind === 'class') {
                    names.add(symbol.name);
                }
            }
        }
        return Array.from(names);
    }

    /**
     * Get all methods/fields for a class
     */
    public getClassMembers(className: string): UserSymbol[] {
        const members: UserSymbol[] = [];
        console.log(`[SymbolManager] Looking for members of class '${className}'`);
        for (const [uri, symbols] of this.documentSymbols.entries()) {
            console.log(`[SymbolManager]   Checking document: ${uri}`);
            for (const symbol of symbols) {
                console.log(`[SymbolManager]     Symbol: ${symbol.name} (${symbol.kind}) parent='${symbol.parent}' (type: ${typeof symbol.parent})`);
                if (symbol.parent === className && (symbol.kind === 'function' || symbol.kind === 'field')) {
                    console.log(`[SymbolManager]       -> MATCH! Adding to members`);
                    members.push(symbol);
                }
            }
        }
        console.log(`[SymbolManager]   Total members found: ${members.length}`);
        return members;
    }

    /**
     * Find a class by name
     */
    public findClass(className: string): UserSymbol | undefined {
        for (const symbols of this.documentSymbols.values()) {
            const cls = symbols.find(s => s.kind === 'class' && s.name === className);
            if (cls) return cls;
        }
        return undefined;
    }

    /**
     * Get local variables at a specific position in a document
     */
    public getLocalVariables(document: TextDocument, line: number): UserSymbol[] {
        return this.parser.extractLocalVariables(document.getText(), line);
    }

    /**
     * Find what class the cursor is currently in
     */
    public getCurrentClass(document: TextDocument, line: number): string | null {
        return this.parser.findCurrentClass(document.getText(), line);
    }

    /**
     * Find a field by name in a class and return its type
     */
    public findFieldType(className: string, fieldName: string): string | undefined {
        console.log(`[SymbolManager] Looking for field '${fieldName}' in class '${className}'`);
        for (const symbols of this.documentSymbols.values()) {
            for (const symbol of symbols) {
                if (symbol.parent === className && 
                    symbol.kind === 'field' && 
                    symbol.name === fieldName) {
                    console.log(`[SymbolManager]   -> Found field with type: ${symbol.type}`);
                    return symbol.type;
                }
            }
        }
        console.log(`[SymbolManager]   -> Field not found`);
        return undefined;
    }

    /**
     * Find a local variable type at a specific position in a document
     */
    public findLocalVariableType(document: TextDocument, line: number, varName: string): string | undefined {
        console.log(`[SymbolManager] Looking for local variable '${varName}' at line ${line}`);
        const localVars = this.parser.extractLocalVariables(document.getText(), line);
        const variable = localVars.find(v => v.name === varName);
        if (variable) {
            console.log(`[SymbolManager]   -> Found local variable with type: ${variable.type}`);
            return variable.type;
        }
        console.log(`[SymbolManager]   -> Local variable not found`);
        return undefined;
    }
}
