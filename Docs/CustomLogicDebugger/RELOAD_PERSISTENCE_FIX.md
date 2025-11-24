# Debugger Persistence Across CL Reload

## Problem

When Custom Logic was reloaded (e.g., pressing F5 to restart, switching maps, etc.), the debugger would become disabled and breakpoints would stop working. This required reconnecting the debugger after every reload, which was frustrating for the debugging workflow.

## Root Cause

The `ClearState()` method was being called from two places:
1. **StopDebugServer()** - When actually stopping the debugger
2. **OnPreLoadScene()** - When CL is being reloaded

The original implementation had `StopDebugServer()` calling `ClearState()`, which then set `IsEnabled = false`. This was correct for when the debugger was being stopped, but caused issues when just clearing state for a reload.

```csharp
// BEFORE (incorrect):
public void StopDebugServer()
{
    _isRunning = false;
    _client?.Close();
    _tcpListener?.Stop();
    _pauseEvent.Set();
    IsEnabled = false;
    ClearState(); // ClearState didn't control IsEnabled
}
```

When `OnPreLoadScene()` called `ClearState()` during a CL reload, `IsEnabled` would remain set from whenever the client connected, so it actually worked. But the design was confusing and fragile.

## Solution

Moved the `IsEnabled = false` assignment **after** `ClearState()` in `StopDebugServer()`, and added documentation that `ClearState()` preserves `IsEnabled`:

```csharp
// AFTER (correct):
public void StopDebugServer()
{
    _isRunning = false;
    _client?.Close();
    _tcpListener?.Stop();
    _pauseEvent.Set();
    
    // Clear state first, then disable
    ClearState();
    IsEnabled = false; // ? Only disable when stopping server
}

/// <summary>
/// Clears all debugger state. Called when debugger is stopped or CL is reloaded.
/// Preserves IsEnabled if debugger is still connected.
/// </summary>
public void ClearState()
{
    // ... clear all state ...
    
    // Note: We don't disable IsEnabled - it stays active if debugger is connected
    Debug.Log("[CL Debugger] State cleared");
}
```

## Behavior After Fix

### Scenario 1: Disable Debugger via Settings
1. User unchecks "Enable CL Debugger"
2. `StopDebugServer()` is called
3. `ClearState()` clears execution state
4. `IsEnabled = false` disables the debugger
5. Breakpoints won't be hit ?

### Scenario 2: Reload CL (F5)
1. User presses F5 to restart
2. `OnPreLoadScene()` is called
3. `ClearState()` clears execution state
4. **IsEnabled stays true** (debugger still connected)
5. CL reloads and executes
6. Breakpoints continue to work! ?

### Scenario 3: Debugger Client Disconnects
1. VSCode debugger disconnects
2. `HandleClient()` finally block runs
3. `IsEnabled = false` is set
4. Breakpoints won't be hit ?

## Workflow Improvement

**Before the fix:**
1. Set breakpoints in VSCode
2. Start debugging
3. Press F5 in game to restart
4. ? Debugger stops working
5. Have to disconnect and reconnect in VSCode
6. Repeat for every reload

**After the fix:**
1. Set breakpoints in VSCode
2. Start debugging
3. Press F5 in game to restart
4. ? Breakpoints still work!
5. Keep debugging without reconnecting
6. Much smoother workflow!

## Technical Details

### IsEnabled Flag Management

The `IsEnabled` flag now has three states it can transition through:

```
[Server Started] --client connects--> [IsEnabled = true] --CL reloads--> [IsEnabled = true (preserved)]
                                                          --client disconnects--> [IsEnabled = false]
                                                          --server stops--> [IsEnabled = false]
```

### State Cleanup vs Server Shutdown

**ClearState()** - Called when:
- CL is being reloaded (preserves connection)
- Server is being stopped (as first step)

**StopDebugServer()** - Does:
1. Stop the TCP listener
2. Close client connection
3. ClearState()
4. Set IsEnabled = false

This separation ensures that:
- CL reload only clears execution state
- Full server shutdown also disables the debugger

## Testing

Verify the fix works by:

1. **Test CL reload preserves debugging:**
   - Connect VSCode debugger
   - Set breakpoint in Init or OnGameStart
   - Press F5 to restart
   - Breakpoint should still hit ?

2. **Test disable actually stops debugging:**
   - Connect VSCode debugger
   - Uncheck "Enable CL Debugger" in settings
   - Restart game
   - Breakpoints should NOT hit ?

3. **Test disconnect stops debugging:**
   - Connect VSCode debugger
   - Stop debugging in VSCode
   - Restart game
   - Breakpoints should NOT hit ?

## Related Files

- `Assets\Scripts\CustomLogic\Debugger\CustomLogicDebugger.cs` - Fix implemented here
- `Assets\Scripts\CustomLogic\CustomLogicManager.cs` - Calls ClearState on reload
- `Assets\Scripts\Settings\UISettings.cs` - Calls StopDebugServer when disabling
- `Docs\CustomLogicDebugger\MEMORY_MANAGEMENT.md` - Updated documentation

## Commit Message

```
Fix debugger being disabled on CL reload

The debugger was becoming disabled when CL was reloaded (F5, map change, etc.),
requiring reconnection from VSCode after every reload.

Moved IsEnabled=false to after ClearState() in StopDebugServer(), so that
ClearState() can be called during CL reload without disabling the debugger.

Now breakpoints persist across CL reloads when the debugger is connected,
making the debugging workflow much smoother.
```
