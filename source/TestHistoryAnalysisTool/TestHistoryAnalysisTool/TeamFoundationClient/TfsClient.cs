using CMF.TestHistoryAnalysisTool.TeamFoundationClient.Exceptions;
using CMF.TestHistoryAnalysisTool.TestHistory;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.TeamFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMF.TestHistoryAnalysisTool.TeamFoundationClient
{
    public class TfsClient
    {
        /// <summary>
        /// The TFS Server
        /// </summary>
        private TeamFoundationServerExt teamFoundationServerExt;
 
        /// <summary>
        /// The TFS Team Project Collection
        /// </summary>
        private TfsTeamProjectCollection tfsProjectCollection;

        /// <summary>
        /// The Version Control Server
        /// </summary>
        private VersionControlServer tfsVersionControl;

        /// <summary>
        /// The Team Project
        /// </summary>
        private TeamProject tfsTeamProject;

        /// <summary>
        /// Initializes a new instance of <see cref="TfsClient"/> class.
        /// </summary>
        /// <param name="teamFoundationServerExt">the TFS Server</param>
        /// <param name="tfsProjectCollection">the TFS team project collection</param>
        /// <param name="tfsVersionControl">the version control server</param>
        /// <param name="tfsTeamProject">the team project</param>
        public TfsClient(TeamFoundationServerExt teamFoundationServerExt, Microsoft.TeamFoundation.Client.TfsTeamProjectCollection tfsProjectCollection, Microsoft.TeamFoundation.VersionControl.Client.VersionControlServer tfsVersionControl, Microsoft.TeamFoundation.VersionControl.Client.TeamProject tfsTeamProject)
        {
            this.teamFoundationServerExt = teamFoundationServerExt;
            this.tfsProjectCollection = tfsProjectCollection;
            this.tfsVersionControl = tfsVersionControl;
            this.tfsTeamProject = tfsTeamProject;
        }

        /// <summary>
        /// Obtain a List of Available Project Build Definitions
        /// </summary>
        /// <returns>list with the build definitions</returns>
        public Dictionary<String, Uri> AvailableBuildDefinitions()
        {
            var buildServer = tfsProjectCollection.GetService(typeof(IBuildServer)) as IBuildServer;
            var result = new List<IBuildDefinition>(buildServer.QueryBuildDefinitions(teamFoundationServerExt.ActiveProjectContext.ProjectName));
            Dictionary<String, Uri> buildDefinitions = new Dictionary<string,Uri>();
            foreach (var bd in result)
            {
                buildDefinitions.Add(bd.Name, bd.Uri);
            }
            return buildDefinitions;
        }

        /// <summary>
        /// Searches and returns the Test Result Data to show
        /// </summary>
        /// <param name="testName">Test Name to Search</param>
        /// <param name="startDate">Begin Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="buildDefinitions">list with Build Definitions Uri's to Search</param>
        /// <returns>Tuple with a list of <see cref="TestResults"/>, <see cref="RepeatedTestNames"/> and <see cref="TestHistoryTimeFormat"/></returns>
        public Tuple<List<TestResult>, RepeatedTestNames, TestHistoryTimeFormat> TestResultsFromDates(String testName, DateTime startDate, DateTime endDate, IList<Uri> buildDefinitions)
        {
            RepeatedTestNames repeatedTestNames = new RepeatedTestNames { IsRepeated = false };
            TestHistoryTimeFormat timeFormat = TestHistoryTimeFormat.Milliseconds;
            ITestManagementTeamProject testManagmentTeamProject = this.tfsProjectCollection.GetService<ITestManagementService>().GetTeamProject(tfsTeamProject.Name);

            var searchBuilds = BuildsToSearch(startDate, endDate, buildDefinitions);
            if (searchBuilds.Count() <= 0)
            {
                throw new TestHistoryWarningException("No Builds available on the defined Dates for the current project.");
            }

            List<TestResult> testResults = new List<TestResult>();
            foreach (IBuildDetail buildDetail in searchBuilds) // Iterate on the Builds Available
            {
                foreach (ITestRun testRun in testManagmentTeamProject.TestRuns.ByBuild(buildDetail.Uri)) // Obtain the TestRun's for each Build Available
                {
                    var results = testRun.QueryResults().Where(tr => tr.TestCaseTitle.Equals(testName)); // Obtain the Test Case Results for the given Test Name Title
                    if (results.Count() > 0) 
                    {
                        #region Repetition of Test Name Occurence Check
                        // Check if there is more than one obtained result and wasn't previously detected the Test Name Repetition
                        if (results.Count() > 1 && !repeatedTestNames.IsRepeated)
                        {
                            repeatedTestNames.IsRepeated = true;
                            repeatedTestNames.RepeatedOccurence = testRun.DateStarted;
                        }
                        #endregion

                        testResults.Add(new TestResult(results.First()));

                        #region Determine Time Format of Presentation
                        // this section tries to view all test time durations and set the Time Presentation Format according to the highest time found
                        var durationTicks = results.First().Duration.Ticks;
                        
                        // check if this test duration is bigger than 1hour and the TimeFormat is different than hours
                        if (durationTicks > TimeSpan.TicksPerHour && timeFormat < TestHistoryTimeFormat.Hours)
                        {
                            timeFormat = TestHistoryTimeFormat.Hours;
                        }
                        // check if this test duration is bigger than 1minute and the TimeFormat is different than minutes
                        else if (durationTicks > TimeSpan.TicksPerMinute && timeFormat < TestHistoryTimeFormat.Minutes)
                        {
                            timeFormat = TestHistoryTimeFormat.Minutes;
                        }
                        // check if this test duration is bigger than 1minute and the TimeFormat is different than seconds
                        else if (durationTicks > TimeSpan.TicksPerSecond && timeFormat < TestHistoryTimeFormat.Seconds)
                        {
                            timeFormat = TestHistoryTimeFormat.Seconds;
                        }
                        #endregion
                    }
                }
            }

            if (testResults.Count <= 0)
            {
                throw new ArgumentException("No Test Results available for the specified parameters.");
            }

            return new Tuple<List<TestResult>,RepeatedTestNames, TestHistoryTimeFormat>(testResults, repeatedTestNames, timeFormat);
        }

        /// <summary>
        /// Obtains the Builds which ran within specified limits
        /// </summary>
        /// <param name="startDate">Build Start Date</param>
        /// <param name="endDate">Build End Date</param>
        /// <param name="buildDefinitionsUri">Build Definitions Uri to Search</param>
        /// <returns>An array of IBuildDetail</returns>
        private IBuildDetail[] BuildsToSearch(DateTime startDate, DateTime endDate, IList<Uri> buildDefinitionsUri)
        {
            var buildServer = tfsProjectCollection.GetService(typeof(IBuildServer)) as IBuildServer;

            IBuildDetailSpec buildDetailSpec = buildServer.CreateBuildDetailSpec(buildDefinitionsUri);
            buildDetailSpec.MinFinishTime = startDate;
            buildDetailSpec.MaxFinishTime = endDate;
            buildDetailSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending;
            buildDetailSpec.MaxBuildsPerDefinition = int.MaxValue;
            buildDetailSpec.QueryOptions = QueryOptions.All;

            return buildServer.QueryBuilds(buildDetailSpec).Builds;
        }
    }
}
