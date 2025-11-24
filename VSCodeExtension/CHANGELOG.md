# Changelog

All notable changes to the Custom Logic Debugger extension will be documented in this file.

## [1.0.0] - 2025-11-23

### Added - Initial Implementation

#### Debug Adapter Protocol (DAP) Client
- **Full DAP implementation** in `src/debugAdapter.ts`:
  - TCP socket connection to C# debugger server (port 4711)
  - DAP message framing with Content-Length headers
  - Request/response correlation with sequence numbers
  - Proper async/await patterns for all requests
  
- **Supported DAP Commands**:
  - `initialize` - Capability negotiation with server
  - `launch` - Start debug session with program path
  - `setBreakpoints` - Manage breakpoints per source file
  - `configurationDone` - Signal configuration complete
  - `threads` - Retrieve thread list (single thread model)
  - `stackTrace` - Get call stack frames
  - `scopes` - Get variable scopes (Local, Class)
  - `variables` - Retrieve variables for a scope
  - `continue` - Resume execution
  - `next` - Step over current line
  - `stepIn` - Step into function call
  - `stepOut` - Step out of current function
  - `pause` - Pause execution
  - `disconnect` - End debug session

- **Event Handling**:
  - `stopped` - Handle breakpoint hits and exceptions
  - `continued` - Handle resume events
  - `output` - Display debugger output
  - `terminated` - Handle session termination
  - `breakpoint` - Handle breakpoint verification

#### Debug Adapter Factory
- **DebugAdapterDescriptorFactory** implementation:
  - Creates debug adapter server descriptors
  - Configurable port (default: 4711)
  - Proper integration with VSCode debug API

#### Language Support
- **Syntax Highlighting** (`syntaxes/cl.tmLanguage.json`):
  - Keywords: `if`, `else`, `while`, `for`, `in`, `return`, `break`, `continue`, `wait`, `class`, `function`, `self`
  - Comments: `//` and `#` line comments, `/* */` block comments
  - String literals: double and single quoted with escape sequences
  - Numeric literals: integers and floating-point with scientific notation
  - Operators: arithmetic, comparison, logical, assignment
  - Built-in types: `int`, `float`, `string`, `bool`, `void`
  - Built-in classes: `Game`, `UI`, `Convert`, `String`, `List`, `Range`, `Int2`
  - Constants: `true`, `false`, `null`
  - Function name highlighting
  - Class name highlighting

- **Language Configuration** (`language-configuration.json`):
  - Auto-closing pairs: brackets, parentheses, quotes
  - Comment toggling support
  - Bracket matching
  - Code folding regions
  - Surrounding pairs

#### Extension Configuration
- **Extension Manifest** (`package.json`):
  - Debugger contribution point
  - Language contribution point
  - Grammar contribution point
  - Breakpoint support for `.cl` files
  - Launch configuration templates
  - Proper activation events

- **Debug Configurations**:
  - Default launch configuration for debugging `.cl` files
  - Configurable port number
  - Stop-on-entry option
  - Trace/logging support
  - Configuration snippets for easy setup

#### Development Setup
- **TypeScript Configuration** (`tsconfig.json`):
  - Proper module resolution
  - Source maps for debugging
  - Strict type checking

- **Build System**:
  - Compile script using TypeScript compiler
  - Watch mode for development
  - Package script using vsce
  - Publish script for marketplace

- **VSCode Integration** (`.vscode/`):
  - Launch configurations for extension development
  - Tasks for building and watching
  - Debug adapter standalone debugging

- **Package Configuration**:
  - `.vscodeignore` for clean packaging
  - Excludes source files and development files
  - Includes only compiled output and assets

#### Documentation
- **README.md**: Comprehensive implementation guide
- **QUICKSTART.md**: User and developer quick start guide
- **Example Script** (`example.cl`): Sample Custom Logic script

### Technical Highlights

#### Robust Message Handling
- Buffered TCP message parsing
- Proper Content-Length header handling
- Error recovery and connection management
- Graceful disconnection and cleanup

#### Path Normalization
- Cross-platform path handling
- Backslash to forward slash conversion
- Absolute path resolution

#### Type Safety
- Full TypeScript typing
- Interface definitions for launch arguments
- Proper use of Debug Adapter Protocol types

#### Error Handling
- Connection error handling
- Request timeout handling
- Server disconnect handling
- Proper error messages to user

### Dependencies
- `vscode-debugadapter@^1.51.0` - Debug adapter framework
- `await-notify@1.0.1` - Async notification utility
- `typescript@^4.9.5` - TypeScript compiler
- `@types/vscode@^1.60.0` - VSCode type definitions
- `@types/node@^14.14.37` - Node.js type definitions
- `@vscode/vsce@^2.19.0` - VSCode extension packaging

### Architecture

#### Client-Server Model
```
VSCode Extension (TypeScript)
    ↕ TCP Socket (port 4711)
    ↕ DAP Messages (JSON-RPC)
    ↕
C# Debugger Server (Unity Game)
    ↕ Breakpoints & Stepping
    ↕
Custom Logic Evaluator (Game Runtime)
```

#### Message Flow
```
VSCode → initialize → Server
VSCode ← initialize response ← Server
VSCode ← initialized event ← Server
VSCode → setBreakpoints → Server
VSCode ← setBreakpoints response ← Server
VSCode → launch → Server
VSCode ← launch response ← Server
...
Server → stopped event → VSCode
VSCode → stackTrace → Server
VSCode ← stackTrace response ← Server
VSCode → scopes → Server
VSCode ← scopes response ← Server
VSCode → variables → Server
VSCode ← variables response ← Server
...
```

### Known Limitations
- Single-threaded debugging only
- No watch expressions
- No conditional breakpoints
- No expression evaluation in debug console
- No variable editing

### Future Enhancements
- [ ] Conditional breakpoints
- [ ] Logpoints
- [ ] Watch expressions
- [ ] Expression evaluation
- [ ] Variable value editing
- [ ] Exception breakpoints
- [ ] Async/coroutine debugging improvements
- [ ] Performance profiling integration
- [ ] Code completion/IntelliSense
- [ ] Go to definition
- [ ] Find references
- [ ] Hover documentation

### Testing
- Tested with example.cl script
- Verified breakpoint setting and hitting
- Verified stepping operations (over, in, out)
- Verified variable inspection
- Verified call stack display
- Verified connection handling

### Platform Support
- Windows ✓
- macOS ✓
- Linux ✓

---

[1.0.0]: https://github.com/AoTTG-2/Aottg2-Unity/releases/tag/v1.0.0
