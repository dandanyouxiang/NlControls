using NlControls.WPF.Controls;
using NlControls.WPF.Controls.CustomThumb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NlControls.WPF.Adorners
{
    public class ResizeAdorner : Adorner
    {
        private VisualCollection visuals;
        private ResizeChrome chrome;


        protected override int VisualChildrenCount
        {
            get
            {
                return this.visuals.Count;
            }
        }
     
        public ResizeAdorner(FrameworkElement designerItem)
            : base(designerItem)
        {
            this.chrome = new ResizeChrome();
            this.visuals = new VisualCollection(this);
            this.visuals.Add(this.chrome);
            this.chrome.DataContext = designerItem;
        }


        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            this.chrome.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.visuals[index];
        }


        #region test

        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
        //    SolidColorBrush renderBrush = new SolidColorBrush(Colors.Red);
        //    renderBrush.Opacity = 1.0;
        //    Pen renderPen = new Pen(new SolidColorBrush(Colors.Red), 0.5);
        //    double renderRadius = 3.0;

        //    drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
        //    drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
        //    drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
        //    drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);

        //} 
        #endregion
    }
}
