namespace CustomLogic
{
    /// <summary>
    /// Represents a single source file in the custom logic compilation pipeline.
    /// </summary>
    public class CustomLogicSourceFile
    {
        /// <summary>
        /// Display name of the file for error messages (e.g., "BaseLogic", "MyMode.cl", "MyMap.maplogic").
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The raw source code content.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// The type of source file (determines loading order and isolation rules).
        /// </summary>
        public CustomLogicSourceType Type { get; }

        /// <summary>
        /// Starting line number in the combined source (calculated during compilation).
        /// </summary>
        public int StartLine { get; internal set; }

        /// <summary>
        /// Ending line number in the combined source (calculated during compilation).
        /// </summary>
        public int EndLine { get; internal set; }

        public CustomLogicSourceFile(string name, string content, CustomLogicSourceType type)
        {
            Name = name;
            Content = content;
            Type = type;
            StartLine = 0;
            EndLine = 0;
        }

        /// <summary>
        /// Gets the number of lines in this file.
        /// </summary>
        public int LineCount => Content.Split('\n').Length;
    }

    /// <summary>
    /// Defines the type of custom logic source file and its loading priority.
    /// Lower values are loaded first.
    /// </summary>
    public enum CustomLogicSourceType
    {
        /// <summary>
        /// C# bindings (highest priority, always loaded first).
        /// These are virtual files representing builtin types.
        /// </summary>
        CSharpBindings = 0,

        /// <summary>
        /// Base logic file (second priority).
        /// Contains common components and utilities.
        /// </summary>
        BaseLogic = 1,

        /// <summary>
        /// Addon files (third priority).
        /// Can define classes and components that extend base functionality.
        /// Loaded before map logic and mode logic.
        /// </summary>
        Addon = 2,

        /// <summary>
        /// Map logic embedded in the map file (fourth priority).
        /// </summary>
        MapLogic = 3,

        /// <summary>
        /// Game mode logic file (lowest priority, loaded last).
        /// Can override classes from addons but not from BaseLogic or C# bindings.
        /// </summary>
        ModeLogic = 4
    }
}
