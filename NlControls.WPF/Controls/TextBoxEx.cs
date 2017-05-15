using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using NlControls.WPF.Common;
using System.Windows.Media;

namespace NlControls.WPF.Controls
{
     [TemplatePart(Name = "gd", Type = typeof(Grid))]
    public class TextBoxEx : TextBox
    {
        public Style InnerButtonStyle
        {
            get { return (Style)GetValue(InnerButtonStyleProperty); }
            set
            {
                SetValue(InnerButtonStyleProperty, value);
            }
        }

        public static readonly DependencyProperty InnerButtonStyleProperty =
            DependencyProperty.Register("InnerButtonStyle", typeof(Style), typeof(TextBoxEx), new PropertyMetadata(null));

        static TextBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEx), new FrameworkPropertyMetadata(typeof(TextBoxEx)));
        }

        private Button innerButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
        }

        

        void innerButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
