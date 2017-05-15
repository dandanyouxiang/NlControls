using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NlControls.WPF.Controls
{
    //扩展自定义Slider
    public class SliderEx : Slider
    {
        static SliderEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderEx), new FrameworkPropertyMetadata(typeof(SliderEx)));
        }
        //箭头颜色
        public Brush ArrowBackGround
        {
            get { return (Brush)GetValue(ArrowBackGroundProperty); }
            set { SetValue(ArrowBackGroundProperty, value); }
        }
        public static readonly DependencyProperty ArrowBackGroundProperty =
            DependencyProperty.Register("ArrowBackGround", typeof(Brush), typeof(SliderEx), new PropertyMetadata(Brushes.LightGray));

        //图片样式
        public Style ImageStyle
        {
            get { return (Style)GetValue(ImageStyleProperty); }
            set { SetValue(ImageStyleProperty, value); }
        }
        public static readonly DependencyProperty ImageStyleProperty =
            DependencyProperty.Register("ImageStyle", typeof(Style), typeof(SliderEx), new PropertyMetadata(null));


        //按钮样式
        public Style ImageButtonStyle
        {
            get { return (Style)GetValue(ImageButtonStyleProperty); }
            set { SetValue(ImageButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty ImageButtonStyleProperty =
            DependencyProperty.Register("ImageButtonStyle", typeof(Style), typeof(SliderEx), new PropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //Button btn = this.Template.FindName("ImageButton", this) as Button;
            //if (btn != null)
            //    btn.Click += _imageButton_Click;
        }

        void _imageButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
