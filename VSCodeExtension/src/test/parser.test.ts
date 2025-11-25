import * as assert from 'assert';
import { CLParser } from '../parser';
import { TextDocument } from 'vscode-languageserver-textdocument';

suite('CLParser Test Suite', () => {
    let parser: CLParser;

    setup(() => {
        parser = new CLParser();
    });

    test('Should parse simple class with field', () => {
        const code = `class Main {
    _tester = null;
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        assert.strictEqual(symbols.length, 2, 'Should have 2 symbols (class + field)');
        
        const classSymbol = symbols.find(s => s.name === 'Main');
        assert.ok(classSymbol, 'Should find Main class');
        assert.strictEqual(classSymbol?.kind, 'class');
        assert.strictEqual(classSymbol?.parent, undefined);

        const fieldSymbol = symbols.find(s => s.name === '_tester');
        assert.ok(fieldSymbol, 'Should find _tester field');
        assert.strictEqual(fieldSymbol?.kind, 'field');
        assert.strictEqual(fieldSymbol?.parent, 'Main', 'Field parent should be Main');
    });

    test('Should parse class with method', () => {
        const code = `class Main {
    function OnGameStart()
    {
        a = 5;
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const classSymbol = symbols.find(s => s.name === 'Main');
        assert.ok(classSymbol, 'Should find Main class');

        const methodSymbol = symbols.find(s => s.name === 'OnGameStart');
        assert.ok(methodSymbol, 'Should find OnGameStart method');
        assert.strictEqual(methodSymbol?.kind, 'function');
        assert.strictEqual(methodSymbol?.parent, 'Main', 'Method parent should be Main');
    });

    test('Should NOT mark local variables as class fields', () => {
        const code = `class Main {
    _field = null;
    
    function Test()
    {
        localVar = 123;
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const fieldSymbol = symbols.find(s => s.name === '_field');
        assert.ok(fieldSymbol, 'Should find _field');
        assert.strictEqual(fieldSymbol?.kind, 'field');
        assert.strictEqual(fieldSymbol?.parent, 'Main');

        const localVarSymbol = symbols.find(s => s.name === 'localVar');
        assert.strictEqual(localVarSymbol, undefined, 'Should NOT find localVar as a field');
    });

    test('Should parse multiple classes', () => {
        const code = `class Main {
    field1 = 1;
}

class Helper {
    field2 = 2;
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const mainClass = symbols.find(s => s.name === 'Main' && s.kind === 'class');
        const helperClass = symbols.find(s => s.name === 'Helper' && s.kind === 'class');
        
        assert.ok(mainClass, 'Should find Main class');
        assert.ok(helperClass, 'Should find Helper class');

        const field1 = symbols.find(s => s.name === 'field1');
        const field2 = symbols.find(s => s.name === 'field2');

        assert.strictEqual(field1?.parent, 'Main', 'field1 should belong to Main');
        assert.strictEqual(field2?.parent, 'Helper', 'field2 should belong to Helper');
    });

    test('Should parse nested braces correctly', () => {
        const code = `class Main {
    function Test()
    {
        if (true) {
            nestedVar = 1;
        }
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const nestedVar = symbols.find(s => s.name === 'nestedVar');
        assert.strictEqual(nestedVar, undefined, 'Nested variable should not be captured as field');
    });

    test('Should parse class with opening brace on same line', () => {
        const code = `class Main {
    myField = 123;
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const classSymbol = symbols.find(s => s.name === 'Main');
        const fieldSymbol = symbols.find(s => s.name === 'myField');

        assert.ok(classSymbol, 'Should find Main class');
        assert.ok(fieldSymbol, 'Should find myField');
        assert.strictEqual(fieldSymbol?.parent, 'Main', 'Field should belong to Main');
    });

    test('Should parse class with opening brace on next line', () => {
        const code = `class Main
{
    myField = 123;
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const classSymbol = symbols.find(s => s.name === 'Main');
        const fieldSymbol = symbols.find(s => s.name === 'myField');

        assert.ok(classSymbol, 'Should find Main class');
        assert.ok(fieldSymbol, 'Should find myField');
        assert.strictEqual(fieldSymbol?.parent, 'Main', 'Field should belong to Main');
    });

    test('Should handle function with parameters', () => {
        const code = `class Main {
    function Test(a, b, c)
    {
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const funcSymbol = symbols.find(s => s.name === 'Test');
        assert.ok(funcSymbol, 'Should find Test function');
        assert.strictEqual(funcSymbol?.parameters?.length, 3, 'Should have 3 parameters');
        assert.deepStrictEqual(funcSymbol?.parameters, ['a', 'b', 'c']);
    });

    test('Should handle empty function parameters', () => {
        const code = `class Main {
    function Test()
    {
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const funcSymbol = symbols.find(s => s.name === 'Test');
        assert.ok(funcSymbol, 'Should find Test function');
        assert.strictEqual(funcSymbol?.parameters?.length, 0, 'Should have 0 parameters');
    });

    test('Should distinguish between class fields and function local vars', () => {
        const code = `class Tester {
    _passingTests = List();
    _failingTests = List();
    
    function ShowResults()
    {
        passingTests = self._passingTests.Count;
        failingTests = self._failingTests.Count;
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        // Should find class fields
        const passingTestsField = symbols.find(s => s.name === '_passingTests');
        const failingTestsField = symbols.find(s => s.name === '_failingTests');
        
        assert.ok(passingTestsField, 'Should find _passingTests field');
        assert.ok(failingTestsField, 'Should find _failingTests field');
        assert.strictEqual(passingTestsField?.kind, 'field');
        assert.strictEqual(failingTestsField?.kind, 'field');
        assert.strictEqual(passingTestsField?.parent, 'Tester');
        assert.strictEqual(failingTestsField?.parent, 'Tester');

        // Should NOT find local variables as fields
        const passingTestsLocal = symbols.find(s => s.name === 'passingTests' && s.kind === 'field');
        const failingTestsLocal = symbols.find(s => s.name === 'failingTests' && s.kind === 'field');
        
        assert.strictEqual(passingTestsLocal, undefined, 'Should not find passingTests as field');
        assert.strictEqual(failingTestsLocal, undefined, 'Should not find failingTests as field');
    });

    test('Should track brace depth correctly across multiple functions', () => {
        const code = `class Main {
    field1 = 1;
    
    function Func1()
    {
        local1 = 1;
    }
    
    field2 = 2;
    
    function Func2()
    {
        local2 = 2;
    }
    
    field3 = 3;
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const field1 = symbols.find(s => s.name === 'field1');
        const field2 = symbols.find(s => s.name === 'field2');
        const field3 = symbols.find(s => s.name === 'field3');

        assert.ok(field1, 'Should find field1');
        assert.ok(field2, 'Should find field2');
        assert.ok(field3, 'Should find field3');

        assert.strictEqual(field1?.parent, 'Main');
        assert.strictEqual(field2?.parent, 'Main');
        assert.strictEqual(field3?.parent, 'Main');

        assert.strictEqual(field1?.kind, 'field');
        assert.strictEqual(field2?.kind, 'field');
        assert.strictEqual(field3?.kind, 'field');

        // Local vars should not be found
        const local1 = symbols.find(s => s.name === 'local1');
        const local2 = symbols.find(s => s.name === 'local2');
        
        assert.strictEqual(local1, undefined, 'local1 should not be found');
        assert.strictEqual(local2, undefined, 'local2 should not be found');
    });

    test('Should handle global functions (no parent class)', () => {
        const code = `function GlobalFunc()
{
    a = 1;
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const funcSymbol = symbols.find(s => s.name === 'GlobalFunc');
        assert.ok(funcSymbol, 'Should find GlobalFunc');
        assert.strictEqual(funcSymbol?.kind, 'function');
        assert.strictEqual(funcSymbol?.parent, undefined, 'Global function should have no parent');
    });

    test('Should parse example.cl Main class correctly', () => {
        const code = `class Main {

    _tester = null;

    function OnGameStart()
    {
        baseList = List(1,2,1,4,115);
        uniqueList = List(1,2,4,115);
    }

    function Sum2(a, b) {
        return a + b;
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        console.log('Parsed symbols:', symbols.map(s => `${s.name} (${s.kind}) parent=${s.parent}`));

        const classSymbol = symbols.find(s => s.name === 'Main' && s.kind === 'class');
        assert.ok(classSymbol, 'Should find Main class');

        const fieldSymbol = symbols.find(s => s.name === '_tester');
        assert.ok(fieldSymbol, 'Should find _tester field');
        assert.strictEqual(fieldSymbol?.kind, 'field', '_tester should be a field');
        assert.strictEqual(fieldSymbol?.parent, 'Main', '_tester parent should be Main');

        const onGameStart = symbols.find(s => s.name === 'OnGameStart');
        assert.ok(onGameStart, 'Should find OnGameStart');
        assert.strictEqual(onGameStart?.kind, 'function');
        assert.strictEqual(onGameStart?.parent, 'Main', 'OnGameStart parent should be Main');

        const sum2 = symbols.find(s => s.name === 'Sum2');
        assert.ok(sum2, 'Should find Sum2');
        assert.strictEqual(sum2?.parent, 'Main', 'Sum2 parent should be Main');

        // Local variables should NOT be fields
        const baseList = symbols.find(s => s.name === 'baseList');
        assert.strictEqual(baseList, undefined, 'baseList should not be a symbol');
    });

    test('Should infer type from assignment in function', () => {
        const code = `class Main {
    _tester = null;

    function OnGameStart()
    {
        self._tester = Tester(self.DisplayInConsole);
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);

        const fieldSymbol = symbols.find(s => s.name === '_tester');
        assert.ok(fieldSymbol, 'Should find _tester field');
        assert.strictEqual(fieldSymbol?.kind, 'field', '_tester should be a field');
        assert.strictEqual(fieldSymbol?.parent, 'Main', '_tester parent should be Main');
        assert.strictEqual(fieldSymbol?.type, 'Tester', '_tester type should be inferred as Tester from assignment in function');
    });

    test('Should track local variable types for built-in classes', () => {
        const code = `class Main {
    function OnGameStart()
    {
        a = List();
        b = Dict();
        c = Vector3(1, 2, 3);
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const text = doc.getText();
        
        // Extract local variables at line 7 (0-based index 6, after all declarations)
        const localVars = parser.extractLocalVariables(text, 7);
        
        assert.strictEqual(localVars.length, 3, 'Should find 3 local variables');
        
        const aVar = localVars.find(v => v.name === 'a');
        const bVar = localVars.find(v => v.name === 'b');
        const cVar = localVars.find(v => v.name === 'c');
        
        assert.ok(aVar, 'Should find variable a');
        assert.ok(bVar, 'Should find variable b');
        assert.ok(cVar, 'Should find variable c');
        
        assert.strictEqual(aVar?.type, 'List', 'Variable a should have type List');
        assert.strictEqual(bVar?.type, 'Dict', 'Variable b should have type Dict');
        assert.strictEqual(cVar?.type, 'Vector3', 'Variable c should have type Vector3');
    });

    test('Should track local variable types for user-defined classes', () => {
        const code = `class Main {
    function OnGameStart()
    {
        myTester = Tester();
        myHelper = Helper(1, 2);
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const text = doc.getText();
        
        // Extract local variables at line 6 (0-based index 5, after all declarations)
        const localVars = parser.extractLocalVariables(text, 6);
        
        assert.strictEqual(localVars.length, 2, 'Should find 2 local variables');
        
        const testerVar = localVars.find(v => v.name === 'myTester');
        const helperVar = localVars.find(v => v.name === 'myHelper');
        
        assert.ok(testerVar, 'Should find variable myTester');
        assert.ok(helperVar, 'Should find variable myHelper');
        
        assert.strictEqual(testerVar?.type, 'Tester', 'Variable myTester should have type Tester');
        assert.strictEqual(helperVar?.type, 'Helper', 'Variable myHelper should have type Helper');
    });

    test('Should update local variable type on reassignment', () => {
        const code = `class Main {
    function OnGameStart()
    {
        a = null;
        a = List();
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const text = doc.getText();
        
        // Extract local variables at line 6 (0-based index 5, after reassignment)
        const localVars = parser.extractLocalVariables(text, 6);
        
        assert.strictEqual(localVars.length, 1, 'Should find 1 local variable');
        
        const aVar = localVars.find(v => v.name === 'a');
        assert.ok(aVar, 'Should find variable a');
        assert.strictEqual(aVar?.type, 'List', 'Variable a should have type List (updated from null)');
    });

    test('Should not infer type from null assignments', () => {
        const code = `class Main {
    _field = null;
    
    function Test()
    {
        localVar = null;
    }
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);
        const text = doc.getText();
        
        const field = symbols.find(s => s.name === '_field');
        assert.strictEqual(field?.type, undefined, 'Field initialized with null should have undefined type');
        
        const localVars = parser.extractLocalVariables(text, 6);
        const localVar = localVars.find(v => v.name === 'localVar');
        assert.strictEqual(localVar?.type, undefined, 'Local variable assigned null should have undefined type');
    });

    test('Complex example: should track types in realistic scenario', () => {
        const code = `class Main {
    _tester = null;

    function OnGameStart()
    {
        self._tester = Tester(self.DisplayInConsole);
        baseList = List(1,2,1,4,115);
        uniqueList = baseList.ToSet().ToList();
        a = List();
    }
}

class Tester {
    _passingTests = List();
    _failingTests = List();
}`;
        const doc = TextDocument.create('test://test.cl', 'cl', 1, code);
        const symbols = parser.parseDocument(doc);
        const text = doc.getText();
        
        // Check field type tracking
        const testerField = symbols.find(s => s.name === '_tester' && s.parent === 'Main');
        assert.ok(testerField, 'Should find _tester field');
        assert.strictEqual(testerField?.type, 'Tester', '_tester should have type Tester');
        
        // Check Tester class fields have correct types
        const passingTestsField = symbols.find(s => s.name === '_passingTests');
        const failingTestsField = symbols.find(s => s.name === '_failingTests');
        assert.strictEqual(passingTestsField?.type, 'List', '_passingTests should have type List');
        assert.strictEqual(failingTestsField?.type, 'List', '_failingTests should have type List');
        
        // Check local variable types - line 10 in the code (0-based would be index 10)
        const localVars = parser.extractLocalVariables(text, 10);
        assert.strictEqual(localVars.length, 3, 'Should find 3 local variables');
        
        const baseList = localVars.find(v => v.name === 'baseList');
        const uniqueList = localVars.find(v => v.name === 'uniqueList');
        const aVar = localVars.find(v => v.name === 'a');
        
        assert.strictEqual(baseList?.type, 'List', 'baseList should have type List');
        assert.strictEqual(uniqueList?.type, undefined, 'uniqueList type cannot be inferred from method chain');
        assert.strictEqual(aVar?.type, 'List', 'a should have type List');
    });
});
