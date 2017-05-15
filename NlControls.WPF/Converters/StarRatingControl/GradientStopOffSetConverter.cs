using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NlControls.WPF.Converters.StarRatingControl
{
    public class GradientStopOffSetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var value = values[0].ExtractDouble();
            var minimum = values[1].ExtractDouble();
            var maximum = values[2].ExtractDouble();

            if (new[] { value, minimum, maximum }.AnyNan())
                return Binding.DoNothing;

             var percent = maximum <= minimum ? 1.0 : (value - minimum)/(maximum - minimum);
             return percent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
