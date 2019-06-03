using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace zakopane
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
          

        public MainWindow()
        {
            InitializeComponent();
            
            this.keszlet_datagrid.DataContext = XMLBuilder.Get;
            this.rendeles_datagrid.DataContext = XMLBuilder.Get;
            
        }                   
      
        private void rendeles_button_Click(object sender, RoutedEventArgs e)
        {
            rendeles_button.Visibility = Visibility.Hidden;
            keszlet_button.Visibility = Visibility.Hidden;
            back_button.Visibility = Visibility.Visible;
            rendeles_datagrid.Visibility = Visibility.Visible;
            ujrendeles_button.Visibility = Visibility.Visible;
            rendelesmegnezese_button.Visibility = Visibility.Visible;
            background222_jpg.Visibility = Visibility.Hidden;
            modify_button.Visibility = Visibility.Hidden;
        }

        private void keszlet_button_Click(object sender, RoutedEventArgs e)
        {
            rendeles_button.Visibility = Visibility.Hidden;
            keszlet_button.Visibility = Visibility.Hidden;
            back_button.Visibility = Visibility.Visible;
            ujtermek_button.Visibility = Visibility.Visible;
            ujtermek_button.Visibility = Visibility.Visible;
            keszlet_datagrid.Visibility = Visibility.Visible;
            rendeles_datagrid.Visibility = Visibility.Hidden;
            ujrendeles_button.Visibility = Visibility.Hidden;
            delete_button.Visibility = Visibility.Visible;
            background222_jpg.Visibility = Visibility.Hidden;
            modify_button.Visibility = Visibility.Visible;
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            rendeles_button.Visibility = Visibility.Visible;
            keszlet_button.Visibility = Visibility.Visible;
            back_button.Visibility = Visibility.Hidden;
            ujtermek_button.Visibility = Visibility.Hidden;
            ujtermek_button.Visibility = Visibility.Hidden;
            keszlet_datagrid.Visibility = Visibility.Hidden;
            rendeles_datagrid.Visibility = Visibility.Hidden;
            ujrendeles_button.Visibility = Visibility.Hidden;
            delete_button.Visibility = Visibility.Hidden;
            rendelesmegnezese_button.Visibility = Visibility.Hidden;
            background222_jpg.Visibility = Visibility.Visible ;
            modify_button.Visibility = Visibility.Hidden;
        }

        private void ujtermek_button_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.85;
            Termek termek = new Termek();
            ujtermek ujtermekWindow = new ujtermek();            
            ujtermekWindow.ShowDialog();
            this.Opacity = 1;            
        }

        private void ujrendeles_button_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.85;
            ujRendeles ujrendelesWindow = new ujRendeles();
            ujrendelesWindow.ShowDialog();
            this.Opacity = 1;
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (keszlet_datagrid.SelectedItem != null)
            {
                this.Opacity = 0.85;
                if (MessageBox.Show("Biztosan kitörlöd a terméket Anya?", "Biztosan?", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    XMLBuilder.Get.Delete((Termek)keszlet_datagrid.SelectedItem);
                }
                this.Opacity = 1;
            }
        }

        private void rendelesmegnezese_button_Click(object sender, RoutedEventArgs e)
        {
            if (rendeles_datagrid.SelectedItem != null)
            {
                this.Opacity = 0.85;

                Rendeles rendeles = (Rendeles)rendeles_datagrid.SelectedItem;
                rendeles_megnezese ablak = new rendeles_megnezese(rendeles,this);

                ablak.ShowDialog();
                this.Opacity = 1;
            }
        }

        private void modify_button_Click(object sender, RoutedEventArgs e)
        {
            if (keszlet_datagrid.SelectedItem != null)
            {
                this.Opacity = 0.85;
                Termek selectedTermek = (Termek)keszlet_datagrid.SelectedItem;
                modosit modositWindow = new modosit(selectedTermek);
                modositWindow.ShowDialog();
                this.Opacity = 1;
            }
        }
    }
}

