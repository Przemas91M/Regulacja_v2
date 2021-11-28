using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regulacja_v2
{
    /// <summary>
    /// Klasa typu listy, która przechowuje dane statusu regulatora.
    /// </summary>
    public class Status
    {
        public Status(string N, char W)
        {
            _nazwa = N;
            _wartosc = W;
        }
        private string _nazwa;
        private char _wartosc;
        public string Nazwa
        {
            get { return _nazwa; }
            set { _nazwa = value; }
        }
        public char Wartosc
        {
            get { return _wartosc; }
            set { _wartosc = value; }
        }
        public string Wyswietl
        {
            get { return Nazwa + Wartosc; }
        }
    }
}
