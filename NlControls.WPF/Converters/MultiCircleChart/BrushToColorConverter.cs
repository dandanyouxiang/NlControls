using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace NlControls.WPF.Converters.MultiCircleChart
{
   public  class BrushToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush brush = value as Brush;
            if (brush!=null)
            {
                Color colorValue = (Color)System.Windows.Media.ColorConverter.ConvertFromString(brush.ToString());
                return colorValue;
            }
            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
