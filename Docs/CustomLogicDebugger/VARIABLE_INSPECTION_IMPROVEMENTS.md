# Variable Inspection Improvements

## Overview
This document describes the improvements made to the Custom Logic debugger to enable proper variable inspection and hover support in VSCode.

## Problems Solved

### 1. No Variable Expansion
**Problem**: Variables showing in the Variables panel couldn't be expanded to view sub-properties.  
**Cause**: All variables returned `variablesReference: 0`, which tells VSCode they have no children.

**Solution**: Implemented a variable handle registry system:
- Each complex object (class instances, lists, dicts) gets a unique handle ID
- VSCode can request children using this ID
- Handles are cleared and regenerated on each pause to prevent memory leaks

### 2. Static/Global Variables Not Visible
**Problem**: Main class static variables weren't accessible during debugging.  
**Cause**: Debugger had no reference to the evaluator's static class registry.

**Solution**: 
- Added `SetGlobals()` method to debugger
- Evaluator calls this during `Init()` after creating static classes
- Debugger exposes globals as a separate scope in the Variables panel

### 3. Hover Tooltips Not Working
**Problem**: Hovering over variables in VSCode showed nothing.  
**Cause**: DAP `evaluate` request not implemented.

**Solution**:
- Implemented `HandleEvaluate()` with expression parsing
- Supports dotted paths: `obj.field.subfield`
- Supports array/list indexing: `list[0]`
- Returns both value and `variablesReference` for expansion

### 4. Internal Variables Cluttering Debug View
**Problem**: Variables like `Type`, `Init`, `IsCharacter` appeared in every class instance.  
**Cause**: No filtering of internal/builtin variables.

**Solution**:
- Added `ShouldFilterVariable()` to hide:
  - `Type`, `Init`, `IsCharacter` (built-in markers)
  - Variables starting with `_` (internal convention)
  - Method bindings (`CLMethodBinding`, `UserMethod`)

## Implementation Details

### Variable Handle Registry
```csharp
private readonly Dictionary<int, object> _variableHandles = new Dictionary<int, object>();
private int _nextVariableHandle = 100;

private int RegisterVariableHandle(object container)
{
    if (container == null) return 0;
    int id = _nextVariableHandle++;
    _variableHandles[id] = container;
    return id;
}
```

### Scopes Structure
1. **Local** - Method parameters and local variables
2. **Class** - Current instance (`self`) variables
3. **Globals** - Static classes (Main, extensions, builtins)

### Supported Container Types
- `Dictionary<string, object>` - Local variables
- `CustomLogicClassInstance` - Class instances (recursive)
- `CustomLogicListBuiltin` - Lists with indexed children
- `CustomLogicDictBuiltin` - Dictionaries with key-based children

### Expression Evaluation
Supports:
- Simple identifiers: `variableName`
- Dotted paths: `obj.field.subfield`
- Array/list indices: `list[0]`
- Dictionary keys: `dict["key"]`

Lookup order:
1. Local variables
2. Current instance (`self`) variables
3. Global static classes

### Value Formatting
Special formatting for common types:
- `Vector3`: `(x, y, z)`
- `Color`: `(R, G, B, A)`
- `List`: `List[count]`
- `Dict`: `Dict[count]`
- Class instances: Calls `__Str__` if available, else shows class name

## Testing Checklist

- [ ] Expand List variables to see elements
- [ ] Expand custom class instances to see properties
- [ ] View Main class static variables in Globals scope
- [ ] Hover over variables shows values
- [ ] Hover over dotted expressions (`obj.field`) works
- [ ] Internal variables (`Type`, `Init`, `_private`) are hidden
- [ ] Method bindings don't appear as variables
- [ ] Nested expansion works (obj ? subobj ? field)

## Performance Considerations

### Handle Cleanup
- Handles are cleared on each pause via `HandleScopes()`
- Prevents unbounded growth during long debug sessions
- Trade-off: Previous expansion state is lost between pauses

### Future Improvements
1. **TTL-based cleanup**: Keep handles for N seconds after last access
2. **Lazy evaluation**: Only evaluate complex values when expanded
3. **Pagination**: Limit children for large collections
4. **Caching**: Cache formatted values to avoid repeated ToString() calls

## Integration Points

### CustomLogicEvaluator.cs
```csharp
private void Init()
{
    // ... create static classes ...
    
    // Expose to debugger
    CustomLogicDebugger.Instance.SetGlobals(_staticClasses);
    
    // ... continue init ...
}
```

### CustomLogicDebugger.cs
Key methods:
- `SetGlobals()` - Called by evaluator to register static classes
- `HandleScopes()` - Returns scope list, clears old handles
- `HandleVariables()` - Returns children for a handle
- `HandleEvaluate()` - Evaluates expressions for hover tooltips
- `GetChildHandleIfComplex()` - Determines if a value is expandable
- `FormatValue()` - Formats values for display
- `ShouldFilterVariable()` - Filters internal variables

## Known Limitations

1. **Property Bindings**: Read-only properties are shown but can't be edited
2. **Methods**: Method references are hidden (can't inspect closure state)
3. **Circular References**: No cycle detection (will cause infinite expansion)
4. **Large Collections**: No pagination (inspecting 10,000-item list will be slow)
5. **Dynamic Types**: `object` types don't provide type-specific formatting

## Future Enhancements

### Conditional Breakpoints
- Add support for `condition` field in `setBreakpoints`
- Evaluate expressions using `HandleEvaluate` logic

### Watch Expressions
- Persist watch expressions across pauses
- Re-evaluate on each stop

### Set Variable
- Implement `setVariable` request
- Allow editing values in Variables panel

### Data Breakpoints
- Break when a specific variable changes
- Requires tracking variable modifications in evaluator
