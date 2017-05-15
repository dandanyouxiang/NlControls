using NlControls.WPF.Adorners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace NlControls.WPF.Behaviors
{
    public class ResizeBehavior : Behavior<FrameworkElement>
    {



        private Adorner adorner;

        public bool ShowDecorator
        {
            get { return (bool)GetValue(ShowDecoratorProperty); }
            set { SetValue(ShowDecoratorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowDecorator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowDecoratorProperty =
            DependencyProperty.Register("ShowDecorator", typeof(bool), typeof(ResizeBehavior), new PropertyMetadata(true));




        protected override void OnAttached()
        {
            base.OnAttached();
            ShowAdorner();
           
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.adorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(this.adorner);
                }

                this.adorner = null;
            }
        }



        #region Helper Methods
        private void HideAdorner()
        {
            if (this.adorner != null)
            {
                this.adorner.Visibility = Visibility.Hidden;
            }
        }

        private void ShowAdorner()
        {
            if (AssociatedObject == null) return;
            if (this.adorner == null)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(50);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);
                        if (adornerLayer != null)
                        {
                            this.adorner = new ResizeAdorner(AssociatedObject);
                            adornerLayer.Add(this.adorner);
                        }
                    }));

                });

            }
        }


        public  void CheckBehavior()
        {
            if (AssociatedObject != null)
            {
                if ( Interaction.GetBehaviors(AssociatedObject).Contains(this))
                {
                    Panel.SetZIndex(AssociatedObject, 0);
                    Interaction.GetBehaviors(AssociatedObject).Remove(this);
                }
   
            }
        }
        #endregion
    }
}
