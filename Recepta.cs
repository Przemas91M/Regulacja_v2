using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regulacja_v2
{
    /// <summary>
    /// Klasa odpowiadajaca za przypisywanie nazwy recepty do numeru programu.
    /// Plik z przypisaniami jest na dysku - plik //Recepty//recipies.bin, funkcja Plik.ZapiszRecepty.
    /// </summary>
    [Serializable]
    public class Recepta
    {
        public Recepta(int N, string R)
        {
            _numer = N;
            _recepta = R;
        }
        private string _recepta;
        private int _numer;
        public int Numer
        {
            get { return _numer; }
            set { _numer = value; }
        }
        public string Recept
        {
            get { return _recepta; }
            set { _recepta = value; }
        }
        public string Wyswietl
        {
            get { return Numer + " (" + Recept + ")"; }
        }
    }
}
