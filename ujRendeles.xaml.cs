using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ujRendeles.xaml
    /// </summary>
    public partial class ujRendeles : Window
    {

        public Rendeles uj;
        private double osszeg;
        bool elso = true;
        public ujRendeles()
        {
            InitializeComponent();
            this.dataGrid.DataContext = XMLBuilder.Get;
            uj = new Rendeles();
            this.DataContext = uj;
            osszeg = 0;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && textBox.Text != ""&& pln_textbox_Copy.Text!="0" && eur_textbox.Text!="0")
            {
                pln_textbox_Copy.Background = Brushes.LightGray;
                eur_textbox.Background = Brushes.LightGray;
                Rendelt_termek r_termek = new Rendelt_termek();
                Termek termek = (Termek)dataGrid.SelectedItem;

                r_termek.TermekID = termek.TermekID;
                r_termek.TermekNameHUN = termek.TermekNameHUN;
                r_termek.TermekNamePL = termek.TermekNamePL;
                r_termek.Suly = termek.Suly;
                r_termek.Ar_Beszerzesi = termek.Ar_Beszerzesi;
                r_termek.Ar_Eladasi = termek.Ar_Eladasi;
                r_termek.Company = termek.Company;
                r_termek.Db = int.Parse(textBox.Text);
                r_termek.KartonDB = termek.Karton_darab;
                bool márvan = false;

                if (elso)
                {
                    #region
                    listBox.Items.Add(termek.TermekNameHUN + " " + termek.Suly.ToString() + "g - " + textBox.Text.ToString() +" karton");

                    uj.Rendelt_termekek.Add(r_termek);

                    foreach (Termek t in XMLBuilder.Get.termeklist)
                    {
                        if (t.TermekID == termek.TermekID)
                        {
                            t.Raktar_db += int.Parse(textBox.Text);
                            t.Cimke_db -= int.Parse(textBox.Text)*t.Karton_darab+int.Parse(textBox.Text);
                            t.Raktar_dbM = t.Raktar_db.ToString();
                            t.Cimke_dbM = t.Cimke_db.ToString();
                            t.DbMEGJ = t.DbMEGJ;
                            XMLBuilder.Get.ModositSzamok(termek.TermekID, int.Parse(textBox.Text));
                        }
                    }

                    if (termek.Company == "Plawecky")
                    {
                        osszeg += (int.Parse(textBox.Text) * termek.Karton_darab) * (termek.Ar_Beszerzesi * int.Parse(pln_textbox_Copy.Text));
                        osszeg_label.Content = String.Format("{0:#,0}", osszeg) + " Ft";
                    }
                    else if (termek.Company == "Milkeffekt")
                    {
                        osszeg += (int.Parse(textBox.Text) * termek.Karton_darab) * (termek.Ar_Beszerzesi * int.Parse(eur_textbox.Text));
                        osszeg_label.Content = String.Format("{0:#,0}", osszeg) + " Ft";
                    }
                    #endregion
                    elso = false;
                }
                else
                {
                    foreach (Rendelt_termek term in uj.Rendelt_termekek.ToList())
                    {
                        if (term.TermekID == r_termek.TermekID && !elso)
                        {
                            márvan = true;
                        }
                    }
                    if (márvan)
                    {
                        MessageBox.Show("Már rendeltél ebből a termékből", "Már volt.", MessageBoxButton.OK);
                    }
                    else
                    {
                        listBox.Items.Add(termek.TermekNameHUN + " " + termek.Suly.ToString() + "g - " + textBox.Text.ToString() +
                                               " karton                    " + "              (" + termek.TermekID.ToString() + ")");

                        uj.Rendelt_termekek.Add(r_termek);

                        foreach (Termek t in XMLBuilder.Get.termeklist)
                        {
                            if (t.TermekID == termek.TermekID)
                            {
                                t.Raktar_db += int.Parse(textBox.Text);
                                t.Cimke_db -= int.Parse(textBox.Text) * t.Karton_darab + int.Parse(textBox.Text);
                                t.Raktar_dbM = t.Raktar_db.ToString();
                                t.Cimke_dbM = t.Cimke_db.ToString();
                                t.DbMEGJ = t.DbMEGJ;
                                XMLBuilder.Get.ModositSzamok(termek.TermekID, int.Parse(textBox.Text));
                            }
                        }
                        if (termek.Company == "Plawristy")
                        {
                            osszeg += (int.Parse(textBox.Text) * termek.Karton_darab) * (termek.Ar_Beszerzesi * int.Parse(pln_textbox_Copy.Text));
                            osszeg_label.Content = String.Format("{0:#,0}", osszeg) + " Ft";
                        }
                        else if (termek.Company == "Milk Co.")
                        {
                            osszeg += (int.Parse(textBox.Text) * termek.Karton_darab) * (termek.Ar_Beszerzesi * int.Parse(eur_textbox.Text));
                            osszeg_label.Content = String.Format("{0:#,0}", osszeg) + " Ft";
                        }
                    }
                }
            }
            if (pln_textbox_Copy.Text == "0")
            {
                pln_textbox_Copy.Background = Brushes.Red;
            }
            if (eur_textbox.Text=="0")
            {
                eur_textbox.Background = Brushes.Red;
            }
        }

        private void done_button_Click(object sender, RoutedEventArgs e)
        {
            if (uj.Rendelt_termekek.Any())
            {

                uj.Date = DateTime.Now;
                uj.Lejarat = DateTime.MinValue;
                uj.Osszeg = osszeg;
                uj.Rendeles_id = XMLBuilder.Get.rendelesDB;
                XMLBuilder.Get.rendeleslist.Add(uj);
                XMLBuilder.Get.Termekek_Szovegge(uj);
                XMLBuilder.Get.AddRendeles(uj);

                XMLBuilder.Get.Write_PDF(uj);

                this.Close();
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Termek selectedItem = (Termek)dataGrid.SelectedItem;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(selectedItem.Path);
            bitmap.EndInit();
            product_image.Source = bitmap;

            productLabel.Content = selectedItem.TermekNameHUN + " " + selectedItem.Suly.ToString() + "g";

        }

        private void kivesz_button_Click(object sender, RoutedEventArgs e)
        {
            if (uj.Rendelt_termekek.Any() && listBox.SelectedItem!=null)
            {
                char[] sep = new char[2];
                sep[0] = '(';
                sep[1] = ')';
                string s = listBox.SelectedItem.ToString();
                string[] tomb = s.Split(sep);
                int idx = int.Parse(tomb[1]);
                int db = 0;
                foreach (Rendelt_termek r_termek in uj.Rendelt_termekek.ToList())
                {

                    if (r_termek.TermekID == idx)
                    {
                        uj.Rendelt_termekek.Remove(r_termek); //Rendelésből kiszedem
                        listBox.Items.Remove(s); //Listboxból is
                        db = r_termek.Db;
                    }
                }

                    foreach (Termek t in XMLBuilder.Get.termeklist)
                    {
                        if (t.TermekID == idx)
                        {
                            t.Raktar_db -= db;
                            t.Cimke_db += t.Karton_darab*db+db;
                            t.DbMEGJ = t.DbMEGJ;
                            t.Raktar_dbM = t.Raktar_db.ToString();
                            t.Cimke_dbM = t.Cimke_db.ToString();
                            XMLBuilder.Get.ModositSzamokKivesz(idx, db);
                        if (t.Company == "Plawristy")
                        {
                            osszeg -= db * t.Karton_darab * (t.Ar_Beszerzesi * int.Parse(pln_textbox_Copy.Text));
                            osszeg_label.Content = String.Format("{0:#,0}", osszeg) + " Ft";
                        }
                        else if (t.Company == "Milk Co.")
                        {
                            osszeg -= db * t.Karton_darab * (t.Ar_Beszerzesi * int.Parse(eur_textbox.Text));
                            osszeg_label.Content = String.Format("{0:#,0}", osszeg) + " Ft";
                        }
                      
                    }
                    }
                
            }
        }
    }
}
