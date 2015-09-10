using Microsoft.TeamFoundation.TestManagement.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CMF.TestHistoryAnalysisTool.TestHistory.ChartSection.Converters
{
    public class TooltipBuildNrConverter : IValueConverter
    {
        /// <summary>
        /// Converts from the Build Uri to the Build Number to be used on the Chart Tooltip
        /// </summary>
        /// <param name="value">The Build Uri</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>The Build Number</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Uri buildUri = (Uri) value;
            if (buildUri == null)
            {
                return String.Empty;   
            }
            
             var str = buildUri.AbsoluteUri;
             return str.Substring(str.LastIndexOf('/') + 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
