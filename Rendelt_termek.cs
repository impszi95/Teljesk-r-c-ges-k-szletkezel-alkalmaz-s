using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zakopane
{
   public class Rendelt_termek
    {
        
        private int termekID;
        private string termekNameHUN;
        private string termekNamePL;
        private int suly;
        private double ar_Beszerzesi;
        private int ar_Eladasi;       
        private string company;
        private int db;
        private int kartonDB;

        public int TermekID
        {
            get
            {
                return termekID;
            }

            set
            {
                termekID = value;
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
            }
        }

        public int Db
        {
            get
            {
                return db;
            }

            set
            {
                db = value;
            }
        }

        public int KartonDB
        {
            get
            {
                return kartonDB;
            }

            set
            {
                kartonDB = value;
            }
        }
    }
}
