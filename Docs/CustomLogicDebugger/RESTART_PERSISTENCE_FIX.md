# Debugger Persistence Across Game Restarts

## Problem

When pressing F5 to restart the game, the debugger connection was lost and breakpoints stopped working. This happened because:

1. **InGameManager.OnDestroy()** was called during restart
2. This called **StopDebugServer()** which closed the TCP connection
3. **InGameManager.Awake()** created a **new** debug server
4. VSCode was still connected to the **old** (now closed) server
5. New CL execution had no active debugger connection
6. **Breakpoints didn't work** until manually reconnecting from VSCode

### Logs showing the problem:
```
Clearing Custom Logic Debugger state in CLM.PreloadScene
[CL Debugger] State cleared
IGM Destroy, clearing debug server...     ? Server stopped!
[CL Debugger] Client disconnected         ? VSCode connection lost
[CL Debugger] State cleared
IGM Awake, starting debug server...       ? New server started
[CL Debugger] Started debug server on port 4711
                                          ? But VSCode still connected to old server!
```

## Root Cause

The debug server was **tied to the InGameManager lifecycle**, which gets destroyed and recreated on every restart. This caused the server to stop and restart, breaking the VSCode connection.

## Solution

Changed the debug server to **persist across scene reloads** by:

1. **Not stopping the server** when InGameManager is destroyed during restart
2. **Only starting the server** if it's not already running
3. **Only stopping the server** when explicitly disabled in settings or on application quit

### Code Changes

**InGameManager.Awake()** - Check if already running:
```csharp
// Start the CL debugger server if enabled (only if not already running)
if (SettingsManager.UISettings.EnableCLDebugger.Value && 
    !CustomLogicDebugger.Instance.IsConnected)
{
    Debug.Log("IGM Awake, starting debug server...");
    CustomLogicDebugger.Instance.StartDebugServer(4711);
}
```

**InGameManager.OnDestroy()** - Don't stop the server:
```csharp
private void OnDestroy()
{
    // Don't stop the debugger server on destroy - it should persist across scene reloads
    // Only stop it when the application quits or debugger is explicitly disabled
    Debug.Log("IGM Destroy (debugger server kept running for persistence across reloads)");
}
```

**UISettings.Apply()** - Only stop when disabled:
```csharp
if (EnableCLDebugger.Value)
{
    if (!CustomLogicDebugger.Instance.IsConnected)
    {
        CustomLogicDebugger.Instance.StartDebugServer();
    }
}
else
{
    CustomLogicDebugger.Instance.ClearState();
    if (CustomLogicDebugger.Instance.IsConnected)
    {
        CustomLogicDebugger.Instance.StopDebugServer();
    }
}
```

## Server Lifecycle

### Before Fix (Broken)
```
[Game Start]
  ? InGameManager.Awake()
  ? StartDebugServer() ?
  ? VSCode connects ?

[F5 Pressed - Restart]
  ? InGameManager.OnDestroy()
  ? StopDebugServer() ? Connection lost!
  ? InGameManager.Awake()
  ? StartDebugServer() (new server)
  ? VSCode still connected to old server ?
  ? Breakpoints don't work ?
```

### After Fix (Working)
```
[Game Start]
  ? InGameManager.Awake()
  ? StartDebugServer() ?
  ? VSCode connects ?

[F5 Pressed - Restart]
  ? InGameManager.OnDestroy()
  ? (server kept running) ?
  ? InGameManager.Awake()
  ? (server already running, skip start) ?
  ? VSCode still connected ?
  ? Breakpoints work! ?
```

## Debug Server Singleton

The `CustomLogicDebugger` is a **singleton** that persists:
- Created on first access: `CustomLogicDebugger.Instance`
- **Not** a MonoBehaviour (no Unity lifecycle)
- **Not** attached to any GameObject
- Lives for the entire application lifetime
- Only stopped when:
  1. User disables debugger in settings
  2. Application quits

## Workflow After Fix

### Happy Path
1. **Enable debugger** in settings
2. **Start game** ? Debug server starts
3. **Connect VSCode** ? Breakpoints work
4. **Press F5** to restart ? Server stays running
5. **Breakpoints still work!** ?
6. **Keep debugging** without reconnecting

### Disable/Re-enable
1. **Disable debugger** in settings
   - Calls `StopDebugServer()`
   - Closes VSCode connection
2. **Enable debugger** in settings
   - Calls `StartDebugServer()`
   - VSCode can reconnect

### Application Quit
1. Application closes
2. TCP listener automatically cleaned up
3. VSCode connection closes

## Testing Checklist

- [x] Start game with debugger enabled ? Server starts ?
- [x] Connect VSCode ? Breakpoints work ?
- [x] Press F5 to restart ? Breakpoints still work ?
- [x] Multiple restarts ? Connection persists ?
- [x] Disable debugger in settings ? Connection closes ?
- [x] Re-enable debugger ? Can reconnect ?
- [x] Quit game ? Server stops cleanly ?

## Benefits

? **No reconnection needed** after F5 restart  
? **Smooth debugging workflow** across reloads  
? **Breakpoints persist** during development  
? **Less interruption** when iterating on CL code  
? **Proper cleanup** when explicitly disabled  

## Edge Cases Handled

### Multiple InGameManager Instances
- Only starts server if not already running
- Uses `IsConnected` check to prevent duplicate servers

### Scene Transitions
- Server persists across all scenes
- InGameManager can be created/destroyed multiple times
- Server remains unaffected

### Settings Changes
- Disabling debugger properly stops server
- Re-enabling starts fresh server
- Can toggle on/off without restart

## Related Files

- `Assets\Scripts\GameManagers\InGameManager.cs` - InGameManager lifecycle
- `Assets\Scripts\Settings\UISettings.cs` - Settings apply logic
- `Assets\Scripts\CustomLogic\Debugger\CustomLogicDebugger.cs` - Debugger singleton
- `Docs\CustomLogicDebugger\RELOAD_PERSISTENCE_FIX.md` - Related CL state fix
- `Docs\CustomLogicDebugger\MEMORY_MANAGEMENT.md` - State cleanup

## Commit Message

```
Fix debugger server stopping on game restart (F5)

The debug server was being stopped when InGameManager was destroyed
during game restart, breaking the VSCode connection. This required
manually reconnecting from VSCode after every F5 press.

Changed InGameManager to not stop the server on destroy, only starting
it if not already running. The server now persists across scene reloads
and only stops when explicitly disabled in settings.

Enables smooth debugging workflow where breakpoints continue working
across multiple game restarts without reconnection.

Fixes:
- Debugger disconnecting on F5 restart
- Need to reconnect VSCode after every reload
- Interruption in debugging workflow
- Lost breakpoint state during development
```
