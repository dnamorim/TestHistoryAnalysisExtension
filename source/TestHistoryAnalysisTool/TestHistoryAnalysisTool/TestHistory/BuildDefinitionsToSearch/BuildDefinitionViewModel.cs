using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMF.TestHistoryAnalysisTool.TestHistory.BuildDefinitionsToSearch
{
    /// <summary>
    /// Represents the View-Model of a Build Definition
    /// </summary>
    public class BuildDefinitionViewModel
    {
        /// <summary>
        /// The Build Definition
        /// </summary>
        BuildDefinition _model;

        /// <summary>
        /// The Build Definition Name
        /// </summary>
        public string Name
        {
            get
            {
                return _model.Name;
            }
        }

        /// <summary>
        /// The Build Definition Uri
        /// </summary>
        public Uri BuildUri
        {
            get
            {
                return _model.BuildUri;
            }
        }

        /// <summary>
        /// Creates a new BuildDefinitionViewModel
        /// </summary>
        /// <param name="model">The Build Definition</param>
        /// <param name="isSelected">Selection State of the Build (true or false)</param>
        public BuildDefinitionViewModel(BuildDefinition model, bool isSelected)
        {
            _model = model;
            IsSelected = isSelected;
        }

        /// <summary>
        /// Informs if the BuildDefinition is Selected
        /// </summary>
        public Nullable<bool> IsSelected { get; set; }
    }

    /// <summary>
    /// Represents the necessary information about a BuildDefinition to the <see cref="BuildDefinitionViewModel"/>
    /// </summary>
    public struct BuildDefinition
    {
        /// <summary>
        /// Build Definition Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Build Definition Uri
        /// </summary>
        public Uri BuildUri { get; set; }
    }
}
