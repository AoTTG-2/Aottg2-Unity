# Custom Logic Debugger - VSCode Extension Implementation Guide

This directory contains the foundation for the Custom Logic debugger VSCode extension. The C# debugger server is already implemented in the main project. This guide will help you complete the VSCode extension.

## Architecture Overview

### C# Side (Already Implemented)
- `CustomLogicDebugger.cs`: Debugger server implementing Debug Adapter Protocol (DAP) over TCP
- Integration with `CustomLogicEvaluator.cs`: Hooks for statement execution, exceptions, and call stack
- Port 4711 server for VSCode connection

### VSCode Side (To Be Implemented)
The extension consists of:
1. **Debug Adapter**: Communicates with the C# server via TCP
2. **Language Support**: Syntax highlighting for .cl files
3. **Extension Activation**: Registers debugger and language

## Files to Create

### 1. src/extension.ts (Extension Entry Point)

```typescript
import * as vscode from 'vscode';
import { DebugAdapterDescriptorFactory } from './debugAdapter';

export function activate(context: vscode.ExtensionContext) {
    context.subscriptions.push(
        vscode.debug.registerDebugAdapterDescriptorFactory('cl', new DebugAdapterDescriptorFactory())
    );
}

export function deactivate() {}
```

### 2. src/debugAdapter.ts (Main Debug Adapter)

This file implements the Debug Adapter Protocol client that communicates with the C# server.

Key responsibilities:
- Connect to localhost:4711 via TCP
- Send/receive DAP messages (JSON-RPC over TCP)
- Handle breakpoints, stepping, variable requests
- Translate between VSCode API and DAP protocol

Structure:
```typescript
import * as Net from 'net';
import { DebugSession } from 'vscode-debugadapter';
import { DebugProtocol } from 'vscode-debugprotocol';

export class CLDebugSession extends DebugSession {
    private socket: Net.Socket;
    private messageBuffer: string = '';
    
    // Implement DAP protocol methods:
    protected initializeRequest(...)
    protected launchRequest(...)
    protected setBreakPointsRequest(...)
    protected threadsRequest(...)
    protected stackTraceRequest(...)
    protected scopesRequest(...)
    protected variablesRequest(...)
    protected continueRequest(...)
    protected nextRequest(...)
    protected stepInRequest(...)
    protected stepOutRequest(...)
    protected pauseRequest(...)
    protected disconnectRequest(...)
}
```

### 3. src/debugAdapterFactory.ts

```typescript
import * as vscode from 'vscode';
import * as Net from 'net';

export class DebugAdapterDescriptorFactory implements vscode.DebugAdapterDescriptorFactory {
    createDebugAdapterDescriptor(
        session: vscode.DebugSession,
        executable: vscode.DebugAdapterExecutable | undefined
    ): vscode.ProviderResult<vscode.DebugAdapterDescriptor> {
        const port = session.configuration.port || 4711;
        return new vscode.DebugAdapterServer(port);
    }
}
```

### 4. syntaxes/cl.tmLanguage.json (Syntax Highlighting)

TextMate grammar for .cl files. Example structure:

```json
{
  "scopeName": "source.cl",
  "patterns": [
    { "include": "#keywords" },
    { "include": "#strings" },
    { "include": "#comments" },
    { "include": "#numbers" }
  ],
  "repository": {
    "keywords": {
      "patterns": [{
        "name": "keyword.control.cl",
        "match": "\\b(if|else|while|for|return|class|void|int|float|string|bool|wait)\\b"
      }]
    },
    "strings": {
      "name": "string.quoted.double.cl",
      "begin": "\"",
      "end": "\"",
      "patterns": [{"name": "constant.character.escape.cl", "match": "\\\\."}]
    },
    "comments": {
      "patterns": [
        {"name": "comment.line.double-slash.cl", "match": "//.*$"},
        {"name": "comment.block.cl", "begin": "/\\*", "end": "\\*/"}
      ]
    },
    "numbers": {
      "name": "constant.numeric.cl",
      "match": "\\b\\d+(\\.\\d+)?\\b"
    }
  }
}
```

## Debug Adapter Protocol Implementation

### Message Format
Messages between VSCode and C# server follow DAP format:
```
Content-Length: <length>\r\n\r\n
<JSON message>
```

### Key Message Types

#### Initialize Sequence
1. VSCode ? Server: `initialize` request
2. Server ? VSCode: `initialize` response + `initialized` event
3. VSCode ? Server: `setBreakpoints` requests
4. VSCode ? Server: `configurationDone` request

#### Launch Sequence
1. VSCode ? Server: `launch` request with program path
2. Server ? VSCode: `launch` response
3. Game execution starts

