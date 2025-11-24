import * as vscode from 'vscode';
import { DebugAdapterDescriptorFactory } from './debugAdapter';

export function activate(context: vscode.ExtensionContext) {
    console.log('Custom Logic Debugger extension is now active!');
    
    context.subscriptions.push(
        vscode.debug.registerDebugAdapterDescriptorFactory('cl', new DebugAdapterDescriptorFactory())
    );
    
    vscode.window.showInformationMessage('Custom Logic Debugger activated! Open a .cl file and press F5 to debug.');
}

export function deactivate() {
    console.log('Custom Logic Debugger extension is now deactivated.');
}