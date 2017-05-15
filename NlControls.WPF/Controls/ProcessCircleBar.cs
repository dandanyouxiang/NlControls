using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace  NlControls.WPF.Controls
{
    [TemplatePart(Name = Path_Geo_Name, Type = typeof(GeometryGroup))]
    [TemplatePart(Name = PathGrid_Name, Type = typeof(GeometryGroup))]
    public class ProcessCircleBar:Control
    {
        private const string Path_Geo_Name = "PART_PathGeo";
        private const string PathGrid_Name = "PART_PathGrid";

        private Grid pathGrid;
        private GeometryGroup geo;





        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(ProcessCircleBar), new PropertyMetadata(10.0));

        



        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimun.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimun", typeof(double), typeof(ProcessCircleBar), new PropertyMetadata(0.0));



        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(ProcessCircleBar), new PropertyMetadata(100.0));



        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ProcessCircleBar), new FrameworkPropertyMetadata(0.0, ValuePropertyChangedCallback));

        private static void ValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var circleBar = d as ProcessCircleBar;
            var value = (int)Math.Round((double)e.NewValue);
           // circleBar.SetProcessBar(value);
        }

        public Style CenterTextStyle
        {
            get { return (Style)GetValue(CenterTextStyleProperty); }
            set { SetValue(CenterTextStyleProperty, value); }
        }

        public static readonly DependencyProperty CenterTextStyleProperty =
            DependencyProperty.Register("CenterTextStyle", typeof(Style), typeof(ProcessCircleBar), new PropertyMetadata(null));

        
        static ProcessCircleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProcessCircleBar), new FrameworkPropertyMetadata(typeof(ProcessCircleBar)));
        }

        public ProcessCircleBar()
        {
           
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //geo = GetTemplateChild(Path_Geo_Name) as GeometryGroup;
            //if (geo==null)
            //{
            //    throw new Exception("PathGeometry is null");
            //}
            pathGrid = GetTemplateChild(PathGrid_Name) as Grid;
            if (pathGrid==null)
            {
                throw new Exception("PathGrid is null");
            }
            if (CenterTextStyle != null)
            {
                TextBlock txt = GetTemplateChild("centerText") as TextBlock;
                if (txt != null)
                {
                    txt.Style = CenterTextStyle;
                }
            }
        }


        private void SetProcessBar( int newValue )
        {
            //if (newValue==0)
            //{

            //   // geo.Children.RemoveAt(0);
            //}
            if (newValue%10!=0)
            {
                return;
            }
            int diff = geo.Children.Count - newValue;
            if (diff!=0)
            {

                var actualWidth = pathGrid.ActualWidth;
                var rectHeight = 7 * (actualWidth-StrokeThickness*2) * Math.PI / 900;
                var percent = Maximum <= Minimum ? 1.0 : (newValue - Minimum) / (Maximum - Minimum);
                var degrees = 280 * percent + 220;

                var radians = degrees * (Math.PI / 180);

                var centre = new Point(actualWidth / 2, actualWidth / 2);
                var hypotenuseRadius = (actualWidth / 2);

                var adjacent = Math.Cos(radians) * hypotenuseRadius;
                var opposite = Math.Sin(radians) * hypotenuseRadius;
                RectangleGeometry rect = new RectangleGeometry(new Rect(centre.X + opposite, centre.Y - adjacent, StrokeThickness/4, rectHeight/2));
                
                geo.Children.Add(rect);
            }
        }
    }
}