#### Breakpoint Hit
1. Server ? VSCode: `stopped` event (reason: "breakpoint")
2. VSCode ? Server: `threads` request
3. Server ? VSCode: `threads` response
4. VSCode ? Server: `stackTrace` request
5. Server ? VSCode: `stackTrace` response
6. VSCode ? Server: `scopes` request
7. Server ? VSCode: `scopes` response
8. VSCode ? Server: `variables` request (for each scope)
9. Server ? VSCode: `variables` response

#### Stepping
- `continue`: Resume execution
- `next`: Step over
- `stepIn`: Step into function
- `stepOut`: Step out of function

### Example DAP Messages

**SetBreakpoints Request:**
```json
{
  "seq": 1,
  "type": "request",
  "command": "setBreakpoints",
  "arguments": {
    "source": {"path": "C:/path/to/main.cl"},
    "breakpoints": [{"line": 15}, {"line": 20}]
  }
}
```

**Stopped Event:**
```json
{
  "seq": 5,
  "type": "event",
  "event": "stopped",
  "body": {
    "reason": "breakpoint",
    "threadId": 1,
    "allThreadsStopped": true
  }
}
```

**Variables Response:**
```json
{
  "seq": 12,
  "type": "response",
  "request_seq": 11,
  "command": "variables",
  "success": true,
  "body": {
    "variables": [
      {"name": "playerCount", "value": "5", "type": "int", "variablesReference": 0},
      {"name": "myVar", "value": "\"hello\"", "type": "string", "variablesReference": 0}
    ]
  }
}
```

## Testing Strategy

### Unit Tests
Test individual components:
- Message parsing/formatting
- Breakpoint management
- Variable formatting

### Integration Tests
1. Mock C# server responding with DAP messages
2. Simulate breakpoint hits, stepping, variable inspection
3. Test error scenarios (connection lost, invalid responses)

### End-to-End Tests
1. Run actual game with debugger enabled
2. Set breakpoints via VSCode
3. Verify execution pauses correctly
4. Test all stepping operations
5. Verify variable inspection works

## Building and Testing

### Setup
```bash
cd VSCodeExtension
npm install
```

### Compile
```bash
npm run compile
```

### Watch Mode
```bash
npm run watch
```

### Package Extension
```bash
npm run package
```

### Debug Extension
1. Open VSCodeExtension folder in VSCode
2. Press F5 to launch Extension Development Host
3. In new window, open a .cl file
4. Try debugging with the game running

## DAP Reference Implementation

The C# server in `CustomLogicDebugger.cs` implements:

? **initialize** - Capability negotiation
? **setBreakpoints** - Breakpoint management  
? **launch** - Start debug session
? **threads** - Return thread list (single thread)
? **stackTrace** - Return call stack
? **scopes** - Return variable scopes (Local, Class)
? **variables** - Return variables for a scope
? **continue** - Resume execution
? **next** - Step over
? **stepIn** - Step into
? **stepOut** - Step out
? **pause** - Pause execution
? **disconnect** - End debug session

The VSCode extension needs to:
1. Send these requests at appropriate times
2. Handle the responses
3. Update VSCode UI (breakpoints, call stack, variables)

## Common Pitfalls

1. **TCP Message Framing**: Remember to include `Content-Length` header
2. **Path Normalization**: Convert Windows paths (backslashes) to forward slashes
3. **Sequence Numbers**: Track request/response seq numbers correctly
4. **Exception Handling**: Server may disconnect unexpectedly
5. **Coroutine Debugging**: Fast execution may skip breakpoints rapidly

## Resources

- [Debug Adapter Protocol Specification](https://microsoft.github.io/debug-adapter-protocol/)
- [VSCode Debug Extension Sample](https://github.com/microsoft/vscode-mock-debug)
- [vscode-debugadapter npm package](https://www.npmjs.com/package/vscode-debugadapter)
- [Debug Adapter Protocol in Action](https://code.visualstudio.com/api/extension-guides/debugger-extension)

## Next Steps

1. Implement `src/debugAdapter.ts` with full DAP message handling
2. Create syntax highlighting grammar for .cl files
3. Add icon and branding to `package.json`
4. Write tests for debug adapter
5. Test end-to-end with game
6. Publish to VSCode marketplace

## Support

For questions or issues:
- GitHub Issues: https://github.com/AoTTG-2/Aottg2-Unity/issues
- Discord: Join AoTTG2 server

---

**Status**: ?? C# server complete, VSCode extension scaffolding ready, implementation needed

The C# debugger server is fully functional and tested. The VSCode extension structure is defined but needs the TypeScript implementation completed as described above.
