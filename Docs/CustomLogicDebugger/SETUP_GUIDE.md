# Custom Logic Debugger Setup Guide

This guide will help you set up VSCode for debugging Custom Logic (.cl) scripts in AoTTG2.

## Prerequisites

- Visual Studio Code
- Node.js (v14 or higher)
- AoTTG2 game with debugger enabled

## Part 1: Enable the Debugger in Game

1. Launch AoTTG2
2. Go to Settings ? UI Settings
3. Enable "Enable CL Debugger"
4. Restart the game
5. The debugger server will start automatically on port 4711

## Part 2: Install VSCode Extension

### Method 1: From Marketplace (Coming Soon)

Search for "Custom Logic Debugger" in the VSCode extensions marketplace.

### Method 2: Manual Installation

1. Navigate to the `VSCodeExtension` folder in this repository
2. Install dependencies:
   ```bash
   npm install
   ```
3. Build the extension:
   ```bash
   npm run compile
   ```
4. Package the extension:
   ```bash
   npm run package
   ```
5. Install the generated `.vsix` file in VSCode:
   - Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
   - Type "Extensions: Install from VSIX"
   - Select the generated `.vsix` file

## Part 3: Configure Launch Settings

1. Open your Custom Logic project folder in VSCode
2. Create a `.vscode` folder in your project root (if it doesn't exist)
3. Create a `launch.json` file in the `.vscode` folder with this content:

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "cl",
            "request": "launch",
            "name": "Debug CL Script",
            "program": "${file}",
            "stopOnEntry": false
        }
    ]
}
```

## Part 4: Start Debugging

1. Open a `.cl` file in VSCode
2. Set breakpoints by clicking in the gutter (left of line numbers)
3. Press `F5` or click "Run ? Start Debugging"
4. The debugger will connect to the game on port 4711
5. Run your Custom Logic script in-game
6. Execution will pause at breakpoints

## Debugging Features

### Breakpoints
- Click in the gutter to set/remove breakpoints
- Breakpoints are shown as red dots
- Execution pauses when a breakpoint is hit

### Step Controls
- **Continue** (F5): Resume execution until next breakpoint
- **Step Over** (F10): Execute current line and move to next
- **Step Into** (F11): Step into function calls
- **Step Out** (Shift+F11): Step out of current function

### Variable Inspection
- **Local Variables**: See variables in current scope
- **Class Variables**: See instance variables
- **Call Stack**: View the current call stack

### Exception Handling
- Debugger automatically pauses on CL runtime exceptions
- View exception details in the debug console
- See the line where the exception occurred

## File Structure

Your Custom Logic project should look like:
```
MyProject/
??? .vscode/
?   ??? launch.json
??? main.cl
??? components/
?   ??? MyComponent1.cl
?   ??? MyComponent2.cl
??? extensions/
    ??? MyExtension.cl
```

## Troubleshooting

### Debugger Won't Connect

1. Verify the game is running and debugger is enabled
2. Check that port 4711 isn't blocked by firewall
3. Ensure you're running the game and VSCode on the same machine

### Breakpoints Don't Work

1. Make sure the file path in VSCode matches the file loaded in-game
2. Verify breakpoints are on executable lines (not comments or blank lines)
3. Check that the script is actually being executed in-game

### Variables Not Showing

1. Ensure you're paused at a breakpoint
2. Check the Variables panel is visible (View ? Run and Debug)
3. Verify variable names don't conflict with reserved keywords

## Advanced Usage

### Conditional Breakpoints

Right-click on a breakpoint and select "Edit Breakpoint" to add conditions:
```
myVariable > 10
playerCount == 5
```

### Log Points

Instead of breaking, log a message to the debug console:
Right-click in gutter ? Add Logpoint ? Enter message

### Watch Expressions

Add expressions to watch in the Watch panel:
- Click "+" in Watch panel
- Enter expression (e.g., `myVar + 10`, `player.Health`)

## Known Limitations

1. Cannot step backwards through code
2. Cannot modify variables at runtime
3. Coroutines are debugged as they execute (may skip rapidly)
4. Built-in C# methods show as single steps

## MapScript Support

For MapScript .txt files containing CL code:

The debugger automatically handles line offsets for MapScript files.
Line numbers in VSCode will correspond to the actual lines in your file,
including the map data sections.

## Getting Help

- Check the [GitHub Issues](https://github.com/AoTTG-2/Aottg2-Unity/issues)
- Join the [AoTTG2 Discord](https://discord.gg/aottg2)
- Read the [Custom Logic Documentation](link-to-docs)

## Example Debug Session

1. Set a breakpoint on line 15 of your `main.cl`:
   ```cl
   class Main
   {
       void OnGameStart()
       {
           int playerCount = Game.Players.Size();  // <- Breakpoint here
           Game.Print("Players: " + playerCount);
       }
   }
   ```

2. Start debugging (F5)
3. Start a game in AoTTG2
4. VSCode will pause at the breakpoint
5. Inspect `playerCount` in the Variables panel
6. Step through code or continue execution

Happy debugging!
