using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NlControls.WPF.Controls
{
    public enum ClipCornerType
    {
        All,
        Above,
        Below

    }
    public class ClippingBorder : Border
    {
        private Geometry _clipRect;
        private object _oldClip;
        protected override void OnRender(DrawingContext dc)
        {
            OnApplyChildClip();
            base.OnRender(dc);
        }
        
        public override UIElement Child
        {
            get
            {
                return base.Child;
            }
            set
            {
                if (this.Child != value)
                {
                    if (this.Child != null)
                    {
                        // Restore original clipping
                        this.Child.SetValue(UIElement.ClipProperty, _oldClip);
                    }

                    if (value != null)
                    {
                        _oldClip = value.ReadLocalValue(UIElement.ClipProperty);
                    }
                    else
                    {
                        // If we dont set it to null we could leak a Geometry object
                        _oldClip = null;
                    }

                    base.Child = value;
                }
            }
        }

        public ClipCornerType ClipCornerType { get; set; }
        

        protected virtual void OnApplyChildClip()
        {
            UIElement child = this.Child;
            if (child != null)
            {
                switch (ClipCornerType)
                {
                    case ClipCornerType.All:
                        ClipAllCorner(child);
                        break;
                    case ClipCornerType.Above:
                        ClipAboveCorner(child);
                        break;
                    case ClipCornerType.Below:
                        ClipBelowCorner(child);
                        break;
                    default:
                        break;
                }

            }
        }

        private void ClipAllCorner(UIElement child)
        {
            if (_clipRect == null || !(_clipRect is RectangleGeometry)) _clipRect = new RectangleGeometry();
            ((RectangleGeometry)_clipRect).RadiusX = ((RectangleGeometry)_clipRect).RadiusY = Math.Max(0.0, this.CornerRadius.TopLeft - (this.BorderThickness.Left * 0.5));
            ((RectangleGeometry)_clipRect).Rect = new Rect(Child.RenderSize);
            child.Clip = _clipRect;
        }
        private void ClipAboveCorner(UIElement child)
        {
            if (_clipRect == null || !(_clipRect is CombinedGeometry)) _clipRect = new CombinedGeometry();
            ((CombinedGeometry)_clipRect).GeometryCombineMode = GeometryCombineMode.Union;
            RectangleGeometry g1 = new RectangleGeometry(new Rect(Child.RenderSize));
            g1.RadiusX = g1.RadiusY = Math.Max(0.0, this.CornerRadius.TopLeft - (this.BorderThickness.Left * 0.5));
            RectangleGeometry g2 = new RectangleGeometry(new Rect(0, child.RenderSize.Height - g1.RadiusY, child.RenderSize.Width, g1.RadiusY));
            ((CombinedGeometry)_clipRect).Geometry1 = g1;
            ((CombinedGeometry)_clipRect).Geometry2 = g2;
            child.Clip = _clipRect;
        }
        private void ClipBelowCorner(UIElement child)
        {
            if (_clipRect == null || !(_clipRect is CombinedGeometry)) _clipRect = new CombinedGeometry();
            ((CombinedGeometry)_clipRect).GeometryCombineMode = GeometryCombineMode.Union;
            RectangleGeometry g1 = new RectangleGeometry(new Rect(Child.RenderSize));
            g1.RadiusX = g1.RadiusY = Math.Max(0.0, this.CornerRadius.TopLeft - (this.BorderThickness.Left * 0.5));
            RectangleGeometry g2 = new RectangleGeometry(new Rect(0, 0, child.RenderSize.Width, g1.RadiusY));
            ((CombinedGeometry)_clipRect).Geometry1 = g1;
            ((CombinedGeometry)_clipRect).Geometry2 = g2;
            child.Clip = _clipRect;
        }

    }
}
