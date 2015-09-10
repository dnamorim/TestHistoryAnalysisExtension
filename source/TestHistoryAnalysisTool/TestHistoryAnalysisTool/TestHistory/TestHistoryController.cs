using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.TeamFoundation.Build;
using CMF.TestHistoryAnalysisTool.TeamFoundationClient;
using System.Collections.ObjectModel;
using CMF.TestHistoryAnalysisTool.TestHistory.BuildDefinitionsToSearch;


namespace CMF.TestHistoryAnalysisTool.TestHistory
{
    /// <summary>
    /// The Test History Controller
    /// </summary>
    public class TestHistoryController
    {
        /// <summary>
        /// The Test History Instance of the Singleton Pattern
        /// </summary>
        private static TestHistoryController instance = null;

        /// <summary>
        /// The Test History Controller Instance
        /// </summary>
        public static TestHistoryController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestHistoryController();
                }
                return instance;
            }
        }

        /// <summary>
        /// The TFS Server
        /// </summary>
        private TeamFoundationServerExt teamFoundationServerExt;

        /// <summary>
        /// The TeamFoundationProjectChanged EventHandler
        /// </summary>
        public event EventHandler TeamFoundationProjectChanged;

        private TfsClient tfsClient = null;
        
        /// <summary>
        /// The TFS Client 
        /// </summary>
        public TfsClient TfsClient {
            get
            {
                return this.tfsClient;
            }

            private set
            {
                if (this.tfsClient == null)
                {
                    this.tfsClient = value;
                }
            }
        }

        /// <summary>
        /// The Build Definitions To Search Container
        /// </summary>
        public ObservableCollection<BuildDefinitionViewModel> BuildDefinitions 
        { 
            get; set; 
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="TestHistoryController"/>
        /// </summary>
        private TestHistoryController()
        {
            var dteService = Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            if (dteService == null)
            {
                Debug.WriteLine("[TestHistoryController] DTE Service is null");
                return;
            }

            var teamExplorer = (ITeamExplorer)(Package.GetGlobalService(typeof(ITeamExplorer)));

            teamFoundationServerExt = (dteService.GetObject("Microsoft.VisualStudio.TeamFoundation.TeamFoundationServerExt")) as TeamFoundationServerExt;
            teamFoundationServerExt.ProjectContextChanged += teamFoundationServerExt_ProjectContextChanged;
            this.BuildDefinitions = new ObservableCollection<BuildDefinitionViewModel>();
        }

        /// <summary>
        /// The TFS Server Project Context Changed Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teamFoundationServerExt_ProjectContextChanged(object sender, EventArgs e)
        {
            if (this.TeamFoundationProjectChanged != null) 
            {
                this.TeamFoundationProjectChanged(this, e);
            }
            InitializeTFSClient();
        }
        #endregion

        /// <summary>
        /// Initializes the TFS Client and check if the project is valid to perform test Searches
        /// </summary>
        public void InitializeTFSClient()
        {
            try
            {
                var tfsProjectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(teamFoundationServerExt.ActiveProjectContext.DomainUri));
                var tfsVersionControl = tfsProjectCollection.GetService<VersionControlServer>();
                var tfsTeamProject = tfsVersionControl.GetTeamProject(teamFoundationServerExt.ActiveProjectContext.ProjectName);
                #region Initialize TfsClient
                this.TfsClient = new TfsClient(this.teamFoundationServerExt, tfsProjectCollection, tfsVersionControl, tfsTeamProject);
                this.UpdateBuildDefinitionsContainer();
                #endregion
            }
            catch (Exception)
            {
                throw new ArgumentException("The team project does not exist. Open the team project and try again.");
            }
        }

        /// <summary>
        /// Reload the Build Definitions to Search Container
        /// </summary>
        private void UpdateBuildDefinitionsContainer()
        {
            this.BuildDefinitions.Clear();
            foreach (var item in TestHistoryController.Instance.TfsClient.AvailableBuildDefinitions())
            {
                BuildDefinitions.Add(new BuildDefinitionViewModel(new BuildDefinition { Name = item.Key, BuildUri = item.Value }, true)); // by default, all build definions are selected
            }
        }

        /// <summary>
        /// Get a List of BuildDefinitions Uri's to Search
        /// </summary>
        /// <returns>list of build definitions Uri</returns>
        public List<Uri> BuildDefinitionsToSearch()
        {
            var bdlist = BuildDefinitions.Where(bd => bd.IsSelected == true).Select(bd => bd.BuildUri).ToList();
            return bdlist;
        }

        /// <summary>
        /// Determines whether [is team project connnected].
        /// </summary>
        /// <returns></returns>
        public bool IsTeamProjectConnnected()
        {
            return this.teamFoundationServerExt.ActiveProjectContext.DomainUri != null;
        }

        /// <summary>
        /// The <see cref="TestHistoryTimeFormat"/>: defines the Time format to be presented on the GUI
        /// </summary>
        public TestHistoryTimeFormat TestHistoryTimeFormat { get; set; }
    }
}
