using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics;

namespace CMF.TestHistoryAnalysisTool.TestHistory
{
    /// <summary>
    /// The Test History Navigation Item for the Team Explorer
    /// </summary>
    [TeamExplorerNavigationItem(GuidList.guidTestHistoryTeamExplorerNavigationItem, Int32.MaxValue)]
    public class TestHistoryNavigationItem : ITeamExplorerNavigationItem
    {
        private Image image = Resources.TestHistoryNavigationItem;
        private bool isVisible;
        private string text = Resources.TestHistoryTitle;

        [ImportingConstructor]
        public TestHistoryNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.IsVisible = TestHistoryController.Instance.IsTeamProjectConnnected();
            TestHistoryController.Instance.TeamFoundationProjectChanged += controller_TeamFoundationProjectChanged;
        }

        void controller_TeamFoundationProjectChanged(object sender, EventArgs e)
        {
            this.IsVisible = TestHistoryController.Instance.IsTeamProjectConnnected();
        }

        private IServiceProvider serviceProvider { get; set; }

        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                this.FirePropertyChanged("Image");
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

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.FirePropertyChanged("Text");
            }
        }

        public void Execute()
        {
            var service = this.GetService<ITeamExplorer>();
            if (service == null)
            {
                Debug.WriteLine("[TestHsitoryNavigationItem] ITeamExplorer service is null.");
                return;
            }

            try
            {
                TestHistoryController.Instance.InitializeTFSClient(); 
                service.NavigateToPage(new Guid(GuidList.guidTestHistoryTeamExplorerNavigationItem), null);
            }
            catch(Exception ex)
            {
                service.ShowNotification(ex.Message, NotificationType.Error, NotificationFlags.None, null, new Guid(GuidList.guidNotificationTestHistoryTeamExplorer));
            }
        }

        public void Invalidate()
        {
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

        public T GetService<T>()
        {
            if (this.serviceProvider != null)
            {
                return (T)this.serviceProvider.GetService(typeof(T));
            }
            return default(T);
        }
    }
}
