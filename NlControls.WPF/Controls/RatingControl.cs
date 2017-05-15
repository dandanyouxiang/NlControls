using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NlControls.WPF.Controls
{

    [TemplatePart(Name = Stars_Name, Type = typeof(Path))]
    public class RatingControl : Control
    {

        private const string Stars_Name = "PART_STARS";
        Path Stars;

        public enum ShapeAllType
        {
            Stars,
            Rects
        }



        public ShapeAllType ShapeType
        {
            get { return (ShapeAllType)GetValue(ShapeTypeProperty); }
            set { SetValue(ShapeTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShapeType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShapeTypeProperty =
            DependencyProperty.Register("ShapeType", typeof(ShapeAllType), typeof(RatingControl), new PropertyMetadata(ShapeAllType.Stars));

        



        public Color StarBackground
        {
            get { return (Color)GetValue(StarBackgroundProperty); }
            set { SetValue(StarBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StarBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StarBackgroundProperty =
            DependencyProperty.Register("StarBackground", typeof(Color), typeof(RatingControl), new PropertyMetadata(Colors.Gray));




        public Color StarForeground
        {
            get { return (Color)GetValue(StarForegroundProperty); }
            set { SetValue(StarForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StarForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StarForegroundProperty =
            DependencyProperty.Register("StarForeground", typeof(Color), typeof(RatingControl), new PropertyMetadata(Colors.Yellow));




        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(RatingControl), new PropertyMetadata(0.0));



        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minmium.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(RatingControl), new PropertyMetadata(0.0));



        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maxmium.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(RatingControl), new PropertyMetadata(100.0));



        static RatingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(typeof(RatingControl)));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Stars = GetTemplateChild(Stars_Name) as Path;
            if (Stars != null)
            {
                Stars.PreviewMouseLeftButtonUp += Stars_PreviewMouseLeftButtonUp;
            }

        }

        private void Stars_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(Stars);
            var percent = point.X / Stars.ActualWidth;
            Value = Maximum * percent;
        }
    }
}
