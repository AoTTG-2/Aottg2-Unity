import * as assert from 'assert';
import { TextDocument } from 'vscode-languageserver-textdocument';
import { CompletionItemKind } from 'vscode-languageserver';
import { SymbolManager } from '../../symbolManager';
import { APIMetadataLoader } from '../../apiMetadata';
import * as path from 'path';

/**
 * Helper function to simulate completion request logic from server.ts
 */
function getCompletions(
    document: TextDocument,
    line: number,
    character: number,
    symbolManager: SymbolManager,
    apiLoader: APIMetadataLoader
): Array<{ label: string; kind: number; detail?: string }> {
    const text = document.getText();
    const offset = document.offsetAt({ line, character });
    const linePrefix = text.substring(0, offset);
    const lastLine = linePrefix.split('\n').pop() || '';
    
    const completions: Array<{ label: string; kind: number; detail?: string }> = [];
    
    // Check if we're after a dot (member access)
    const dotMatch = lastLine.match(/(\w+(?:\.\w+)*)\.$/);
    if (dotMatch) {
        let objectName = dotMatch[1];
        let resolvedType: string | undefined = undefined;
        
        // Handle chained member access like self._tester.
        if (objectName.includes('.')) {
            const parts = objectName.split('.');
            
            // Start with the first part
            let currentType: string | undefined;
            
            if (parts[0] === 'self') {
                // Get current class
                currentType = symbolManager.getCurrentClass(document, line + 1) || undefined;
            } else {
                // Check if first part is a local variable
                const localVarType = symbolManager.findLocalVariableType(document, line + 1, parts[0]);
                if (localVarType) {
                    currentType = localVarType;
                } else {
                    // Assume it's a class name or field
                    currentType = parts[0];
                }
            }
            
            // Resolve each part in the chain
            for (let i = 1; i < parts.length && currentType; i++) {
                const fieldName = parts[i];
                currentType = symbolManager.findFieldType(currentType, fieldName);
            }
            
            resolvedType = currentType;
        } else if (objectName === 'self') {
            // Handle simple self.
            resolvedType = symbolManager.getCurrentClass(document, line + 1) || undefined;
        } else {
            // Check if it's a local variable
            const localVarType = symbolManager.findLocalVariableType(document, line + 1, objectName);
            if (localVarType) {
                resolvedType = localVarType;
            }
        }
        
        if (!resolvedType) {
            return completions;
        }
        
        // Check if it's a built-in class
        const methods = apiLoader.getMethods(resolvedType);
        const fields = apiLoader.getFields(resolvedType);
        
        for (const method of methods) {
            const params = method.parameters.map(p => 
                `${p.name}${p.isOptional ? '?' : ''}: ${p.type.name}`
            ).join(', ');
            
            completions.push({
                label: method.label,
                kind: CompletionItemKind.Method,
                detail: `(${params}) → ${method.returnType.name}`
            });
        }
        
        for (const field of fields) {
            completions.push({
                label: field.label,
                kind: field.readonly ? CompletionItemKind.Constant : CompletionItemKind.Field,
                detail: field.type.name
            });
        }
        
        // Check if it's a user-defined class
        const userMembers = symbolManager.getClassMembers(resolvedType);
        for (const member of userMembers) {
            if (member.kind === 'function') {
                const params = member.parameters?.join(', ') || '';
                completions.push({
                    label: member.name,
                    kind: CompletionItemKind.Method,
                    detail: `(${params})`
                });
            } else if (member.kind === 'field') {
                completions.push({
                    label: member.name,
                    kind: CompletionItemKind.Field
                });
            }
        }
    }
    
    return completions;
}

