using NlControls.WPF.Adorners;
using NlControls.WPF.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace NlControls.WPF.Controls.CustomThumb
{
    public class MoveThumb : Thumb
    {
        private RotateTransform rotateTransform;
        private ContentControl designerItem;
        private Adorner locationChromeAdorner;

        public static ResizeBehavior dib;
        AdornerLayer adornerlayyer;

        static MoveThumb()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MoveThumb), new FrameworkPropertyMetadata(typeof(MoveThumb)));
        }
        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
            DragCompleted += MoveThumb_DragCompleted;
        }



        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as ContentControl;
            adornerlayyer = AdornerLayer.GetAdornerLayer(this);
            if (this.designerItem != null)
            {
                this.rotateTransform = this.designerItem.RenderTransform as RotateTransform;
                if (adornerlayyer != null)
                {
                    if (Canvas.GetTop(designerItem) < 0 || Canvas.GetLeft(designerItem) < 0) return;
                    locationChromeAdorner = new LocationAdorner(designerItem);
                    adornerlayyer.Add(locationChromeAdorner);
                }
                if (dib == null)
                {
                    dib = new ResizeBehavior();
                }
                if (!Interaction.GetBehaviors(designerItem).Contains(dib))
                {
                    dib.CheckBehavior();
                    Interaction.GetBehaviors(designerItem).Add(dib);
                    Panel.SetZIndex(designerItem, 10);
                }

            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null)
            {
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                if (this.rotateTransform != null)
                {
                    dragDelta = this.rotateTransform.Transform(dragDelta);
                }
                if (adornerlayyer != null && locationChromeAdorner != null)
                {

                    if (Canvas.GetTop(designerItem) < 0 || Canvas.GetLeft(designerItem) < 0)
                    {

                        adornerlayyer.Remove(locationChromeAdorner);
                    }
                    else
                    {
                        adornerlayyer.Remove(locationChromeAdorner);
                        locationChromeAdorner = new LocationAdorner(designerItem);
                        adornerlayyer.Add(locationChromeAdorner);
                    }

                }
                Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + dragDelta.X);
                Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + dragDelta.Y);
            }
        }

        void MoveThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (adornerlayyer != null && locationChromeAdorner != null)
            {
                adornerlayyer.Remove(locationChromeAdorner);
            }

        }






    }
}
