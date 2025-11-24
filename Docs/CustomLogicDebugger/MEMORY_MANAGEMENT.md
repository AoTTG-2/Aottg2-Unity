# Custom Logic Debugger - Memory Management

## Overview

The Custom Logic Debugger now properly manages its memory to prevent stale state and memory leaks when the debugger is disabled or when Custom Logic is reloaded.

## Memory Cleanup Scenarios

### 1. When Debugger is Disabled

When the user disables the debugger via the UI settings:
- **UISettings.Apply()** calls `ClearState()` before stopping the server
- All execution state, call stacks, and variable handles are cleared
- The debugger is ready to be re-enabled without stale data

### 2. When Custom Logic is Reloaded

When a scene is unloaded (which happens before CL reload):
- **OnPreLoadScene()** calls `ClearState()` to clean up debugger memory
- This happens automatically when:
  - Switching between game modes
  - Reloading a map
  - Leaving a room
  - Returning to main menu

### 3. When Debugger Server Stops

When the TCP server is stopped:
- **StopDebugServer()** calls `ClearState()` automatically
- Ensures clean state even if stopped programmatically

## What Gets Cleared

The `ClearState()` method clears the following:

### Execution State
- `_currentInstance` - Current class instance being executed
- `_currentLocalVariables` - Current local variables
- `_currentStatement` - Current AST statement
- `_currentFileName` - Reset to "main.cl"

### Call Stack
- `_callStack` - Cleared completely
- `_stepDepth` - Reset to 0
- `_stepMode` - Reset to None

### Variable Handles
- `_variableHandles` - All expandable object references cleared
- `_nextVariableHandle` - Reset to 100

### Global References
- `_globalStaticClasses` - Reference to static classes cleared

### Pause State
- `_isPaused` - Reset to false
- `_pauseEvent` - Set to allow execution to continue

## What Persists

### Breakpoints
- Breakpoints are **NOT** cleared during state cleanup
- This allows users to set breakpoints once and have them persist across:
  - Debugger enable/disable cycles
  - Custom Logic reloads
  - Scene transitions
- Breakpoints are only cleared when:
  - VSCode sends a new `setBreakpoints` request
  - The debugger is explicitly told to clear them

### Server State
- The TCP server continues listening if it was running
- Connection state is preserved
- Port binding remains active

## Performance Optimizations

### Stack Frame Operations
When the debugger is **disabled** (`IsEnabled = false`):
- `PushStackFrame()` returns immediately without allocating
- `PopStackFrame()` returns immediately without stack operations
- Zero overhead for non-debugging scenarios

This is critical because these methods are called for **every method invocation** in Custom Logic scripts.

## Usage Examples

### Manual State Clear
```csharp
// Clear debugger state manually if needed
CustomLogic.Debugger.CustomLogicDebugger.Instance.ClearState();
```

### Check State Before Debugging
```csharp
if (CustomLogicDebugger.Instance.IsEnabled)
{
    // Debugger is active and state is valid
}
```

## Memory Impact

### Before Cleanup
Without proper cleanup, each CL reload would:
- Keep references to old class instances (memory leak)
- Maintain outdated variable handles (stale state)
- Hold onto old call stack frames (incorrect debugging)

### After Cleanup
With proper cleanup:
- Memory is freed when CL is unloaded
- Fresh state for each debugging session
- No stale references to old CL objects
- Predictable memory usage

## Implementation Details

### UISettings.Apply()
```csharp
if (EnableCLDebugger.Value)
{
    // Start if not connected
    if (!CustomLogicDebugger.Instance.IsConnected)
        CustomLogicDebugger.Instance.StartDebugServer();
}
else
{
    // Clear state when disabling
    CustomLogicDebugger.Instance.ClearState();
    
    // Stop server if connected
    if (CustomLogicDebugger.Instance.IsConnected)
        CustomLogicDebugger.Instance.StopDebugServer();
}
```

### CustomLogicManager.OnPreLoadScene()
```csharp
// Clear debugger state when CL is being unloaded
if (SettingsManager.UISettings.EnableCLDebugger.Value)
{
    CustomLogic.Debugger.CustomLogicDebugger.Instance.ClearState();
}
```

## Best Practices

1. **Always call ClearState() before reloading CL**
   - Prevents stale references
   - Ensures clean debugging state

2. **Don't clear breakpoints unnecessarily**
   - Breakpoints should persist across reloads
   - Only clear when explicitly requested by VSCode

3. **Check IsEnabled before debugging operations**
   - Avoids unnecessary work when debugging is off
   - Improves performance in production

4. **Allow graceful degradation**
   - If debugger fails, CL should still run
   - Errors should be logged but not crash the game

## Testing Checklist

- [ ] Enable debugger, set breakpoints, disable debugger, re-enable ? breakpoints persist
- [ ] Enable debugger, reload CL ? no stale variable references
- [ ] Disable debugger ? all state cleared except breakpoints
- [ ] Switch scenes with debugger enabled ? state cleared appropriately
- [ ] Run CL without debugger ? zero performance overhead

## Related Files

- `Assets\Scripts\CustomLogic\Debugger\CustomLogicDebugger.cs` - Main debugger implementation
- `Assets\Scripts\Settings\UISettings.cs` - Settings UI with debugger toggle
- `Assets\Scripts\CustomLogic\CustomLogicManager.cs` - CL lifecycle management

## See Also

- [Setup Guide](SETUP_GUIDE.md) - Initial setup and configuration
- [Implementation Summary](IMPLEMENTATION_SUMMARY.md) - Overall architecture
- [Quick Reference](QUICK_REFERENCE.md) - Common debugging tasks
