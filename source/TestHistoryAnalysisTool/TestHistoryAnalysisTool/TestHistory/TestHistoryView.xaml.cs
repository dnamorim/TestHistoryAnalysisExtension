using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.TeamFoundation.Controls;
using System.Windows.Media.Animation;
using CMF.TestHistoryAnalysisTool.TestHistory.Exceptions;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.Build;
using CMF.TestHistoryAnalysisTool.TestHistory.ChartSection;
using System.Collections.ObjectModel;
using CMF.TestHistoryAnalysisTool.TeamFoundationClient.Exceptions;
using CMF.TestHistoryAnalysisTool.TestHistory.DetailsSection;
using CMF.TestHistoryAnalysisTool.TestHistory.BuildDefinitionsToSearch;
using CMF.TestHistoryAnalysisTool.TeamFoundationClient;

namespace CMF.TestHistoryAnalysisTool.TestHistory
{
    /// <summary>
    /// Interaction logic for TestHistoryView.xaml
    /// </summary>
    /// 
    [ProvideBindingPath]
    public partial class TestHistoryView : UserControl
    {
        /// <summary>
        /// The Notification Guid
        /// </summary>
        private Guid notificationGuid = new Guid(GuidList.guidNotificationTestHistoryTeamExplorer);
        
        /// <summary>
        /// The Team Explorer Service
        /// </summary>
        private ITeamExplorer teamExplorerService;

        /// <summary>
        /// The Background Worker
        /// </summary>
        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// Initializes a new Test History View
        /// </summary>
        public TestHistoryView()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            this.teamExplorerService = TestHistoryAnalysisToolPackage.GetGlobalService(typeof(ITeamExplorer)) as ITeamExplorer;
            this.setDefaultsDateTime();
            teamExplorerService.HideNotification(notificationGuid);
        }

        /// <summary>
        /// Initializes the BackgroundWorker
        /// </summary>
        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.WorkerReportsProgress = false;
            this.backgroundWorker.WorkerSupportsCancellation = false;

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// Present the Results or Errors after the background worker run has terminated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error.GetType() == typeof(TestHistoryWarningException))
                {
                    teamExplorerService.ShowNotification(e.Error.Message, NotificationType.Warning, NotificationFlags.None, null, notificationGuid);
                }
                else
                {
                    teamExplorerService.ShowNotification(e.Error.Message, NotificationType.Error, NotificationFlags.None, null, notificationGuid);
                }

                // On the Sections forces to present a Instuction Message Tooltip
                (this.teamExplorerService.CurrentPage.GetSection(Guid.Parse(GuidList.guidChartTestHistoryTeamExplorerSection)).SectionContent as ChartTestHistoryTeamExplorerView).NoTestResultsAvailable();
                (this.teamExplorerService.CurrentPage.GetSection(Guid.Parse(GuidList.guidDetailsTestHistoryTeamExplorerSection)).SectionContent as DetailsTestHistoryTeamExplorerView).NoTestResultsAvailable();
            }
            else
            {
                // Obtain the Results from the Background Worker
                Tuple<List<TestResult>, RepeatedTestNames, TestHistoryTimeFormat> tupleResult = (Tuple<List<TestResult>, RepeatedTestNames, TestHistoryTimeFormat>) e.Result;
                if (tupleResult.Item2.IsRepeated)
                {
                    teamExplorerService.ShowNotification(String.Format("Found tests with repeated names on {0}. Showing the first result available.", tupleResult.Item2.RepeatedOccurence), NotificationType.Warning, NotificationFlags.None, null, notificationGuid);
                }
                TestHistoryController.Instance.TestHistoryTimeFormat = tupleResult.Item3; // Define the Time Format to Show
                (this.teamExplorerService.CurrentPage.GetSection(Guid.Parse(GuidList.guidChartTestHistoryTeamExplorerSection)).SectionContent as ChartTestHistoryTeamExplorerView).LoadTestResults(tupleResult.Item1);
                (this.teamExplorerService.CurrentPage.GetSection(Guid.Parse(GuidList.guidDetailsTestHistoryTeamExplorerSection)).SectionContent as DetailsTestHistoryTeamExplorerView).LoadTestResults(tupleResult.Item1);
            }
           setBusyState(false);
        }

        /// <summary>
        /// Initializes an async Test Result's search from the specified parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Tuple<string, DateTime, DateTime> parameters = (Tuple<string, DateTime, DateTime>) e.Argument; 
            e.Result = TestHistoryController.Instance.TfsClient.TestResultsFromDates(parameters.Item1, parameters.Item2, parameters.Item3, TestHistoryController.Instance.BuildDefinitionsToSearch());
        }

        /// <summary>
        /// Set the Default Dates to narrow the Test History Search (From Last Month)
        /// </summary>
        private void setDefaultsDateTime() {
            datepickerEnd.SelectedDate = DateTime.Now;
            datepickerBegin.SelectedDate = DateTime.Today.AddMonths(-1);
        }

        /// <summary>
        /// Validate the input data and starts the search for Test Results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                setBusyState(true);
                teamExplorerService.HideNotification(notificationGuid);
                var testName = txtTestName.Text;
                if (String.IsNullOrEmpty(testName) || String.IsNullOrWhiteSpace(testName))
                {
                    throw new ArgumentException("Insert a Test Name to Search.");
                }
                validateDatesToSearch();
                
                if (backgroundWorker.IsBusy != true)
                {
                    // On the End Date, put's the DateTime at 23:59:59 to give all the Builds from the last day and not till midnight
                    backgroundWorker.RunWorkerAsync(
                        new Tuple<string, DateTime, DateTime>(txtTestName.Text, datepickerBegin.SelectedDate.Value, 
                            datepickerEnd.SelectedDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59)));
                }
                else
                {
                    teamExplorerService.ShowNotification("Test History is Busy. Try again later.", NotificationType.Warning, NotificationFlags.None, null, notificationGuid);
                }
            } catch(Exception ex) {
                teamExplorerService.ShowNotification(ex.Message, NotificationType.Error, NotificationFlags.None, null, notificationGuid);
                setBusyState(false);
            }
        }

        /// <summary>
        /// Checks if the Selected Dates are valid
        /// </summary>
        private void validateDatesToSearch()
        {
            DateTime beginDate = datepickerBegin.SelectedDate.Value;
            DateTime endDate = datepickerEnd.SelectedDate.Value;

            if (beginDate > endDate)
            {
                throw new InvalidDateException("The Begin Date can't be greater than End Date.");
            }

            if (beginDate > DateTime.Today)
            {
                throw new InvalidDateException("The Begin Date can't be greater than today's date.");
            }
           
        }

        /// <summary>
        /// Set the Team Explorer Busy state to show the Test Explorer Busy Bar
        /// </summary>
        /// <param name="flag">Busy state</param>
        private void setBusyState(bool flag)
        {
            (this.teamExplorerService.CurrentPage as TestHistoryPage).IsBusy = flag;
        }

        /// <summary>
        /// Opens the Builds Definitions To Search Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkBuildDefinitionsToSearch_Click(object sender, RoutedEventArgs e)
        {
            BuildDefinitionsToSearchView bdts = new BuildDefinitionsToSearchView();
            bdts.ShowDialog();
            
        }
    }
}
