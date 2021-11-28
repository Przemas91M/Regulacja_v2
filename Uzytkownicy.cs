using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regulacja_v2
{
    /// <summary>
    /// Klasa typu listy, dotyczaca uzytkownikow programu.
    /// Zawiera w sobie nazwe uzytkownika, haslo i poziom dostepu do programu.
    /// </summary>
    [Serializable]
    public class Uzytkownicy
    {
        public Uzytkownicy(string n, string h, int sl)
        {
            _nazwa = n;
            _haslo = h;
            _seclevel = sl;
        }
        private string _nazwa, _haslo;
        private int _seclevel;
        public string Nazwa
        {
            get { return _nazwa; }
            set { _nazwa = value; }
        }
        public string Haslo
        {
            get { return _haslo; }
            set { _haslo = value; }
        }
        public int Seclevel
        {
            get { return _seclevel; }
            set { _seclevel = value; }
        }
        public string Wyswietl_calosc
        {
            get { return Nazwa + ", " + Haslo + ", " + Seclevel; }
        }
        public string Wyswietl
        {
            get { return Nazwa + ", " + Seclevel; }
        }
    }
}
