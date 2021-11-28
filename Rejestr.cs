using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regulacja_v2
{
    public class Rejestr
    {
        private ushort _Adres;
        private ushort _Wartosc;

        public ushort Adres
        {
            get
            {
                return _Adres;
            }
            set
            {
                _Adres = value;
            }

        }
        public ushort Wartosc
        {
            get
            {
                return _Wartosc;
            }
            set
            {
                _Wartosc = value;
            }

        }

    }
}
