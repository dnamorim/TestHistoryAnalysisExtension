using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMF.TestHistoryAnalysisTool.TestHistory.DetailsSection
{
    /// <summary>
    /// The Test Details
    /// </summary>
    public class TestDetails
    {
        /// <summary>
        /// The Previous Test Result
        /// </summary>
        private TestResult _previous;

        /// <summary>
        /// The Current Test Result
        /// </summary>
        private TestResult _current;

        /// <summary>
        /// The BuildUri of the current Test Details
        /// </summary>
        public Uri BuildUri
        {
            get
            {
                return _current.BuildUri;
            }
        }

        /// <summary>
        /// The Date of the current Test Details
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _current.Date;
            }
        }

        /// <summary>
        /// The Duration from this Test Details
        /// </summary>
        public TimeSpan CurrentDuration
        {
            get
            {
                return _current.Duration;
            }
        }

        /// <summary>
        /// The Duration from the previous Test Result
        /// </summary>
        public TimeSpan PreviousDuration
        {
            get
            {
                if (_previous == null)
                {
                    return TimeSpan.Zero;
                }
                return _previous.Duration;
            }
        }

        /// <summary>
        /// The Difference between the current and the previous Test Results
        /// </summary>
        public TimeSpan TimeDifference
        {
            get
            {
                if (_previous == null)
                {
                    return TimeSpan.Zero;
                }

                return (this.CurrentDuration - this.PreviousDuration);
            }
        }

        /// <summary>
        /// Initializes a new instance of Test Details 
        /// </summary>
        /// <param name="current">The current Test Result</param>
        /// <param name="previous">The previously Test Result</param>
        public TestDetails(TestResult current, TestResult previous = null)
        {
            _current = current;
            _previous = previous;
        }
    }
}
