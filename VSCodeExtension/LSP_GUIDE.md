# Language Server Implementation Guide

## Overview

To provide autocomplete, syntax highlighting, and IntelliSense for Custom Logic, you need to implement a Language Server Protocol (LSP) server.

## Architecture

```
VSCode Extension
    ├── Debug Adapter (existing)
    └── Language Server (new)
        ├── Parser (parse .cl files)
        ├── Symbol Provider (classes, methods, variables)
        ├── Completion Provider (autocomplete)
        ├── Hover Provider (documentation on hover)
        └── Diagnostic Provider (errors/warnings)
```

## Implementation Steps

### 1. Install LSP Dependencies

```bash
npm install vscode-languageclient vscode-languageserver vscode-languageserver-textdocument
```

### 2. Create Language Server

**src/server.ts** - The language server process
- Parses .cl files
- Provides completions from JSON metadata
- Tracks user-defined symbols
- Validates syntax

**src/extension.ts** - Updated to start language client
- Launches language server
- Communicates via LSP

### 3. Load JSON Metadata

Parse your JSON files (Game.json, etc.) on server startup to build completion items:

```typescript
interface CLMethod {
    label: string;
    parameters: Parameter[];
    returnType: Type;
    info: Documentation;
}

// Load from json/Game.json
const gameAPI = loadAPIFromJSON('./json/Game.json');
```

### 4. Implement Completion Provider

Provide autocomplete for:
- Built-in classes (Game, UI, etc.)
- Built-in methods
- User-defined classes
- User-defined methods
- Local variables
- Keywords

### 5. Implement Symbol Provider

Track user symbols:
- Parse class definitions
- Parse function definitions
- Track variable scopes
- Build symbol table

### 6. Implement Hover Provider

Show documentation on hover using info from JSON files.

## File Structure

```
VSCodeExtension/
├── json/                      # API metadata
│   ├── Game.json
│   ├── UI.json
│   └── ...
├── src/
│   ├── extension.ts          # Extension entry (updated)
│   ├── debugAdapter.ts       # Debug adapter (existing)
│   ├── server.ts             # Language server (new)
│   ├── parser.ts             # CL parser (new)
│   ├── symbolProvider.ts     # Symbol tracking (new)
│   └── completionProvider.ts # Autocomplete (new)
└── syntaxes/
    └── cl.tmLanguage.json    # Syntax highlighting (existing)
```

## Resources

- [LSP Specification](https://microsoft.github.io/language-server-protocol/)
- [VSCode Language Server Example](https://github.com/microsoft/vscode-extension-samples/tree/main/lsp-sample)
- [Language Server Guide](https://code.visualstudio.com/api/language-extensions/language-server-extension-guide)
