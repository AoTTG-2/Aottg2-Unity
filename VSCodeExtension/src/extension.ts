import * as vscode from 'vscode';
import * as path from 'path';
import { LanguageClient, LanguageClientOptions, ServerOptions, TransportKind } from 'vscode-languageclient/node';
import { DebugAdapterDescriptorFactory } from './debugAdapter';

let client: LanguageClient;

export function activate(context: vscode.ExtensionContext) {
    console.log('Custom Logic Debugger extension is now active!');
    
    // Register debug adapter
    context.subscriptions.push(
        vscode.debug.registerDebugAdapterDescriptorFactory('cl', new DebugAdapterDescriptorFactory())
    );
    
    // Start language server for IntelliSense
    startLanguageServer(context);
    
    vscode.window.showInformationMessage('Custom Logic Debugger activated! Open a .cl file and press F5 to debug.');
}

function startLanguageServer(context: vscode.ExtensionContext) {
    // The server is implemented in node
    const serverModule = context.asAbsolutePath(path.join('out', 'server.js'));
    
    // Debug options for the server
    const debugOptions = { execArgv: ['--nolazy', '--inspect=6009'] };
    
    // If the extension is launched in debug mode then the debug server options are used
    // Otherwise the run options are used
    const serverOptions: ServerOptions = {
        run: { module: serverModule, transport: TransportKind.ipc },
        debug: {
            module: serverModule,
            transport: TransportKind.ipc,
            options: debugOptions
        }
    };
    
    // Options to control the language client
    const clientOptions: LanguageClientOptions = {
        // Register the server for custom logic documents
        documentSelector: [{ scheme: 'file', language: 'cl' }],
        synchronize: {
            // Notify the server about file changes to '.cl files contained in the workspace
            fileEvents: vscode.workspace.createFileSystemWatcher('**/*.cl')
        }
    };
    
    // Create the language client and start the client
    client = new LanguageClient(
        'clLanguageServer',
        'CL Language Server',
        serverOptions,
        clientOptions
    );
    
    // Start the client. This will also launch the server
    client.start();
}

export function deactivate(): Thenable<void> | undefined {
    console.log('Custom Logic Debugger extension is now deactivated.');
    if (!client) {
        return undefined;
    }
    return client.stop();
}