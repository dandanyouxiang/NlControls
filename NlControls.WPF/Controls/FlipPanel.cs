using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace NlControls.WPF.Controls
{
    [TemplatePart(Name = "FlipButton", Type = typeof(ToggleButton)),
    TemplatePart(Name = "PART_FrontButton", Type = typeof(RadioButton)),
    TemplatePart(Name = "PART_BackButton", Type = typeof(RadioButton)),
    TemplatePart(Name = "FlipButtonAlternate", Type = typeof(ToggleButton)),
    TemplateVisualState(Name = "Normal", GroupName = "ViewStates"),
    TemplateVisualState(Name = "Flipped", GroupName = "ViewStates")]
    public class FlipPanel : Control
    {

        private RadioButton frontBtn;
        private RadioButton backBtn;


        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent", typeof(object), typeof(FlipPanel), null);
       
        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent", typeof(object), typeof(FlipPanel), null);
       
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(FlipPanel), null);
       
        public static readonly DependencyProperty IsFlippedProperty =
            DependencyProperty.Register("IsFlipped", typeof(bool), typeof(FlipPanel), new FrameworkPropertyMetadata(false, OnIsFlippedChanged));
        

        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register("HeaderForeground", typeof(Brush), typeof(FlipPanel), new PropertyMetadata(Brushes.Transparent));
       
        public static readonly DependencyProperty FrontHeaderProperty =
            DependencyProperty.Register("FrontHeader", typeof(string), typeof(FlipPanel), new PropertyMetadata("FrontHeader"));
        
        public static readonly DependencyProperty BackHeaderProperty =
            DependencyProperty.Register("BackHeader", typeof(string), typeof(FlipPanel), new PropertyMetadata("BackHeader"));
       
        public static readonly DependencyProperty HeaderCheckedBrushProperty =
             DependencyProperty.Register("HeaderCheckedBrush", typeof(Brush), typeof(FlipPanel), new PropertyMetadata(Brushes.AliceBlue));
      
        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(FlipPanel), new PropertyMetadata(Brushes.Transparent));
        public static readonly RoutedEvent FlipChangedEvent =
            EventManager.RegisterRoutedEvent("FlipChanged", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(FlipPanel));
        

        public string FrontHeader
        {
            get { return (string)GetValue(FrontHeaderProperty); }
            set { SetValue(FrontHeaderProperty, value); }
        }

        public string BackHeader
        {
            get { return (string)GetValue(BackHeaderProperty); }
            set { SetValue(BackHeaderProperty, value); }
        }

        public Brush HeaderCheckedBrush
        {
            get { return (Brush)GetValue(HeaderCheckedBrushProperty); }
            set { SetValue(HeaderCheckedBrushProperty, value); }
        }

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public object FrontContent
        {
            get
            {
                return GetValue(FrontContentProperty);
            }
            set
            {
                SetValue(FrontContentProperty, value);
            }
        }

        public object BackContent
        {
            get
            {
                return GetValue(BackContentProperty);
            }
            set
            {
                SetValue(BackContentProperty, value);
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        public bool IsFlipped
        {
            get
            {
                return (bool)GetValue(IsFlippedProperty);
            }
            set
            {
                SetValue(IsFlippedProperty, value);

                ChangeVisualState(true);
            }
        }




        public event RoutedEventHandler FlipChanged
        {
            add { this.AddHandler(FlipChangedEvent, value); }
            remove { this.RemoveHandler(FlipChangedEvent, value); }
        }



        static FlipPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipPanel), new FrameworkPropertyMetadata(typeof(FlipPanel)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            frontBtn = GetTemplateChild("PART_FrontButton") as RadioButton;
            if (frontBtn != null) frontBtn.Checked += RadioButton_Checked;
            backBtn = GetTemplateChild("PART_BackButton") as RadioButton;
            if (backBtn != null) backBtn.Checked += RadioButton_Checked;
            this.ChangeVisualState(false);
        }


        private static void OnIsFlippedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlipPanel panel = d as FlipPanel;
            RoutedEventArgs arg = new RoutedEventArgs(FlipChangedEvent, panel);
            panel.RaiseEvent(arg);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            string btnTag = ((RadioButton)sender).Tag.ToString();
            if (btnTag == "Back" && IsFlipped == false)
            {

                IsFlipped = true;
            }
            else if (btnTag == "Front" && IsFlipped == true)
            {

                IsFlipped = false;
            }
        }

        private void flipButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsFlipped = !this.IsFlipped;
        }



        private void ChangeVisualState(bool useTransitions)
        {
            frontBtn.Background = HeaderBackground;
            backBtn.Background = HeaderBackground;
            if (!this.IsFlipped)
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Flipped", useTransitions);
            }

            // Disable flipped side to prevent tabbing to invisible buttons.            
            UIElement front = FrontContent as UIElement;
            if (front != null)
            {
                if (IsFlipped)
                {
                    front.Visibility = Visibility.Hidden;
                    backBtn.Foreground = HeaderCheckedBrush;
                    backBtn.FontWeight = FontWeights.Bold;
                    frontBtn.Foreground = HeaderForeground;
                    frontBtn.FontWeight = FontWeights.Normal;
                }
                else
                {
                    front.Visibility = Visibility.Visible;
                    backBtn.Foreground = HeaderForeground;
                    backBtn.FontWeight = FontWeights.Normal;
                    frontBtn.Foreground = HeaderCheckedBrush;
                    frontBtn.FontWeight = FontWeights.Bold;
                }
            }
            UIElement back = BackContent as UIElement;
            if (back != null)
            {
                if (IsFlipped)
                {
                    back.Visibility = Visibility.Visible;
                }
                else
                {
                    back.Visibility = Visibility.Hidden;
                }
            }
        }

    }

}
