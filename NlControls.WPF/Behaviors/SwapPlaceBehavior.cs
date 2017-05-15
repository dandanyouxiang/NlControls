using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace NlControls.WPF.Behaviors
{
    public class SwapPlaceBehavior : Behavior<FrameworkElement>
    {
        private Button swapButton;
        private event EventHandler _swapCompleted;
        private event EventHandler _swapStarting;
        #region Properties





        public FrameworkElement TargetElement
        {
            get { return (FrameworkElement)GetValue(TargetElementProperty); }
            set { SetValue(TargetElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register("TargetElement", typeof(FrameworkElement), typeof(SwapPlaceBehavior), new PropertyMetadata(null));




        public string TargetElementName
        {
            get { return (string)GetValue(TargetElementNameProperty); }
            set { SetValue(TargetElementNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetElementName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetElementNameProperty =
            DependencyProperty.Register("TargetElementName", typeof(string), typeof(SwapPlaceBehavior), new PropertyMetadata(null));




        public string TriggerButtonName
        {
            get { return (string)GetValue(TriggerButtonNameProperty); }
            set { SetValue(TriggerButtonNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TriggerButtonName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TriggerButtonNameProperty =
            DependencyProperty.Register("TriggerButtonName", typeof(string), typeof(SwapPlaceBehavior), new PropertyMetadata(null));






        public event EventHandler SwapStarting
        {
            add
            {
                _swapStarting += value;
            }
            remove
            {
                _swapStarting -= value;
            }
        }



        public event EventHandler SwapCompleted
        {
            add
            {
                _swapCompleted += value;
            }
            remove
            {
                _swapCompleted -= value;
            }
        }
        #endregion

        #region Override Methods


        protected override void OnAttached()
        {
            base.OnAttached();
            AttachSwapButton(TriggerButtonName);



        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            DetachSwapButton();

        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (AssociatedObject != null && (e.Property == TriggerButtonNameProperty))
            {
                DetachSwapButton();
                if (e.Property == TriggerButtonNameProperty)
                {
                    string btnnewName = e.NewValue as string;
                    if (btnnewName != null)
                    {
                        AttachSwapButton(btnnewName);
                    }
                }

            }

        }

        #endregion

        #region SwapButton Methods

        private void AttachSwapButton(string name)
        {
            if (name != null)
            {
                swapButton = AssociatedObject.FindName(name) as Button;
            }
            if (swapButton != null)
                swapButton.Click += swapButton_Click;
        }

        private void DetachSwapButton()
        {
            if (swapButton != null)
            {
                swapButton.Click -= swapButton_Click;
                swapButton = null;
            }
        }

        void swapButton_Click(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject != null && (TargetElement != null || TargetElementName != null))
            {
                Panel panel = AssociatedObject.Parent as Panel;
                if (TargetElement == null)
                    TargetElement = panel.FindName(TargetElementName) as FrameworkElement;
                if (panel == null && TargetElement != null && panel != TargetElement.Parent)
                {
                    return;
                }
                OnHideAnimationStarting();
                SwapHideStoryBegin(AssociatedObject, TargetElement);
            }
        }

        #endregion

        #region Storyboard Methods

        private void SwapHideStoryBegin(FrameworkElement sourceUI, FrameworkElement targetUI)
        {
            sourceUI.RenderTransform = new ScaleTransform();
            sourceUI.RenderTransformOrigin = new Point(0.5, 0.5);
            targetUI.RenderTransform = new ScaleTransform();
            targetUI.RenderTransformOrigin = new Point(0.5, 0.5);
            Storyboard hides = new Storyboard();

            DoubleAnimation scalex1 = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            Storyboard.SetTarget(scalex1, sourceUI);
            Storyboard.SetTargetProperty(scalex1, new PropertyPath("(FrameworkElement.RenderTransform).(ScaleTransform.ScaleX)"));


            DoubleAnimation scalex2 = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            Storyboard.SetTarget(scalex2, targetUI);
            Storyboard.SetTargetProperty(scalex2, new PropertyPath("(FrameworkElement.RenderTransform).(ScaleTransform.ScaleX)"));

            hides.Children.Add(scalex1);
            hides.Children.Add(scalex2);
            hides.Completed += hides_Completed;

            hides.Begin();
        }

        void hides_Completed(object sender, EventArgs e)
        {
            SwapAssociateObjectAndTarget();
            SwapShowStoryBegin(AssociatedObject, TargetElement);
        }

        private void SwapShowStoryBegin(FrameworkElement sourceUI, FrameworkElement targetUI)
        {
            BounceEase bounceease = new BounceEase();
            bounceease.Bounces = 1;
            bounceease.Bounciness = 10;
            bounceease.EasingMode = EasingMode.EaseOut;
            bounceease.Freeze();
            Storyboard shows = new Storyboard();

            DoubleAnimation scalex1 = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            Storyboard.SetTarget(scalex1, sourceUI);
            Storyboard.SetTargetProperty(scalex1, new PropertyPath("(FrameworkElement.RenderTransform).(ScaleTransform.ScaleX)"));

            scalex1.EasingFunction = bounceease;

            DoubleAnimation scalex2 = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            Storyboard.SetTarget(scalex2, targetUI);
            Storyboard.SetTargetProperty(scalex2, new PropertyPath("(FrameworkElement.RenderTransform).(ScaleTransform.ScaleX)"));
            scalex2.EasingFunction = bounceease;
            shows.Children.Add(scalex1);
            shows.Children.Add(scalex2);
            shows.Completed += shows_Completed;
            shows.Begin();
        }

        void shows_Completed(object sender, EventArgs e)
        {
            OnShowAnimationCompleted();
        }

        #endregion

        #region Helper Methods
        private void OnHideAnimationStarting()
        {
            if (_swapStarting != null)
            {
                _swapStarting(this, new EventArgs());
            }
        }
        private void OnShowAnimationCompleted()
        {
            if (_swapCompleted != null)
            {
                _swapCompleted(this, new EventArgs());
            }
        }

        private void SwapAssociateObjectAndTarget()
        {
            double cw = AssociatedObject.Width;
            double ch = AssociatedObject.Height;
            Thickness ct = AssociatedObject.Margin;
            HorizontalAlignment cha = AssociatedObject.HorizontalAlignment;
            VerticalAlignment cva = AssociatedObject.VerticalAlignment;
            double ctop = Canvas.GetTop(AssociatedObject);
            double cleft = Canvas.GetLeft(AssociatedObject);
            AssociatedObject.Width = TargetElement.ActualWidth;
            AssociatedObject.Height = TargetElement.ActualHeight;
            AssociatedObject.Margin = TargetElement.Margin;
            AssociatedObject.HorizontalAlignment = TargetElement.HorizontalAlignment;
            AssociatedObject.VerticalAlignment = TargetElement.VerticalAlignment;
            TargetElement.Width = cw;
            TargetElement.Height = ch;
            TargetElement.Margin = ct;
            TargetElement.HorizontalAlignment = cha;
            TargetElement.VerticalAlignment = cva;
            if (!double.IsNaN(ctop) && !double.IsNaN(cleft))
            {
                Canvas.SetTop(TargetElement, ctop);
                Canvas.SetLeft(TargetElement, cleft);
            }
        }
        #endregion
    }
}
