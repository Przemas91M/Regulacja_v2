using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regulacja_v2
{/// <summary>
/// Klasa ze zmiennymi do listy z Raportami
/// zawiera w sobie dane:
/// data - czas rozpoczecia procesu wygrzewania (potrzebny do wykresu)
/// piec - nazwa urzadzenia w ktorym rozpoczeto proces
/// user - uzytkownik, ktory rozpoczal proces
/// nazwapliku - nazwa pliku, w ktorym sa zapisane pomiary
/// program - nazwa realizowanego programu wygrzewania
/// zlecenia - zagniezdzona lista z numerami zleceń
/// start - czas rozpoczenia procesu wygrzewania
/// stop - czas zakonczenia procesu wygrzewania
/// </summary>
    [Serializable]
    public class Raporty
    {
        public Raporty(DateTime data, string piec, string user, string nazwapliku, string program, List<string> zlecenia, DateTime start, DateTime stop)
        {
            _data = data; _piec = piec; _user = user; _nazwapliku = nazwapliku; _program = program; Zlecenia = zlecenia; _start = start; _stop = stop;
        }
        private DateTime _data, _start, _stop;
        //private TimeSpan _godzina;
        private string _piec, _user, _nazwapliku, _program;
        public DateTime Data { get { return _data; } set { _data = value; } }
        //public TimeSpan Godzina { get { return _godzina; } set { _godzina = value; } }
        public DateTime Start { get { return _start; } set { _start = value; } }
        public DateTime Stop { get { return _stop; } set { _stop = value; } }
        public string Piec { get { return _piec; } set { _piec = value; } }
        public string User { get { return _user; } set { _user = value; } }
        public string Nazwapliku { get { return _nazwapliku; } set { _nazwapliku = value; } }
        public string Program { get { return _program; } set { _program = value; } }
        public List<string> Zlecenia { get; set; }
        public string Wyswietl_Raport_Krotki
        {
            get { return Data + "  " + Piec; }
        }
    }
}
