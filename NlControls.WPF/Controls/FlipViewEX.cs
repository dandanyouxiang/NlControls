using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace NlControls.WPF.Controls
{

    // [TemplatePart(Name = "PART_Presenter", Type = typeof(TransitioningContentControl))]
    [TemplatePart(Name = "PART_BackButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_ForwardButton", Type = typeof(RepeatButton))]
    public class FlipViewEX : Selector
    {

        private const string PART_Presenter = "PART_Presenter";
        private const string PART_BackButton = "PART_BackButton";
        private const string PART_ForwardButton = "PART_ForwardButton";
        private const string PART_Scroller = "PART_Scroller";

        public static readonly DependencyProperty ItemGroupNameProperty =
            DependencyProperty.Register("ItemGroupName", typeof(string), typeof(FlipViewEX), new PropertyMetadata(""));
        /// <summary>
        /// 集合组名称,用于当模板里面有RadioButton时使用，其它时候请忽略
        /// </summary>
        public string ItemGroupName
        {
            get { return (string)GetValue(ItemGroupNameProperty); }
            set { SetValue(ItemGroupNameProperty, value); }
        }

        //  private TransitioningContentControl presenter;
        private RepeatButton backButton;
        private RepeatButton forwardButton;
        private ScrollViewer Scroller;

        private Storyboard hideControlStoryboard;
        private Storyboard showControlStoryboard;

        /// <summary>
        /// To counteract the double Loaded event issue.
        /// </summary>
        private bool loaded;

        static FlipViewEX()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipViewEX), new FrameworkPropertyMetadata(typeof(FlipViewEX)));
        }

        public FlipViewEX()
        {
            this.Unloaded += FlipViewEX_Unloaded;
            this.Loaded += FlipViewEX_Loaded;
            this.MouseLeftButtonDown += FlipViewEX_MouseLeftButtonDown;
        }

        void FlipViewEX_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is FlipViewEXItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {

            return new FlipViewEXItem() { HorizontalAlignment = HorizontalAlignment.Stretch };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (element != item)
                element.SetCurrentValue(DataContextProperty, item); //dont want to set the datacontext to itself. taken from MetroTabControl.cs
            if (ItemContainerStyle != null && element != null)
            {
                element.SetCurrentValue(Control.StyleProperty, ItemContainerStyle);
            }
            base.PrepareContainerForItemOverride(element, item);
        }





        void FlipViewEX_Loaded(object sender, RoutedEventArgs e)
        {

            if (backButton == null || forwardButton == null) //OnApplyTemplate hasn't been called yet.
                ApplyTemplate();

            if (loaded) return; //Counteracts the double 'Loaded' event issue.

            backButton.Click += backButton_Click;
            forwardButton.Click += forwardButton_Click;
            Scroller.ScrollChanged += Scroller_ScrollChanged;

            this.PreviewKeyDown += FlipViewEX_PreviewKeyDown;

            SelectedIndex = 0;

            loaded = true;
        }



        void FlipViewEX_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= FlipViewEX_Unloaded;
            this.MouseLeftButtonDown -= FlipViewEX_MouseLeftButtonDown;

            this.PreviewKeyDown -= FlipViewEX_PreviewKeyDown;
            backButton.Click -= backButton_Click;
            forwardButton.Click -= forwardButton_Click;
            Scroller.ScrollChanged -= Scroller_ScrollChanged;
            loaded = false;
        }

        void Scroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            DetachButtonStatus();
        }
        void FlipViewEX_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    GoBack();
                    e.Handled = true;
                    break;
                case Key.Right:
                    GoForward();
                    e.Handled = true;
                    break;
            }

            if (e.Handled)
                this.Focus();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            showControlStoryboard = ((Storyboard)this.Template.Resources["ShowControlStoryboard"]).Clone();
            hideControlStoryboard = ((Storyboard)this.Template.Resources["HideControlStoryboard"]).Clone();

            //  presenter = GetTemplateChild(PART_Presenter) as TransitioningContentControl;
            backButton = GetTemplateChild(PART_BackButton) as RepeatButton;
            forwardButton = GetTemplateChild(PART_ForwardButton) as RepeatButton;
            Scroller = GetTemplateChild(PART_Scroller) as ScrollViewer;
        }

        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            SelectedIndex = 0;
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            DetachButtonStatus();
        }

        void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            GoForward();
        }

        void backButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        public void GoBack()
        {

            Scroller.PageLeft();
        }

        public void GoForward()
        {

            Scroller.PageRight();
        }


        private void DetachButtonStatus()
        {
            if (Scroller == null) return;
            if (Scroller.HorizontalOffset == Scroller.ScrollableWidth && Scroller.HorizontalOffset != 0)
            {
                backButton.Visibility = Visibility.Visible;
                forwardButton.Visibility = Visibility.Visible;
                backButton.IsEnabled = true;
                forwardButton.IsEnabled = false;
            }
            else if (Scroller.HorizontalOffset != Scroller.ScrollableWidth && Scroller.HorizontalOffset == 0)
            {
                backButton.Visibility = Visibility.Visible;
                forwardButton.Visibility = Visibility.Visible;
                backButton.IsEnabled = false;
                forwardButton.IsEnabled = true;
            }
            else if (Scroller.HorizontalOffset == Scroller.ScrollableWidth && Scroller.HorizontalOffset == 0)
            {
                backButton.Visibility = Visibility.Hidden;
                forwardButton.Visibility = Visibility.Hidden;
                //backButton.IsEnabled = false;
                //forwardButton.IsEnabled = true;
            }
            else
            {
                backButton.Visibility = Visibility.Visible;
                forwardButton.Visibility = Visibility.Visible;
                backButton.IsEnabled = true;
                forwardButton.IsEnabled = true;
            }
        }

    }
}
