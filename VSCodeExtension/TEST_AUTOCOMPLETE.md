# Testing Autocomplete Features

This guide explains how to test the autocomplete and IntelliSense features of the Custom Logic extension.

## Setup

1. **Build the extension**:
   ```powershell
   npm run compile
   ```

2. **Launch Extension Development Host**:
   - Press `F5` in VS Code (or use Run > Start Debugging)
   - This opens a new VS Code window with the extension loaded

## Testing User-Defined Symbols

Create a new `.cl` file in the Extension Development Host and test the following:

### 1. Class Completion

Type any non-dot character to see class suggestions:

```cl
// Type 'My' to see MyClass
MyClass

class MyClass
{
    void DoSomething()
    {
    }
}
```

### 2. Method/Field Completion (User Classes)

Type `.` after an object to see its members:

```cl
class Player
{
    string Name;
    int Health;
    
    void TakeDamage(int amount)
    {
        Health -= amount;
    }
}

void Test()
{
    Player p;
    p.  // Should show: Name, Health, TakeDamage
}
```

### 3. Local Variable Completion

Variables defined in the current function should appear:

```cl
void MyFunction()
{
    int myLocalVar = 5;
    string anotherVar = "test";
    
    // Type 'm' or 'a' to see myLocalVar and anotherVar
}
```

### 4. Self Member Access

Inside a class method, `self.` should show class members:

```cl
class Enemy
{
    int health;
    
    void Die()
    {
        self.  // Should show: health, Die
    }
}
```

## Testing Built-In API Completion

If you have JSON metadata files in the `json/` folder (e.g., `Game.json`, `UI.json`):

### 1. Class Completion

Type class names to see built-in classes:

```cl
Game  // Should show Game class from metadata
UI    // Should show UI class from metadata
```

### 2. Method/Field Completion (Built-In Classes)

Type `.` after built-in objects:

```cl
void OnInit()
{
    Game.  // Should show Game's methods and fields from metadata
}
```

## Testing Hover Documentation

Hover your mouse over any symbol to see documentation:

- **User-defined classes**: Shows class name and any comment above it
- **User-defined functions**: Shows function signature and comments
- **Built-in classes**: Shows class information from metadata
- **Built-in methods**: Shows method signature and documentation

## Expected Behavior

### When Autocomplete Triggers

- **After typing `.`**: Shows members of the object before the dot
- **After typing any letter**: Shows keywords, classes, functions, and variables
- **Inside a function**: Shows local variables in scope

### Completion Item Kinds

You should see different icons for:
- 📦 Classes (user and built-in)
- 🔧 Functions/Methods
- 📄 Fields/Properties
- 🔤 Variables
- 🔑 Keywords

### Context-Aware Features

- **Local scope**: Only variables defined before the current line appear
- **Class scope**: `self.` shows only members of the current class
- **Object scope**: `object.` shows only members of that object's class

## Troubleshooting

### No autocomplete appears

1. Verify the extension is running (check bottom-left status bar)
2. Check Output panel > "Custom Logic Language Server" for errors
3. Ensure the file has `.cl` extension

### User symbols don't appear

1. Make sure the class/function is defined in an open `.cl` file
2. Try typing a few characters to trigger completion
3. Check that syntax is valid (e.g., proper class/function definitions)

### Built-in API doesn't appear

1. Verify JSON metadata files exist in `json/` folder
2. Check the Output panel for metadata loading errors
3. Ensure JSON files follow the expected schema (see `src/apiMetadata.ts`)

## Sample Test File

Create `test.cl` with this content to test all features:

```cl
// Test class definition
class TestClass
{
    // This is a test field
    int myField;
    
    // Constructor
    void TestClass()
    {
        self.myField = 10;
    }
    
    // Test method
    void MyMethod(int param)
    {
        int localVar = param * 2;
        self.myField = localVar;
    }
}

// Test global function
void GlobalFunction()
{
    TestClass obj;
    obj.  // Should autocomplete: myField, MyMethod
}

// Test with built-in APIs (if metadata exists)
void OnInit()
{
    Game.  // Should show Game API
}
```

## What to Test

- [ ] Type `Test` - should suggest TestClass
- [ ] Inside `MyMethod`, type `loc` - should suggest localVar
- [ ] Type `self.` inside `MyMethod` - should suggest myField, MyMethod
- [ ] Type `obj.` inside `GlobalFunction` - should suggest myField, MyMethod
- [ ] Hover over `TestClass` - should show documentation
- [ ] Type `Game.` - should show built-in API (if metadata loaded)
