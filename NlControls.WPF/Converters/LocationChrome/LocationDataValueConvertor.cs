using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NlControls.WPF.Converters.LocationChrome
{
    public class LocationDataValueConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FrameworkElement item = value as FrameworkElement;
            Canvas canvas = item.Parent as Canvas;
            int para = int.Parse(parameter.ToString());
            if(canvas==null||item==null)return 0;
            switch (para)
            {
                case 0:
                    return Math.Round(Canvas.GetLeft(item));
                    break;
                case 1:
                    return Math.Round(Canvas.GetTop(item));
                    break;
                case 2:
                    return Math.Round(canvas.ActualWidth - Canvas.GetLeft(item) - item.Width);
                    break;
                case 3:
                    return Math.Round(canvas.ActualHeight - Canvas.GetTop(item) - item.Height);
                    break;
                default:
                    return 0;
                    break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
