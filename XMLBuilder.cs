using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using iTextSharp;

namespace zakopane
{
    class XMLBuilder : INotifyPropertyChanged
    {
        public ObservableCollection<Termek> termeklist { get; private set; }
        public ObservableCollection<string> termeknevek { get; private set; }
        public ObservableCollection<Rendeles> rendeleslist { get; private set; }
        public ObservableCollection<Termek> rendeles_termekei { get; set; }
        public int rendelesDB { get; private set; }
        public int termekDB { get; private set; }
        XDocument doc;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private static XMLBuilder VM;

        public static XMLBuilder Get
        {
            get
            {
                if (VM == null)
                    VM = new XMLBuilder();

                return VM;
            }
        }

        public XMLBuilder()
        {
            doc = XDocument.Load("adatok.xml");
            this.termeklist = new ObservableCollection<Termek>(ToLista());
            termeknevek = termekNevek();
            rendeleslist = new ObservableCollection<Rendeles>(ToListaRendeles());
            rendeles_termekei = new ObservableCollection<Termek>();
            rendelesDB = rendeleslist.Count + 1;
            if (termeklist.Any())
            {
                termekDB = termeklist.Max(x => x.TermekID);
            }
            else
            {
                termekDB = 0;
            }
        }
        public ObservableCollection<string> termekNevek()
        {
            ObservableCollection<string> nevek = new ObservableCollection<string>();

            foreach (Termek termek in termeklist)
            {
                nevek.Add(termek.TermekNameHUN);
            }
            return nevek;
        }
        public List<Rendeles> ToListaRendeles()
        {


            List<Rendeles> list = (from xml in doc.Elements("Adatok").Elements("rendelés")
                                   select new Rendeles
                                   {

                                       Rendeles_id = int.Parse(xml.Element("id")?.Value),
                                       Date = DateTime.Parse(xml.Element("date")?.Value),
                                       Osszeg = int.Parse(xml.Element("összeg")?.Value),
                                       Pln = double.Parse(xml.Element("pln")?.Value),
                                       Eur = double.Parse(xml.Element("eur")?.Value),
                                       Rendelt_termekek = Szoveg_Listava(xml.Element("termékek")?.Value),
                                       Lejarat = DateTime.Parse(xml.Element("lejárat")?.Value)
                                   }).ToList();
            return list;
        }
        public List<Termek> ToLista()
        {


            List<Termek> list = (from xml in doc.Elements("Adatok").Elements("termék")
                                 select new Termek
                                 {

                                     TermekID = int.Parse(xml.Element("id")?.Value),
                                     TermekNameHUN = xml.Element("névHUN")?.Value,
                                     TermekNamePL = xml.Element("névPL")?.Value,
                                     Suly = int.Parse(xml.Element("súly")?.Value),
                                     Ar_Beszerzesi = double.Parse(xml.Element("beszerzési")?.Value),
                                     Ar_Eladasi = int.Parse(xml.Element("eladási")?.Value),
                                     Raktar_db = int.Parse(xml.Element("raktár")?.Value),
                                     Karton_darab = int.Parse(xml.Element("karton_db")?.Value),
                                     Cimke_db = int.Parse(xml.Element("címke")?.Value),
                                     Path = xml.Element("path")?.Value,
                                     Company = xml.Element("company")?.Value
                                 }).ToList();
            return list;
        }
        public void Add(Termek termek, string filepath)
        {
            if (termeklist.Any())
            {
                termekDB = termeklist.Max(x => x.TermekID) + 1;
            }
            else
            {
                termekDB = 1;
            }
            termek.TermekID = termekDB;
            var q = new XElement("termék",
                    new XElement("id", termek.TermekID.ToString()),
                         new XElement("névHUN", termek.TermekNameHUN),
                          new XElement("névPL", termek.TermekNamePL),
                          new XElement("súly", termek.Suly.ToString()),
                          new XElement("beszerzési", termek.Ar_Beszerzesi.ToString()),
                          new XElement("eladási", termek.Ar_Eladasi.ToString()),
                          new XElement("raktár", termek.Raktar_db.ToString()),
                          new XElement("karton_db",termek.Karton_darab.ToString()),
                          new XElement("címke", termek.Cimke_db.ToString()),
                          new XElement("path", filepath),
                          new XElement("company", termek.Company)
           );
            doc.Root.Add(q);
            doc.Save("adatok.xml");
            termekDB++;
        }
        public void Modosit(Termek termek, string filepath)
        {
            if (termek != null)
            {
                var q = doc.Root.Descendants("termék").Where(
                elem => elem.Element("id").Value == termek.TermekID.ToString()).FirstOrDefault();
                q.Element("névHUN").Value = termek.TermekNameHUN;
                q.Element("névPL").Value = termek.TermekNamePL;
                q.Element("súly").Value = termek.Suly.ToString();
                q.Element("beszerzési").Value = termek.Ar_Beszerzesi.ToString();
                q.Element("eladási").Value = termek.Ar_Eladasi.ToString();
                q.Element("raktár").Value = termek.Raktar_db.ToString();
                q.Element("karton_db").Value = termek.Karton_darab.ToString();
                q.Element("címke").Value = termek.Cimke_db.ToString();
                q.Element("path").Value = filepath;
                q.Element("company").Value = termek.Company;
                doc.Save("adatok.xml");

                foreach (Termek t in termeklist)
                {
                    if (t.TermekID == termek.TermekID)
                    {
                        termeklist[termeklist.IndexOf(t)].TermekNameHUN = termek.TermekNameHUN;
                        termeklist[termeklist.IndexOf(t)].TermekNamePL = termek.TermekNamePL;
                        termeklist[termeklist.IndexOf(t)].Suly = termek.Suly;
                        termeklist[termeklist.IndexOf(t)].Ar_Beszerzesi = termek.Ar_Beszerzesi;
                        termeklist[termeklist.IndexOf(t)].Ar_Eladasi = termek.Ar_Eladasi;
                        termeklist[termeklist.IndexOf(t)].Raktar_db = termek.Raktar_db;
                        termeklist[termeklist.IndexOf(t)].Karton_darab = termek.Karton_darab;
                        termeklist[termeklist.IndexOf(t)].Cimke_db = termek.Cimke_db;
                        termeklist[termeklist.IndexOf(t)].Path = filepath;
                        termeklist[termeklist.IndexOf(t)].Company = termek.Company;
                        termeklist[termeklist.IndexOf(t)].SulyM = termek.SulyM;
                        termeklist[termeklist.IndexOf(t)].Ar_BeszerzesiM = termek.Ar_BeszerzesiM;
                        termeklist[termeklist.IndexOf(t)].Ar_EladasiM = termek.Ar_EladasiM;
                        termeklist[termeklist.IndexOf(t)].Raktar_dbM = termek.Raktar_dbM;
                        termeklist[termeklist.IndexOf(t)].Cimke_dbM = termek.Cimke_dbM;
                    }
                }
            }
        }
        public void LejaratiModosit(Rendeles rendeles, DateTime lejarati)
        {
            if (rendeles != null)
            {
                var q = doc.Root.Descendants("rendelés").Where(
                elem => elem.Element("id").Value == rendeles.Rendeles_id.ToString()).FirstOrDefault();
                q.Element("lejárat").Value = rendeles.Lejarat.ToString();
                doc.Save("adatok.xml");

                foreach (Rendeles t in rendeleslist)
                {
                    if (t.Rendeles_id == rendeles.Rendeles_id)
                    {
                        rendeleslist[rendeleslist.IndexOf(t)].Lejarat = rendeles.Lejarat;
                        rendeleslist[rendeleslist.IndexOf(t)].Hatra = rendeles.Hatra;
                        rendeleslist[rendeleslist.IndexOf(t)].LejaratMEGJ = rendeles.LejaratMEGJ;
                        rendeleslist[rendeleslist.IndexOf(t)].HatraMEGJ = rendeles.HatraMEGJ;
                    }
                }                
            }
        }
        public void AddRendeles(Rendeles rendeles)
        {
            var q = new XElement("rendelés",
                new XElement("id", rendeles.Rendeles_id.ToString()),
                new XElement("termékek", Termekek_Szovegge(rendeles)),
                new XElement("date", rendeles.Date.ToString()),
                new XElement("pln", rendeles.Pln.ToString()),
                new XElement("eur", rendeles.Eur.ToString()),
                new XElement("összeg", rendeles.Osszeg.ToString()),
                new XElement("lejárat", rendeles.Lejarat.ToString()));
                doc.Root.Add(q);
            doc.Save("adatok.xml");
            rendelesDB++;
        }
        public void Delete(Termek termek)
        {

            if (termek != null)
            {
                var q = doc.Root.Descendants("termék").Where(
                elem => elem.Element("id").Value == termek.TermekID.ToString()).FirstOrDefault();
                termeklist.Remove(termek);
                q.Remove();
                doc.Save("adatok.xml");
            }
        }
        public string Termekek_Szovegge(Rendeles rendeles)
        {
            string s = "";

            foreach (Rendelt_termek termek in rendeles.Rendelt_termekek)
            {
                s += ":" + termek.TermekID.ToString() + ":" + termek.TermekNameHUN + ":" + termek.TermekNamePL + ":" + termek.Suly + ":" + termek.Ar_Beszerzesi + ":" + termek.Ar_Eladasi + ":" + termek.Db + ":" +termek.KartonDB+":" + termek.Company + ";";

            }
            return s;
        }
        public ObservableCollection<Rendelt_termek> Szoveg_Listava(string s)
        {
            ObservableCollection<Rendelt_termek> list = new ObservableCollection<Rendelt_termek>();
            List<string> elsoList = s.Split(';').ToList();
            elsoList.Remove(elsoList.Last());

            foreach (string sor in elsoList)
            {
                Rendelt_termek termek = new Rendelt_termek();
                string[] tomb = sor.Split(':');
                termek.TermekID = int.Parse(tomb[1]);
                termek.TermekNameHUN = tomb[2];
                termek.TermekNamePL = tomb[3];
                termek.Suly = int.Parse(tomb[4]);
                termek.Ar_Beszerzesi = double.Parse(tomb[5]);
                termek.Ar_Eladasi = int.Parse(tomb[6]);
                termek.Db = int.Parse(tomb[7]);
                termek.KartonDB = int.Parse(tomb[8]);
                termek.Company = tomb[9];
                list.Add(termek);
            }
            return list;
        }
        public void ModositSzamok(int id, int db)
        {
            var q = doc.Root.Descendants("termék").Where(
                    elem => elem.Element("id").Value == id.ToString()).FirstOrDefault();

            q.Element("raktár").Value = (int.Parse(q.Element("raktár").Value) + db).ToString();
            q.Element("címke").Value = (int.Parse(q.Element("címke").Value) - (((int.Parse(q.Element("karton_db").Value))*db)+db)).ToString();

            doc.Save("adatok.xml");


        }
        public void ModositSzamokKivesz(int id, int db)
        {
            var q = doc.Root.Descendants("termék").Where(
                    elem => elem.Element("id").Value == id.ToString()).FirstOrDefault();

            q.Element("raktár").Value = (int.Parse(q.Element("raktár").Value) - db).ToString();
            q.Element("címke").Value = (int.Parse(q.Element("címke").Value) + db).ToString();

            doc.Save("adatok.xml");
        }
        public void Write_PDF(Rendeles rendeles)
        {
            List<Rendelt_termek> egyikLista = new List<Rendelt_termek>();
            List<Rendelt_termek> masikLista = new List<Rendelt_termek>();
            string pdfNev = "";
            foreach (Rendelt_termek r_termek in rendeles.Rendelt_termekek) //Termékek szétválogatása (cég)
            {
                if (r_termek.Company == "Plawristy")
                {
                    egyikLista.Add(r_termek);
                }
                else if (r_termek.Company == "Milk Co.")
                {
                    masikLista.Add(r_termek);
                }
            }

            if (!egyikLista.Any())
            {
                pdfNev = rendeles.Date.ToString("yyyy-MM-dd") + "_Milk Co.";

                FileStream fs = new FileStream(@"rendelesek/"+pdfNev + ".pdf", FileMode.Create);
                Document doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                int db = 1;

                doc.Open();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
                Font font = new Font(bfTimes, 18, Font.NORMAL);

                doc.Add(new Paragraph("Order of Excellent Food Bt.", font));
                doc.Add(new Paragraph("Date: " + rendeles.Date.ToString("yyyy.MM.dd.")));
                doc.Add(new Paragraph("-----------------"));
                double priceSum = 0;
                double weightSum = 0;
                foreach (Rendelt_termek r_termek in masikLista)
                {
                    doc.Add(new Paragraph(db++.ToString() + ". " +
                        r_termek.TermekNamePL + " (" + r_termek.TermekNameHUN + ") - " +
                        r_termek.Suly.ToString() + "g - " +
                        r_termek.Ar_Beszerzesi.ToString() + "€" + " - "+
                        r_termek.Db.ToString() + " unit"
                        ));
                    priceSum += r_termek.Ar_Beszerzesi*r_termek.Db*r_termek.KartonDB;
                    weightSum += r_termek.Suly * r_termek.Db * r_termek.KartonDB;
                }
                doc.Add(new Paragraph("-----------------"));
                doc.Add(new Paragraph("Ár összesen: " + priceSum.ToString() + "€"));
                doc.Add(new Paragraph("Összsúly: " + (weightSum/1000).ToString()+"kg"));
                doc.Close();
                writer.Close();
                fs.Close();
            }
            else if (!masikLista.Any())
            {
                pdfNev = rendeles.Date.ToString("yyyy-MM-dd") + "_Plawristy";

                FileStream fs = new FileStream(@"rendelesek/" + pdfNev + ".pdf", FileMode.Create);
                Document doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                int db = 1;

                doc.Open();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
                Font font = new Font(bfTimes, 18, Font.NORMAL);

                doc.Add(new Paragraph("Order of Excellent Food Bt.", font));
                doc.Add(new Paragraph("Date: " + rendeles.Date.ToString("yyyy.MM.dd.")));
                doc.Add(new Paragraph("-----------------"));
                double priceSum = 0;
                double weightSum = 0;
                foreach (Rendelt_termek r_termek in egyikLista)
                {
                    doc.Add(new Paragraph(db++.ToString() + ". " +
                        r_termek.TermekNamePL + " (" + r_termek.TermekNameHUN + ") - " +
                        r_termek.Suly.ToString() + "g - " +
                         r_termek.Ar_Beszerzesi.ToString() + " zl" + " - " +
                        r_termek.Db.ToString() + " unit"
                        ));
                    priceSum += r_termek.Ar_Beszerzesi * r_termek.Db*r_termek.KartonDB;
                    weightSum += r_termek.Suly * r_termek.Db * r_termek.KartonDB;
                }
                doc.Add(new Paragraph("-----------------"));
                doc.Add(new Paragraph("Ár összesen: " + priceSum.ToString() + "zl"));
                doc.Add(new Paragraph("Összsúly: " + (weightSum / 1000).ToString() + "kg"));
                doc.Close();
                writer.Close();
                fs.Close();
            }
            else if (!egyikLista.Any() && !masikLista.Any()) //Üres a rendelés
            {
                MessageBox.Show("Nem rendeltél semmit!", "Hiba", MessageBoxButton.OK);
            }
            else //Mind2ből vannak
            {
                string pdfNev1 = rendeles.Date.ToString("yyyy-MM-dd") + "_Plawristy";
                string pdfNev2 = rendeles.Date.ToString("yyyy-MM-dd") + "_Milk Co.";

                FileStream fs1 = new FileStream(@"rendelesek/" + pdfNev1 + ".pdf", FileMode.Create);
                Document doc1 = new Document(PageSize.A4);
                PdfWriter writer1 = PdfWriter.GetInstance(doc1, fs1);

                int db1 = 1; //Egyik cégé

                doc1.Open();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
                Font font = new Font(bfTimes, 18, Font.NORMAL);

                doc1.Add(new Paragraph("Order of Excellent Food Bt.", font));
                doc1.Add(new Paragraph("Date: " + rendeles.Date.ToString("yyyy.MM.dd.")));
                doc1.Add(new Paragraph("-----------------"));
                double priceSum = 0;
                double weightSum = 0;
                foreach (Rendelt_termek r_termek in egyikLista)
                {
                    doc1.Add(new Paragraph(db1++.ToString() + ". " +
                        r_termek.TermekNamePL + " (" + r_termek.TermekNameHUN + ") - " +
                        r_termek.Suly.ToString() + "g - " +
                         r_termek.Ar_Beszerzesi.ToString() + " zl" + " - " +
                        r_termek.Db.ToString() + " unit"
                        ));
                    priceSum += r_termek.Ar_Beszerzesi * r_termek.Db*r_termek.KartonDB;
                    weightSum += r_termek.Suly * r_termek.Db * r_termek.KartonDB;
                }
                doc1.Add(new Paragraph("-----------------"));
                doc1.Add(new Paragraph("Ár összesen: " + priceSum.ToString() + "zl"));
                doc1.Add(new Paragraph("Összsúly: " + (weightSum / 1000).ToString() + "kg"));
                doc1.Close();
                writer1.Close();
                fs1.Close();

                FileStream fs2 = new FileStream(@"rendelesek/" + pdfNev2 + ".pdf", FileMode.Create);
                Document doc2 = new Document(PageSize.A4);
                PdfWriter writer2 = PdfWriter.GetInstance(doc2, fs2);

                int db2 = 1; //Másik cégé

                doc2.Open();

                doc2.Add(new Paragraph("Order of Excellent Food Bt.", font));
                doc2.Add(new Paragraph("Date: " + rendeles.Date.ToString("yyyy.MM.dd.")));
                doc2.Add(new Paragraph("-----------------"));
                double priceSum2 = 0;
                double weightSum2 = 0;
                foreach (Rendelt_termek r_termek in masikLista)
                {
                    doc2.Add(new Paragraph(db2++.ToString() + ". " +
                        r_termek.TermekNamePL + " (" + r_termek.TermekNameHUN + ") - " +
                        r_termek.Suly.ToString() + "g - " +
                         r_termek.Ar_Beszerzesi.ToString() + "€" + " - "+
                        r_termek.Db.ToString() + " unit"
                        ));
                    priceSum2 += r_termek.Ar_Beszerzesi * r_termek.Db*r_termek.KartonDB;
                    weightSum2 += r_termek.Suly * r_termek.Db * r_termek.KartonDB;
                }
                doc2.Add(new Paragraph("-----------------"));
                doc2.Add(new Paragraph("Ár összesen: " + priceSum2.ToString() + "€"));
                doc2.Add(new Paragraph("Összsúly: " + (weightSum2 / 1000).ToString() + "kg"));
                doc2.Close();
                writer2.Close();
                fs2.Close();
            }

        }
    }
}
