using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NlControls.WPF.Converters.LocationChrome
{
    public class LocationMarginConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FrameworkElement item = value as FrameworkElement;
            Canvas canvas = item.Parent as Canvas;
            int para = int.Parse(parameter.ToString());
            if (para == 0 && null != canvas)
            {
                return new Thickness(-Canvas.GetLeft(item), 0, item.Width, 0);
                
            }
            else if (para == 1 && null != canvas)
            {
                return new Thickness(0, -Canvas.GetTop(item), 0, item.Height);
            }
            else if (para == 2 && null != canvas)
            {
                double r = Canvas.GetRight(item);
                if (double.IsNaN(r))
                {
                    r=  canvas.ActualWidth - Canvas.GetLeft(item) - item.Width;
                }
                return new Thickness(item.Width, 0, -r, 0);
            }
            else if (para == 3 && null != canvas)
            {
                double b = Canvas.GetBottom(item);
                if (double.IsNaN(b))
                {
                    b = canvas.ActualHeight - Canvas.GetTop(item) - item.Height; 
                }
                return new Thickness(0, item.Height, 0, -b);
            }
            return new Thickness(0, 0, 0, 0);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
