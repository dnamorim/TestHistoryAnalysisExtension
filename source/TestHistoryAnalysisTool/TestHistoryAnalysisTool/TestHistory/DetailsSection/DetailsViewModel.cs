using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Controls;

namespace CMF.TestHistoryAnalysisTool.TestHistory.DetailsSection
{
    /// <summary>
    /// The Details View Model
    /// </summary>
    public class DetailsViewModel
    {
        /// <summary>
        /// The Test Details Container
        /// </summary>
        public ObservableCollection<TestDetails> TestDetails;

        /// <summary>
        /// Initilializes the Details View Model
        /// </summary>
        public DetailsViewModel()
        {
            this.TestDetails = new ObservableCollection<TestDetails>();
        }

        /// <summary>
        /// Reload TestResults to the Test Details Container
        /// </summary>
        /// <param name="list">the test results list</param>
        internal void AddTestResults(IList<TestResult> list)
        {
            TestDetails.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    TestDetails.Add(new TestDetails(list[i])); // the first Test Details doesn't have a previously test, so it shows in blank
                }
                else
                {
                    TestDetails.Add(new TestDetails(list[i], list[i - 1]));
                }
            }
        }
    }
}
