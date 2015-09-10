using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.TeamFoundation.TestManagement.Client;

namespace CMF.TestHistoryAnalysisTool.TestHistory
{
    /// <summary>
    /// The Test Result Class
    /// </summary>
    public class TestResult : INotifyPropertyChanged
    {
        /// <summary>
        /// The Test Case Result of the TFS
        /// </summary>
        private ITestCaseResult testCaseResult;

        private const int IndexAssemblyDLL = 2;
        private const char Separator = ',';

        /// <summary>
        /// The Test Result Duration
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return testCaseResult.Duration;
            }
        }

        /// <summary>
        /// The Test Result Date
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this.testCaseResult.DateStarted;
            }
        }

        /// <summary>
        /// The Test Result Outcome
        /// </summary>
        public TestOutcome Outcome
        {
            get
            {
                return testCaseResult.Outcome;
            }
        }

        /// <summary>
        /// The Test Build Uri 
        /// </summary>
        public Uri BuildUri
        {
            get
            {
                return new Uri(testCaseResult.BuildNumber);
            }
        }

        /// <summary>
        /// The Test Assembly DLL
        /// </summary>
        public string AssemblyDLL
        {
            get
            {
                return testCaseResult.Implementation.ToString().Split(Separator)[IndexAssemblyDLL];
            }
        }
        
        /// <summary>
        /// Initializes a new test result
        /// </summary>
        /// <param name="result">TFS Test Case Result</param>
        public TestResult(ITestCaseResult result)
        {
            this.testCaseResult = result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
