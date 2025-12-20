using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomLogic
{
    /// <summary>
    /// Manages the compilation of multiple custom logic source files.
    /// Handles ordering, line number mapping, and error reporting.
    /// </summary>
    public class CustomLogicCompiler
    {
        /// <summary>
        /// Metadata about a compiled file without storing its content.
        /// </summary>
        private class FileRange
        {
            public string Name { get; set; }
            public CustomLogicSourceType Type { get; set; }
            public int StartLine { get; set; }
            public int EndLine { get; set; }
            public int LineCount { get; set; }
        }

        private List<CustomLogicSourceFile> _sourceFiles = new List<CustomLogicSourceFile>();
        private List<FileRange> _fileRanges = new List<FileRange>();
        private string _combinedSource;

        /// <summary>
        /// Adds a source file to the compilation pipeline.
        /// </summary>
        public void AddSourceFile(CustomLogicSourceFile file)
        {
            _sourceFiles.Add(file);
        }

        /// <summary>
        /// Adds multiple source files to the compilation pipeline.
        /// </summary>
        public void AddSourceFiles(IEnumerable<CustomLogicSourceFile> files)
        {
            _sourceFiles.AddRange(files);
        }

        /// <summary>
        /// Compiles all source files into a single combined source string.
        /// Files are ordered by their CustomLogicSourceType.
        /// After compilation, source file contents are released to save memory.
        /// </summary>
        /// <returns>The combined source code.</returns>
        public string Compile()
        {
            // Sort files by type (priority)
            var sortedFiles = _sourceFiles.OrderBy(f => (int)f.Type).ToList();

            // Reset state
            _fileRanges.Clear();
            int currentLine = 0;
            List<string> combinedParts = new List<string>();

            // Process each file in order
            foreach (var file in sortedFiles)
            {
                int lineCount = file.LineCount;
                
                // Store file range metadata
                var range = new FileRange
                {
                    Name = file.Name,
                    Type = file.Type,
                    StartLine = currentLine,
                    EndLine = currentLine + lineCount - 1,
                    LineCount = lineCount
                };
                _fileRanges.Add(range);
                
                // Update source file's line positions
                file.StartLine = currentLine;
                file.EndLine = currentLine + lineCount - 1;

                // Add the content
                combinedParts.Add(file.Content);

                // Update current line position
                currentLine += lineCount;
            }

            _combinedSource = string.Join("\n", combinedParts);
            
            // Clear source file list to free memory
            // Keep only the metadata in _fileRanges
            _sourceFiles.Clear();
            
            return _combinedSource;
        }

        /// <summary>
        /// Gets the combined source code (must call Compile() first).
        /// </summary>
        public string GetCombinedSource()
        {
            return _combinedSource;
        }

        /// <summary>
        /// Formats a line number for error messages, including file name and local line number.
        /// Uses efficient range-based lookup - O(log n) with binary search.
        /// </summary>
        /// <param name="globalLine">The line number in the combined source (0-based).</param>
        /// <returns>A formatted string like "45 (MyMode.cl:12)"</returns>
        public string FormatLineNumber(int globalLine)
        {
            var range = FindFileRange(globalLine);
            
            if (range != null)
            {
                int localLine = globalLine - range.StartLine + 1; // 1-based for display
                
                // Special handling for C# bindings
                if (range.Type == CustomLogicSourceType.CSharpBindings)
                {
                    return $"{globalLine} (Internal C# Bindings)";
                }
                
                return $"{globalLine} ({range.Name}:{localLine})";
            }

            return globalLine.ToString();
        }

        /// <summary>
        /// Gets the file metadata for a given global line number.
        /// Uses binary search for O(log n) lookup.
        /// </summary>
        private FileRange FindFileRange(int globalLine)
        {
            // Binary search through file ranges
            int left = 0;
            int right = _fileRanges.Count - 1;
            
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                var range = _fileRanges[mid];
                
                if (globalLine >= range.StartLine && globalLine <= range.EndLine)
                {
                    return range;
                }
                else if (globalLine < range.StartLine)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Gets the file name that contains the given global line number.
        /// </summary>
        public string GetFileNameForLine(int globalLine)
        {
            var range = FindFileRange(globalLine);
            return range?.Name;
        }

        /// <summary>
        /// Gets the file type that contains the given global line number.
        /// </summary>
        public CustomLogicSourceType? GetFileTypeForLine(int globalLine)
        {
            var range = FindFileRange(globalLine);
            return range?.Type;
        }

        /// <summary>
        /// Converts a global line number to a local line number within its file.
        /// Returns -1 if the line is not found.
        /// </summary>
        public int GetLocalLineNumber(int globalLine)
        {
            var range = FindFileRange(globalLine);
            if (range != null)
            {
                return globalLine - range.StartLine + 1; // 1-based
            }
            return -1;
        }

        /// <summary>
        /// Gets information about all compiled files (without their content).
        /// </summary>
        public IEnumerable<(string Name, CustomLogicSourceType Type, int StartLine, int EndLine)> GetFileInfo()
        {
            foreach (var range in _fileRanges)
            {
                yield return (range.Name, range.Type, range.StartLine, range.EndLine);
            }
        }

        /// <summary>
        /// Gets the number of files that were compiled.
        /// </summary>
        public int FileCount => _fileRanges.Count;

        /// <summary>
        /// Gets the total number of lines in the combined source.
        /// </summary>
        public int TotalLines => _fileRanges.Count > 0 ? _fileRanges[_fileRanges.Count - 1].EndLine + 1 : 0;

        /// <summary>
        /// Clears all source files and compiled data from the compiler.
        /// </summary>
        public void Clear()
        {
            _sourceFiles.Clear();
            _fileRanges.Clear();
            _combinedSource = null;
        }

        /// <summary>
        /// Gets the offset for base logic (first non-C# binding file).
        /// This is used for backwards compatibility with the old system.
        /// </summary>
        public int GetBaseLogicOffset()
        {
            var baseLogicRange = _fileRanges.FirstOrDefault(r => r.Type == CustomLogicSourceType.BaseLogic);
            return baseLogicRange?.StartLine ?? 0;
        }
    }
}
