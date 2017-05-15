using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NlControls.WPF.Controls
{
    public class CircleMenuPanel:Panel
    {


        public int CentralAngle
        {
            get { return (int)GetValue(CentralAngleProperty); }
            set { SetValue(CentralAngleProperty, value); }
        }

        public static readonly DependencyProperty CentralAngleProperty =
            DependencyProperty.Register("CentralAngle", typeof(int), typeof(CircleMenuPanel), new PropertyMetadata(40));


        
        protected override Size MeasureOverride(Size availableSize)
        {
            Size resultSize = new Size(0, 0);

            foreach (UIElement child in this.Children)
            {
                child.Measure(availableSize);
                resultSize.Width = Math.Max(resultSize.Width, child.DesiredSize.Width);
                resultSize.Height = Math.Max(resultSize.Height, child.DesiredSize.Height);
            }

            resultSize.Width =
                double.IsPositiveInfinity(availableSize.Width) ?
                resultSize.Width : availableSize.Width;

            resultSize.Height =
                double.IsPositiveInfinity(availableSize.Height) ?
                resultSize.Height : availableSize.Height;

            return resultSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            ArrangeChild();
            return base.ArrangeOverride(finalSize);
        }


        private void ArrangeChild()
        {
            int count = 0;
            if (double.IsNaN(this.Width))
            {
                this.Width = 200;
            }
            if (double.IsNaN(this.Height))
            {
                this.Height = 200;
            }
            int l = Children.Count * CentralAngle;
            int startAngle = -10;
           
            foreach (FrameworkElement element in this.Children)
            {
                element.RenderTransformOrigin = new Point(1, 1);
                TransformGroup tg = new TransformGroup();
                RotateTransform r = new RotateTransform();
                r.Angle = startAngle + CentralAngle * count++;
                SkewTransform s = new SkewTransform();        
                s.AngleX =90 - CentralAngle;               
                tg.Children.Add(s);
                tg.Children.Add(r);
                
                element.RenderTransform = tg;

                if (!(double.IsNaN(this.Width)) && !(double.IsNaN(this.Height)) && !(double.IsNaN(element.DesiredSize.Width)) && !(double.IsNaN(element.DesiredSize.Height)))
                {

                    element.Arrange(new Rect(Width / 2 - element.DesiredSize.Width, Height / 2 - element.DesiredSize.Height, element.DesiredSize.Width, element.DesiredSize.Height));
                }
            }
        }
    }
}
