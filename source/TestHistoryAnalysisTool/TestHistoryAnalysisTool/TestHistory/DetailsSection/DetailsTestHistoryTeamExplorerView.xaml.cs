using Microsoft.VisualStudio.TeamFoundation.Build;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CMF.TestHistoryAnalysisTool.TestHistory.DetailsSection
{
    /// <summary>
    /// Interaction logic for DetailsTestHistoryTeamExplorerSectionView.xaml
    /// </summary>
    /// 
    public partial class DetailsTestHistoryTeamExplorerView : UserControl
    {
        /// <summary>
        /// The Service Provider
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// The Details View Model
        /// </summary>
        private DetailsViewModel viewModel;

        /// <summary>
        /// Initializes the DetailsTestHistoryTeamExplorerSectionView
        /// </summary>
        /// <param name="serviceProvider">the service provider</param>
        public DetailsTestHistoryTeamExplorerView(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.viewModel = new DetailsViewModel();
            this.TestDetailsDatagrid.DataContext = this.viewModel.TestDetails;
            NoTestResultsAvailable();
        }

        /// <summary>
        /// Reload the Test Results for the section's datagrid
        /// </summary>
        /// <param name="list">the list with the test results</param>
        internal void LoadTestResults(List<TestResult> list)
        {
            this.viewModel.AddTestResults(list);
            this.TestDetailsDatagrid.Visibility = Visibility.Visible;
            this.txtNoResultsAvailable.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Hides the Datagrid and show an informative Message
        /// </summary>
        internal void NoTestResultsAvailable()
        {
            this.TestDetailsDatagrid.Visibility = Visibility.Collapsed;
            this.txtNoResultsAvailable.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Open the Build Summary of the selected Test Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestDetailsDatagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dteService = TestHistoryAnalysisToolPackage.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            if (dteService == null)
            {
                Debug.WriteLine("[DetailsTestHistoryTeamExplorerView] DTE Service is null.");
                return;
            }

            IList<DataGridCellInfo> cells = e.AddedCells;
            if (cells.Count > 0)
            {
                TestDetails item = (TestDetails)cells[0].Item;

                if (item != null)
                {
                    var vsTFSBuild = (dteService.GetObject("Microsoft.VisualStudio.TeamFoundation.Build.VsTeamFoundationBuild") as VsTeamFoundationBuild);
                    vsTFSBuild.OpenBuild(item.BuildUri);
                }
                else
                {
                    Debug.WriteLine("[DetailsTestHistoryTeamExplorerView] Build Uri from Test Result is null.");
                    return;
                }
            }
        }
    }
}
