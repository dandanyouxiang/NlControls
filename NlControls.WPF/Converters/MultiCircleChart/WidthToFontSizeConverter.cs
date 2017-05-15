using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NlControls.WPF.Converters.MultiCircleChart
{
   public  class WidthToFontSizeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

                double width = (double)value;
                if (double.IsNaN(width))
                {
                    return 0.0d;
                }
                return (Double)(width / 6.0d);
      
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
