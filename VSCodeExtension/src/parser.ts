import { TextDocument } from 'vscode-languageserver-textdocument';

export interface UserSymbol {
    name: string;
    kind: 'class' | 'function' | 'variable' | 'field';
    line: number;
    documentation?: string;
    type?: string; // For fields/variables, the inferred type (class name)
    parameters?: string[];
    parent?: string; // For methods/fields, the class they belong to
}

/**
 * Parses Custom Logic files to extract user-defined symbols
 */
export class CLParser {
    private classPattern = /class\s+(\w+)\s*{/g;
    private functionPattern = /function\s+(\w+)\s*\(/g;
    private fieldPattern = /^\s*(\w+)\s*=/gm;
    private parameterPattern = /function\s+\w+\s*\(([^)]*)\)/;

    /**
     * Parse a document and extract all symbols
     */
    public parseDocument(document: TextDocument): UserSymbol[] {
        const text = document.getText();
        const symbols: UserSymbol[] = [];
        const lines = text.split('\n');

        // Track current class context
        let currentClass: string | null = null;
        let classDepth = 0;
        let braceDepth = 0;
        let inFunction = false;
        let functionDepth = 0;
        
        // Map to track field symbols for type updates
        const fieldMap = new Map<string, UserSymbol>();

        // Parse line by line to maintain context
        for (let i = 0; i < lines.length; i++) {
            const line = lines[i];
            const trimmedLine = line.trim();

            // Count braces on this line
            const openBraces = (line.match(/{/g) || []).length;
            const closeBraces = (line.match(/}/g) || []).length;
            
            // Check if we've exited the current function BEFORE updating depth
            if (inFunction && braceDepth + openBraces - closeBraces <= functionDepth) {
                inFunction = false;
            }
            
            // Check if we've exited the current class BEFORE updating depth
            if (currentClass && braceDepth + openBraces - closeBraces <= classDepth) {
                currentClass = null;
                inFunction = false;
            }

            // Find class declarations
            const classMatch = /class\s+(\w+)/.exec(trimmedLine);
            if (classMatch) {
                const className = classMatch[1];
                currentClass = className;
                // Set classDepth to current depth (before counting braces on this line)
                classDepth = braceDepth;
                inFunction = false;
                
                console.log(`[Parser] Line ${i + 1}: Found class '${className}', setting currentClass='${currentClass}', classDepth=${classDepth}, braceDepth=${braceDepth}`);

                symbols.push({
                    name: className,
                    kind: 'class',
                    line: i + 1,
                    documentation: this.extractComment(lines, i)
                });
                // Don't continue - we need to process field declarations on subsequent lines
            }

            // Find function declarations
            const functionMatch = /function\s+(\w+)\s*\(([^)]*)\)/.exec(trimmedLine);
            if (functionMatch) {
                const funcName = functionMatch[1];
                const params = functionMatch[2]
                    .split(',')
                    .map(p => p.trim())
                    .filter(p => p.length > 0);

                symbols.push({
                    name: funcName,
                    kind: 'function',
                    line: i + 1,
                    parameters: params,
                    parent: currentClass || undefined,
                    documentation: this.extractComment(lines, i)
                });
                
                // Mark that we're now inside a function
                // Set functionDepth to current depth (before counting braces on this line)
                inFunction = true;
                functionDepth = braceDepth;
                // Don't continue - we might have same-line braces
            }

            // Find field declarations (name = value)
            // Only count as field if we're in a class but NOT in a function
            const fieldMatch = /^\s*(\w+)\s*=/.exec(line);
            const selfFieldMatch = /^\s*self\.(\w+)\s*=/.exec(line);
            
            if (fieldMatch && !selfFieldMatch) {
                const fieldName = fieldMatch[1];
                
                // Debug: log what we found
                console.log(`[Parser] Line ${i + 1}: Found '${fieldName}' = ..., currentClass=${currentClass}, inFunction=${inFunction}, braceDepth=${braceDepth}, functionDepth=${functionDepth}`);
                
                if (currentClass && !inFunction && !this.isKeyword(fieldName)) {
                    console.log(`[Parser]   -> Adding as FIELD`);
                    
                    // Try to infer type from assignment
                    const inferredType = this.inferType(line);
                    
                    const fieldSymbol: UserSymbol = {
                        name: fieldName,
                        kind: 'field',
                        line: i + 1,
                        parent: currentClass,
                        type: inferredType,
                        documentation: this.extractComment(lines, i)
                    };
                    
                    symbols.push(fieldSymbol);
                    
                    // Track field for potential type updates
                    const key = `${currentClass}.${fieldName}`;
                    fieldMap.set(key, fieldSymbol);
                } else if (!currentClass && trimmedLine !== '' && !trimmedLine.startsWith('//') && !trimmedLine.startsWith('#')) {
                    const varName = fieldMatch[1];
                    
                    // Global variable
                    if (!this.isKeyword(varName)) {
                        const inferredType = this.inferType(line);
                        symbols.push({
                            name: varName,
                            kind: 'variable',
                            line: i + 1,
                            type: inferredType,
                            documentation: this.extractComment(lines, i)
                        });
                    }
                }
            } else if (selfFieldMatch && currentClass && inFunction) {
                // Assignment to a field inside a function (like self._tester = Tester(...))
                const selfFieldName = selfFieldMatch[1];
                const key = `${currentClass}.${selfFieldName}`;
                const existingField = fieldMap.get(key);
                
                console.log(`[Parser] Line ${i + 1}: Found self.${selfFieldName} assignment in function`);
                
                if (existingField) {
                    // Update the type if we can infer a better one
                    const inferredType = this.inferType(line);
                    if (inferredType && (!existingField.type || existingField.type === 'null')) {
                        console.log(`[Parser]   -> Updating field '${selfFieldName}' type from '${existingField.type}' to '${inferredType}'`);
                        existingField.type = inferredType;
                    }
                }
            }
            
            // Update brace depth AFTER all processing for this line
            braceDepth += openBraces - closeBraces;
        }

        return symbols;
    }

