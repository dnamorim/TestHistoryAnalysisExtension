using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Controls;
using System.Windows;

namespace CMF.TestHistoryAnalysisTool.TestHistory.DetailsSection
{
    /// <summary>
    /// Represents the Details Section of the Test History Page
    /// </summary>
    [TeamExplorerSection(GuidList.guidDetailsTestHistoryTeamExplorerSection, GuidList.guidTestHistoryTeamExplorerNavigationItem, 200)]
    public class DetailsTestHistoryTeamExplorerSection : ITeamExplorerSection
    {
        private IServiceProvider serviceProvider;
        private bool isBusy;
        private bool isExpanded = false;
        private bool isVisible = true;

        /// <summary>
        /// The Details Section GUI
        /// </summary>
        private DetailsTestHistoryTeamExplorerView sectionContent;

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
                return this.sectionContent = (this.sectionContent ?? new DetailsTestHistoryTeamExplorerView(this.serviceProvider));
            }
        }

        public string Title
        {
            get { return Resources.TestHistoryDetailsSectionTitle; }
        }

        public void Dispose()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