suite('Language Server Integration Tests', () => {
    let symbolManager: SymbolManager;
    let apiLoader: APIMetadataLoader;

    setup(() => {
        symbolManager = new SymbolManager();
        
        // Load API metadata from json directory
        const jsonDir = path.join(__dirname, '../../../json');
        apiLoader = new APIMetadataLoader(jsonDir);
        apiLoader.loadAll();
    });

    test('Should provide List API methods and properties for local variable', () => {
        const code = `class Main {
    function OnGameStart()
    {
        a = List();
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        symbolManager.updateDocument(doc);
        
        // Get local variables at line 4 (where 'a' is defined)
        const localVars = symbolManager.getLocalVariables(doc, 5);
        const aVar = localVars.find(v => v.name === 'a');
        
        assert.ok(aVar, 'Should find variable a');
        assert.strictEqual(aVar?.type, 'List', 'Variable a should have type List');
        
        // Verify List API is loaded
        const listMethods = apiLoader.getMethods('List');
        const listFields = apiLoader.getFields('List');
        
        // Check that all instance methods are present
        const expectedMethods = [
            'Clear', 'Get', 'Set', 'Add', 'InsertAt', 'RemoveAt', 
            'Remove', 'Contains', 'Sort', 'SortCustom', 'Filter', 
            'Map', 'Reduce', 'Randomize', 'ToSet'
        ];
        
        for (const methodName of expectedMethods) {
            const method = listMethods.find(m => m.label === methodName);
            assert.ok(method, `Should find method ${methodName}`);
        }
        
        // Check that the Count field is present
        const countField = listFields.find(f => f.label === 'Count');
        assert.ok(countField, 'Should find Count field');
        assert.strictEqual(countField?.type.name, 'int', 'Count should be of type int');
        assert.strictEqual(countField?.readonly, true, 'Count should be readonly');
    });

    test('Should verify List method signatures', () => {
        const methods = apiLoader.getMethods('List');
        
        // Test Clear() -> null
        const clear = methods.find(m => m.label === 'Clear');
        assert.ok(clear, 'Should find Clear method');
        assert.strictEqual(clear?.parameters.length, 0, 'Clear should have 0 parameters');
        assert.strictEqual(clear?.returnType.name, 'null', 'Clear should return null');
        
        // Test Get(int index) -> Object
        const get = methods.find(m => m.label === 'Get');
        assert.ok(get, 'Should find Get method');
        assert.strictEqual(get?.parameters.length, 1, 'Get should have 1 parameter');
        assert.strictEqual(get?.parameters[0].name, 'index', 'First parameter should be index');
        assert.strictEqual(get?.parameters[0].type.name, 'int', 'index should be int');
        assert.strictEqual(get?.returnType.name, 'Object', 'Get should return Object');
        
        // Test Set(int index, Object value) -> null
        const set = methods.find(m => m.label === 'Set');
        assert.ok(set, 'Should find Set method');
        assert.strictEqual(set?.parameters.length, 2, 'Set should have 2 parameters');
        assert.strictEqual(set?.parameters[0].name, 'index', 'First parameter should be index');
        assert.strictEqual(set?.parameters[0].type.name, 'int', 'index should be int');
        assert.strictEqual(set?.parameters[1].name, 'value', 'Second parameter should be value');
        assert.strictEqual(set?.parameters[1].type.name, 'Object', 'value should be Object');
        assert.strictEqual(set?.returnType.name, 'null', 'Set should return null');
        
        // Test Add(Object value) -> null
        const add = methods.find(m => m.label === 'Add');
        assert.ok(add, 'Should find Add method');
        assert.strictEqual(add?.parameters.length, 1, 'Add should have 1 parameter');
        assert.strictEqual(add?.parameters[0].name, 'value', 'First parameter should be value');
        assert.strictEqual(add?.parameters[0].type.name, 'Object', 'value should be Object');
        assert.strictEqual(add?.returnType.name, 'null', 'Add should return null');
        
        // Test Contains(Object value) -> bool
        const contains = methods.find(m => m.label === 'Contains');
        assert.ok(contains, 'Should find Contains method');
        assert.strictEqual(contains?.parameters.length, 1, 'Contains should have 1 parameter');
        assert.strictEqual(contains?.returnType.name, 'bool', 'Contains should return bool');
        
        // Test Filter(function method) -> List
        const filter = methods.find(m => m.label === 'Filter');
        assert.ok(filter, 'Should find Filter method');
        assert.strictEqual(filter?.parameters.length, 1, 'Filter should have 1 parameter');
        assert.strictEqual(filter?.parameters[0].type.name, 'function', 'Filter parameter should be function');
        assert.strictEqual(filter?.returnType.name, 'List', 'Filter should return List');
        
        // Test Map(function method) -> List
        const map = methods.find(m => m.label === 'Map');
        assert.ok(map, 'Should find Map method');
        assert.strictEqual(map?.parameters.length, 1, 'Map should have 1 parameter');
        assert.strictEqual(map?.parameters[0].type.name, 'function', 'Map parameter should be function');
        assert.strictEqual(map?.returnType.name, 'List', 'Map should return List');
        
        // Test Reduce(function method, Object initialValue) -> Object
        const reduce = methods.find(m => m.label === 'Reduce');
        assert.ok(reduce, 'Should find Reduce method');
        assert.strictEqual(reduce?.parameters.length, 2, 'Reduce should have 2 parameters');
        assert.strictEqual(reduce?.parameters[0].type.name, 'function', 'First parameter should be function');
        assert.strictEqual(reduce?.parameters[1].type.name, 'Object', 'Second parameter should be Object');
        assert.strictEqual(reduce?.returnType.name, 'Object', 'Reduce should return Object');
        
        // Test ToSet() -> Set
        const toSet = methods.find(m => m.label === 'ToSet');
        assert.ok(toSet, 'Should find ToSet method');
        assert.strictEqual(toSet?.parameters.length, 0, 'ToSet should have 0 parameters');
        assert.strictEqual(toSet?.returnType.name, 'Set', 'ToSet should return Set');
    });

    test('Should resolve type through field access chain', () => {
        const code = `class Main {
    _tester = null;

    function OnGameStart()
    {
        self._tester = Tester(self.DisplayInConsole);
    }
}

class Tester {
    _passingTests = List();
    _failingTests = List();
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        symbolManager.updateDocument(doc);
        
        // Verify _tester field has type Tester
        const testerField = symbolManager.findFieldType('Main', '_tester');
        assert.strictEqual(testerField, 'Tester', '_tester should have type Tester');
        
        // Verify Tester class fields have type List
        const passingTestsField = symbolManager.findFieldType('Tester', '_passingTests');
        const failingTestsField = symbolManager.findFieldType('Tester', '_failingTests');
        
        assert.strictEqual(passingTestsField, 'List', '_passingTests should have type List');
        assert.strictEqual(failingTestsField, 'List', '_failingTests should have type List');
        
        // Verify we can resolve the chain: self._tester._passingTests would need type List
        // This simulates: self -> Main, Main._tester -> Tester, Tester._passingTests -> List
    });

    test('Should provide List autocomplete for local variable with dot access', () => {
        const code = `class Main {
    function OnGameStart()
    {
        a = List();
        a.
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        symbolManager.updateDocument(doc);
        
        // Simulate completion request at position after 'a.'
        // Line 4 is where 'a.' appears (0-indexed)
        // getLocalVariables expects 1-based, so pass 5
        const localVars = symbolManager.getLocalVariables(doc, 5);
        const aVar = localVars.find(v => v.name === 'a');
        assert.ok(aVar, 'Should find local variable a');
        assert.strictEqual(aVar?.type, 'List', 'Variable a should have type List');
        
        // Get completions at line 4 (0-indexed), character 10
        const completions = getCompletions(doc, 4, 10, symbolManager, apiLoader);
        
        // Verify we get List methods
        assert.ok(completions.length > 0, 'Should have completions for List');
        
        const expectedMethods = [
            'Clear', 'Get', 'Set', 'Add', 'InsertAt', 'RemoveAt', 
            'Remove', 'Contains', 'Sort', 'SortCustom', 'Filter', 
            'Map', 'Reduce', 'Randomize', 'ToSet'
        ];
        
        for (const methodName of expectedMethods) {
            const completion = completions.find(c => c.label === methodName);
            assert.ok(completion, `Should have completion for ${methodName}`);
            assert.strictEqual(completion?.kind, CompletionItemKind.Method, `${methodName} should be a method`);
        }
        
        // Verify Count field is present
        const countCompletion = completions.find(c => c.label === 'Count');
        assert.ok(countCompletion, 'Should have completion for Count');
        assert.strictEqual(countCompletion?.kind, CompletionItemKind.Constant, 'Count should be a constant');
        assert.strictEqual(countCompletion?.detail, 'int', 'Count should be of type int');
    });

    test('Should provide List autocomplete for field with dot access', () => {
        const code = `class Main {
    _myList = List();

    function OnGameStart()
    {
        self._myList.
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        symbolManager.updateDocument(doc);
        
        // Simulate completion request at position after 'self._myList.'
        // Line 5 (0-based index 5), character 21 (after 'self._myList.')
        const completions = getCompletions(doc, 5, 21, symbolManager, apiLoader);
        
        assert.ok(completions.length > 0, 'Should have completions for List');
        
        // Verify some key methods
        const add = completions.find(c => c.label === 'Add');
        assert.ok(add, 'Should have Add method');
        
        const filter = completions.find(c => c.label === 'Filter');
        assert.ok(filter, 'Should have Filter method');
        assert.ok(filter?.detail?.includes('List'), 'Filter should return List');
        
        const count = completions.find(c => c.label === 'Count');
        assert.ok(count, 'Should have Count field');
    });

    test('Should provide List autocomplete through nested field access', () => {
        const code = `class Main {
    _tester = null;

    function OnGameStart()
    {
        self._tester = Tester();
        self._tester._passingTests.
    }
}

class Tester {
    _passingTests = List();
    _failingTests = List();
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        symbolManager.updateDocument(doc);
        
        // Simulate completion request at position after 'self._tester._passingTests.'
        // Line 6 (0-based index 6), character 35 (after the dot)
        const completions = getCompletions(doc, 6, 35, symbolManager, apiLoader);
        
        assert.ok(completions.length > 0, 'Should have completions for List through nested access');
        
        // Verify List methods are available
        const toSet = completions.find(c => c.label === 'ToSet');
        assert.ok(toSet, 'Should have ToSet method');
        assert.ok(toSet?.detail?.includes('Set'), 'ToSet should return Set');
        
        const map = completions.find(c => c.label === 'Map');
        assert.ok(map, 'Should have Map method');
        
        const count = completions.find(c => c.label === 'Count');
        assert.ok(count, 'Should have Count field');
    });

    test('Should handle multiple local variables with different types', () => {
        const code = `class Main {
    function OnGameStart()
    {
        myList = List();
        myDict = Dict();
        myList.
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        symbolManager.updateDocument(doc);
        
        // Debug: Check local variables (1-based line number for extractLocalVariables)
        const localVars = symbolManager.getLocalVariables(doc, 7);
        const myListVar = localVars.find(v => v.name === 'myList');
        assert.ok(myListVar, 'Should find myList variable');
        assert.strictEqual(myListVar?.type, 'List', 'myList should have type List');
        
        // Get completions for myList at 0-indexed line 5 (where myList. appears)
        const listCompletions = getCompletions(doc, 5, 15, symbolManager, apiLoader);
        
        // Should have List methods
        const listAdd = listCompletions.find(c => c.label === 'Add');
        assert.ok(listAdd, 'Should have Add method for List');
        
        // Should NOT have Dict methods
        const dictGet = listCompletions.find(c => c.label === 'GetKey');
        assert.strictEqual(dictGet, undefined, 'Should not have Dict methods for List variable');
    });

    test('Should update local variable type on reassignment', () => {
        const code = `class Main {
    function OnGameStart()
    {
        a = List();
        a = Vector3(1,2,3);
        a.
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        symbolManager.updateDocument(doc);
        
        // Get local variables at line 6 (after reassignment)
        const localVars = symbolManager.getLocalVariables(doc, 6);
        const aVar = localVars.find(v => v.name === 'a');
        
        assert.ok(aVar, 'Should find variable a');
        assert.strictEqual(aVar?.type, 'Vector3', 'Variable a should have type Vector3 after reassignment');
        
        // Get completions at line 5 (0-indexed), character 10
        const completions = getCompletions(doc, 5, 10, symbolManager, apiLoader);
        
        // Should have Vector3 methods, not List methods
        assert.ok(completions.length > 0, 'Should have completions for Vector3');
        
        // Vector3 should have properties like X, Y, Z
        const xProp = completions.find(c => c.label === 'X');
        assert.ok(xProp, 'Should have X property for Vector3');
        
        // Should NOT have List methods
        const addMethod = completions.find(c => c.label === 'Add');
        assert.strictEqual(addMethod, undefined, 'Should not have List Add method after reassignment to Vector3');
        
        const toSet = completions.find(c => c.label === 'ToSet');
        assert.strictEqual(toSet, undefined, 'Should not have List ToSet method after reassignment to Vector3');
    });
});
