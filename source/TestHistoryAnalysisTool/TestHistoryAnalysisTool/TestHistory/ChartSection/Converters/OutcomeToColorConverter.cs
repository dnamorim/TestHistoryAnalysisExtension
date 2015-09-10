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
    public class OutcomeToDateConverter : IValueConverter
    {
        /// <summary>
        /// Converts from the TestOutcome type to a Column Color to be used on the Chart
        /// </summary>
        /// <param name="value">The Value to convert</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>The Column Color Brush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TestOutcome outcome = (TestOutcome)value;
            string returnColor;
            switch (outcome)
            {
                case TestOutcome.Passed:
                    returnColor = "#7fba00";
                    break;
                case TestOutcome.Failed:
                    returnColor = "#bf3636";
                    break;
                case TestOutcome.Inconclusive:
                default:
                    returnColor = "#ffb900";
                    break;
            }

            return (SolidColorBrush)(new BrushConverter().ConvertFrom(returnColor));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
