using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;



namespace NlControls.WPF.Controls
{
    public class LocationChrome : Control
    {
        static LocationChrome()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(LocationChrome), new FrameworkPropertyMetadata(typeof(LocationChrome)));
        }
    }

}
