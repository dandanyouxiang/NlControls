using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NlControls.WPF.Controls
{
    public class NewsFlowListPanel : Panel
    {


        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }

        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register("RowCount", typeof(int), typeof(NewsFlowListPanel), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsArrange));

        

        protected override Size ArrangeOverride(System.Windows.Size finalSize)
        {

            var itemsHeight = (finalSize.Height) / (RowCount > 0 ? RowCount : 1);
            double heightcache = 0;
            for (var i = 0; i < Children.Count; i++)
            {
                double cah = ((FrameworkElement)Children[i]).ActualHeight;
                double ch = itemsHeight >= cah && cah > 0 ? cah : itemsHeight;
                Children[i].Arrange(new Rect(0, heightcache, finalSize.Width, ch));
                heightcache += ch;
            }

            return finalSize;
        }


    }
}
