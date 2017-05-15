using NlControls.WPF.Adorners;
using NlControls.WPF.Controls.CustomThumb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace NlControls.WPF.Behaviors
{
    public class DesignItemBehavior : Behavior<FrameworkElement>
    {
        public event MouseEventHandler MouseRightButtonDown;
        private Adorner adorner;
        protected override void OnAttached()
        {
            base.OnAttached();
            AddMask();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            RemoveMask();
        }

        private void AddMask()
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
                            this.adorner = new MoveThumbAdorner(AssociatedObject);
                            adorner.MouseRightButtonDown += adorner_MouseRightButtonDown;
                            adornerLayer.Add(this.adorner);
                        }
                    }));

                });

            }
        }

 
     
        private void RemoveMask()
        {
            if (this.adorner != null)
            {

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);
                if (adornerLayer != null)
                {
                    adorner.MouseRightButtonDown -= adorner_MouseRightButtonDown;
                    adornerLayer.Remove(this.adorner);
                }

                this.adorner = null;
            }
        }


        void adorner_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MouseRightButtonDown!=null)
            {
                MouseRightButtonDown(AssociatedObject, e);
            }
        }
      

        public  void ClearDesignItemBehavior()
        {
            if (MoveThumb.dib != null)
            {
                MoveThumb.dib.CheckBehavior();
                MoveThumb.dib = null;
            }
        }
    }
}
