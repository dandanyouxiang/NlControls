
using NlControls.WPF.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace NlControls.WPF.Controls
{
    public enum AnimateDirection
    {
        Up,
        Down


    }
    [TemplatePart(Name = NewsFlowList.ElementSelector, Type = typeof(Selector))]
    public class NewsFlowList : Control
    {
        #region Template part and style names


        /// <summary>
        /// Specifies the name of the Selector TemplatePart.
        /// </summary>
        private const string ElementSelector = "Selector";


        /// <summary>
        /// The name for the adapter's item container style.
        /// </summary>
        private const string ElementItemContainerStyle = "ItemContainerStyle";

        #endregion

        /// <summary>
        /// Gets or sets a local cached copy of the items data.
        /// </summary>
        private List<object> _items;

        /// <summary>
        /// Gets or sets the observable collection that contains references to 
        /// </summary>
        private ObservableCollection<object> _view;

        /// <summary>
        /// Gets or sets the DispatcherTimer 
        /// condition for auto completion.
        /// </summary>
        private DispatcherTimer _animateTimer;

        /// <summary>
        /// A weak event listener for the collection changed event.
        /// </summary>
        private WeakEventListener<NewsFlowList, object, NotifyCollectionChangedEventArgs> _collectionChangedWeakEventListener;

        #region Template parts

        private Selector _listSelector;
        #endregion

        #region Animate private filed
        private bool _isItemsUpdate = false;
        private bool _animationStarted = false;
        #endregion

        #region Animate Public Property


        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        public static readonly DependencyProperty AutoPlayProperty =
            DependencyProperty.Register("AutoPlay", typeof(bool), typeof(NewsFlowList), new PropertyMetadata(false));



        public bool PauseOnHover
        {
            get { return (bool)GetValue(PauseOnHoverProperty); }
            set { SetValue(PauseOnHoverProperty, value); }
        }

        public static readonly DependencyProperty PauseOnHoverProperty =
            DependencyProperty.Register("PauseOnHover", typeof(bool), typeof(NewsFlowList), new PropertyMetadata(true));



        public AnimateDirection Direction { get; set; }

        public int NewsTickerInterval { get; set; }


        public int NewsPerPage
        {
            get { return (int)GetValue(NewsPerPageProperty); }
            set { SetValue(NewsPerPageProperty, value); }
        }

        public static readonly DependencyProperty NewsPerPageProperty =
            DependencyProperty.Register("NewsPerPage", typeof(int), typeof(NewsFlowList), new PropertyMetadata(4));




        #endregion

        #region Public int AlternationCount


        public int AlternationCount
        {
            get { return (int)GetValue(AlternationCountProperty); }
            set { SetValue(AlternationCountProperty, value); }
        }
        public static readonly DependencyProperty AlternationCountProperty =
            DependencyProperty.Register("AlternationCount", typeof(int), typeof(NewsFlowList), new PropertyMetadata(0));


        #endregion

        #region Public int AnimationIndex   

        private static readonly DependencyPropertyKey AnimationIndexPropertyKey =
            DependencyProperty.RegisterReadOnly("AnimationIndex", typeof(int), typeof(NewsFlowList), new PropertyMetadata(0));

        public static readonly DependencyProperty AnimationIndexProperty =
           AnimationIndexPropertyKey.DependencyProperty;

        public int AnimationIndex
        {
            get { return (int)GetValue(AnimationIndexProperty); }
            private set { SetValue(AnimationIndexPropertyKey, value); }
        }
        #endregion        

        #region public DataTemplate ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(NewsFlowList), new PropertyMetadata(null));

        #endregion

        #region public Style ItemContainerStyle


        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        public static readonly DependencyProperty ItemContainerStyleProperty =
            DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(NewsFlowList), new PropertyMetadata(null, null));



        #endregion

        #region public IEnumerable ItemsSource

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(NewsFlowList), new PropertyMetadata(OnItemsSourcePropertyChanged));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NewsFlowList newslist = d as NewsFlowList;
            newslist.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            OnPause();
            _isItemsUpdate = true;
            AnimationIndex = 0;
            // Remove handler for oldValue.CollectionChanged (if present)
            INotifyCollectionChanged oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;
            if (null != oldValueINotifyCollectionChanged && null != _collectionChangedWeakEventListener)
            {
                _collectionChangedWeakEventListener.Detach();
                _collectionChangedWeakEventListener = null;
            }

            // Add handler for newValue.CollectionChanged (if possible)
            INotifyCollectionChanged newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (null != newValueINotifyCollectionChanged)
            {
                _collectionChangedWeakEventListener = new WeakEventListener<NewsFlowList, object, NotifyCollectionChangedEventArgs>(this);
                _collectionChangedWeakEventListener.OnEventAction = (instance, source, eventArgs) => instance.ItemsSourceCollectionChanged(source, eventArgs);
                _collectionChangedWeakEventListener.OnDetachAction = (weakEventListener) => newValueINotifyCollectionChanged.CollectionChanged -= weakEventListener.OnEvent;
                newValueINotifyCollectionChanged.CollectionChanged += _collectionChangedWeakEventListener.OnEvent;
            }

            // Store a local cached copy of the data
            _items = newValue == null ? null : new List<object>(newValue.Cast<object>().ToList());

            // Clear and set the view on the selection adapter
            ClearView();
            if (_listSelector != null && _listSelector.ItemsSource != _view)
            {
                _listSelector.ItemsSource = _view;
            }
            RefreshView();
            OnResume();
            if (_animateTimer == null && AutoPlay)
            {
                Animate();
            }
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
           {
               OnPause();
               _isItemsUpdate = true;
               // Update the cache
               if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
               {
                   for (int index = 0; index < e.OldItems.Count; index++)
                   {
                       _items.RemoveAt(e.OldStartingIndex);
                   }
               }
               if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null && _items.Count >= e.NewStartingIndex)
               {
                   for (int index = 0; index < e.NewItems.Count; index++)
                   {
                       _items.Insert(e.NewStartingIndex + index, e.NewItems[index]);
                   }
               }
               if (e.Action == NotifyCollectionChangedAction.Replace && e.NewItems != null && e.OldItems != null)
               {
                   for (int index = 0; index < e.NewItems.Count; index++)
                   {
                       _items[e.NewStartingIndex] = e.NewItems[index];
                   }
               }

               if (e.Action == NotifyCollectionChangedAction.Reset)
               {
                   // Significant changes to the underlying data.

                   ClearView();
                   if (ItemsSource != null)
                   {
                       _items = new List<object>(ItemsSource.Cast<object>().ToList());
                   }


               }

               // Refresh the observable collection 
               RefreshView();
               OnResume();
           }));

        }
        #endregion

        #region public object SelectedItem


        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(NewsFlowList), new PropertyMetadata(OnSelectedItemPropertyChanged));

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }



        #endregion

        #region SelectionChanged

        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(NewsFlowList));


        public event SelectionChangedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }
        #endregion
        static NewsFlowList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NewsFlowList), new FrameworkPropertyMetadata(typeof(NewsFlowList)));
        }

        public NewsFlowList()
        {
            Unloaded += NewsFlowList_Unloaded;
        }

        void NewsFlowList_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearAnimate();
            if (_listSelector != null)
            {
                _listSelector.SelectionChanged -= _listSelector_SelectionChanged;
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _listSelector = GetTemplateChild(ElementSelector) as Selector;
            if (_listSelector != null)
            {
                _listSelector.SelectionChanged += _listSelector_SelectionChanged;
                _listSelector.ItemsSource = _view;
                if (AutoPlay && _listSelector.ItemsSource != null)
                {
                    Animate();
                }
            }
        }
        #region Control Event Handler

        void _listSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems == null || e.AddedItems.Count < 1) return;
            SelectedItem = e.AddedItems[0];
            SelectionChangedEventArgs newe = new SelectionChangedEventArgs(SelectionChangedEvent, e.RemovedItems, e.AddedItems);
            RaiseEvent(newe);
        }
        #endregion

        #region Helper Methods

        private void ClearView()
        {

            if (_view == null)
            {
                _view = new ObservableCollection<object>();
            }
            else
            {
                _view.Clear();
            }
        }

        private void RefreshView()
        {
            if (_items == null)
            {
                ClearView();
                return;
            }
            int view_index = 0;
            int view_count = _view.Count;
            List<object> items = _items;
            foreach (object item in items)
            {

                if (view_count > view_index && _view[view_index] == item)
                {
                    // Item is still in the view
                    view_index++;
                }
                else if (view_count > view_index && _view[view_index] == item)
                {

                    // Remove the item
                    _view.RemoveAt(view_index);
                    view_count--;
                }
                else
                {
                    // Insert the item
                    if (view_count > view_index && _view[view_index] != item)
                    {
                        // Replace item
                        // Unfortunately replacing via index throws a fatal 
                        // exception: View[view_index] = item;
                        // Cost: O(n) vs O(1)
                        _view.RemoveAt(view_index);
                        _view.Insert(view_index, item);
                        view_index++;
                    }
                    else
                    {
                        // Add the item
                        if (view_index == view_count)
                        {
                            // Constant time is preferred (Add).
                            _view.Add(item);
                        }
                        else
                        {
                            _view.Insert(view_index, item);
                        }
                        view_index++;
                        view_count++;
                    }
                }
            }
            while (items.Count < _view.Count)
            {
                _view.RemoveAt(_view.Count - 1);
            }
        }

        #endregion

        #region Aminate Methods

        public void Animate()
        {
            ClearAnimate();
            if (NewsTickerInterval <= 0)
            {
                NewsTickerInterval = 1000;
            }
            _animateTimer = new DispatcherTimer();
            _animateTimer.Interval = TimeSpan.FromMilliseconds(NewsTickerInterval);
            _animateTimer.Tick += _animateTimer_Tick;
            _animateTimer.Start();
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
        public void OnReset()
        {
            Animate(); 
        }

        public void OnPrevious()
        {
            if (_view == null || _view.Count <= NewsPerPage) return;
            if (_animationStarted)
            {
                return;
            }
            _animationStarted = true;
            ListBoxItem element = (ListBoxItem)_listSelector.ItemContainerGenerator.ContainerFromItem(_view.FirstOrDefault());
            double itemheight = 0;
            if (element != null)
            {
                itemheight = element.ActualHeight + element.Margin.Top + element.Margin.Bottom;
                if (itemheight == 0)
                {
                    itemheight = 20;
                }
            }
            else
            {
                return;
            }
            _view.Move(_view.Count - 1, 0);
            //TODO: need to optimize the animation

            ListBoxItem firstelement = (ListBoxItem)_listSelector.ItemContainerGenerator.ContainerFromItem(_view.FirstOrDefault());
            DoubleAnimation heightAnimation = new DoubleAnimation(0, itemheight, TimeSpan.FromSeconds(0.5));
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            heightAnimation.Completed += (s, e) =>
            {
                _animationStarted = false;
            };
            firstelement.BeginAnimation(ListBoxItem.HeightProperty, heightAnimation);
            if (_isItemsUpdate)
            {
                _isItemsUpdate = false;
            }
        }


        public void OnNext()
        {
            if (_view == null || _view.Count <= NewsPerPage) return;
            if (_animationStarted)
            {
                return;
            }
            _animationStarted = true;
            ListBoxItem element = (ListBoxItem)_listSelector.ItemContainerGenerator.ContainerFromItem(_view.FirstOrDefault()); 
            double itemheight = 0;
            if (element != null)
            {
                itemheight = element.ActualHeight + element.Margin.Top + element.Margin.Bottom;
                if (itemheight == 0)
                {
                    itemheight = 20;
                }
            }
            else
            {
                return;
            }

            List<object> itemscache = new List<object>(_listSelector.ItemsSource.Cast<object>().ToList());
            _view.Add(itemscache[0]);
            ListBoxItem lastElement = (ListBoxItem)_listSelector.ItemContainerGenerator.ContainerFromItem(itemscache[0]);
            if (lastElement != null)
            {
                lastElement.Visibility = Visibility.Collapsed;
            }
            ListBoxItem firstElement = (ListBoxItem)_listSelector.ItemContainerGenerator.ContainerFromItem(_view.FirstOrDefault());
            DoubleAnimation heightAnimation = new DoubleAnimation(itemheight, 0, TimeSpan.FromSeconds(0.5));
            heightAnimation.FillBehavior = FillBehavior.Stop;
            heightAnimation.Completed += (s, e) =>
            {
                if (_animateTimer != null && _animateTimer.IsEnabled && !_isItemsUpdate)
                {
                    if (AnimationIndex == 9999)
                    {
                        AnimationIndex = 0;
                    }
                    else
                    {
                        AnimationIndex++;
                    }
                    _view.RemoveAt(0);
                }
                _animationStarted = false;
            };
            if (_isItemsUpdate)
            {
                _isItemsUpdate = false;
            }
            firstElement.BeginAnimation(ListBoxItem.HeightProperty, heightAnimation);
            lastElement.Visibility = Visibility.Visible;
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
                if (Direction == AnimateDirection.Up)
                {
                    OnNext();
                }
                else
                {
                    OnPrevious();
                }
            }

        }

        #endregion
    }


}
