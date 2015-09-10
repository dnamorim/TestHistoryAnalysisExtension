using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Microsoft.TeamFoundation.Controls;

namespace CMF.TestHistoryAnalysisTool.TestHistory
{
    /// <summary>
    /// The Test History Page
    /// </summary>
    [TeamExplorerPage(GuidList.guidTestHistoryTeamExplorerNavigationItem)]
    public class TestHistoryPage : ITeamExplorerPage
    {
        private IServiceProvider serviceProvider;
        private bool isBusy = false;
        
        public void Cancel()
        {
        }

        public object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        public void Initialize(object sender, PageInitializeEventArgs e)
        {
            this.serviceProvider = e.ServiceProvider;
            if (this.PageContent == null)
            {
                this.PageContent = new TestHistoryView();
            }
        }

        public void Loaded(object sender, PageLoadedEventArgs e)
        {
        }

        public object PageContent { get; set; }

        public void Refresh()
        {
        }

        public void SaveContext(object sender, PageSaveContextEventArgs e)
        {

        }

        public string Title
        {
            get
            {
                return Resources.TestHistoryTitle;
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            set
            {
                this.isBusy = value;
                this.FirePropertyChanged("IsBusy");
            }
        }

        public void Dispose()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
