using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regulacja_v2
{
    /// <summary>
    /// Klasa typu listy, w której przechowywane sa parametry programu wygrzewania.
    /// </summary>
    [Serializable]
    public class Programy
    {
        public Programy(string P, int I, ushort W)
        {
            _Parametr = P;
            _Indeks = I;
            _Wartosc = W;
        }
        private string _Parametr;
        private ushort _Wartosc;
        private int _Indeks;
        public string Parametr
        {
            get { return _Parametr; }
        }
        public ushort Wartosc
        {
            get { return _Wartosc; }
            set { _Wartosc = value; }
        }
        public int Indeks
        {
            get { return _Indeks; }
            set { _Indeks = value; }
        }
        public string Wyswietl
        {
            get { return Parametr + ":   " + Wartosc; }
        }
    }
   
}