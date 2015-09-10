using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CMF.TestHistoryAnalysisTool.TestHistory.BuildDefinitionsToSearch
{
    /// <summary>
    /// Interaction logic for BuildDefinitionsToSearchView.xaml
    /// </summary>
    public partial class BuildDefinitionsToSearchView : Window
    {
        public BuildDefinitionsToSearchView()
        {
            InitializeComponent();     
            
            // Associates the BuildDefinition ObservableCollection within the Controller as the DataContext of the ListView
            this.DataContext = TestHistoryController.Instance.BuildDefinitions;
        }
    }
}
