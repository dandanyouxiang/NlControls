
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace NlControls.WPF.Controls
{
    public class ListCirclePanel : Panel
    {
        #region property

        //private BindingEvaluator<double> _valueBindingEvaluator;
        //public Binding ValueMemberBinding
        //{
        //    get
        //    {
        //        return _valueBindingEvaluator != null ? _valueBindingEvaluator.ValueBinding : null;
        //    }
        //    set
        //    {
        //        _valueBindingEvaluator = new BindingEvaluator<int>(value);
        //    }
        //}

        //private string _valueMemberPath
        //{
        //    get
        //    {
        //        return (ValueMemberBinding != null) ? ValueMemberBinding.Path.Path : null;
        //    }
        //    set
        //    {
        //        ValueMemberBinding = value == null ? null : new Binding(value);
        //    }
        //}


        public string ValueMemberPath
        {
            get { return (string)GetValue(ValueMemberPathProperty); }
            set { SetValue(ValueMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMemberPathProperty =
            DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(ListCirclePanel), new PropertyMetadata(""));





        public PointCollection ItemsPlacePoints
        {
            get { return (PointCollection)GetValue(ItemPlacePointProperty); }
            set { SetValue(ItemPlacePointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemPlacePoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemPlacePointProperty =
            DependencyProperty.Register("ItemsPlacePoints", typeof(PointCollection), typeof(ListCirclePanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));




        public double SumValue
        {
            get { return (double)GetValue(SumValueProperty); }
            set { SetValue(SumValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SumValueProperty =
            DependencyProperty.Register("SumValue", typeof(double), typeof(ListCirclePanel), new PropertyMetadata(0.0));

        

        #endregion

        #region Override Methods

        protected override Size MeasureOverride(Size availableSize)
        {

            Size resultSize = new Size(0, 0);
            foreach (UIElement child in this.Children)
            {
                child.Measure(availableSize);
                resultSize.Width = Math.Max(resultSize.Width, child.DesiredSize.Width);
                resultSize.Height = Math.Max(resultSize.Height, child.DesiredSize.Height);
            }
            resultSize.Width =
                double.IsPositiveInfinity(availableSize.Width) ?
                resultSize.Width : availableSize.Width;

            resultSize.Height =
                double.IsPositiveInfinity(availableSize.Height) ?
                resultSize.Height : availableSize.Height;

            return resultSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            Refresh();
            return finalSize;
        }
        
        #endregion

        #region Update View

        private void Refresh()
        {
            int index = 1;
            if (DataContext == null) return;
            var list = (DataContext as IEnumerable);

            if (list != null)
            {
                IList<double> _listSizeValue = new List<double>();
                foreach (FrameworkElement element in this.Children)
                {
                    _listSizeValue.Add(GetSizeValue(element.DataContext));
                }
                if (_listSizeValue.Count<1)
                {
                    return;
                }
                //_valueBindingEvaluator = new BindingEvaluator<double>(new Binding("Value")); 
                SumValue = _listSizeValue.Sum();
                double alignX = 0;
                double alignY = 0;
                int count = 0;
                Random random = new Random();
                foreach (FrameworkElement element in this.Children)
                {
                    double radius = CalculateCircleItemRadius(SumValue, _listSizeValue[count++]);
                    if (double.IsNaN(radius)) return;
                    SetCirclePosition(count, index++, out alignX, out alignY);
                    if (radius==0)
                    {
                        radius = random.Next(20, 35);
                    }
                    element.Arrange(new Rect((int)alignX, (int)alignY, 2 * radius, 2 * radius));
                }
            }
        } 
        #endregion

        #region Calculate Helper Methods

        private double CalculateCircleItemRadius(double sum, double itemValue)
        {
            double r = Math.Sqrt(itemValue / sum * ActualWidth * ActualHeight / 4 / Math.PI);
            double ar=ActualWidth>ActualHeight?ActualHeight:ActualWidth;
            return r>ar/3?ar/3:r;
        }


        private void SetCirclePosition(int count, int index, out double px, out double py)
        {
            px = 0;
            py = 0;
            if (count > 10)
            {
                throw new Exception("ListCircle的数目太多太多");
            }
            if (ItemsPlacePoints!=null&&ItemsPlacePoints.Count>=count)
            {
                px = ItemsPlacePoints[index-1].X;
                py = ItemsPlacePoints[index-1].Y;
            }
            else
            {
                if (Children.Count>=6)
                switch (index)
                {
                    case 1: px = ActualWidth / (count * 5);
                        py = ActualHeight / 12;
                        break;
                    case 2: px = ActualWidth / (count * 2);
                        py = ActualHeight / 2;
                        break;
                    case 3:
                        px = ActualWidth * 2 / count;
                        py = ActualHeight / 4;
                        break;
                    case 4:
                        px = ActualWidth * 2 / count + ActualWidth / 5;
                        py = ActualHeight / 2;
                        break;
                    case 5:
                        px = ActualWidth * 2 / count + ActualWidth * 3 / 10;
                        py = ActualHeight / 6;
                        break;
                    case 6:
                        px = ActualWidth * 5 / count;
                        py = ActualHeight * 2 / 7;
                        break;
                    default:
                        break;
                }
                else if(Children.Count==5)
                {
                    switch (index)
                    {
                        case 1: px = ActualWidth / (count * 2);
                            py = ActualHeight / 2;
                            break;
                        case 2:
                            px = ActualWidth * 2 / count;
                            py = ActualHeight / 4;
                            break;
                        case 3:
                            px = ActualWidth * 2 / count + ActualWidth / 5;
                            py = ActualHeight / 2;
                            break;
                        case 4:
                            px = ActualWidth * 2 / count + ActualWidth * 3 / 10;
                            py = ActualHeight / 6;
                            break;
                        case 5:
                            px = ActualWidth * 5 / count;
                            py = ActualHeight * 2 / 7;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (index)
                    {
  
                        case 1:
                            px = ActualWidth * 2 / count;
                            py = ActualHeight / 4;
                            break;
                        case 2:
                            px = ActualWidth * 2 / count + ActualWidth / 5;
                            py = ActualHeight / 2;
                            break;
                        case 3:
                            px = ActualWidth * 2 / count + ActualWidth * 3 / 10;
                            py = ActualHeight / 6;
                            break;
                        case 4:
                            px = ActualWidth * 5 / count;
                            py = ActualHeight * 2 / 7;
                            break;
                        default:
                            break;
                    }
                }
            }
           
        }


        private double GetSizeValue(object value)
        {
            double result;
            string strvalue;
            try
            {
                strvalue = value.GetType().GetProperty(ValueMemberPath).GetValue(value, null).ToString();
            }
            catch (Exception)
            {

                throw new Exception("ValueMemberPath is not correct!");
            }

            return double.TryParse(strvalue, out result) ? result : 0.0;

        }

        #endregion

        #region Binding To Get Value

        //private double FormatValue(object value, bool clearDataContext)
        //{
        //    double str = FormatValue(value);
        //    if (clearDataContext && _valueBindingEvaluator != null)
        //    {
        //        _valueBindingEvaluator.ClearDataContext();
        //    }
        //    return str;
        //}

        //protected virtual double FormatValue(object value)
        //{
        //    if (_valueBindingEvaluator != null)
        //    {
        //        return _valueBindingEvaluator.GetDynamicValue(value);
        //    }

        //    return 0;
        //}
        #endregion
    }

}
