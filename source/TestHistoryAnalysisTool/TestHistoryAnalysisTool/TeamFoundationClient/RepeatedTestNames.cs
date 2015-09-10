using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMF.TestHistoryAnalysisTool.TeamFoundationClient
{
    /// <summary>
    /// Represents useful info about a Test Repetition occurence when searching for Test Runs of a specified Test Name
    /// </summary>
    public struct RepeatedTestNames
    {
        /// <summary>
        /// Informs if the test name is Repeated
        /// </summary>
        public bool IsRepeated { get; set; }

        /// <summary>
        /// Date of the Repetition Occurence first detected
        /// </summary>
        public DateTime RepeatedOccurence { get; set; }
    }
}
