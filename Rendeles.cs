using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace zakopane
{
  public  class Rendeles : INotifyPropertyChanged
    {
        private int rendeles_id;
        private ObservableCollection<Rendelt_termek> rendelt_termekek;
        private DateTime date;
        private double pln;
        private double eur;
        private double osszeg;
        private DateTime lejarat;
        private string dateMEGJ;
        private string osszegMEGJ;
        private string plnMEGJ;
        private string eurMEGJ;
        private string lejaratMEGJ;
        private int hatra;
        private string hatraMEGJ;

        public Rendeles()
        {
            rendelt_termekek = new ObservableCollection<Rendelt_termek>();
        }

        public ObservableCollection<Rendelt_termek> Rendelt_termekek
        {
            get
            {
                return rendelt_termekek;
            }

            set
            {
                rendelt_termekek = value;
                OnPropertyChanged();
            }
        }

        public int Rendeles_id
        {
            get
            {
                return rendeles_id;
            }

            set
            {
                rendeles_id = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {

                date = value;
                OnPropertyChanged();
            }
        }

        public double Osszeg
        {
            get
            {
                return osszeg;
            }

            set
            {
                osszeg = value;
                OnPropertyChanged();
            }
        }

        public double Pln
        {
            get
            {
                return pln;
            }

            set
            {
                pln = value;
                OnPropertyChanged();
            }
        }

        public string DateMEGJ
        {
            get
            {
                return date.ToString("yyyy. MMMM d.");
            }

            set
            {
                dateMEGJ = value;
            }
        }

        public string OsszegMEGJ
        {
            get
            {               
                
                return String.Format("{0:#,0}", osszeg) + " Ft";
            }

            set
            {
                osszegMEGJ = value;
            }
        }

        public string PlnMEGJ
        {
            get
            {
                return pln.ToString() + " zl";
            }

            set
            {
                plnMEGJ = value;
                
            }
        }

        public DateTime Lejarat
        {
            get
            {
                return lejarat;
            }

            set
            {
                lejarat = value;
                OnPropertyChanged();
            }
        }

        public string LejaratMEGJ
        {
            get
            {
                if (lejarat == DateTime.MinValue)
                {
                    return "Még nem lett beríva";
                }
                else
                {
                    return lejarat.ToString("yyyy. MMMM d.");
                }            
            }

            set
            {
                lejaratMEGJ = value;
                OnPropertyChanged();
            }
        }

        public int Hatra
        {
            get
            {
                return hatra;
            }

            set
            {
                hatra = value;
                OnPropertyChanged();
            }
        }

        public string HatraMEGJ
        {
            get
            {
                if ((lejarat - DateTime.Now).Days < 0 && lejarat != DateTime.MinValue)
                {
                    return "Lejárt";
                }
                else if (lejarat == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return (lejarat - DateTime.Now).Days.ToString() + " nap";
                }
            }

            set
            {
                hatraMEGJ = value;
                OnPropertyChanged();
            }
        }

        public double Eur
        {
            get
            {
                return eur;
            }

            set
            {
                eur = value;
                OnPropertyChanged();
            }
        }

        public string EurMEGJ
        {
            get
            {
                return eur.ToString() + "€";
            }

            set
            {
                eurMEGJ = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
