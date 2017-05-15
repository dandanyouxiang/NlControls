
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
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace NlControls.WPF.Controls
{
    [TemplatePart(Name = ListBoxName, Type = typeof(ListBox))]
    [TemplatePart(Name = ErrorContentName, Type = typeof(ContentPresenter))]
    [ContentProperty("ItemsSource")]
    public class MultiCircleChart : Control
    {
        private const string ListBoxName = "PART_list";
        private const string ErrorContentName = "PART_ErrorContent";

        private ListBox listbox;
        private ContentPresenter ErrorContent;
        private WeakEventListener<MultiCircleChart, object, NotifyCollectionChangedEventArgs> _collectionChangedWeakEventListener;

        private List<object> _items;


        public Brush CircleBrush
        {
            get { return (Brush)GetValue(CircleBrushProperty); }
            set { SetValue(CircleBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CircleBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CircleBrushProperty =
            DependencyProperty.Register("CircleBrush", typeof(Brush), typeof(MultiCircleChart), new PropertyMetadata(Brushes.Cyan));

        public string ValueMemberPath
        {
            get { return (string)GetValue(ValueMemberPathProperty); }
            set { SetValue(ValueMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMemberPathProperty =
            DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(MultiCircleChart), new PropertyMetadata(""));




        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(MultiCircleChart), new PropertyMetadata(null));




        public PointCollection ItemsPlacePoints
        {
            get { return (PointCollection)GetValue(ItemsPlacePointsProperty); }
            set { SetValue(ItemsPlacePointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsPlacePoints.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsPlacePointsProperty =
            DependencyProperty.Register("ItemsPlacePoints", typeof(PointCollection), typeof(MultiCircleChart), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.AffectsArrange));




        public object ErrorTip
        {
            get { return (object)GetValue(ErrorTipProperty); }
            set { SetValue(ErrorTipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorTip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorTipProperty =
            DependencyProperty.Register("ErrorTip", typeof(object), typeof(MultiCircleChart), new PropertyMetadata("error"));

        


        #region SelectedItem

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(MultiCircleChart), new PropertyMetadata(OnSelectedItemPropertyChanged));

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }


        #endregion

        #region SelectionChanged

        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(MultiCircleChart));

        /// <summary>
        /// Occurs when the selected item in the drop-down portion of the
        /// <see cref="T:System.Windows.Controls.AutoCompleteBox" /> has
        /// changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }
        #endregion

        #region ItemsSource

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(MultiCircleChart), new PropertyMetadata(OnItemsSourcePropertyChanged));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiCircleChart chart = d as MultiCircleChart;
            chart.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }
        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
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
                _collectionChangedWeakEventListener = new WeakEventListener<MultiCircleChart, object, NotifyCollectionChangedEventArgs>(this);
                _collectionChangedWeakEventListener.OnEventAction = (instance, source, eventArgs) => instance.ItemsSourceCollectionChanged(source, eventArgs);
                _collectionChangedWeakEventListener.OnDetachAction = (weakEventListener) => newValueINotifyCollectionChanged.CollectionChanged -= weakEventListener.OnEvent;
                newValueINotifyCollectionChanged.CollectionChanged += _collectionChangedWeakEventListener.OnEvent;
            }

            // Store a local cached copy of the data
            _items = newValue == null ? null : new List<object>(newValue.Cast<object>().ToList());

            //// Clear and set the view on the selection adapter
            RefreshView();
        }
        /// <summary>
        /// Method that handles the ObservableCollection.CollectionChanged event for the ItemsSource property.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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

            // Update the view
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                for (int index = 0; index < e.OldItems.Count; index++)
                {
                    _items.Remove(e.OldItems[index]);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // Significant changes to the underlying data.
                if (ItemsSource != null)
                {
                    _items = new List<object>(ItemsSource.Cast<object>().ToList());
                }
            }

            // Refresh the observable collection used in the selection adapter.
            RefreshView();
        }
        #endregion


        static MultiCircleChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiCircleChart), new FrameworkPropertyMetadata(typeof(MultiCircleChart)));
        }

        public override void OnApplyTemplate()
        {
            listbox = GetTemplateChild(ListBoxName) as ListBox;
            if (listbox != null)
            {
                listbox.SelectionChanged += OnSelectedItemChanged;
            }
            ErrorContent = GetTemplateChild(ErrorContentName) as ContentPresenter;
            if (ErrorContent==null)
            {
                throw new ArgumentNullException();
            }
            base.OnApplyTemplate();
        }

        private void OnSelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems == null || e.AddedItems.Count < 1) return;
            SelectedItem = e.AddedItems[0];
            SelectionChangedEventArgs newe = new SelectionChangedEventArgs(SelectionChangedEvent, e.RemovedItems, e.AddedItems);
            RaiseEvent(newe);
        }






        /// <summary>
        /// Walks through the items enumeration. Performance is not going to be 
        /// perfect with the current implementation.
        /// </summary>
        private void RefreshView()
        {
            if (listbox==null)
            {
                return;
            }
            if (_items == null||_items.Count==0)
            {
                listbox.ItemsSource = null;
                ErrorContent.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                var _view = new ObservableCollection<object>(_items);
                ErrorContent.Visibility = Visibility.Collapsed;
                listbox.ItemsSource = _view;
            }
        
        }



    }
}
