using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NlControls.WPF.Test
{
    /// <summary>
    /// Interaction logic for TestWindow1.xaml
    /// </summary>
    public partial class TestWindow1 : Window
    {
        public TestWindow1()
        {
            InitializeComponent();
            InitFontsizeList();
        }
        private void InitFontsizeList()
        {
            List<int> lstSize = new List<int>()
            {
                8,10,12,14,18,20,24
            };
            comobox.ItemsSource = lstSize;
        }
    }
}
