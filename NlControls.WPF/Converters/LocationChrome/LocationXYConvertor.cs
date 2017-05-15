using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NlControls.WPF.Converters.LocationChrome
{
    public class LocationXYConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string para = parameter.ToString();
            FrameworkElement item = value as FrameworkElement;

            if (para == "X1")
            {
                return -Canvas.GetLeft(item)/2-20;
            }
            else if (para == "X2")
            {
                return item.Width / 2 + 20;
            }
            else if (para == "X3")
            {
                return item.Width + 10;
            }
            else if (para == "Y1")
            {
                return item.Height / 2 - 20;
            }
            else if (para == "Y2")
            {
                return item.Height + 10;
            }
            else
            {
                return  -Canvas.GetTop(item) / 2-20;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
