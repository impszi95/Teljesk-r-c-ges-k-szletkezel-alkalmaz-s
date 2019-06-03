using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for modosit.xaml
    /// </summary>
    public partial class modosit : Window
    {
        public Termek t;
        public string kepnev;
        public string filepath;

        public modosit(Termek selectedTermek)
        {
            InitializeComponent();
            comboBox.Items.Add("Plawristy");
            comboBox.Items.Add("Milk Co.");
            t = selectedTermek;
            this.DataContext = t;

            filepath = selectedTermek.Path;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filepath);
            bitmap.EndInit();
            image.Source = bitmap;
        }

        private void picture_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                image.Source = bitmap;
            }
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source != null)
            {
                Random rnd = new Random();
                string szam = rnd.Next(1, 1000).ToString();
                string szam2 = rnd.Next(1, 100).ToString();
                kepnev = szam + szam2;
                filepath = @"kepek\" + kepnev + ".jpg";
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                using (FileStream stream = new FileStream(filepath, FileMode.Create))
                    encoder.Save(stream);
                kepnev = kepnev + ".jpg";

            }
            XMLBuilder.Get.Modosit(t, filepath);
            
           
            this.Close();
        }
    }
}
