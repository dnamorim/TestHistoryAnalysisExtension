using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CMF.TestHistoryAnalysisTool.TestHistory.DetailsSection.Converters
{
    public class RelativeTimeDiffConverter : IValueConverter
    {
        /// <summary>
        /// Converts the Test Details Difference for a percentage value
        /// </summary>
        /// <param name="value">the Test Details</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>the test details difference percentage</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TestDetails testDetails = (TestDetails)value;
            if (testDetails == null || testDetails.TimeDifference == TimeSpan.Zero)
            {
                return String.Empty;
            }

            return String.Format("{0:0.000} %", ((double) testDetails.TimeDifference.Ticks / (double) testDetails.CurrentDuration.Ticks) * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
