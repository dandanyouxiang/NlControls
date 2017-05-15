using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NlControls.WPF.Controls
{
    public class FlipViewEXItem : ContentControl
    {

        static FlipViewEXItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipViewEXItem), new FrameworkPropertyMetadata(typeof(FlipViewEXItem)));
        }
    }
}
