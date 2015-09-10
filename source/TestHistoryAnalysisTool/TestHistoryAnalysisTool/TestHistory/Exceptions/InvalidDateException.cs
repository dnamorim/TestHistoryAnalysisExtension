using System;

namespace CMF.TestHistoryAnalysisTool.TestHistory.Exceptions
{
    /// <summary>
    /// Represents an Invalid Date Exception for the Test History Extension
    /// </summary>
    public class InvalidDateException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InvalidDateException() 
        {
        }

        /// <summary>
        /// Constructor with Message
        /// </summary>
        /// <param name="message">Message of the Exception to launch</param>
        public InvalidDateException(string message) : base(message) 
        {
        }

        /// <summary>
        /// Constructor with Message and Inner Exception
        /// </summary>
        /// <param name="message">Message of the Exception to Launch</param>
        /// <param name="inner">Inner Exception</param>
        public InvalidDateException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
