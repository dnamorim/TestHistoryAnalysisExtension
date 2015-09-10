using CMF.TestHistoryAnalysisTool.TeamFoundationClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CMF.TestHistoryAnalysisTool.TestHistory.ChartSection.Converters
{
    public class TooltipDurationConverter : IValueConverter
    {
        /// <summary>
        /// Converts from the Test Duration to the propper format to show on the Chart Tooltip
        /// </summary>
        /// <param name="value">The Test Duration</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>The Test Duration Converted</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TimeSpan duration = (TimeSpan) value;
             switch (TestHistoryController.Instance.TestHistoryTimeFormat)
             {
                 case TestHistoryTimeFormat.Milliseconds:
                     return String.Format("{0:0.000} ms", duration.TotalMilliseconds);
                 case TestHistoryTimeFormat.Seconds:
                     return String.Format("{0:0.000} s", duration.TotalSeconds);
                 case TestHistoryTimeFormat.Minutes:
                     return String.Format("{0:0.000} m", duration.TotalMinutes);
                 case TestHistoryTimeFormat.Hours:
                     return String.Format("{0:0.000} h", duration.TotalHours);
                 default:
                     return String.Empty;
             }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}