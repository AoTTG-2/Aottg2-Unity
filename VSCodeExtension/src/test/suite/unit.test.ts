import * as assert from 'assert';

suite('Debug Adapter Unit Tests', () => {
    
    test('Path normalization - backslashes to forward slashes', () => {
        // Simple path normalization test
        const testPath = 'C:\\Users\\Test\\file.cl';
        const normalized = testPath.replace(/\\/g, '/');
        assert.strictEqual(normalized, 'C:/Users/Test/file.cl');
    });

    test('Path normalization - already normalized', () => {
        const testPath = 'C:/Users/Test/file.cl';
        const normalized = testPath.replace(/\\/g, '/');
        assert.strictEqual(normalized, 'C:/Users/Test/file.cl');
    });

    test('Path normalization - mixed slashes', () => {
        const testPath = 'C:\\Users/Test\\file.cl';
        const normalized = testPath.replace(/\\/g, '/');
        assert.strictEqual(normalized, 'C:/Users/Test/file.cl');
    });

    test('DAP Message format - Content-Length header', () => {
        const testMessage = '{"type":"response","success":true}';
        const header = `Content-Length: ${Buffer.byteLength(testMessage, 'utf8')}\r\n\r\n`;
        const fullMessage = header + testMessage;
        
        assert.ok(fullMessage.includes('Content-Length:'));
        assert.ok(fullMessage.includes('\r\n\r\n'));
        assert.ok(fullMessage.endsWith(testMessage));
    });

    test('DAP Message parsing - extract content length', () => {
        const message = 'Content-Length: 42\r\n\r\n{"test":"data"}';
        const match = message.match(/Content-Length: (\d+)\r\n\r\n/);
        
        assert.ok(match);
        assert.strictEqual(match[1], '42');
    });

    test('JSON message serialization', () => {
        const request = {
            seq: 1,
            type: 'request',
            command: 'initialize',
            arguments: {}
        };
        
        const json = JSON.stringify(request);
        assert.ok(json.includes('"type":"request"'));
        assert.ok(json.includes('"command":"initialize"'));
    });

    test('JSON message deserialization', () => {
        const json = '{"seq":1,"type":"response","success":true}';
        const message = JSON.parse(json);
        
        assert.strictEqual(message.type, 'response');
        assert.strictEqual(message.success, true);
        assert.strictEqual(message.seq, 1);
    });
});

suite('Message Buffer Handling', () => {
    
    test('Should handle complete message', () => {
        const testMessage = '{"type":"response"}';
        const header = `Content-Length: ${testMessage.length}\r\n\r\n`;
        const fullMessage = header + testMessage;
        
        const headerMatch = fullMessage.match(/Content-Length: (\d+)\r\n\r\n/);
        assert.ok(headerMatch);
        
        const contentLength = parseInt(headerMatch[1], 10);
        const messageStart = headerMatch[0].length;
        const messageEnd = messageStart + contentLength;
        
        assert.ok(fullMessage.length >= messageEnd);
        const extracted = fullMessage.substring(messageStart, messageEnd);
        assert.strictEqual(extracted, testMessage);
    });

    test('Should handle partial message', () => {
        const testMessage = '{"type":"response"}';
        const header = `Content-Length: ${testMessage.length}\r\n\r\n`;
        const partialMessage = header + testMessage.substring(0, 5);
        
        const headerMatch = partialMessage.match(/Content-Length: (\d+)\r\n\r\n/);
        assert.ok(headerMatch);
        
        const contentLength = parseInt(headerMatch[1], 10);
        const messageStart = headerMatch[0].length;
        const messageEnd = messageStart + contentLength;
        
        // Should detect incomplete message
        assert.ok(partialMessage.length < messageEnd);
    });
});
