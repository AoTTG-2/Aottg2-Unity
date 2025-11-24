# Custom Logic Debugger - Multi-File Support

## Overview

The Custom Logic debugger now properly supports debugging across three different file sources that comprise a complete CL execution environment:

1. **BaseLogic.txt** - Built-in utility classes and components
2. **Mode Logic** - Custom game mode `.cl` files or built-in mode logic  
3. **Map Logic** - Logic embedded in map `.txt` files (when using "Map Logic" mode)

## File Source Architecture

### 1. BaseLogic (Internal Logic)

- **Location**: `Assets\Resources\Data\Modes\BaseLogic.txt`
- **Line Numbers**: Negative (e.g., -150, -200)
- **Contains**: Built-in components, extensions, and utility classes
- **Offset**: `_baseLogicOffset` tracks the number of BaseLogic lines

### 2. Mode Logic

- **Builtin Modes**: `Assets\Resources\Data\Modes\{ModeName}Logic.txt`
- **Custom Modes**: `CustomLogic\{ModeName}.cl`
- **Line Numbers**: Positive, direct line numbers in the file
- **Contains**: Game mode implementation (Main class, extensions, etc.)

### 3. Map Logic

- **Builtin Maps**: `Assets\Resources\Data\BuiltinMaps\{Category}\{MapName}Map.txt`
- **Custom Maps**: `CustomMaps\{MapName}.txt`
- **Line Numbers**: Positive, with `MapScript.LogicStart` offset
- **Contains**: Logic section after map data in the same file

## Line Number Mapping

The evaluator uses `GetFileAndLine(int astLine)` to convert AST line numbers to actual file positions:

```csharp
public (string filePath, int lineNumber) GetFileAndLine(int astLine)
{
    // Negative lines are in BaseLogic
    if (astLine < 0)
    {
        int actualLine = astLine + _baseLogicOffset;
        return (_builtinLogicPath, actualLine);
    }

    // Positive lines are in Mode/Map logic
    if (_isMapLogic)
    {
        // Map logic: add the map offset
        int actualLine = _mapLogicStartLine + astLine;
        return (_mapLogicPath, actualLine);
    }
    else
    {
        // Mode logic: line number is direct
        return (_modeLogicPath, astLine);
    }
}
```

## File Path Resolution

### Map Logic Mode
```csharp
string mapPath = mapCategory == "Custom" 
    ? $"{BuiltinLevels.CustomMapFolderPath}/{mapName}.txt"
    : $"Resources/BuiltinMaps/{mapCategory}/{mapName}Map.txt";

Evaluator.SetFileInfo(mapPath, isMapLogic: true, mapPath, MapManager.MapScript.LogicStart);
```

### Mode Logic
```csharp
bool isBuiltin = BuiltinLevels.IsLogicBuiltin(gameMode);
string modePath = isBuiltin
    ? $"Resources/Modes/{gameMode}Logic.txt"
    : $"{BuiltinLevels.CustomLogicFolderPath}/{gameMode}.cl";

Evaluator.SetFileInfo(modePath, isMapLogic: false);
```

## Breakpoint Matching

Breakpoints are matched using both file path and line number:

1. VSCode sends breakpoint with file path (e.g., `C:\Users\...\CustomLogic\MyMode.cl`)
2. Evaluator converts AST line to (filePath, actualLine)
3. Debugger compares using:
   - Exact path match
   - Partial path match (suffix comparison for relative vs absolute paths)

## Example Scenarios

### Scenario 1: Debugging Custom Mode

**Setup:**
- Mode: `CustomLogic\MyMode.cl`
- Map: Any map

**File Sources:**
1. BaseLogic.txt (lines 1-500 ? AST lines -500 to -1)
2. MyMode.cl (lines 1-100 ? AST lines 1-100)

**Breakpoint Example:**
- User sets breakpoint at `MyMode.cl:50`
- AST line 50 maps to (`CustomLogic\MyMode.cl`, 50)
- Breakpoint hits when executing line 50

### Scenario 2: Debugging Map Logic

**Setup:**
- Mode: "Map Logic"
- Map: `CustomMaps\TestMap.txt` with LogicStart = 25

**File Sources:**
1. BaseLogic.txt (lines 1-500 ? AST lines -500 to -1)
2. TestMap.txt (logic starts at line 26 ? AST lines 1-N map to file lines 26+)

**Breakpoint Example:**
- User sets breakpoint at `TestMap.txt:30`
- AST line 5 maps to (`CustomMaps\TestMap.txt`, 30) because 25 + 5 = 30
- Breakpoint hits when executing AST line 5

