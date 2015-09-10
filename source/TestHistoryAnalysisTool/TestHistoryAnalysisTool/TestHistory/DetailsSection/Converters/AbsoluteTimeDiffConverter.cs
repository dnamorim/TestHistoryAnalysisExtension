using CMF.TestHistoryAnalysisTool.TeamFoundationClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CMF.TestHistoryAnalysisTool.TestHistory.DetailsSection.Converters
{
    class AbsoluteTimeDiffConverter : IValueConverter
    {
        /// <summary>
        /// Converts from the Time Difference of the Test Details and formats it according to the Time Format
        /// </summary>
        /// <param name="value">The Time Difference</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>the formatted time difference</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TimeSpan timediff = (TimeSpan) value;
            if (timediff == null || timediff == TimeSpan.Zero)
            {
                return String.Empty;
            }

            switch (TestHistoryController.Instance.TestHistoryTimeFormat)
            {
                 case TestHistoryTimeFormat.Milliseconds:
                     return String.Format("{0:0.00} ms", timediff.TotalMilliseconds);
                 case TestHistoryTimeFormat.Seconds:
                     return String.Format("{0:0.00} s", timediff.TotalSeconds);
                 case TestHistoryTimeFormat.Minutes:
                     return String.Format("{0:0.00} m", timediff.TotalMinutes);
                 case TestHistoryTimeFormat.Hours:
                     return String.Format("{0:0.00} h", timediff.TotalHours);
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
