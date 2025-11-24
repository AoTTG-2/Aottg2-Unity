# Debugger Line Number Fix

## Problem

When debugging BaseLogic (or any code with line number offsets), the debugger was showing incorrect line numbers in the stack trace. For example:
- BaseLogic has AST lines from -500 to -1
- Actual file lines are 1 to 500
- But VSCode was receiving -3400 instead of the correct file line

## Root Cause

In `HandleStackTrace()`, the debugger was sending `_currentStatement.Line` which contains the **AST line number**, not the **actual file line number**.

```csharp
// BEFORE (incorrect):
frames.Add(new
{
    source = new { path = _currentFileName },
    line = _currentStatement.Line,  // ? AST line (-3400)
});
```

## Solution

Added a new field `_currentLineNumber` to track the actual file line number:

1. **Added field:**
   ```csharp
   private int _currentLineNumber = 0; // Actual file line number (not AST line)
   ```

2. **Store actual line in OnBeforeStatement:**
   ```csharp
   internal void OnBeforeStatement(CustomLogicBaseAst statement, string fileName, int actualLineNumber, ...)
   {
       _currentFileName = fileName;
       _currentLineNumber = actualLineNumber; // ? Store converted line number
   }
   ```

3. **Use actual line in stack trace:**
   ```csharp
   // AFTER (correct):
   frames.Add(new
   {
       source = new { path = _currentFileName },
       line = _currentLineNumber,  // ? Actual file line number
   });
   ```

## Line Number Flow

### For BaseLogic (Negative AST Lines)

1. **Evaluator**: AST line -350
2. **GetFileAndLine**: -350 + baseLogicOffset(500) = 150
3. **OnBeforeStatement**: Receives actualLineNumber = 150
4. **Debugger stores**: `_currentLineNumber = 150`
5. **Stack trace sends**: line = 150 ?

### For Map Logic (Positive with Offset)

1. **Evaluator**: AST line 10
2. **GetFileAndLine**: 10 + LogicStart(25) = 35
3. **OnBeforeStatement**: Receives actualLineNumber = 35
4. **Debugger stores**: `_currentLineNumber = 35`
5. **Stack trace sends**: line = 35 ?

### For Mode Logic (Direct)

1. **Evaluator**: AST line 50
2. **GetFileAndLine**: 50 (no offset)
3. **OnBeforeStatement**: Receives actualLineNumber = 50
4. **Debugger stores**: `_currentLineNumber = 50`
5. **Stack trace sends**: line = 50 ?

## Stack Frames

The call stack frames already receive the correct line numbers from `PushStackFrame` because the evaluator passes them through `GetFileAndLine` before pushing:

```csharp
// In CustomLogicEvaluator.EvaluateMethod:
var (filePath, lineNumber) = GetFileAndLine(methodAst.Line);
CustomLogicDebugger.Instance.PushStackFrame(methodName, className, filePath, lineNumber);
```

So the stack frames already had the correct line numbers - only the current frame was broken.

## Testing

Verify the fix works by:

1. **Set breakpoint in BaseLogic.txt at line 150**
   - Should hit at actual line 150 (not -3400)
   - Stack trace should show line 150

2. **Set breakpoint in MapLogic at line 35**
   - Should hit at line 35 in the map file
   - Stack trace should show line 35

3. **Step through code crossing files**
   - Each frame should show correct line number
   - No negative or offset numbers visible to user

## Related Files

- `Assets\Scripts\CustomLogic\Debugger\CustomLogicDebugger.cs` - Fix implemented here
- `Assets\Scripts\CustomLogic\CustomLogicEvaluator.cs` - Converts AST lines to file lines
- `Docs\CustomLogicDebugger\MULTI_FILE_SUPPORT.md` - Overall architecture

## Commit Message

```
Fix debugger stack trace showing incorrect line numbers

The debugger was sending AST line numbers instead of actual file line
numbers in the stack trace. This caused VSCode to show wrong lines,
especially for BaseLogic where AST lines are negative.

Added _currentLineNumber field to track the actual file line number
passed from OnBeforeStatement, and use it in HandleStackTrace instead
of _currentStatement.Line.

Fixes line numbers for BaseLogic, Map Logic with offsets, and all
multi-file debugging scenarios.
```
