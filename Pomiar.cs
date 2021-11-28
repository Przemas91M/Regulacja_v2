using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Regulacja_v2
{
    /// <summary>
    /// Klasa typu listy, w ktorej przechowywane sa dane pomiarowe i zapisywane pozniej do pliku.
    /// </summary>
    [Serializable]
    public class Pomiar
    {
        public Pomiar(string X, float tz, float tzm)
        {
            _tempzad = tz;
            _tempzm = tzm;
            _X = X;
        }
        private float _tempzad, _tempzm;
        private string _X;
        public string X
        {
            get { return _X; }
            set { _X = value; }
        }
        public float TempZad
        {
            get { return _tempzad; }
            set { _tempzad = value; }
        }
        public float TempZm
        {
            get { return _tempzm; }
            set { _tempzm = value; }
        }
    }
}
