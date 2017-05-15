using NlControls.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace NlControls.WPF.Adorners
{
    public class LocationAdorner:Adorner
    {
         private LocationChrome chrome;
        private VisualCollection visuals;
        private ContentControl designerItem;

        protected override int VisualChildrenCount
        {
            get
            {
                return this.visuals.Count;
            }
        }

        public LocationAdorner(ContentControl designerItem)
            : base(designerItem)
        {
            this.SnapsToDevicePixels = true;
            this.designerItem = designerItem;
            this.chrome = new LocationChrome();
            this.chrome.DataContext = designerItem;
            this.visuals = new VisualCollection(this);
            this.visuals.Add(this.chrome);
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.visuals[index];
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            this.chrome.Arrange(new Rect(new Point(0.0, 0.0), arrangeBounds));
            return arrangeBounds;
        }
    }
}
