using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NlControls.WPF.Converters.ProcessCircleBar
{
    public class StartPointConverter : IValueConverter
    {
        [Obsolete]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && ((double) value > 0.0))
            {
                var actualWidth = (double)value;
                var radians = 220 * (Math.PI / 180);

                var centre = new Point(actualWidth / 2, actualWidth / 2);
                var hypotenuseRadius = (actualWidth / 2);

                var adjacent = Math.Cos(radians) * hypotenuseRadius;
                var opposite = Math.Sin(radians) * hypotenuseRadius;

                return new Point(centre.X + opposite, centre.Y - adjacent);
            }
          
            return new Point();
        }

        [Obsolete]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

    }
}