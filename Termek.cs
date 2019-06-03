using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace zakopane
{
    public class Termek : INotifyPropertyChanged
    {
        private string path;       
        private int termekID;
        private string termekNameHUN;
        private string termekNamePL;
        private int suly;
        private double ar_Beszerzesi;
        private int ar_Eladasi;
        private int raktar_db;
        private int karton_darab;
        private int cimke_db;
        private string company;

        private string dbMEGJ;                
        private string sulyM;
        private string ar_BeszerzesiM;
        private string ar_EladasiM;
        private string raktar_dbM;
        private string cimke_dbM;

        public Termek()
        {
            this.raktar_db = 0;
        }      

        public int TermekID
        {
            get
            {
                return termekID;
            }

            set
            {
                termekID = value;
                OnPropertyChanged();
            }
        }

        public string TermekNameHUN
        {
            get
            {
                return termekNameHUN;
            }

            set
            {
                termekNameHUN = value;
                OnPropertyChanged();
            }
        }

        public string TermekNamePL
        {
            get
            {
                return termekNamePL;
            }

            set
            {
                termekNamePL = value;
                OnPropertyChanged();
            }
        }

        public int Suly
        {
            get
            {
                return suly;
            }

            set
            {
                suly = value;
                OnPropertyChanged();
            }
        }

        public double Ar_Beszerzesi
        {
            get
            {
                return ar_Beszerzesi;
            }

            set
            {
                ar_Beszerzesi = value;
                OnPropertyChanged();
            }
        }

        public int Ar_Eladasi
        {
            get
            {
                return ar_Eladasi;
            }

            set
            {
                ar_Eladasi = value;
                OnPropertyChanged();
            }
        }

        public int Raktar_db
        {
            get
            {
                return raktar_db;
            }

            set
            {
                raktar_db = value;
                OnPropertyChanged();
            }
        }
        
        public int Cimke_db
        {
            get
            {
                return cimke_db;
            }

            set
            {
                cimke_db = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory.ToString()  + path;
            }

            set
            {
                path = value;
                OnPropertyChanged();
            }
        }     

        public string Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
                OnPropertyChanged();
            }
        }               

        public string SulyM
        {
            get
            {
                return suly.ToString()+"g";
            }

            set
            {
                sulyM = value;
                OnPropertyChanged();
            }
        }

        public string Ar_BeszerzesiM
        {
            get
            {
                if (company == "Plawristy")
                {
                    return  ar_Beszerzesi + " zl";
                }
                else if(company== "Milk Co.")
                {
                    return ar_Beszerzesi + "€";
                }
                else
                {
                    throw new Exception();
                }
                
            }

            set
            {
                ar_BeszerzesiM = value;
                OnPropertyChanged();
            }
        }

        public string Ar_EladasiM
        {
            get
            {
                return String.Format("{0:#,0}", ar_Eladasi) + "Ft";
            }

            set
            {
                ar_EladasiM = value;
                OnPropertyChanged();
            }
        }

        public string Raktar_dbM
        {
            get
            {
                return raktar_db.ToString() + " karton";
            }

            set
            {
                raktar_dbM = value;
                OnPropertyChanged();
            }
        }

        public string Cimke_dbM
        {
            get
            {
                return cimke_db.ToString() + " db";
            }

            set
            {
                cimke_dbM = value;
                OnPropertyChanged();
            }
        }

        public int Karton_darab
        {
            get
            {
                return karton_darab;
            }

            set
            {
                karton_darab = value;
                OnPropertyChanged();
            }
        }

        public string DbMEGJ
        {
            get
            {
                return (karton_darab*raktar_db).ToString()+" db";
            }

            set
            {
                dbMEGJ = value;
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