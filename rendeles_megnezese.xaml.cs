using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace zakopane
{
    /// <summary>
    /// Interaction logic for rendeles_megnezese.xaml
    /// </summary>
    public partial class rendeles_megnezese : Window
    {
        public Rendeles r;
        private MainWindow main;

        public rendeles_megnezese(Rendeles rendeles,MainWindow main)
        {
            InitializeComponent();
           
            r = rendeles;
            this.DataContext = rendeles;
            this.main = main;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            XMLBuilder.Get.LejaratiModosit(r, datepicker.SelectedDate.Value);
           
        }
    }
}
