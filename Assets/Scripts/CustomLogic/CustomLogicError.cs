using System;

namespace CustomLogic
{
    /// <summary>
    /// Represents a captured error from Custom Logic execution with full context.
    /// Used for offline testing to verify error reporting without interrupting execution.
    /// </summary>
    public class CustomLogicError
    {
        /// <summary>
        /// The error message describing what went wrong.
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// The class name where the error occurred.
        /// </summary>
        public string ClassName { get; set; }
        
        /// <summary>
        /// The method name where the error occurred.
        /// </summary>
        public string MethodName { get; set; }
        
        /// <summary>
        /// The line number in the source file where the error occurred.
        /// </summary>
        public int LineNumber { get; set; }
        
        /// <summary>
        /// Formatted line number with file information (e.g., "line 42 (BaseLogic.cl)").
        /// </summary>
        public string FormattedLineNumber { get; set; }
        
        /// <summary>
        /// The namespace/file type where the error occurred.
        /// </summary>
        public CustomLogicSourceType? Namespace { get; set; }
        
        /// <summary>
        /// Full error message combining all context.
        /// </summary>
        public string FullMessage
        {
            get
            {
                string location = string.IsNullOrEmpty(FormattedLineNumber) ? 
                    $"at {ClassName}.{MethodName}" : 
                    $"at line {FormattedLineNumber} in {ClassName}.{MethodName}";
                return $"Custom logic runtime error {location}: {Message}";
            }
        }
        
        public CustomLogicError(string message, string className, string methodName, int lineNumber, string formattedLineNumber, CustomLogicSourceType? ns)
        {
            Message = message;
            ClassName = className;
            MethodName = methodName;
            LineNumber = lineNumber;
            FormattedLineNumber = formattedLineNumber;
            Namespace = ns;
        }
        
        public override string ToString()
        {
            return FullMessage;
        }
    }
}
