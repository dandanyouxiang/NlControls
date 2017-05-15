using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NlControls.WPF.Common;
using System.Windows;

namespace NlControls.WPF.Controls
{
    public class ComboBoxEx:ComboBox
    {
        static ComboBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxEx), new FrameworkPropertyMetadata(typeof(ComboBoxEx)));
        }
    }
}
