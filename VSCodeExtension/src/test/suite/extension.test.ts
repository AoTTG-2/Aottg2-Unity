import * as assert from 'assert';
import * as vscode from 'vscode';

suite('Extension Test Suite', () => {
    vscode.window.showInformationMessage('Start all tests.');

    test('Extension should be present', () => {
        assert.ok(vscode.extensions.getExtension('aottg2.cl-debugger'));
    });

    test('Should activate', async () => {
        const ext = vscode.extensions.getExtension('aottg2.cl-debugger');
        await ext?.activate();
        assert.ok(true);
    });

    test('Should register cl language', () => {
        const languages = vscode.languages.getLanguages();
        assert.ok(languages);
    });
});