    /**
     * Extract local variables from a function scope
     */
    public extractLocalVariables(text: string, line: number): UserSymbol[] {
        const lines = text.split('\n');
        const symbols: UserSymbol[] = [];

        // Find the function this line is in
        let functionStart = -1;
        let braceDepth = 0;

        for (let i = line - 1; i >= 0; i--) {
            const currentLine = lines[i];
            
            // Check for function BEFORE updating brace depth
            // braceDepth <= 0 handles both: brace on same line (0) and brace on next line (-1)
            if (/function\s+\w+\s*\(/.test(currentLine) && braceDepth <= 0) {
                functionStart = i;
                break;
            }
            
            const closeBraces = (currentLine.match(/}/g) || []).length;
            const openBraces = (currentLine.match(/{/g) || []).length;
            
            braceDepth += closeBraces - openBraces;
        }

        if (functionStart === -1) return symbols;

        // Parse variables in the function scope up to the current line
        braceDepth = 0;
        for (let i = functionStart; i < line && i < lines.length; i++) {
            const currentLine = lines[i];
            braceDepth += (currentLine.match(/{/g) || []).length;
            braceDepth -= (currentLine.match(/}/g) || []).length;

            // Look for variable assignments (not self.field assignments)
            const varMatch = /^\s*(\w+)\s*=/.exec(currentLine);
            if (varMatch && !/^\s*self\./.test(currentLine)) {
                const varName = varMatch[1];
                if (!this.isKeyword(varName)) {
                    const existing = symbols.find(s => s.name === varName);
                    const inferredType = this.inferType(currentLine);
                    
                    if (existing) {
                        // Always update type on reassignment if we can infer a type
                        // This handles cases like: a = List(); a = Vector3();
                        if (inferredType) {
                            existing.type = inferredType;
                        }
                    } else {
                        symbols.push({
                            name: varName,
                            kind: 'variable',
                            line: i + 1,
                            type: inferredType
                        });
                    }
                }
            }

            // Look for 'for' loop variables
            const forMatch = /for\s*\(\s*(\w+)\s+in/.exec(currentLine);
            if (forMatch) {
                const varName = forMatch[1];
                if (!symbols.find(s => s.name === varName)) {
                    symbols.push({
                        name: varName,
                        kind: 'variable',
                        line: i + 1
                    });
                }
            }
        }

        return symbols;
    }

    /**
     * Find what class we're currently in
     */
    public findCurrentClass(text: string, line: number): string | null {
        const lines = text.split('\n');
        let currentClass: string | null = null;
        let braceDepth = 0;
        let classDepth = 0;

        for (let i = 0; i < line && i < lines.length; i++) {
            const currentLine = lines[i];
            const openBraces = (currentLine.match(/{/g) || []).length;
            const closeBraces = (currentLine.match(/}/g) || []).length;

            if (currentClass) {
                braceDepth += openBraces - closeBraces;
                if (braceDepth <= classDepth) {
                    currentClass = null;
                }
            }

            const classMatch = /class\s+(\w+)/.exec(currentLine);
            if (classMatch) {
                currentClass = classMatch[1];
                classDepth = braceDepth;
                braceDepth += openBraces - closeBraces;
            }
        }

        return currentClass;
    }

    /**
     * Extract comment above a line
     */
    private extractComment(lines: string[], lineIndex: number): string | undefined {
        if (lineIndex === 0) return undefined;

        const prevLine = lines[lineIndex - 1].trim();
        
        // Check for single-line comment
        if (prevLine.startsWith('//') || prevLine.startsWith('#')) {
            return prevLine.replace(/^(\/\/|#)\s*/, '');
        }

        return undefined;
    }

    /**
     * Infer the type from an assignment expression
     * Examples:
     *   _tester = Tester(...) -> "Tester"
     *   x = List() -> "List"
     *   self._field = SomeClass(...) -> "SomeClass"
     *   _tester = null -> undefined (no type info)
     */
    private inferType(line: string): string | undefined {
        // Check for null assignment
        if (/=\s*null\s*[;#]?/.test(line)) {
            return undefined;
        }
        
        // Match constructor calls: ClassName(...)
        const constructorMatch = /=\s*(\w+)\s*\(/.exec(line);
        if (constructorMatch) {
            const typeName = constructorMatch[1];
            // Don't return language keywords as types, but allow built-in classes
            if (!this.isKeyword(typeName)) {
                return typeName;
            }
        }
        
        return undefined;
    }

    /**
     * Check if a word is a keyword
     */
    private isKeyword(word: string): boolean {
        const keywords = [
            'if', 'else', 'while', 'for', 'in', 'return', 'break', 'continue',
            'class', 'function', 'self', 'wait', 'true', 'false', 'null',
            'int', 'float', 'string', 'bool', 'void'
        ];
        return keywords.includes(word);
    }
}