### Scenario 3: Debugging in BaseLogic

**Setup:**
- Any mode/map
- Error occurs in BaseLogic component

**File Sources:**
1. BaseLogic.txt (lines 1-500)
2. Mode/Map logic

**Breakpoint Example:**
- User sets breakpoint at `BaseLogic.txt:150`
- AST line -350 (if baseLogicOffset = 500) maps to (`BaseLogic.txt`, 150)
- Breakpoint hits when executing component code

## Implementation Details

### Evaluator Changes

1. **Added file tracking fields:**
   ```csharp
   private string _builtinLogicPath = "BaseLogic.txt";
   private string _modeLogicPath = "unknown.cl";
   private string _mapLogicPath = "unknown_map.txt";
   private int _mapLogicStartLine = 0;
   private bool _isMapLogic = false;
   ```

2. **Added SetFileInfo method:**
   ```csharp
   public void SetFileInfo(string modeLogicPath, bool isMapLogic, 
       string mapLogicPath = null, int mapLogicStartLine = 0)
   ```

3. **Added GetFileAndLine method:**
   ```csharp
   public (string filePath, int lineNumber) GetFileAndLine(int astLine)
   ```

4. **Updated all debugger calls:**
   - `OnBeforeStatement` now receives actual file path and line
   - `PushStackFrame` uses GetFileAndLine for proper stack traces
   - Exception handlers use GetFileAndLine for error reporting

### Debugger Changes

1. **Updated OnBeforeStatement signature:**
   ```csharp
   internal void OnBeforeStatement(CustomLogicBaseAst statement, string fileName, 
       int actualLineNumber, CustomLogicClassInstance instance, 
       Dictionary<string, object> localVariables)
   ```

2. **Breakpoint matching uses actual line numbers:**
   ```csharp
   if (HasBreakpoint(fileName, actualLineNumber))
   {
       shouldPause = true;
       SendStoppedEvent("breakpoint", fileName, actualLineNumber);
   }
   ```

## VSCode Extension Considerations

The VSCode extension should:

1. **Support multiple file types:**
   - `.cl` files (custom modes)
   - `.txt` files (maps with logic, built-in modes)

2. **Handle file path variations:**
   - Absolute paths
   - Relative paths
   - Resource paths (for built-in files)

3. **Provide proper source mapping:**
   - Map logic: Line numbers are offset by LogicStart
   - BaseLogic: Line numbers are offset by baseLogicOffset
   - Mode logic: Line numbers are direct

## Testing Checklist

- [ ] Set breakpoint in custom mode .cl file ? hits correctly
- [ ] Set breakpoint in map logic .txt file ? hits at correct line (accounting for LogicStart)
- [ ] Set breakpoint in BaseLogic.txt component ? hits correctly
- [ ] Step through code that crosses file boundaries ? correct file shown
- [ ] View call stack with frames from multiple files ? all frames show correct file/line
- [ ] Exception in any file ? shows correct file and line in error
- [ ] Variable inspection works across all file types

## Known Limitations

1. **BaseLogic.txt Location**: Currently hardcoded as "BaseLogic.txt" - may need full path for VSCode
2. **Resource Files**: Built-in mode/map files in Resources folder may need special handling
3. **Path Normalization**: Windows vs Unix path separators need consistent handling

## Future Enhancements

1. **Source Maps**: Generate proper source maps for better debugging
2. **Inline Breakpoints**: Support column-level breakpoints
3. **Conditional Breakpoints**: Add support for condition expressions
4. **Watch Expressions**: Real-time evaluation of expressions
5. **Hot Reload**: Modify code while debugging without restart

## Related Files

- `Assets\Scripts\CustomLogic\CustomLogicEvaluator.cs` - File tracking and line mapping
- `Assets\Scripts\CustomLogic\CustomLogicManager.cs` - File info initialization
- `Assets\Scripts\CustomLogic\Debugger\CustomLogicDebugger.cs` - Breakpoint handling
- `Assets\Scripts\Map\BuiltinLevels.cs` - File path resolution
- `Assets\Scripts\Map\MapScript.cs` - LogicStart tracking

## See Also

- [Implementation Summary](IMPLEMENTATION_SUMMARY.md) - Overall debugger architecture
- [Setup Guide](SETUP_GUIDE.md) - Getting started with debugging
- [Memory Management](MEMORY_MANAGEMENT.md) - State cleanup and lifecycle
