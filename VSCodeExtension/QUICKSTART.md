# Quick Start Guide - Custom Logic Debugger

This guide will help you get started with developing and using the Custom Logic Debugger VSCode extension.

## Prerequisites

- Node.js (v14 or higher)
- Visual Studio Code
- TypeScript knowledge for development

## Installation & Setup

### 1. Install Dependencies

```bash
cd VSCodeExtension
npm install
```

### 2. Compile the Extension

```bash
npm run compile
```

Or use watch mode for development:

```bash
npm run watch
```

## Development

### Testing the Extension

1. Open the `VSCodeExtension` folder in VSCode
2. Press `F5` to launch the Extension Development Host
3. In the new VSCode window:
   - Open a `.cl` file (e.g., `example.cl`)
   - Set breakpoints by clicking in the gutter
   - Start debugging with `F5`
   - Select "Debug CL Script" configuration

### Project Structure

```
VSCodeExtension/
├── src/
│   ├── extension.ts        # Extension entry point
│   └── debugAdapter.ts     # Debug adapter implementation
├── syntaxes/
│   └── cl.tmLanguage.json  # Syntax highlighting
├── .vscode/
│   ├── launch.json         # Debug configurations
│   └── tasks.json          # Build tasks
├── package.json            # Extension manifest
└── language-configuration.json  # Language config
```

## Using the Debugger

### 1. Ensure the Game is Running

The C# debugger server must be running on port 4711. Start the AoTTG2 game with debugging enabled.

### 2. Create a Launch Configuration

Create or use `.vscode/launch.json` in your Custom Logic project:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "cl",
      "request": "launch",
      "name": "Debug CL Script",
      "program": "${file}",
      "stopOnEntry": false,
      "port": 4711
    }
  ]
}
```

### 3. Start Debugging

1. Open your `.cl` file
2. Set breakpoints by clicking in the gutter (left of line numbers)
3. Press `F5` or click "Run and Debug"
4. Select "Debug CL Script"

### 4. Debug Controls

- **Continue (F5)**: Resume execution
- **Step Over (F10)**: Execute current line and move to next
- **Step Into (F11)**: Step into function calls
- **Step Out (Shift+F11)**: Step out of current function
- **Pause**: Pause execution at current position

### 5. Inspect Variables

When paused at a breakpoint:
- View local variables in the **Variables** panel
- View class members in the **Class** scope
- See the call stack in the **Call Stack** panel

## Syntax Highlighting

The extension provides syntax highlighting for `.cl` files with support for:

- **Keywords**: `if`, `else`, `while`, `for`, `in`, `return`, `break`, `continue`, `wait`, `class`, `function`, `self`
- **Comments**: `//` line comments, `#` line comments, `/* */` block comments
- **Strings**: Double and single quoted strings with escape sequences
- **Numbers**: Integers and floating-point numbers
- **Operators**: Arithmetic, comparison, logical, and assignment operators
- **Built-in types**: `int`, `float`, `string`, `bool`, `void`
- **Built-in classes**: `Game`, `UI`, `Convert`, `String`, `List`, `Range`
- **Constants**: `true`, `false`, `null`

## Packaging the Extension

To create a `.vsix` package for distribution:

```bash
npm run package
```

This creates a `cl-debugger-1.0.0.vsix` file that can be installed in VSCode.

## Publishing the Extension

To publish to the VSCode Marketplace:

1. Create a Personal Access Token on Azure DevOps
2. Login with vsce:
   ```bash
   npx vsce login aottg2
   ```
3. Publish:
   ```bash
   npm run publish
   ```

## Troubleshooting

### Connection Issues

**Problem**: "Failed to connect to debugger server on port 4711"

**Solutions**:
- Ensure the game is running
- Check that debugging is enabled in game settings
- Verify no firewall is blocking port 4711
- Check the port number in launch configuration matches the game

### Breakpoints Not Working

**Problem**: Breakpoints are set but not hit

**Solutions**:
- Verify the file path matches exactly (case-sensitive)
- Ensure the script is actually loaded by the game
- Check that the breakpoint is on an executable line (not comments/blank lines)
- Try setting breakpoints after launching the debugger

### Variables Not Showing

**Problem**: Variables panel is empty or shows incorrect values

**Solutions**:
- Ensure execution is paused at a breakpoint
- Check that variables are in scope at the current execution point
- Try stepping through code to see variable updates

### Syntax Highlighting Issues

**Problem**: Code is not colored correctly

**Solutions**:
- Ensure file extension is `.cl`
- Reload VSCode window (Ctrl+Shift+P → "Developer: Reload Window")
- Check that the language is set to "Custom Logic" in the bottom right

## Advanced Features

### Custom Port Configuration

If the game uses a different port, modify your launch configuration:

```json
{
  "type": "cl",
  "request": "launch",
  "name": "Debug CL Script (Custom Port)",
  "program": "${file}",
  "port": 5000
}
```

### Stop on Entry

To pause execution immediately when launching:

```json
{
  "type": "cl",
  "request": "launch",
  "name": "Debug CL Script (Stop on Entry)",
  "program": "${file}",
  "stopOnEntry": true
}
```

### Debug Protocol Tracing

To see raw DAP messages for debugging the debugger:

```json
{
  "type": "cl",
  "request": "launch",
  "name": "Debug CL Script (Trace)",
  "program": "${file}",
  "trace": true
}
```

## Development Tips

### Hot Reload

When developing the extension:
1. Use `npm run watch` to automatically recompile on changes
2. In the Extension Development Host, press `Ctrl+R` to reload

### Debugging the Debugger

To debug the debug adapter itself:
1. Set breakpoints in `src/debugAdapter.ts`
2. Use the "Debug CL Adapter" launch configuration
3. Attach to the running adapter process

### Testing Changes

After making changes:
1. Recompile with `npm run compile`
2. Reload Extension Development Host
3. Test with a `.cl` file

## Resources

- [Debug Adapter Protocol](https://microsoft.github.io/debug-adapter-protocol/)
- [VSCode Extension API](https://code.visualstudio.com/api)
- [TextMate Grammars](https://macromates.com/manual/en/language_grammars)
- [AoTTG2 GitHub](https://github.com/AoTTG-2/Aottg2-Unity)

## Support

For issues or questions:
- GitHub Issues: https://github.com/AoTTG-2/Aottg2-Unity/issues
- Discord: Join the AoTTG2 community server

---

Happy debugging! 🐛🔍
