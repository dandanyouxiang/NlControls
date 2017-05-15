using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace NlControls.WPF.Controls.Book
{
    public partial class SingleBook : ItemsControl
    {
        #region private filed

        private PageStatus _status = PageStatus.None;
        private DataTemplate defaultDataTemplate;
        /// <summary>
        /// Gets or sets the DispatcherTimer 
        /// condition for auto completion.
        /// </summary>
        private DispatcherTimer _animateTimer;
        private bool _isAnimateStopFlag;
        #endregion

        #region Contructor

        public SingleBook()
        {
            Width = 300;
            Height = 200;
            InitializeComponent();
            Unloaded+=(s,e)=>{
                ClearAnimate();
            };
        }


        #endregion

        #region public property

        #region DisplayMode
        public static DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(BookDisplayMode), typeof(SingleBook), new PropertyMetadata(BookDisplayMode.Normal, new PropertyChangedCallback(OnDisplayModeChanged)));

        public BookDisplayMode DisplayMode
        {
            get { return (BookDisplayMode)GetValue(SingleBook.DisplayModeProperty); }
            set
            {
                if (!GetValue(SingleBook.DisplayModeProperty).Equals(value))
                    SetValue(SingleBook.DisplayModeProperty, value);
            }
        }
        private static void OnDisplayModeChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            BookDisplayMode mode = (BookDisplayMode)args.NewValue;
            SingleBook book = source as SingleBook;
            if (mode == BookDisplayMode.Normal)
            {
                //book.translate.X = 0;
                //book.scale.ScaleX = 1;
                //book.scale.ScaleY = 1;
                book.translate.BeginAnimation(TranslateTransform.XProperty, null);
                book.scale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            }
            else
            {
                if (book.CurrentPage == BookCurrentPage.LeftSheet)
                    book.AnimateToLeftSheet();
                else
                    book.AnimateToRightSheet();
            }
        }



        #endregion

        #region CurrentPage
        public static DependencyProperty CurrentPageProperty= DependencyProperty.Register("CurrentPage", typeof(BookCurrentPage), typeof(SingleBook), new PropertyMetadata(BookCurrentPage.RightSheet, new PropertyChangedCallback(OnCurrentPageChanged)));
        public BookCurrentPage CurrentPage
        {
            get { return (BookCurrentPage)GetValue(SingleBook.CurrentPageProperty); }
            set
            {
                if (!GetValue(SingleBook.CurrentPageProperty).Equals(value))
                    SetValue(SingleBook.CurrentPageProperty, value);
            }
        }
        private static void OnCurrentPageChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            BookCurrentPage currentPage = (BookCurrentPage)args.NewValue;
            SingleBook b = source as SingleBook;
            if (currentPage == BookCurrentPage.LeftSheet)
                b.AnimateToRightSheet();
            else
                b.AnimateToLeftSheet();
        }
        #endregion

        #region CurrentSheetIndex
        private int _currentSheetIndex = 0;
        public int CurrentSheetIndex
        {
            get { return _currentSheetIndex; }
            set
            {
                if (_status != PageStatus.None) return;

                if (_currentSheetIndex != value)
                {
                    if ((value >= 0) && (value <= GetItemsCount() / 2))
                    {
                        if (value==GetItemsCount()/2&&IsPlayCyclically)
                        {
                            _currentSheetIndex =  0;
                            
                        }
                        else
                        {
                            _currentSheetIndex = value;
                        }
                        RefreshSheetsContent();
                    }
                    else
                        throw new Exception("Index out of bounds");
                }
            }
        }
        #endregion

        #region IsPlayCyclically


        public bool IsPlayCyclically
        {
            get { return (bool)GetValue(IsPlayCyclicallyProperty); }
            set { SetValue(IsPlayCyclicallyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayCyclicallyProperty =
            DependencyProperty.Register("IsPlayCyclically", typeof(bool), typeof(SingleBook), new PropertyMetadata(false));

        

        #endregion

        #region TickerInterval


        public int TickerInterval
        {
            get { return (int)GetValue(TickerIntervalProperty); }
            set { SetValue(TickerIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickerIntervalProperty =
            DependencyProperty.Register("TickerInterval", typeof(int), typeof(SingleBook), new PropertyMetadata(20000));

        
        #endregion

        #region PauseOnHover

        public bool PauseOnHover
        {
            get { return (bool)GetValue(PauseOnHoverProperty); }
            set { SetValue(PauseOnHoverProperty, value); }
        }

        public static readonly DependencyProperty PauseOnHoverProperty =
            DependencyProperty.Register("PauseOnHover", typeof(bool), typeof(SingleBook), new PropertyMetadata(true));
        
        #endregion

        #region AutoPlay
         public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        public static readonly DependencyProperty AutoPlayProperty =
            DependencyProperty.Register("AutoPlay", typeof(bool), typeof(SingleBook), new PropertyMetadata(false));

        #endregion
        #endregion

        #region private helper methods
        protected virtual bool CheckCurrentSheetIndex()
        {
            return CurrentSheetIndex > (GetItemsCount() / 2);
        }
        private void AnimateToLeftSheet()
        {
            DoubleAnimation translateAnimation = new DoubleAnimation(Width / 4, new Duration(TimeSpan.FromMilliseconds(500)));

            BezierSegment bs =
                new BezierSegment(new Point(0, 1), new Point(1, 0.5), new Point(2, 1), true);

            PathGeometry path = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(0, 1);
            figure.Segments.Add(bs);
            figure.IsClosed = false;
            path.Figures.Add(figure);

            DoubleAnimationUsingPath scaleAnimation = new DoubleAnimationUsingPath();
            scaleAnimation.PathGeometry = path;
            scaleAnimation.Source = PathAnimationSource.Y;
            scaleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            translate.BeginAnimation(TranslateTransform.XProperty, translateAnimation);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        private void AnimateToRightSheet()
        {
            DoubleAnimation translateAnimation = new DoubleAnimation(-Width / 4, new Duration(TimeSpan.FromMilliseconds(500)));

            BezierSegment bs =
                new BezierSegment(new Point(0, 1), new Point(1, 0.5), new Point(2, 1), true);

            PathGeometry path = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(0, 1);
            figure.Segments.Add(bs);
            figure.IsClosed = false;
            path.Figures.Add(figure);

            DoubleAnimationUsingPath scaleAnimation = new DoubleAnimationUsingPath();
            scaleAnimation.PathGeometry = path;
            scaleAnimation.Source = PathAnimationSource.Y;
            scaleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            translate.BeginAnimation(TranslateTransform.XProperty, translateAnimation);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        private void RefreshSheetsContent()
        {



            BookPage bp1 = GetTemplateChild("sheet1") as BookPage;
            if (bp1 == null)
                return;



            ContentPresenter sheet1Page0Content = bp1.FindName("page0") as ContentPresenter;
            ContentPresenter sheet1Page1Content = bp1.FindName("page1") as ContentPresenter;
            ContentPresenter sheet1Page2Content = bp1.FindName("page2") as ContentPresenter;

       
            Visibility bp1Visibility = Visibility.Visible;

            bp1.IsTopRightCornerEnabled = true;
            bp1.IsBottomRightCornerEnabled = true;

            Visibility sheet1Page0ContentVisibility = Visibility.Visible;
            Visibility sheet1Page1ContentVisibility = Visibility.Visible;
            Visibility sheet1Page2ContentVisibility = Visibility.Visible;

            DataTemplate dt = ItemTemplate;
            if (dt == null)
                dt = defaultDataTemplate;

            sheet1Page0Content.ContentTemplate = dt;
            sheet1Page1Content.ContentTemplate = dt;
            sheet1Page2Content.ContentTemplate = dt;

            int count = GetItemsCount();
            int sheetCount = count / 2;
            bool isOdd = (count % 2) == 1;

            if (_currentSheetIndex == sheetCount && !IsPlayCyclically)
            {
                if (isOdd)
                {
                    bp1.IsTopRightCornerEnabled = false;
                    bp1.IsBottomRightCornerEnabled = false;
                }
                else
                    bp1Visibility = Visibility.Hidden;
            }

            if (_currentSheetIndex == sheetCount - 1 && !IsPlayCyclically)
            {
                 if (!isOdd)
                        sheet1Page2ContentVisibility = Visibility.Hidden;                            
            }           
            sheet1Page0Content.Content = GetPage(2 * CurrentSheetIndex);
            sheet1Page1Content.Content = GetPage(2 * CurrentSheetIndex + 1);
            sheet1Page2Content.Content = GetPage(2 * CurrentSheetIndex + 2);

            bp1.Visibility = bp1Visibility;
            sheet1Page0Content.Visibility = sheet1Page0ContentVisibility;
            sheet1Page1Content.Visibility = sheet1Page1ContentVisibility;
            sheet1Page2Content.Visibility = sheet1Page2ContentVisibility;

        }

        internal object GetPage(int index)
        {
            if ((index >= 0) && (index < Items.Count))
                return Items[index];
            if (index==Items.Count&&IsPlayCyclically)
            {
                return Items[0];
            }
            Canvas c = new Canvas();
           // c.Background = Brushes.Yellow;

            return c;
        }

     
        #endregion

        #region override methods

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (CheckCurrentSheetIndex())
            {
                CurrentSheetIndex = GetItemsCount() / 2;
            }
            else
                RefreshSheetsContent();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (CheckCurrentSheetIndex())
            {
                CurrentSheetIndex = GetItemsCount() / 2;
            }
            else
                RefreshSheetsContent();
        }
        #endregion

        #region Animate Methods
        public void Animate()
        {
            ClearAnimate();
            if (TickerInterval <= 0)
            {
                TickerInterval = 20000;
            }
            _animateTimer = new DispatcherTimer();
            _animateTimer.Interval = TimeSpan.FromMilliseconds(TickerInterval);
            _animateTimer.Tick += _animateTimer_Tick;
            _animateTimer.Start();
        }
        private void ClearAnimate()
        {
            if (_animateTimer != null)
            {
                _animateTimer.Stop();
                _animateTimer.Tick -= _animateTimer_Tick;
                _animateTimer = null;
            }
        }
        private void _animateTimer_Tick(object sender, EventArgs e)
        {
            if (!(PauseOnHover && IsMouseOver))
            {
                OnPause();
                AnimateToNextPage(true, 800);
              //  Focus();
                OnResume();
            }
        }
        public void OnStop()
        {
            ClearAnimate();
        }

        public void OnPause()
        {
            if (_animateTimer != null)
                _animateTimer.Stop();
        }

        public void OnResume()
        {
            if (_animateTimer != null)
                _animateTimer.Start();
        }
        #endregion

        #region public methods
        
        public void MoveToNextPage()
        {
            if (CurrentPage == BookCurrentPage.LeftSheet)
                AnimateToNextPage(false, 500);
            CurrentPage =
                CurrentPage == BookCurrentPage.LeftSheet ?
                    BookCurrentPage.RightSheet : BookCurrentPage.LeftSheet;
        }

        public void MoveToPreviousPage()
        {
            if (CurrentPage == BookCurrentPage.RightSheet)
                AnimateToPreviousPage();
            CurrentPage =
                CurrentPage == BookCurrentPage.LeftSheet ?
                    BookCurrentPage.RightSheet : BookCurrentPage.LeftSheet;
        }
        public int GetItemsCount()
        {
            if (ItemsSource != null)
            {
                if (ItemsSource is ICollection)
                    return (ItemsSource as ICollection).Count;
                int count = 0;
                foreach (object o in ItemsSource) count++;
                return count;
            }
            return Items.Count;
        }


        public void AnimateToNextPage(bool fromTop, int duration)
        {
            if (CurrentSheetIndex + 1 <= GetItemsCount() / 2)
            {   
                BookPage bp1 = GetTemplateChild("sheet1") as BookPage;

                if (bp1 == null)
                    return;
                Canvas.SetZIndex((bp1 as BookPage), 1);
                bp1.AutoTurnPage(fromTop ? CornerOrigin.TopLeft : CornerOrigin.BottomLeft, duration);
            }
        }

        public void AnimateToPreviousPage()
        {
            if (CurrentSheetIndex > 0)
            {
                BookPage bp1 = GetTemplateChild("sheet1") as BookPage;

                if ( (bp1 == null))
                    return;

                Canvas.SetZIndex((bp1 as BookPage), 0);
                CurrentSheetIndex--;
               
            }
        }

        #endregion

        #region evnet handler
        private void OnLoaded(object sender, RoutedEventArgs args)
        {

            BookPage bp1 = GetTemplateChild("sheet1") as BookPage;

            if (bp1 == null)
                return;

            defaultDataTemplate = (DataTemplate)Resources["defaultDataTemplate"];
            Read<PageStatus> GetStatus = delegate() { return _status; };
            Action<PageStatus> SetStatus = delegate(PageStatus ps) { _status = ps; };
 
            bp1.GetStatus += GetStatus;
            bp1.SetStatus += SetStatus;

            RefreshSheetsContent();
            if (AutoPlay)
            {
                Animate();
            }
            
        }


        private void OnRightMouseDown(object sender, MouseButtonEventArgs args)
        {
        
            BookPage bp1 = GetTemplateChild("sheet1") as BookPage;
 
            Canvas.SetZIndex((bp1 as BookPage), 1);
        }
        private void OnLeftPageTurned(object sender, RoutedEventArgs args)
        {
            CurrentSheetIndex--;
        }
        private void OnRightPageTurned(object sender, RoutedEventArgs args)
        {
            CurrentSheetIndex++;
        }
        #endregion
    }

}
