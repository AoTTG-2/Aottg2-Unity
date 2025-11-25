import * as assert from 'assert';
import { TextDocument } from 'vscode-languageserver-textdocument';
import { SymbolManager } from '../../symbolManager';
import { APIMetadataLoader } from '../../apiMetadata';
import * as path from 'path';

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
});
