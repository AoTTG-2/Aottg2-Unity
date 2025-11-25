import * as path from 'path';
import * as fs from 'fs';

export interface CLType {
    name: string;
    arguments: string[];
}

export interface CLParameter {
    name: string;
    type: CLType;
    defaultValue: string;
    isOptional: boolean;
    info: CLDocumentation | null;
}

export interface CLDocumentation {
    summary: string;
    remarks: string;
    code: string;
    parameters: { [key: string]: string } | null;
    returns: string | null;
}

export interface CLField {
    type: CLType;
    label: string;
    info: CLDocumentation;
    readonly: boolean;
    obsoleteMessage: string;
    isObsolete: boolean;
}

export interface CLMethod {
    parameters: CLParameter[];
    returnType: CLType;
    label: string;
    info: CLDocumentation;
    obsoleteMessage: string;
    isObsolete: boolean;
}

export interface CLClass {
    name: string;
    info: CLDocumentation;
    constructors: any[] | null;
    staticFields: CLField[];
    instanceFields: CLField[];
    staticMethods: CLMethod[];
    instanceMethods: CLMethod[];
    kind: string;
    baseTypeName: string;
    obsoleteMessage: string;
    isObsolete: boolean;
}

/**
 * Loads and manages API metadata from JSON files
 */
export class APIMetadataLoader {
    private classes: Map<string, CLClass> = new Map();

    constructor(private jsonDir: string) {}

    /**
     * Load all JSON files from the json directory
     */
    public loadAll(): void {
        if (!fs.existsSync(this.jsonDir)) {
            console.error(`JSON directory not found: ${this.jsonDir}`);
            return;
        }

        const files = fs.readdirSync(this.jsonDir);
        for (const file of files) {
            if (file.endsWith('.json')) {
                this.loadFile(path.join(this.jsonDir, file));
            }
        }

        console.log(`[CL Language Server] Loaded ${this.classes.size} API classes`);
    }

    /**
     * Load a single JSON file
     */
    private loadFile(filePath: string): void {
        try {
            const content = fs.readFileSync(filePath, 'utf8');
            const classData: CLClass = JSON.parse(content);
            this.classes.set(classData.name, classData);
        } catch (error) {
            console.error(`Error loading ${filePath}:`, error);
        }
    }

    /**
     * Get a class by name
     */
    public getClass(name: string): CLClass | undefined {
        return this.classes.get(name);
    }

    /**
     * Get all class names
     */
    public getClassNames(): string[] {
        return Array.from(this.classes.keys());
    }

    /**
     * Get all classes
     */
    public getAllClasses(): CLClass[] {
        return Array.from(this.classes.values());
    }

    /**
     * Find methods in a class
     */
    public getMethods(className: string): CLMethod[] {
        const cls = this.classes.get(className);
        if (!cls) return [];
        return [...cls.staticMethods, ...cls.instanceMethods];
    }

    /**
     * Find fields in a class
     */
    public getFields(className: string): CLField[] {
        const cls = this.classes.get(className);
        if (!cls) return [];
        return [...cls.staticFields, ...cls.instanceFields];
    }
}
