# Installation Guide

## Option 1: Install from Source (Recommended for Node 18 users)

Since vsce packaging tool requires Node 20+ due to dependencies, you can install the extension directly in development mode:

### Steps:

1. **Compile the extension:**
   ```powershell
   cd VSCodeExtension
   npm install
   npm run compile
   ```

2. **Install in VS Code:**
   - Open VS Code
   - Press `Ctrl+Shift+P`
   - Type "Developer: Install Extension from Location"
   - Browse to the `VSCodeExtension` folder
   - Click "Select Folder"

3. **Reload VS Code:**
   - Press `Ctrl+Shift+P`
   - Type "Developer: Reload Window"

The extension is now installed and ready to use!

## Option 2: Package with Node 20+ (If Available)

If you have Node.js 20 or higher:

```powershell
cd VSCodeExtension
npm install
npm run compile
npm run package
```

This will create `cl-debugger-1.0.0.vsix` which can be installed via:
- VS Code UI: Extensions → ... → Install from VSIX
- Command line: `code --install-extension cl-debugger-1.0.0.vsix`

## Option 3: Symlink for Development

Create a symbolic link in your VS Code extensions folder:

### Windows:
```powershell
# Find your VSCode extensions folder (usually):
cd "$env:USERPROFILE\.vscode\extensions"

# Create symlink
New-Item -ItemType SymbolicLink -Path "aottg2.cl-debugger-1.0.0" -Target "C:\Users\Michael\Documents\Github\Aottg2-Unity\VSCodeExtension"
```

### macOS/Linux:
```bash
cd ~/.vscode/extensions
ln -s /path/to/Aottg2-Unity/VSCodeExtension aottg2.cl-debugger-1.0.0
```

Then reload VS Code.

## Verification

After installation, verify the extension is loaded:

1. Open VS Code
2. Press `Ctrl+Shift+X` to open Extensions view
3. Search for "Custom Logic Debugger"
4. You should see it listed

Or check the command palette (`Ctrl+Shift+P`) for "CL Debug" commands.

## Troubleshooting

### Extension not appearing
- Make sure you compiled first: `npm run compile`
- Check the `out/` folder exists with `.js` files
- Reload VS Code window

### Compilation errors
- Delete `node_modules` and `package-lock.json`
- Run `npm install` again
- Ensure TypeScript is installed: `npm list typescript`

### Can't connect to debugger
- Ensure the game is running
- Check that debugging is enabled in game settings
- Verify port 4711 is not blocked by firewall
