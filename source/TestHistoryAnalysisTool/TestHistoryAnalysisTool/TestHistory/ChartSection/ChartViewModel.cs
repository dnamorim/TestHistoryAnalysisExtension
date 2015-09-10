using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CMF.TestHistoryAnalysisTool.TestHistory.ChartSection
{
    /// <summary>
    /// The Chart View Model
    /// </summary>
    class ChartViewModel 
    {
        /// <summary>
        /// The Test Results Container
        /// </summary>
        public ObservableCollection<TestResult> TestResults { get; set; }

        /// <summary>
        /// Initializes the Chart View Model
        /// </summary>
        public ChartViewModel()
        {
            TestResults = new ObservableCollection<TestResult>();
        }

        /// <summary>
        /// Reload Test Results on the the Test Results container
        /// </summary>
        /// <param name="list">the results to load</param>
        internal void AddTestResults(IList<TestResult> list)
        {
            TestResults.Clear();
            foreach (var item in list)
            {
                TestResults.Add(item);
            }
        }
    }
}
