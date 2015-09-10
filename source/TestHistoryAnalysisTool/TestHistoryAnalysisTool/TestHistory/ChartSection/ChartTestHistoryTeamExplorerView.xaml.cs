using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.TeamFoundation.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.TeamFoundation.Build;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics;
using System.Threading;

namespace CMF.TestHistoryAnalysisTool.TestHistory.ChartSection
{
    /// <summary>
    /// Interaction logic for ChartTestHistoryTeamExplorerSection.xaml
    /// </summary>
    /// 
    [ProvideBindingPath]
    public partial class ChartTestHistoryTeamExplorerView : UserControl
    {
        /// <summary>
        /// The Service Provider
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// The Chart View Model
        /// </summary>
        private ChartViewModel viewModel;
        
        /// <summary>
        /// Initializes the Chart GUI
        /// </summary>
        /// <param name="serviceProvider">the service provider</param>
        public ChartTestHistoryTeamExplorerView(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            viewModel = new ChartViewModel();
            this.TestsChart.DataContext = viewModel;
            NoTestResultsAvailable();
        }

        /// <summary>
        /// Open the Build Summary for the Selected Test Result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dteService = TestHistoryAnalysisToolPackage.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            System.Windows.Controls.DataVisualization.Charting.ColumnSeries cs = (System.Windows.Controls.DataVisualization.Charting.ColumnSeries)sender;
            var tr = cs.SelectedItem as TestResult;
            
            if (tr != null)
            {
                var vsTFSBuild = (dteService.GetObject("Microsoft.VisualStudio.TeamFoundation.Build.VsTeamFoundationBuild") as VsTeamFoundationBuild);
                vsTFSBuild.OpenBuild(tr.BuildUri);
            }
            else
            {
                Debug.WriteLine("[ChartTestHistoryTeamExplorerView] Build Uri from Test Result is null.");
            }
        }

        /// <summary>
        /// Loads <see cref="TestResult"/>s for the Chart and make it visible
        /// </summary>
        /// <param name="list">the list with Test Results</param>
        public void LoadTestResults(IList<TestResult> list)
        {
            this.TestsChart.Visibility = Visibility.Visible;
            this.txtNoResultsAvailable.Visibility = Visibility.Collapsed;
            viewModel.AddTestResults(list);
            this.TestsChart.UpdateLayout();
        }

        /// <summary>
        /// Put the Chart Section UI with no presentation of the Chart, only an informative message
        /// </summary>
        internal void NoTestResultsAvailable()
        {
            this.TestsChart.Visibility = Visibility.Collapsed;
            this.txtNoResultsAvailable.Visibility = Visibility.Visible;
        }
    }
}
