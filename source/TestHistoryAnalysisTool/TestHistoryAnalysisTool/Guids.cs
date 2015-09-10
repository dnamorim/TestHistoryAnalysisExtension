// Guids.cs
// MUST match guids.h
using System;

namespace CMF.TestHistoryAnalysisTool
{
    static class GuidList
    {
        public const string guidTestHistoryAnalysisToolPkgString = "981acc0c-c3c9-40ff-b852-179be8f03fc5";
        public const string guidTestHistoryAnalysisToolCmdSetString = "9add6c88-dc4f-4118-90d1-47ada91b21c0";
        
        /// <summary>
        /// The GUID for the Test History Navigation Item
        /// </summary>
        public const string guidTestHistoryTeamExplorerNavigationItem = "1ab126f3-5063-41ac-a0e4-5f6c42b2e6f9";
        
        /// <summary>
        /// The GUID for the Details Section of the Test History Page
        /// </summary>
        public const string guidDetailsTestHistoryTeamExplorerSection = "1975192e-5b61-4bd6-a64c-10012c454628";
        
        /// <summary>
        /// The GUID for the Chart Section of the Test History Page
        /// </summary>
        public const string guidChartTestHistoryTeamExplorerSection = "71c98087-5799-409d-8dcb-f2e8d8fc22c0";
        
        /// <summary>
        /// The GUID of the Test History Notifications within the Team Explorer Page
        /// </summary>
        public const string guidNotificationTestHistoryTeamExplorer = "2f2e70f2-090e-4d4c-9249-5fe42dde300e";

        public static readonly Guid guidTestHistoryAnalysisToolCmdSet = new Guid(guidTestHistoryAnalysisToolCmdSetString);
    };
}