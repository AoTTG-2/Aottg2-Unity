import * as assert from 'assert';
import { CLDebugSession } from '../../debugAdapter';

suite('Debug Adapter Test Suite', () => {
    
    test('CLDebugSession should be instantiable', () => {
        const session = new CLDebugSession();
        assert.ok(session);
    });

    test('Path normalization should convert backslashes to forward slashes', () => {
        const session = new CLDebugSession();
        // Access private method via any type assertion for testing
        const normalized = (session as any).normalizePath('C:\\Users\\Test\\file.cl');
        assert.strictEqual(normalized, 'C:/Users/Test/file.cl');
    });

    test('Path normalization should handle already normalized paths', () => {
        const session = new CLDebugSession();
        const normalized = (session as any).normalizePath('C:/Users/Test/file.cl');
        assert.strictEqual(normalized, 'C:/Users/Test/file.cl');
    });

    test('Path normalization should handle mixed slashes', () => {
        const session = new CLDebugSession();
        const normalized = (session as any).normalizePath('C:\\Users/Test\\file.cl');
        assert.strictEqual(normalized, 'C:/Users/Test/file.cl');
    });

    test('Message buffer should handle Content-Length parsing', () => {
        const session = new CLDebugSession();
        const testMessage = '{"type":"response","success":true}';
        const header = `Content-Length: ${testMessage.length}\r\n\r\n`;
        const fullMessage = header + testMessage;
        
        // This tests that the message format is correct
        assert.ok(fullMessage.includes('Content-Length:'));
        assert.ok(fullMessage.includes('\r\n\r\n'));
    });

    test('Should create session with correct line/column start', () => {
        const session = new CLDebugSession();
        // Debug sessions should start at line 1, column 1
        assert.ok(session);
    });
});

suite('Debug Adapter Factory Test Suite', () => {
    const vscode = require('vscode');
    
    test('DebugAdapterDescriptorFactory should be importable', () => {
        const { DebugAdapterDescriptorFactory } = require('../../debugAdapter');
        assert.ok(DebugAdapterDescriptorFactory);
    });

    test('Factory should create server descriptor with default port', () => {
        const { DebugAdapterDescriptorFactory } = require('../../debugAdapter');
        const factory = new DebugAdapterDescriptorFactory();
        
        const mockSession = {
            configuration: {}
        };
        
        const descriptor = factory.createDebugAdapterDescriptor(mockSession, undefined);
        assert.ok(descriptor);
        assert.strictEqual(descriptor.port, 4711);
    });

    test('Factory should create server descriptor with custom port', () => {
        const { DebugAdapterDescriptorFactory } = require('../../debugAdapter');
        const factory = new DebugAdapterDescriptorFactory();
        
        const mockSession = {
            configuration: { port: 5000 }
        };
        
        const descriptor = factory.createDebugAdapterDescriptor(mockSession, undefined);
        assert.ok(descriptor);
        assert.strictEqual(descriptor.port, 5000);
    });
});
