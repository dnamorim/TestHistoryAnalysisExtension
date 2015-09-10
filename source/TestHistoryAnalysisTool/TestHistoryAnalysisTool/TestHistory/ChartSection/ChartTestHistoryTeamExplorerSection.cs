using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Controls;

namespace CMF.TestHistoryAnalysisTool.TestHistory.ChartSection
{
    /// <summary>
    /// Represents the Chart Section of the Test History Page
    /// </summary>
    [TeamExplorerSection(GuidList.guidChartTestHistoryTeamExplorerSection, GuidList.guidTestHistoryTeamExplorerNavigationItem, 100)]
    public class ChartTestHistoryTeamExplorerSection : ITeamExplorerSection
    {
     private IServiceProvider serviceProvider;
        private bool isBusy;
        private bool isExpanded = true;
        private bool isVisible = true;

        /// <summary>
        /// The Chart Section GUI 
        /// </summary>
        private ChartTestHistoryTeamExplorerView sectionContent;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize(object sender, SectionInitializeEventArgs e)
        {
            this.serviceProvider = e.ServiceProvider;
        }

        public void Cancel()
        {
        }

        public object GetExtensibilityService(Type serviceType)
        {
            return null;
        }
        
        public bool IsBusy
        {
            get { return this.isBusy; }
            private set
            {
                this.isBusy = value;
                this.FirePropertyChanged("IsBusy");
            }
        }

        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }
            set
            {
                this.isExpanded = value;
                this.FirePropertyChanged("IsExpanded");
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                this.isVisible = value;
                this.FirePropertyChanged("IsVisible");
            }
        }

        public void Loaded(object sender, SectionLoadedEventArgs e)
        {
        }

        public void Refresh()
        {
        }

        public void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
        }

        public object SectionContent
        {
            get 
            {
                return this.sectionContent = (this.sectionContent ?? new ChartTestHistoryTeamExplorerView(this.serviceProvider)); 
            }
        }

        public string Title
        {
            get { return Resources.TestHistoryChartSectionTitle; }
        }

        public void Dispose()
        {
        }

        private void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
