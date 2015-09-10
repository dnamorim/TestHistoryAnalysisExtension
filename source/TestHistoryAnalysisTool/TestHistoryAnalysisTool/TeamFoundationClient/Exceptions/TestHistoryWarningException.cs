using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMF.TestHistoryAnalysisTool.TeamFoundationClient.Exceptions
{
    /// <summary>
    /// Represents a Warning Exception for the Test History Extension
    /// </summary>
    public class TestHistoryWarningException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TestHistoryWarningException() 
        {
        }

        /// <summary>
        /// Constructor with Message
        /// </summary>
        /// <param name="message">Message of the Exception to launch</param>
        public TestHistoryWarningException(string message) : base(message) 
        {
        }

        /// <summary>
        /// Constructor with Message and Inner Exception
        /// </summary>
        /// <param name="message">Message of the Exception to Launch</param>
        /// <param name="inner">Inner Exception</param>
        public TestHistoryWarningException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}