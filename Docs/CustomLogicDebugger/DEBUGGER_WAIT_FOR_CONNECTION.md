# Custom Logic Debugger: Wait for Connection Feature

## Overview

The debugger server starts automatically when the game launches (if enabled), and can wait for a VSCode connection before executing Custom Logic. This allows you to debug initialization code like `Init()` and `OnGameStart()`.

## Settings

Three settings control the debugger behavior:

1. **EnableCLDebugger** (bool): Starts the debugger server when the game launches
2. **WaitForDebuggerConnection** (bool): When enabled, the game will pause before executing CL and wait for the debugger to connect
3. **DebuggerConnectionTimeout** (float): Maximum seconds to wait (0 = wait forever, default = 30 seconds)

## How to Use

### Option 1: Using UI Settings (Recommended)
1. Open Settings > UI
2. Enable "Enable CL Debugger" (starts server on game launch)
3. Enable "Wait For Debugger Connection" (optional - only if you want to wait)
4. Optionally adjust "Debugger Connection Timeout"
5. Restart the game for the debugger server to start
6. Connect from VSCode at any time!

### Option 2: Programmatic
```csharp
// In your code:
SettingsManager.UISettings.EnableCLDebugger.Value = true;  // Restart required
SettingsManager.UISettings.WaitForDebuggerConnection.Value = true;
SettingsManager.UISettings.DebuggerConnectionTimeout.Value = 60f; // 60 second timeout
```

## Workflow

### New Early Start Workflow
1. Enable "Enable CL Debugger" in settings
2. Restart game - debugger server starts immediately
3. Start VSCode debugger (F5) - connects to already-running server
4. Load into any game mode
5. Breakpoints work immediately!

### With Wait Enabled
1. Enable both "Enable CL Debugger" and "Wait For Debugger Connection"
2. Restart game - debugger server starts
3. Start a game mode
4. Game shows: "Waiting for debugger connection..."
5. Start VSCode debugger (F5)
6. Game continues and hits breakpoints in Init/OnGameStart

### Without Wait (Quick Attach)
1. Enable "Enable CL Debugger" only
2. Restart game - debugger server starts
3. Load into a game mode (CL executes immediately)
4. Start VSCode debugger (F5) at any time
5. Breakpoints in ongoing code (OnTick, OnFrame, etc.) work immediately

## Technical Details

- **Server starts in `ApplicationStart.Init()`**: Available from game launch
- **Connection can happen anytime**: Before or during gameplay
- **Wait logic is in `CustomLogicManager.StartLogic()`**: Only delays CL execution if enabled
- The wait uses `Thread.Sleep()` in a loop with 100ms polling
- Unity console will show:
  - "[CL Debugger] Started debug server on port 4711" (on game launch)
  - "[CL Debugger] Server started early - ready for connections before game start"
  - "[CL Debugger] Waiting for debugger connection (timeout: 30s)..." (if wait enabled)
  - "[CL Debugger] Debugger connected!" (on successful connection)

## Timeout Behavior

- **timeout > 0**: Wait for specified seconds, then continue without debugger
- **timeout = 0**: Wait forever (not recommended for normal gameplay)
- **Default**: 30 seconds

## Connection Flexibility

### Scenario 1: Debug From Game Start
- Enable both settings
- Server starts on launch
- Wait pauses execution
- Connect and debug everything

### Scenario 2: Hot Attach During Gameplay
- Enable only "Enable CL Debugger"
- Server starts on launch
- Load game (CL runs immediately)
- Attach debugger anytime during gameplay
- Debug ongoing events (OnTick, OnFrame, player interactions, etc.)

### Scenario 3: Pre-Connect Before Game Load
- Enable only "Enable CL Debugger"
- Server starts on launch
- Connect debugger from main menu
- Load game mode
- Already connected when Init runs!

## Tips

1. **For development**: Enable "Enable CL Debugger" permanently, enable "Wait" only when needed
2. **For testing**: Keep server running, hot-attach when issues occur
3. **For initialization debugging**: Enable "Wait" to catch Init/OnGameStart
4. **For runtime debugging**: Disable "Wait", attach during gameplay
5. **Server persists**: Once started, you can connect/disconnect multiple times without restarting

## Example Workflows

### Quick Debug Session
```
1. Game already running with debugger enabled
2. See an issue in-game
3. Press F5 in VSCode
4. Immediately connected
5. Set breakpoints
6. Reproduce issue
7. Hit breakpoints!
```

### Full Debug Session
```
1. Enable both "Enable CL Debugger" and "Wait"
2. Restart game
3. Start VSCode debugger (F5)
4. Load game mode
5. Game waits for connection
6. Debugger connects
7. Step through Init, OnGameStart, everything!
```

## Benefits Over Previous Implementation

? **No rush to connect** - Server is ready before you even load a game
? **Hot attach support** - Connect during active gameplay
? **Pre-connect support** - Connect from main menu
? **Flexible workflow** - Choose when to connect based on what you're debugging
? **Server persistence** - Stays running, can reconnect without restart
? **Optional waiting** - Choose if you want execution to pause or not

## Example

```csharp
class Main
{
    int _value = 0;  // Set breakpoint here
    
    function Init()
    {
        Debug("Initializing..."); // Set breakpoint here
        _value = 42;
    }
    
    function OnTick()
    {
        // Can attach debugger DURING gameplay and hit this breakpoint!
        if (_value > 100)
        {
            Debug("High value!");  // Hot-attach works here!
        }
    }
}
```

**Without wait**: Init runs immediately, but you can still debug OnTick by hot-attaching
**With wait**: Everything is debuggable from the start!
