using NlControls.WPF.Controls.CustomThumb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NlControls.WPF.Adorners
{
    public class MoveThumbAdorner:Adorner
    {
         private VisualCollection visuals;
         private FrameworkElement designerItem;
         private MoveThumb moveThumb;
        protected override int VisualChildrenCount
        {
            get
            {
                return this.visuals.Count;
            }
        }

        public MoveThumbAdorner(FrameworkElement designerItem)
            : base(designerItem)
        {
            this.SnapsToDevicePixels = true;
            this.designerItem = designerItem;
            this.visuals = new VisualCollection(this);
           moveThumb = new MoveThumb();
           visuals.Add(moveThumb);
           this.moveThumb.DataContext = designerItem;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.visuals[index];
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
           this.moveThumb.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }
    }
}
