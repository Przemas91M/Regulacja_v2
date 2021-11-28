using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using static Regulacja_v2.ModbusRTU;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Regulacja_v2
{
    public partial class Form1 : Form
    {
        public static Form1 Static; //statyczne pole rzutujace na interfejs
        public Form1()
        {
            InitializeComponent();
            Static = this;
            Form2.Show();
            Form2.Activate();
            Form2.Szukaj();
        }
        public volatile int progP1, odcp1, minodcp1, godzodcp1,  //zmienne modyfikowane przez watki
        progP2, odcp2, minodcp2, godzodcp2,
        progP3, odcp3, minodcp3, godzodcp3,
        progP4, odcp4, minodcp4, godzodcp4,
        progP5, odcp5, minodcp5, godzodcp5, godzodcp6,
        status1, status2, status3, status4, status5;
        public volatile float tSP1, tzP1, tSP2, tzP2, tSP3, tzP3, tSP4, tzP4, tSP5, tzP5;
        private string file;
        readonly private ModbusRTU Modbus = new ModbusRTU();
        readonly private Uzytkownik Uzytkownik = new Uzytkownik();
        private readonly Form2 Form2 = new Form2();
        readonly Plik Plik = new Plik();
        Piec1 p1;        //deklaracja klasy watku pieca 1 - usuwac zasoby przy zakonczeniu watku
        Piec2 p2;        //deklaracja klasy watku pieca 2 - usuwac zasoby przy zakonczeniu watku
        Piec3 p3;
        Piec4 p4;
        Piec5 p5;
        Form4 Raportowanie = new Form4();
        ZlecenieStart Zlecenie;
        WatekGlowny WG;
        readonly static Listy L = new Listy();
        private List<Pomiar> PomiarP1 = new List<Pomiar>();
        private List<Pomiar> PomiarP2 = new List<Pomiar>();
        private List<Pomiar> PomiarP3 = new List<Pomiar>();
        private List<Pomiar> PomiarP4 = new List<Pomiar>();
        private List<Pomiar> PomiarP5 = new List<Pomiar>();
        private ObservableCollection<Pomiar> pomiarWykres1 = new ObservableCollection<Pomiar>();
        private ObservableCollection<Pomiar> pomiarWykres2 = new ObservableCollection<Pomiar>();
        private ObservableCollection<Pomiar> pomiarWykres3 = new ObservableCollection<Pomiar>();
        private ObservableCollection<Pomiar> pomiarWykres4 = new ObservableCollection<Pomiar>();
        private ObservableCollection<Pomiar> pomiarWykres5 = new ObservableCollection<Pomiar>();
        private readonly List<Status> StatusRegulatora = L.GetStatusRegulatora;
        Thread Piec1Thread = null;    //watek pierwszego pieca
        Thread Piec2Thread = null;    //watek drugiego pieca
        Thread Piec3Thread = null;
        Thread Piec4Thread = null;
        Thread Piec5Thread = null;
        Thread GlownyWatek;
        private List<Programy> LProg = L.GetListProgramy;
        private List<Recepta> recepty = L.GetRecepty;
        private readonly List<Programy> LParamKonf = L.GetParametryKonfiguracyjne;
        private readonly List<Programy> LParamReg = L.GetParametryRegulacji;
        private readonly List<Programy> LParamAlm = L.GetParametryAlarmow;
        private readonly List<Chart> wykresy = new List<Chart>();
        //private readonly List<Pomiar> pomiary = new List<Pomiar>();
        private readonly System.Windows.Forms.Timer Czasomierz = new System.Windows.Forms.Timer
        {
            Interval = 500
        };
        private int _securityLevel, program;
        private string _uzytkownik;
        private bool _p1s = false, _p2s = false, _p3s = false, _p4s = false, _p5s = false;
        private bool _OB1 = false, _OB2 = false, _OB3 = false, _OB4 = false, _OB5 = false, _OB1p = false, _OB2p = false, _OB3p = false, _OB4p = false, _OB5p = false; // zmienne wskazujace obecność urzadzeń w sieci
        public string uzytkownik
        {
            get { return _uzytkownik; }
            set
            {
                _uzytkownik = value;
                if (_uzytkownik == "Brak") { Pr_Login.Text = "Zaloguj"; }
                else { Pr_Login.Text = "Wyloguj"; }
            }
        }
        public int securityLevel
        {
            get { return _securityLevel; }
            set
            {
                _securityLevel = value;
                Interfejs(_securityLevel);
            }
        }
        /// <summary>
        /// Zmienna odpowiedzialna za stan pracy pieca 1 (pracuje/zatrzymany)
        /// </summary>
        public bool p1s
        {
            get { return _p1s; }
            set
            {
                _p1s = value;
                if (_p1s)
                {
                    Piec1_kontrolka.BackColor = Color.Green;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                    {
                        b_start_p1.Text = "Stop";
                        b_start_p1.BackColor = Color.Red;
                        labelStanP1.Text = "Pracuje";
                    }
                        ));
                    }
                    else
                    {
                        b_start_p1.Text = "Stop";
                        b_start_p1.BackColor = Color.Red;
                        labelStanP1.Text = "Pracuje";
                    }
                }
                else
                {
                    Piec1_kontrolka.BackColor = Color.Red;
                    progP1 = 0;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p1.Text = "Start Piec 1";
                            b_start_p1.BackColor = Color.LimeGreen;
                            labelStanP1.Text = "Dostępny";
                            if (!_OB1)
                            {
                                labelStanP1.Text = "Niedostępny";
                            }
                        }
                        ));
                    }
                    else
                    {
                        b_start_p1.Text = "Start Piec 1";
                        b_start_p1.BackColor = Color.LimeGreen;
                        labelStanP1.Text = "Dostępny";
                        if (!_OB1)
                        {
                            labelStanP1.Text = "Niedostępny";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Zmienna odpowiedzialna za stan pracy pieca 2 (pracuje/zatrzymany)
        /// </summary>
        public bool p2s
        {
            get { return _p2s; }
            set
            {
                _p2s = value;
                if (_p2s)
                {
                    Piec2_kontrolka.BackColor = Color.Green;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p2.Text = "Stop";
                            b_start_p2.BackColor = Color.Red;
                            labelStanP2.Text = "Pracuje";
                        }
                        ));
                    }
                    else
                    {
                        b_start_p2.Text = "Stop";
                        b_start_p2.BackColor = Color.Red;
                        labelStanP2.Text = "Pracuje";
                    }
                }
                else
                {
                    Piec2_kontrolka.BackColor = Color.Red;
                    progP2 = 0;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p2.Text = "Start Piec 2";
                            b_start_p2.BackColor = Color.LimeGreen;
                            labelStanP2.Text = "Dostępny";
                            if (!_OB2)
                            {
                                labelStanP2.Text = "Niedostępny";
                            }
                        }
                        ));
                    }
                    else
                    {
                        b_start_p2.Text = "Start Piec 2";
                        b_start_p2.BackColor = Color.LimeGreen;
                        labelStanP2.Text = "Dostępny";
                        if (!_OB2)
                        {
                            labelStanP2.Text = "Niedostępny";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Zmienna odpowiedzialna za stan pracy pieca 3 (pracuje/zatrzymany)
        /// </summary>
        public bool p3s
        {
            get { return _p3s; }
            set
            {
                _p3s = value;
                if (_p3s)
                {
                    Piec3_kontrolka.BackColor = Color.Green;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p3.Text = "Stop";
                            b_start_p3.BackColor = Color.Red;
                            labelStanP3.Text = "Pracuje";
                        }
                        ));
                    }
                    else
                    {
                        b_start_p3.Text = "Stop";
                        b_start_p3.BackColor = Color.Red;
                        labelStanP3.Text = "Pracuje";
                    }
                }
                else
                {
                    Piec3_kontrolka.BackColor = Color.Red;
                    progP3 = 0;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p3.Text = "Start Piec 3";
                            b_start_p3.BackColor = Color.LimeGreen;
                            labelStanP3.Text = "Dostępny";
                            if (!_OB3)
                            {
                                labelStanP3.Text = "Niedostępny";
                            }
                        }
                        ));
                    }
                    else
                    {
                        b_start_p3.Text = "Start Piec 3";
                        b_start_p3.BackColor = Color.LimeGreen;
                        labelStanP3.Text = "Dostępny";
                        if (!_OB3)
                        {
                            labelStanP3.Text = "Niedostępny";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Zmienna odpowiedzialna za stan pracy pieca 4 (pracuje/zatrzymany)
        /// </summary>
        public bool p4s
        {
            get { return _p4s; }
            set
            {
                _p4s = value;
                if (_p4s)
                {
                    Piec4_kontrolka.BackColor = Color.Green;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p4.Text = "Stop";
                            b_start_p4.BackColor = Color.Red;
                            labelStanP4.Text = "Pracuje";
                        }
                        ));
                    }
                    else
                    {
                        b_start_p4.Text = "Stop";
                        b_start_p4.BackColor = Color.Red;
                        labelStanP4.Text = "Pracuje";
                    }
                }
                else
                {
                    Piec4_kontrolka.BackColor = Color.Red;
                    progP4 = 0;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p4.Text = "Start Piec 4";
                            b_start_p4.BackColor = Color.LimeGreen;
                            labelStanP4.Text = "Dostępny";
                            if (!_OB4)
                            {
                                labelStanP4.Text = "Niedostępny";
                            }
                        }
                        ));
                    }
                    else
                    {
                        b_start_p4.Text = "Start Piec 4";
                        b_start_p4.BackColor = Color.LimeGreen;
                        labelStanP4.Text = "Dostępny";
                        if (!_OB4)
                        {
                            labelStanP4.Text = "Niedostępny";

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Zmienna odpowiedzialna za stan pracy pieca 5 (pracuje/zatrzymany)
        /// </summary>
        public bool p5s
        {
            get { return _p5s; }
            set
            {
                _p5s = value;
                if (_p5s)
                {
                    Piec5_kontrolka.BackColor = Color.Green;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p5.Text = "Stop";
                            b_start_p5.BackColor = Color.Red;
                            labelStanP5.Text = "Pracuje";
                        }
                        ));
                    }
                    else
                    {
                        b_start_p5.Text = "Stop";
                        b_start_p5.BackColor = Color.Red;
                        labelStanP5.Text = "Pracuje";
                    }
                }
                else
                {
                    Piec5_kontrolka.BackColor = Color.Red;
                    progP5 = 0;
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            b_start_p5.Text = "Start Piec 5";
                            b_start_p5.BackColor = Color.LimeGreen;
                            labelStanP5.Text = "Dostępny";
                            if (!_OB5)
                            {
                                labelStanP5.Text = "Niedostępny";
                            }
                        }
                        ));
                    }
                    else
                    {
                        b_start_p5.Text = "Start Piec 5";
                        b_start_p5.BackColor = Color.LimeGreen;
                        labelStanP5.Text = "Dostępny";
                        if (!_OB5)
                        {
                            labelStanP5.Text = "Niedostępny";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Flaga obecności pieca 1
        /// </summary>
        public bool OB1
        {
            get { return _OB1; }
            set
            {
                _OB1 = value;
                if (_OB1p != _OB1)
                {
                    if (_OB1)
                    {
                        labelStanP1.Text = "Dostępny";
                        Interfejs(securityLevel, 1);
                        comboUrzadzenia.Items.Add("Piec1");
                        comboUrzadzenia2.Items.Add("Piec1");
                    }
                    else
                    {
                        labelStanP1.Text = "Niedostępny";
                        Interfejs(securityLevel, 1);
                        comboUrzadzenia.Items.Remove("Piec1");
                        comboUrzadzenia2.Items.Remove("Piec1");
                    }
                    _OB1p = _OB1;
                }
            }
        }

        /// <summary>
        /// Flaga obecności pieca 2
        /// </summary>
        public bool OB2
        {
            get { return _OB2; }
            set
            {
                _OB2 = value;
                if (_OB2p != _OB2)
                {
                    if (_OB2)
                    {
                        labelStanP2.Text = "Dostępny";
                        Interfejs(securityLevel, 2);
                        comboUrzadzenia.Items.Add("Piec2");
                        comboUrzadzenia2.Items.Add("Piec2");
                    }
                    else
                    {
                        labelStanP2.Text = "Niedostępny";
                        Interfejs(securityLevel, 2);
                        comboUrzadzenia.Items.Remove("Piec2");
                        comboUrzadzenia2.Items.Remove("Piec2");
                    }
                    _OB2p = _OB2;
                }
            }
        }

        /// <summary>
        /// Flaga obecności pieca 3
        /// </summary>
        public bool OB3
        {
            get { return _OB3; }
            set
            {
                _OB3 = value;
                if (_OB3p != _OB3)
                {
                    if (_OB3)
                    {
                        labelStanP3.Text = "Dostępny";
                        Interfejs(securityLevel, 3);
                        comboUrzadzenia.Items.Add("Piec3");
                        comboUrzadzenia2.Items.Add("Piec3");
                    }
                    else
                    {
                        labelStanP3.Text = "Niedostępny";
                        Interfejs(securityLevel, 3);
                        comboUrzadzenia.Items.Remove("Piec3");
                        comboUrzadzenia2.Items.Remove("Piec3");
                    }
                    _OB3p = _OB3;
                }
            }
        }

        /// <summary>
        /// Flaga obecności pieca 4
        /// </summary>
        public bool OB4
        {
            get { return _OB4; }
            set
            {
                _OB4 = value;
                if (_OB4p != _OB4)
                {
                    if (_OB4)
                    {
                        labelStanP4.Text = "Dostępny";
                        Interfejs(securityLevel, 4);
                        comboUrzadzenia.Items.Add("Piec4");
                        comboUrzadzenia2.Items.Add("Piec4");
                    }
                    else
                    {
                        labelStanP4.Text = "Niedostępny";
                        Interfejs(securityLevel, 4);
                        comboUrzadzenia.Items.Remove("Piec4");
                        comboUrzadzenia2.Items.Remove("Piec4");
                    }
                    _OB4p = _OB4;
                }
            }
        }

        /// <summary>
        /// Flaga obecności pieca 5
        /// </summary>
        public bool OB5
        {
            get { return _OB5; }
            set
            {
                _OB5 = value;
                if (_OB5p != _OB5)
                {
                    if (_OB5)
                    {
                        labelStanP5.Text = "Dostępny";
                        Interfejs(securityLevel, 5);
                        comboUrzadzenia.Items.Add("Piec5");
                        comboUrzadzenia2.Items.Add("Piec5");
                    }
                    else
                    {
                        labelStanP5.Text = "Niedostępny";
                        Interfejs(securityLevel, 5);
                        comboUrzadzenia.Items.Remove("Piec5");
                        comboUrzadzenia2.Items.Remove("Piec5");
                    }
                    _OB5p = _OB5;
                }
            }
        }
        /// <summary>
        /// Zmienna przechowująca port COM, do którego jest podłączony konwerter RS485/USB
        /// </summary>
        public string portCOM { get; set; }

        /// <summary>
        /// Funkcja aktywujaca / dezaktywujaca elementy interfejsu podczas logowania uzytkownika
        /// </summary>
        /// <param name="level">Poziom dostępu użytkownika</param>
        private void Interfejs(int level)
        {
            if (level == 0)              //brak zalogowanego uzytkownika - mozliwosc przegladania raportow, temperatur w piecach i możliwość zalogowania.
            {
                tabControl1.TabPages.Remove(tabUstawienia);   //ustawienia - nie pokazuj
                tabControl1.TabPages.Remove(tabProgram);      //program - nie pokazuj
                tabControl1.TabPages.Remove(tabParametry);    //parametry - nie pokazuj
                tabControl1.TabPages.Remove(tabDebug);
                tabUstawienia.Hide();
                tabProgram.Hide();
                tabParametry.Hide();
                tabDebug.Hide();

                if (p1s) { b_start_p1.Enabled = true; labelStanP1.Text = "Pracuje"; }
                else { b_start_p1.Enabled = false; }
                if (p2s) { b_start_p2.Enabled = true; labelStanP2.Text = "Pracuje"; }
                else { b_start_p2.Enabled = false; }
                if (p3s) { b_start_p3.Enabled = true; labelStanP3.Text = "Pracuje"; }
                else { b_start_p3.Enabled = false; }
                if (p4s) { b_start_p4.Enabled = true; labelStanP4.Text = "Pracuje"; }
                else { b_start_p4.Enabled = false; }
                if (p5s) { b_start_p5.Enabled = true; labelStanP5.Text = "Pracuje"; }
                else { b_start_p5.Enabled = false; }
            }
            else if (level == 1)         //piecowy - startowanie piecow, podglad raportów
            {
                tabControl1.TabPages.Remove(tabUstawienia);   //ustawienia - nie pokazuj
                tabControl1.TabPages.Remove(tabProgram);      //program - nie pokazuj
                tabControl1.TabPages.Remove(tabParametry);    //parametry - nie pokazuj
                tabControl1.TabPages.Remove(tabDebug);        //debug - nie pokazuj
                tabUstawienia.Hide();
                tabProgram.Hide();
                tabParametry.Hide();
                tabDebug.Hide();

                if (OB1) { b_start_p1.Enabled = true; }              //piec1 obecny 
                else { b_start_p1.Enabled = false; }                 //piec 1 nieobecny    

                if (OB2) { b_start_p2.Enabled = true; }              //***analogicznie dla pozostałych obiektów***//
                else { b_start_p2.Enabled = false; }

                if (OB3) { b_start_p3.Enabled = true; }
                else { b_start_p3.Enabled = false; }

                if (OB4) { b_start_p4.Enabled = true; }
                else { b_start_p4.Enabled = false; }

                if (OB5) { b_start_p5.Enabled = true; }
                else { b_start_p5.Enabled = false; }

            }
            else if (level == 2)       //automatyk/UR/technolog - wygaszony domyślnie - bez dostepu do ustawień programu
            {
                tabParametry.Show();
                tabProgram.Show();                               //pokaz program
                tabUstawienia.Show();
                tabControl1.TabPages.Insert(5, tabProgram);
                tabControl1.TabPages.Insert(6, tabParametry);
                tabControl1.TabPages.Insert(7, tabUstawienia);      //usun ustawienia
                tabControl1.TabPages.Remove(tabDebug);        //debug - nie pokazuj
                tabDebug.Hide();
                if (OB1) { b_start_p1.Enabled = true; }          //piec1 obecny 
                else { b_start_p1.Enabled = false; }             //piec 1 nieobecny 

                if (OB2) { b_start_p2.Enabled = true; }       //reszta analogicznie
                else { b_start_p2.Enabled = false; }

                if (OB3) { b_start_p3.Enabled = true; }       //reszta analogicznie
                else { b_start_p3.Enabled = false; }

                if (OB4) { b_start_p4.Enabled = true; }       //reszta analogicznie
                else { b_start_p4.Enabled = false; }

                if (OB5) { b_start_p5.Enabled = true; }       //reszta analogicznie
                else { b_start_p5.Enabled = false; }
            }
            else if (level == 3)         //administrator - pelny dostep do funkcjonalnosci programu
            {
                tabParametry.Show();
                tabUstawienia.Show();
                tabProgram.Show();
                tabDebug.Show();
                tabControl1.TabPages.Insert(5, tabProgram);
                tabControl1.TabPages.Insert(6, tabParametry);
                tabControl1.TabPages.Insert(7, tabUstawienia);
                tabControl1.TabPages.Insert(8, tabDebug);
                if (OB1) { b_start_p1.Enabled = true; }       //piec1 obecny 
                else { b_start_p1.Enabled = false; }          //piec 1 nieobecny    

                if (OB2) { b_start_p2.Enabled = true; }      //reszta analogicznie
                else { b_start_p2.Enabled = false; }

                if (OB3) { b_start_p3.Enabled = true; }      //reszta analogicznie
                else { b_start_p3.Enabled = false; }

                if (OB4) { b_start_p4.Enabled = true; }      //reszta analogicznie
                else { b_start_p4.Enabled = false; }

                if (OB5) { b_start_p5.Enabled = true; }      //reszta analogicznie
                else { b_start_p5.Enabled = false; }
            }
        }

        /// <summary>
        /// Funkcja wywoływana podczas wykrycia zmiany obecności pieca. 
        /// </summary>
        /// <param name="level">Poziom dostępu użytkownika</param>
        /// <param name="piec"></param>
        private void Interfejs(int level, int piec)
        {
            switch (piec)
            {
                case 1:
                    if (OB1 && !p1s) //piec1 obecny - nie pracuje
                    {
                        if (level == 0)
                            b_start_p1.Enabled = false;
                        else
                            b_start_p1.Enabled = true;
                    }
                    else if (OB1 && p1s) { b_start_p1.Enabled = true; labelStanP1.Text = "Pracuje"; }   //piec obecny i pracuje - wywołane po wyjściu ze stanu awarii
                    else if (!OB1 && p1s) { b_start_p1.Enabled = false; labelStanP1.Text = "AWARIA"; }  //piec1 nieobecny, lecz pracuje - awaria
                    else { b_start_p1.Enabled = false; }                                                //piec 1 nieobecny    
                    break;
                case 2:
                    if (OB2 && !p2s)
                    {
                        if (level == 0)
                            b_start_p2.Enabled = false;
                        else
                            b_start_p2.Enabled = true;
                    }
                    else if (OB2 && p2s) { b_start_p2.Enabled = true; labelStanP2.Text = "Pracuje"; }
                    else if (!OB2 && p2s) { b_start_p2.Enabled = false; labelStanP2.Text = "AWARIA"; }
                    else { b_start_p2.Enabled = false; }
                    break;
                case 3:
                    if (OB3 && !p3s)
                    {
                        if (level == 0)
                            b_start_p3.Enabled = false;
                        else
                            b_start_p3.Enabled = true;
                    }
                    else if (OB3 && p3s) { b_start_p3.Enabled = true; labelStanP3.Text = "Pracuje"; }
                    else if (!OB3 && p3s) { b_start_p3.Enabled = false; labelStanP3.Text = "AWARIA"; }
                    else { b_start_p3.Enabled = false; }
                    break;
                case 4:
                    if (OB4 && !p4s)
                    {
                        if (level == 0)
                            b_start_p4.Enabled = false;
                        else
                            b_start_p4.Enabled = true;
                    }
                    else if (OB4 && p4s) { b_start_p4.Enabled = true; labelStanP4.Text = "Pracuje"; }
                    else if (!OB4 && p4s) { b_start_p4.Enabled = false; labelStanP4.Text = "AWARIA"; }
                    else { b_start_p4.Enabled = false; }
                    break;
                case 5:
                    if (OB5 && !p5s) //piec1 obecny - nie pracuje
                    {
                        if (level == 0)
                            b_start_p5.Enabled = false;
                        else
                            b_start_p5.Enabled = true;
                    }
                    else if (OB5 && p5s) { b_start_p5.Enabled = true; labelStanP5.Text = "Pracuje"; }
                    else if (!OB5 && p5s) { b_start_p5.Enabled = false; labelStanP5.Text = "AWARIA"; }
                    else { b_start_p5.Enabled = false; }
                    break;
            }
            // }
        }
        /// <summary>
        /// Funkcja wywoływana przez form2. Zalogowanie użytkownika - do momentu wylogowania, badź zamknięcia programu.
        /// Zalogowanie zmienia poziom dostępu, uruchamia poszczególne elementy interfejsu.
        /// </summary>
        /// <param name="user"> String z nazwa użytkownika </param>
        /// <param name="seclevel"> Poziom dostępu </param>
        public void Login(string user, int seclevel)
        {
            uzytkownik = user;
            securityLevel = seclevel;
            User_Label.Text = user;
            Securitylvl_Label.Text = seclevel.ToString();
        }
        /// <summary>
        /// Metoda wywoływana podczas zamykania programu.
        /// Zatrzymuje główny watek,
        /// zatrzymuje zegar i czysci zasoby przez niego uzywane.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("Na pewno zamknąć program?", "Uwaga!", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                if (Piec1Thread != null)
                {
                    Piec1Thread.Abort();
                }
                if (Piec2Thread != null)
                {
                    Piec2Thread.Abort();
                }
                if (Piec3Thread != null)
                {
                    Piec3Thread.Abort();
                }
                if (Piec4Thread != null)
                {
                    Piec4Thread.Abort();
                }
                if (Piec5Thread != null)
                {
                    Piec5Thread.Abort();
                }
                if (WG != null)
                {
                    WG.SetStop();
                    WG = null;
                    GlownyWatek = null;
                    Czasomierz.Stop();
                    Czasomierz.Dispose();
                }
                //zabić watki pieców!!!!
                Form2.Dispose();
                Raportowanie = null;
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }
        /// <summary>
        /// Funkcja ustawiajaca nazwe pliku i numer programu do przekazania dla watku.
        /// </summary>
        /// <param name="plik"> Nazwa pliku do ktorego beda zapisywane pomiary </param>
        /// <param name="prog"> Numer programu wygrzewania </param>
        public void Filename_Prog(string plik, int prog)
        {
            file = plik;
            program = prog;
        }
        /// <summary>
        /// Przycisk otwierający plik .dat z zapisanymi aktualnymi procesami wygrzewania okładzin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Procesy_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            path = string.Concat(path, "\\Data\\procesy.dat");
            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// Funkcja zapisu pojedynczego rejestru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rej_zap_Click(object sender, EventArgs e)
        {
            try
            {
                byte adres = Convert.ToByte(textBox3.Text);
                ushort rejestr = Convert.ToUInt16(textBox2.Text);
                uint parametr = Convert.ToUInt32(textBox1.Text);
                byte[] ramka = FormujRamke(adres, 6, rejestr, parametr);
                int kod = Modbus.Zapisz_Rejestr(ramka);
                if (kod == 0)
                {
                    MessageBox.Show("Zapis zakończony powodzeniem.");
                    Plik.ZapiszDoLog($"Zapisano {parametr} do rejestru numer: {rejestr} pod adres {adres}");
                }
                else
                {
                    MessageBox.Show($"Błąd podczas zapisu parametru.\nKod błędu: {kod}");
                    Plik.ZapiszDoLog($"Nieudany zapis {parametr} do rejestru: {rejestr} \r\nKod błędu: {kod}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Funkcja służaca do odczytu parametrów regulatora
        /// Użytkownik wybiera urzadzenie, grupę parametrów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_PobierzParamertyRegulatora_Click(object sender, EventArgs e)
        {
            try
            {
                byte adres = 0;
                string piec;
                int index;
                byte[] ramka;
                ushort[] dane;
                if (comboUrzadzenia2.SelectedIndex < 0)                  //sprawdzam, czy zostało wybrane urzadzenie
                {
                    MessageBox.Show("Wybierz urządzenie!");
                    return;
                }
                if (comboParametryGrupy.SelectedIndex < 0)               //sprawdzam, czy zostala wybrana grupa
                {
                    MessageBox.Show("Wybierz grupę parametrów!");
                    return;
                }
                listBoxParametry.DataSource = null;                     //czyszczenie okienka wyswietlajacego parametry
                index = comboParametryGrupy.SelectedIndex;              //zapis indeksu wybranej grupy parametrów
                piec = comboUrzadzenia2.SelectedItem.ToString();        //zapis wybranego urzadzenia
                switch (piec)                                           //przypisanie adresu do konkretnego pieca
                {
                    case "Piec1":
                        adres = 1;
                        break;
                    case "Piec2":
                        adres = 2;
                        break;
                    case "Piec3":
                        adres = 3;
                        break;
                    case "Piec4":
                        adres = 4;
                        break;
                    case "Piec5":
                        adres = 5;
                        break;
                }
                if (index == 0)                                          //wybrano parametry konfiguracyjne
                {
                    ramka = FormujRamke(adres, 3, 4014, 20);  //formowanie ramki dla danych konfiguracyjnych
                    dane = Odbierz_Dane(ramka, 100);          //odbiór danych
                    for (int i = 0; i < LParamKonf.Count; i++)           //przepisanie wybranych danych do listy (reszta pozostaje bez zmian)
                    {
                        LParamKonf[i].Wartosc = dane[LParamKonf[i].Indeks];
                    }
                    listBoxParametry.DisplayMember = "Wyswietl";        //wybranie jakie dane klasy maja byc wyswietlone
                    listBoxParametry.ValueMember = "Wartosc";           //jakie wartosci maja byc wyswietlone
                    listBoxParametry.DataSource = LParamKonf;           //przypisanie listy do okienka prezentacji danych
                }
                else if (index == 1)                                     //wybrano parametry regulacji
                {
                    ramka = FormujRamke(adres, 3, 4034, 87);  //***algorytm analogiczny do pierwszego***//
                    dane = Odbierz_Dane(ramka, 150);
                    for (int i = 0; i < LParamReg.Count; i++)
                    {
                        LParamReg[i].Wartosc = dane[LParamReg[i].Indeks];
                    }
                    listBoxParametry.DisplayMember = "Wyswietl";
                    listBoxParametry.ValueMember = "Wartosc";
                    listBoxParametry.DataSource = LParamReg;
                }
                else if (index == 2)                                     //wybrano parametry alarmów
                {
                    ramka = FormujRamke(adres, 3, 4065, 16);
                    dane = Odbierz_Dane(ramka, 100);
                    for (int i = 0; i < LParamAlm.Count; i++)
                    {
                        LParamAlm[i].Wartosc = dane[LParamAlm[i].Indeks];
                    }
                    listBoxParametry.DisplayMember = "Wyswietl";
                    listBoxParametry.ValueMember = "Wartosc";
                    listBoxParametry.DataSource = LParamAlm;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }
        /// <summary>
        /// Metoda wywoływana przy wejściu na kartę parametrów regulatora.
        /// Aktywuje listę wyboru urzadzenia oraz grupy parametrów. 
        /// Deaktywuje możliwość edytowania parametru i zapisu bez uprzedniego wczytania parametrów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabParametry_Enter(object sender, EventArgs e)
        {
            comboUrzadzenia2.Enabled = true;                //aktywacja listy wyboru pieca
            comboParametryGrupy.Enabled = true;             //aktywacja listy wyboru grupy parametrów
            Pr_PobierzParamertyRegulatora.Enabled = true;   //aktywacja przycisku pobierania parametrów
            Pr_Param_Edit.Enabled = false;                  //deaktywacja przycisku edycji parametru
            Pr_ParametryEdycja.Text = "Rozpocznij edytowanie";
            Pr_Param_Zapisz.Enabled = false;                //deaktywacja przycisku zapisu parametrów
            textBox4.Enabled = false;                       //dezaktywacja pola wprowadzania parametru
        }

        private void Pr_ParametryEdycja_Click(object sender, EventArgs e)
        {
            if (comboUrzadzenia2.Enabled == true)    //jeżeli zaczynam edycję
            {
                if (comboUrzadzenia2.SelectedIndex < 0 || comboParametryGrupy.SelectedIndex < 0)
                {
                    return;
                }
                Pr_PobierzParamertyRegulatora.PerformClick();
                comboUrzadzenia2.Enabled = false;               //dezaktywacja listy wyboru pieca
                comboParametryGrupy.Enabled = false;            //dezaktywacja listy wyboru grup parametrów
                Pr_PobierzParamertyRegulatora.Enabled = false;  //dezaktywacja przycisku pobierania parametrów
                textBox4.Enabled = true;                        //aktywacja pola wprowadzania parametru
                Pr_Param_Edit.Enabled = true;                   //aktywacja przycisku edycji parametru
                Pr_Param_Zapisz.Enabled = true;                 //aktywacja przycisku zapisu parametrów
                Pr_ParametryEdycja.Text = "Zakończ edytowanie";     //zmiana wyświetlanego opisu przycisku
            }
            else                                    //kończę edycję
            {
                comboUrzadzenia2.Enabled = true;                //***wszystko na odwrót***//
                comboParametryGrupy.Enabled = true;
                Pr_PobierzParamertyRegulatora.Enabled = true;
                textBox4.Enabled = false;
                Pr_Param_Edit.Enabled = false;
                Pr_Param_Zapisz.Enabled = false;
                Pr_ParametryEdycja.Text = "Rozpocznij edytowanie";
            }
        }

        private void Pr_Param_Zapisz_Click(object sender, EventArgs e)
        {
            try
            {
                byte adres = 0;
                byte[] ramka = { 0, 0 };
                ushort[] dane;
                int index = comboParametryGrupy.SelectedIndex;              //zapis indeksu wybranej grupy parametrów
                string piec = comboUrzadzenia2.SelectedItem.ToString();        //zapis wybranego urzadzenia
                switch (piec)                                           //przypisanie adresu do konkretnego pieca
                {
                    case "Piec1":
                        adres = 1;
                        break;
                    case "Piec2":
                        adres = 2;
                        break;
                    case "Piec3":
                        adres = 3;
                        break;
                    case "Piec4":
                        adres = 4;
                        break;
                    case "Piec5":
                        adres = 5;
                        break;
                }
                if (index == 0)         //wybrano parametry konfiguracyjne
                {
                    ramka = FormujRamke(adres, 3, 4014, 20);  //formowanie ramki dla danych konfiguracyjnych - pobieram dane jeszcze raz, dla pewnosci
                    dane = Odbierz_Dane(ramka, 100);          //odbiór danych
                    for (int i = 0; i < LParamKonf.Count; i++)
                    {
                        dane[LParamKonf[i].Indeks] = LParamKonf[i].Wartosc;
                    }
                    ramka = FormujRamkeREJ(adres, 4014, dane);
                }
                else if (index == 1)    //parametry regulacji
                {
                    ramka = FormujRamke(adres, 3, 4034, 87);  //***algorytm analogiczny do pierwszego***//
                    dane = Odbierz_Dane(ramka, 150);
                    for (int i = 0; i < LParamReg.Count; i++)
                    {
                        dane[LParamReg[i].Indeks] = LParamReg[i].Wartosc;
                    }
                    ramka = FormujRamkeREJ(adres, 4034, dane);
                }
                else if (index == 2)     //parametry alarmów
                {
                    ramka = FormujRamke(adres, 3, 4065, 16);
                    dane = Odbierz_Dane(ramka, 100);
                    for (int i = 0; i < LParamAlm.Count; i++)
                    {
                        dane[LParamAlm[i].Indeks] = LParamAlm[i].Wartosc;
                    }
                    ramka = FormujRamkeREJ(adres, 4065, dane);
                }
                int kod = Zapisz_Rejestry(ramka);
                if (kod == 0)
                {
                    MessageBox.Show("Zapis zakończony powodzeniem");
                }
                else if (kod == 2)
                {
                    MessageBox.Show("Błąd zapisu parametrów!\nSprawdź wartości parametrów - czy nie sa poza zakresem!");
                    return;
                }
                else if (kod == 3)
                {
                    MessageBox.Show("Błąd zapisu parametrów!\nSprawdź wartości parametrów - czy nie sa poza zakresem!");
                    return;
                }
                else
                {
                    MessageBox.Show("Błąd zapisu parametrów!\nSprawdź regulator i jego połączenie!");
                    return;
                }
                //niech pobiera nazwe parametru
                Plik.ZapiszDoLog($"Piec{adres}\r\nZmiana parametrów grupy nr {index} \r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void Pr_SprawdzStatusReg_Click(object sender, EventArgs e)
        {
            byte adres;
            string status, alarm, fail;
            try
            {
                adres = Convert.ToByte(piecBoxStatus.Text);
            }
            catch (Exception)
            {
                return;
            }
            try
            {
                byte[] ramka = ModbusRTU.FormujRamke(adres, 3, 4003, 3);
                byte[] dane = ModbusRTU.Odbierz_DaneRAW(ramka, 50);
                status = Convert.ToString(dane[0], 2).PadLeft(8, '0') + Convert.ToString(dane[1], 2).PadLeft(8, '0');
                status = OdwrocString(status);
                alarm = Convert.ToString(dane[2], 2).PadLeft(8, '0') + Convert.ToString(dane[3], 2).PadLeft(8, '0');
                alarm = OdwrocString(alarm);
                fail = Convert.ToString(dane[4], 2).PadLeft(8, '0') + Convert.ToString(dane[5], 2).PadLeft(8, '0');
                fail = OdwrocString(fail);
                StatusRegulatora[0].Wartosc = status[4];
                StatusRegulatora[1].Wartosc = status[5];
                StatusRegulatora[2].Wartosc = status[6];
                StatusRegulatora[3].Wartosc = status[7];
                StatusRegulatora[4].Wartosc = status[8];
                StatusRegulatora[5].Wartosc = status[13];
                StatusRegulatora[6].Wartosc = status[14];
                StatusRegulatora[7].Wartosc = status[15];
                StatusRegulatora[8].Wartosc = alarm[0];
                StatusRegulatora[9].Wartosc = alarm[1];
                StatusRegulatora[10].Wartosc = alarm[2];
                StatusRegulatora[11].Wartosc = alarm[3];
                StatusRegulatora[12].Wartosc = alarm[4];
                StatusRegulatora[13].Wartosc = alarm[5];
                StatusRegulatora[14].Wartosc = alarm[6];
                StatusRegulatora[15].Wartosc = alarm[7];
                StatusRegulatora[16].Wartosc = alarm[8];
                StatusRegulatora[17].Wartosc = alarm[9];
                StatusRegulatora[18].Wartosc = alarm[10];
                StatusRegulatora[19].Wartosc = alarm[11];
                StatusRegulatora[20].Wartosc = fail[0];
                StatusRegulatora[21].Wartosc = fail[1];
                StatusRegulatora[22].Wartosc = fail[2];
                StatusRegulatora[23].Wartosc = fail[3];
                StatusRegulatora[24].Wartosc = fail[15];
                statusBox.DataSource = null;
                statusBox.DataSource = StatusRegulatora;
                statusBox.DisplayMember = "Wyswietl";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private string OdwrocString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public void Start_Zegara()
        {
            if (GlownyWatek == null)
            {
                WG = new WatekGlowny();
                GlownyWatek = new Thread(WG.WatekStart);
                WG.SetStart();
                GlownyWatek.Start();
                Czasomierz.Tick += new EventHandler(Czasomierz_Tick);
                Czasomierz.Start();
                wykresy.Add(chartPiec1);
                wykresy.Add(chartPiec2);
                wykresy.Add(chartPiec3);
                wykresy.Add(chartPiec4);
                wykresy.Add(chartPiec5);
            }
        }

        /// <summary>
        /// Metoda wywoływana po naciśnięciu przycisku edycji parametru regulatora.
        /// Zmienia on tylko wartość w liście, nie zmienia go w regulatorze.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Param_Edit_Click(object sender, EventArgs e)
        {
            ushort parametr;
            if (textBox4.TextLength == 0)                //sprawdzam, czy został wprowadzony parametr do zmiany
            {
                MessageBox.Show("Proszę wprowadzić wartość parametru!");
                return;
            }
            try                                         //sprawdzam, czy wprowadzona wartość jest liczba
            {
                parametr = Convert.ToUInt16(textBox4.Text);
            }
            catch (Exception)                           //jeżeli to nie liczba, przerywam wykonanie metody
            {
                MessageBox.Show("Wprowadzono nieprawidłowa wartość.\r\nWartość musi być liczba z odpowiedniego zakresu.");
                return;
            }
            if (comboParametryGrupy.SelectedIndex == 0)       //wybrana jest grupa parametrów konfiguracyjnych
            {
                int index = listBoxParametry.SelectedIndex;
                LParamKonf[index].Wartosc = parametr;
                listBoxParametry.DataSource = null;
                listBoxParametry.DataSource = LParamKonf;
                listBoxParametry.DisplayMember = "Wyswietl";
            }
            else if (comboParametryGrupy.SelectedIndex == 1)  //wybrana jest grupa parametrów regulacji
            {
                int index = listBoxParametry.SelectedIndex;
                LParamReg[index].Wartosc = parametr;
                listBoxParametry.DataSource = null;
                listBoxParametry.DataSource = LParamReg;
                listBoxParametry.DisplayMember = "Wyswietl";
            }
            else if (comboParametryGrupy.SelectedIndex == 2)  //wybrana jest grupa alarmów
            {
                int index = listBoxParametry.SelectedIndex;
                LParamAlm[index].Wartosc = parametr;
                listBoxParametry.DataSource = null;
                listBoxParametry.DataSource = LParamAlm;
                listBoxParametry.DisplayMember = "Wyswietl";
            }

        }

        /// <summary>
        /// Przycisk uruchamiajacy cykl sterowania - piec 1.
        /// !!! utworzyc osobne przyciski dla kazdego pieca, wprowadzic metode sprawdzajaca, czy piec nie jest juz w obiegu
        /// np. awaria komputera - do ustalenia czy cykl ma trwac, czy zaczynac od nowa!!!
        /// Zapis do LOG - start, godzina, receptura i uzytkownik - moze tez nazwe pliku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_start_p1_Click(object sender, EventArgs e)
        {
            if (b_start_p1.Text == "Start Piec 1")  //jezeli piec nie pracuje
            {
                Zlecenie = new ZlecenieStart(uzytkownik, "Piec 1");                //otwiera okienko w ktorym wpisuje sie nazwy zlecen i wybiera program
                try
                {
                    DialogResult res = Zlecenie.ShowDialog();                           //jezeli wsystko sie zgadza rozpoczynam proces
                    if (res == DialogResult.OK)
                    {
                        pomiarWykres1.Clear();                                          //czyszczenie listy z pomiarami                      
                        p1 = null;                                                      //kasowanie obiektu pieca
                        p1 = new Piec1(file, 1, program);                               //tworzenie nowego obiektu pieca
                        Piec1Thread = null;                                             //kasowanie zasobów watku
                        WykresStart(1);
                        Piec1Thread = new Thread(() => p1.PiecWatek());    //tworzenie nowego watku, przekazanie danych 
                        p1.SetStart();                                                  //ustawienie boola
                        Piec1Thread.Start();                                            //uruchomienie watku
                        Plik.ZapiszDoLog("Start pieca 1\r\nOperator: " + uzytkownik);   //zapisanie danych do logu
                        file = string.Empty;                                            //kasowanie sciezki do pliku
                        program = 0;                                                    //zerowanie zmiennej programu
                    }
                    else                                                                //jezeli uzytkownik anulowal proces uruchamiania pieca
                    {
                        MessageBox.Show("Anulowano start cyklu.");                      //wyswietl komunikat i nic nie rob
                        return;
                    }
                }
                catch (Exception ex)                                                    //w przypadku bledu
                {
                    MessageBox.Show(ex.ToString());                                     //wyswietl komunikat
                }
                finally
                {
                    Zlecenie.Dispose();                                                 //kasowanie zasobow uzywanych przez okno zlecenie
                }
            }
            else  //jezeli piec pracuje
            {
                try
                {
                    DialogResult res = MessageBox.Show("Piec jest w trakcie pracy!\r\nCzy na pewno chcesz zakończyć proces?", null, MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        if (Piec1Thread != null)     //w przypadku aktywnego watku - piec pracuje normalnie - dopisac okno wyboru czy uzytkownik jest pewien zatrzymania dla obu przypadków!!    
                        {
                            p1.SetStop();                                                   //zatrzymanie watku
                            StartStop_Cykl(1, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            Plik.ZapiszDoLog("Ręczne zatrzymanie pieca 1");                 //zapisanie danych do loga
                            MessageBox.Show("Ręczne zatrzymanie cyklu wygrzewania");        //wyswietlenie komunikatu
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika(kontynuacja pracy programu po wylaczeniu)
                            {
                                securityLevel = 0;
                            }
                        }
                        else                        //w przypadku awaryjnego zatrzymania pieca - awaria programu itp....
                        {
                            StartStop_Cykl(1, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            p1s = false;                                                    //ustawienie zmiennej stanu regulatora
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika
                            {
                                securityLevel = 0;
                            }
                            //przeslac rozkaz przerwania regulacji do sterownika, wyswietlic komunikat z informacjami, zapis do Log 
                            Plik.ZapiszDoLog("Awaryjne zatrzymanie pieca 1");               //zapisanie danych do logu
                        }
                    }
                    else { return; }                                                        //użytkownik nie chce przerywać pracy - powrót
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    StartStop_Cykl(1, 0, false);                          //przesłanie rozkazu bezpośrednio do sterownika
                    p1s = false;                                                    //ustawienie zmiennej stanu regulatora
                }
            }
        }
        private void b_start_p2_Click(object sender, EventArgs e)
        {
            if (b_start_p2.Text == "Start Piec 2")  //jezeli piec nie pracuje
            {
                Zlecenie = new ZlecenieStart(uzytkownik, "Piec 2");                //otwiera okienko w ktorym wpisuje sie nazwy zlecen i wybiera program
                try
                {
                    DialogResult res = Zlecenie.ShowDialog();                           //jezeli wsystko sie zgadza rozpoczynam proces
                    if (res == DialogResult.OK)
                    {
                        pomiarWykres2.Clear();                                               //czyszczenie listy z pomiarami
                        p2 = null;                                                      //kasowanie obiektu pieca
                        p2 = new Piec2(file, 2, program);                                            //tworzenie nowego obiektu pieca
                        Piec2Thread = null;                                             //kasowanie zasobów watku
                        WykresStart(2);                         //kasowanie wykresu
                        Piec2Thread = new Thread(() => p2.PiecWatek());    //tworzenie nowego watku, przekazanie danych 
                        p2.SetStart();                                                  //ustawienie boola
                        Piec2Thread.Start();                                            //uruchomienie watku
                        Plik.ZapiszDoLog("Start pieca 2\r\nOperator: " + uzytkownik);   //zapisanie danych do logu
                        file = string.Empty;                                            //kasowanie sciezki do pliku
                        program = 0;                                                    //zerowanie zmiennej programu
                    }
                    else                                                                //jezeli uzytkownik anulowal proces uruchamiania pieca
                    {
                        MessageBox.Show("Anulowano start cyklu.");                      //wyswietl komunikat i nic nie rob
                        return;
                    }
                }
                catch (Exception ex)                                                    //w przypadku bledu
                {
                    MessageBox.Show(ex.Message.ToString());                                     //wyswietl komunikat
                }
                finally
                {
                    Zlecenie.Dispose();                                                 //kasowanie zasobow uzywanych przez okno zlecenie
                }
            }
            else  //jezeli piec pracuje
            {
                try
                {
                    DialogResult res = MessageBox.Show("Piec jest w trakcie pracy!\r\nCzy na pewno chcesz zakończyć proces?", null, MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        if (Piec2Thread != null)     //w przypadku aktywnego watku - piec pracuje normalnie - dopisac okno wyboru czy uzytkownik jest pewien zatrzymania dla obu przypadków!!    
                        {
                            p2.SetStop();                                                   //zatrzymanie watku
                            StartStop_Cykl(2, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            Plik.ZapiszDoLog("Ręczne zatrzymanie pieca 2");                 //zapisanie danych do loga
                            MessageBox.Show("Ręczne zatrzymanie cyklu wygrzewania");        //wyswietlenie komunikatu
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika(kontynuacja pracy programu po wylaczeniu)
                            {
                                securityLevel = 0;
                            }
                        }
                        else                        //w przypadku awaryjnego zatrzymania pieca - awaria programu itp....
                        {
                            StartStop_Cykl(2, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            p2s = false;                                                    //ustawienie zmiennej stanu regulatora
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika
                            {
                                securityLevel = 0;
                            }
                            Plik.ZapiszDoLog("Awaryjne zatrzymanie pieca 2");               //zapisanie danych do logu
                        }
                    }
                    else { return; }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    ModbusRTU.StartStop_Cykl(2, 0, false);                          //przesłanie rozkazu bezpośrednio do sterownika
                    p2s = false;                                                    //ustawienie zmiennej stanu regulatora
                }
            }
        }
        private void b_start_p3_Click(object sender, EventArgs e)
        {
            if (b_start_p3.Text == "Start Piec 3")  //jezeli piec nie pracuje
            {
                Zlecenie = new ZlecenieStart(uzytkownik, "Piec 3");                //otwiera okienko w ktorym wpisuje sie nazwy zlecen i wybiera program
                try
                {
                    DialogResult res = Zlecenie.ShowDialog();                           //jezeli wsystko sie zgadza rozpoczynam proces
                    if (res == DialogResult.OK)
                    {
                        pomiarWykres3.Clear();                                               //czyszczenie listy z pomiarami
                        p3 = null;                                                      //kasowanie obiektu pieca
                        p3 = new Piec3(file, 3, program);                                               //tworzenie nowego obiektu pieca
                        Piec3Thread = null;                                             //kasowanie zasobów watku
                        WykresStart(3);                                                  //kasowanie wykresu
                        Piec3Thread = new Thread(() => p3.PiecWatek());    //tworzenie nowego watku, przekazanie danych 
                        p3.SetStart();                                                  //ustawienie boola
                        Piec3Thread.Start();                                            //uruchomienie watku
                        Plik.ZapiszDoLog("Start pieca 3\r\nOperator: " + uzytkownik);   //zapisanie danych do logu
                        file = string.Empty;                                            //kasowanie sciezki do pliku
                        program = 0;                                                    //zerowanie zmiennej programu
                    }
                    else                                                                //jezeli uzytkownik anulowal proces uruchamiania pieca
                    {
                        MessageBox.Show("Anulowano start cyklu.");                      //wyswietl komunikat i nic nie rob
                        return;
                    }
                }
                catch (Exception ex)                                                    //w przypadku bledu
                {
                    MessageBox.Show(ex.Message.ToString());                                     //wyswietl komunikat
                }
                finally
                {
                    Zlecenie.Dispose();                                                 //kasowanie zasobow uzywanych przez okno zlecenie
                }
            }
            else  //jezeli piec pracuje
            {
                try
                {
                    DialogResult res = MessageBox.Show("Piec jest w trakcie pracy!\r\nCzy na pewno chcesz zakończyć proces?", null, MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        if (Piec3Thread != null)     //w przypadku aktywnego watku - piec pracuje normalnie - dopisac okno wyboru czy uzytkownik jest pewien zatrzymania dla obu przypadków!!    
                        {
                            p3.SetStop();                                                   //zatrzymanie watku
                            StartStop_Cykl(3, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            Plik.ZapiszDoLog("Ręczne zatrzymanie pieca 3");                 //zapisanie danych do loga
                            MessageBox.Show("Ręczne zatrzymanie cyklu wygrzewania");        //wyswietlenie komunikatu
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika(kontynuacja pracy programu po wylaczeniu)
                            {
                                securityLevel = 0;
                            }
                        }
                        else                        //w przypadku awaryjnego zatrzymania pieca - awaria programu itp....
                        {
                            StartStop_Cykl(3, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            p3s = false;                                                    //ustawienie zmiennej stanu regulatora
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika
                            {
                                securityLevel = 0;
                            }
                            //przeslac rozkaz przerwania regulacji do sterownika, wyswietlic komunikat z informacjami, zapis do Log 
                            Plik.ZapiszDoLog("Awaryjne zatrzymanie pieca 3");               //zapisanie danych do logu
                        }
                    }
                    else { return; }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    ModbusRTU.StartStop_Cykl(3, 0, false);                          //przesłanie rozkazu bezpośrednio do sterownika
                    p3s = false;                                                    //ustawienie zmiennej stanu regulatora
                }
            }
        }
        private void b_start_p4_Click(object sender, EventArgs e)
        {
            if (b_start_p4.Text == "Start Piec 4")  //jezeli piec nie pracuje
            {
                Zlecenie = new ZlecenieStart(uzytkownik, "Piec 4");                //otwiera okienko w ktorym wpisuje sie nazwy zlecen i wybiera program
                try
                {
                    DialogResult res = Zlecenie.ShowDialog();                           //jezeli wsystko sie zgadza rozpoczynam proces
                    if (res == DialogResult.OK)
                    {
                        pomiarWykres4.Clear();                                               //czyszczenie listy z pomiarami
                        p4 = null;                                                      //kasowanie obiektu pieca
                        p4 = new Piec4(file, 4, program);                                               //tworzenie nowego obiektu pieca
                        Piec4Thread = null;                                             //kasowanie zasobów watku
                        WykresStart(4);                                   //kasowanie wykresu
                        Piec4Thread = new Thread(() => p4.PiecWatek());    //tworzenie nowego watku, przekazanie danych 
                        p4.SetStart();                                                  //ustawienie boola
                        Piec4Thread.Start();                                            //uruchomienie watku
                        Plik.ZapiszDoLog("Start pieca 4\r\nOperator: " + uzytkownik);   //zapisanie danych do logu
                        file = string.Empty;                                            //kasowanie sciezki do pliku
                        program = 0;                                                    //zerowanie zmiennej programu
                    }
                    else                                                                //jezeli uzytkownik anulowal proces uruchamiania pieca
                    {
                        MessageBox.Show("Anulowano start cyklu.");                      //wyswietl komunikat i nic nie rob
                        return;
                    }
                }
                catch (Exception ex)                                                    //w przypadku bledu
                {
                    MessageBox.Show(ex.Message.ToString());                                     //wyswietl komunikat
                }
                finally
                {
                    Zlecenie.Dispose();                                                 //kasowanie zasobow uzywanych przez okno zlecenie
                }
            }
            else  //jezeli piec pracuje
            {
                try
                {
                    DialogResult res = MessageBox.Show("Piec jest w trakcie pracy!\r\nCzy na pewno chcesz zakończyć proces?", null, MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        if (Piec4Thread != null)     //w przypadku aktywnego watku - piec pracuje normalnie - dopisac okno wyboru czy uzytkownik jest pewien zatrzymania dla obu przypadków!!    
                        {
                            p4.SetStop();                                                   //zatrzymanie watku
                            StartStop_Cykl(4, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            Plik.ZapiszDoLog("Ręczne zatrzymanie pieca 4");                 //zapisanie danych do loga
                            MessageBox.Show("Ręczne zatrzymanie cyklu wygrzewania");        //wyswietlenie komunikatu
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika(kontynuacja pracy programu po wylaczeniu)
                            {
                                securityLevel = 0;
                            }
                        }
                        else                        //w przypadku awaryjnego zatrzymania pieca - awaria programu itp....
                        {
                            StartStop_Cykl(4, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            p4s = false;                                                    //ustawienie zmiennej stanu regulatora
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika
                            {
                                securityLevel = 0;
                            }
                            //przeslac rozkaz przerwania regulacji do sterownika, wyswietlic komunikat z informacjami, zapis do Log 
                            Plik.ZapiszDoLog("Awaryjne zatrzymanie pieca 4");               //zapisanie danych do logu
                        }
                    }
                    else { return; }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    ModbusRTU.StartStop_Cykl(4, 0, false);                          //przesłanie rozkazu bezpośrednio do sterownika
                    p4s = false;                                                    //ustawienie zmiennej stanu regulatora
                }
            }
        }
        private void b_start_p5_Click(object sender, EventArgs e)
        {
            if (b_start_p5.Text == "Start Piec 5")  //jezeli piec nie pracuje
            {
                Zlecenie = new ZlecenieStart(uzytkownik, "Piec 5");                //otwiera okienko w ktorym wpisuje sie nazwy zlecen i wybiera program
                try
                {
                    DialogResult res = Zlecenie.ShowDialog();                           //jezeli wsystko sie zgadza rozpoczynam proces
                    if (res == DialogResult.OK)
                    {
                        pomiarWykres5.Clear();                                               //czyszczenie listy z pomiarami
                        p5 = null;                                                      //kasowanie obiektu pieca
                        p5 = new Piec5(file, 5, program);                                               //tworzenie nowego obiektu pieca
                        Piec5Thread = null;                                             //kasowanie zasobów watku
                        WykresStart(5);                                   //kasowanie wykresu
                        Piec5Thread = new Thread(() => p5.PiecWatek());    //tworzenie nowego watku, przekazanie danych 
                        p5.SetStart();                                                  //ustawienie boola
                        Piec5Thread.Start();                                            //uruchomienie watku
                        Plik.ZapiszDoLog("Start pieca 5\r\nOperator: " + uzytkownik);   //zapisanie danych do logu
                        file = string.Empty;                                            //kasowanie sciezki do pliku
                        program = 0;                                                    //zerowanie zmiennej programu
                    }
                    else                                                                //jezeli uzytkownik anulowal proces uruchamiania pieca
                    {
                        MessageBox.Show("Anulowano start cyklu.");                      //wyswietl komunikat i nic nie rob
                        return;
                    }
                }
                catch (Exception ex)                                                    //w przypadku bledu
                {
                    MessageBox.Show(ex.Message.ToString());                                     //wyswietl komunikat
                }
                finally
                {
                    Zlecenie.Dispose();                                                 //kasowanie zasobow uzywanych przez okno zlecenie
                }
            }
            else  //jezeli piec pracuje
            {
                try
                {
                    DialogResult res = MessageBox.Show("Piec jest w trakcie pracy!\r\nCzy na pewno chcesz zakończyć proces?", null, MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        if (Piec5Thread != null)     //w przypadku aktywnego watku - piec pracuje normalnie - dopisac okno wyboru czy uzytkownik jest pewien zatrzymania dla obu przypadków!!    
                        {
                            p5.SetStop();                                                   //zatrzymanie watku
                            StartStop_Cykl(5, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            Plik.ZapiszDoLog("Ręczne zatrzymanie pieca 5");                 //zapisanie danych do loga
                            chartPiec5.DataSource = null;                                   //kasowanie wykresu
                            MessageBox.Show("Ręczne zatrzymanie cyklu wygrzewania");        //wyswietlenie komunikatu
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika(kontynuacja pracy programu po wylaczeniu)
                            {
                                securityLevel = 0;
                            }
                        }
                        else                        //w przypadku awaryjnego zatrzymania pieca - awaria programu itp....
                        {
                            StartStop_Cykl(5, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                            p5s = false;                                                    //ustawienie zmiennej stanu regulatora
                            if (securityLevel == 0)                                         //jezeli nie bylo zalogowanego uzytkownika - przycisk znika
                            {
                                securityLevel = 0;
                            }
                            //przeslac rozkaz przerwania regulacji do sterownika, wyswietlic komunikat z informacjami, zapis do Log 
                            Plik.ZapiszDoLog("Awaryjne zatrzymanie pieca 5");               //zapisanie danych do logu
                        }
                    }
                    else { return; }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    ModbusRTU.StartStop_Cykl(5, 0, false);                          //przesłanie rozkazu bezpośrednio do sterownika
                    p5s = false;                                                    //ustawienie zmiennej stanu regulatora
                }
            }
        }
        /// <summary>
        /// Przycisk wczytywania parametrów programu z regulatora
        /// Sprawdzic, czy regulator nie pracuje aktualnie!!
        /// Mutex? - w klasie modbusa, port musi być zamkniety!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Wcz_Prog_Click(object sender, EventArgs e)
        {
            try
            {
                Pr_Wcz_Prog.Enabled = false;
                ProgBox.SelectionMode = SelectionMode.One;
                ProgBox.DataSource = null;
                if (comboUrzadzenia.SelectedIndex < 0)
                {
                    MessageBox.Show("Wybierz urzadzenie!");
                    Pr_Wcz_Prog.Enabled = true;
                    return;
                }
                string urz = comboUrzadzenia.SelectedItem.ToString();
                byte adres;
                int program;
                ushort rejestr;
                int offsetprog = 114;
                int offset;
                //zapelnienie listy parametrami
                switch (urz)
                {
                    case "Piec1":
                        adres = 1;
                        break;
                    case "Piec2":
                        adres = 2;
                        break;
                    case "Piec3":
                        adres = 3;
                        break;
                    case "Piec4":
                        adres = 4;
                        break;
                    case "Piec5":
                        adres = 5;
                        break;
                    default:
                        MessageBox.Show("Wybierz urzadzenie");
                        return;
                }
                program = comboProgram.SelectedIndex + 1;
                if (program < 1)
                {
                    MessageBox.Show("Wybierz program!");
                    return;
                }
                program--;                              //ile razy bede mnozyc offset
                offsetprog *= program;                  //o ile przesunac rejestry
                offset = offsetprog + 4170;             //koncowa wartosc rejestru poczatkowego
                rejestr = Convert.ToUInt16(offset);     //konwersja na ushort
                byte[] ramka = ModbusRTU.FormujRamke(adres, 3, rejestr, 114);   //formowanie ramki
                ushort[] results = ModbusRTU.Odbierz_Dane(ramka, 500);          //odbiór danych
                                                                                //pobierz wartosci z regulatora i wyswietl w listboxie - prosty sposób
                for (int i = 0; i < LProg.Count; i++)
                {
                    LProg[i].Wartosc = results[LProg[i].Indeks];
                }
                ProgBox.Items.Clear();
                ProgBox.DisplayMember = "Wyswietl";
                ProgBox.ValueMember = "Wartosc";
                ProgBox.DataSource = LProg;
                Pr_Wcz_Prog.Enabled = true;
                ProgBox.SelectionMode = SelectionMode.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            /*WCZYTANE WARTOSCI PARAMETROW POZOSTAJA W PAMIECI PO WCZYTANIU ICH Z URZADZENIA*/
        }

        private void Pr_ProgZAP_Click(object sender, EventArgs e)
        {
            //proces odwrotny do odczytywania parametrów
            ushort parametr;
            int program = comboProgram.SelectedIndex;               //zeby zmienic program trzeba bedzie wcisnac przycisk dezaktywujacy mozliwosc wybrania urzadzenia i programu (anti-idiot)
            string urz = comboUrzadzenia.SelectedItem.ToString();   //wybrane urzadzenie
            int index = ProgBox.SelectedIndex;                      //wybrany parametr
            string param = LProg[index].Parametr;
            if (index < 0)
            {
                MessageBox.Show("Wybierz parametr do modyfikacji!");
                return;
            }
            if (ProgParamBox.TextLength == 0)
            {
                MessageBox.Show("Wprowadź wartość parametru!");
                return;
            }
            try
            {
                parametr = Convert.ToUInt16(ProgParamBox.Text);     //sprawdzic czy to co wpisalem jest liczba
            }
            catch (Exception)
            {
                MessageBox.Show("Wprowadzona wartość musi być liczba!");
                return;
            }
            byte adres = 0;
            ushort rejestr;
            int offsetprog = 114;
            int offset;
            switch (urz)
            {
                case "Piec1":
                    adres = 1;
                    break;
                case "Piec2":
                    adres = 2;
                    break;
                case "Piec3":
                    adres = 3;
                    break;
                case "Piec4":
                    adres = 4;
                    break;
                case "Piec5":
                    adres = 5;
                    break;
            }
            try
            {
                //mam wybrane urzadzenie, program, parametr
                offsetprog *= program;                  //114 * numer programu
                offset = offsetprog + 4170;             //4170 + przesuniecie o wartosc programu - pierwszy rejestr danego programu
                offset += LProg[index].Indeks;          //koncowy adres rejestru do ktorego bede zapisywac parametr  np 4170 + it[6](11 -> 7 parametr) = 4181
                rejestr = Convert.ToUInt16(offset);     //konwersja na ushort
                byte[] ramka = FormujRamke(adres, 6, rejestr, parametr); //formowanie ramki - wybrany adres, kod funkcji 6, wyliczony rejestr i wpisany parametr
                int kod = Modbus.Zapisz_Rejestr(ramka);
                if (kod == 2)
                {
                    MessageBox.Show("Błąd zapisu parametru!\nPodana wartość jest poza zakresem!", "Błąd");
                    return;
                }
                else if (kod == 4)
                {
                    MessageBox.Show($"Błąd połączenia/zapisu parametru\r\nKod błędu: {kod}", "Błąd");
                    return;
                }
                //niech pobiera nazwe parametru
                Plik.ZapiszDoLog($"{urz}\r\nZmiana parametru programu nr {program + 1} \r\n{param} na: {parametr} (Rejestr nr {rejestr})");
                //dopisac obsluge kodu!!
                LProg[index].Wartosc = parametr;
                ProgBox.DataSource = null;
                Thread.Sleep(10);
                ProgBox.Items.Clear();
                ProgBox.DataSource = LProg;
                ProgBox.DisplayMember = "Wyswietl";
                ProgBox.ValueMember = "Wartosc";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Przycisk [TEST] - zakładka debug.
        /// Przycisk do sprawdzania działania metod.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Zapisz_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            string plik = string.Concat(path, "\\Data\\procesy.dat");
            //using (StreamWriter file = new StreamWriter(plik, true))
            //{
            //    file.WriteLine($"Piec 1,{path}\\Data\\20200728_180722.bin");
            //    file.WriteLine($"Piec 2,{path}20200728_180722.bin");
            //    file.WriteLine($"Piec 3,{path}20200728_180722.bin");
            //    file.WriteLine($"Piec 4,{path}20200728_180722.bin");
            //    file.WriteLine($"Piec 5,{path}20200728_180722.bin");
            //}
            string[] lista = File.ReadAllLines(plik).Where(l => l.StartsWith("Piec 1")).ToArray();
            lista = lista[0].Split(',');
            plik = lista[1];
        }
        /// <summary>
        /// Funkcja otwiera plik log - zakładka debug
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_OtworzLog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("log.txt");
        }
        /// <summary>
        /// Przycisk logowania/wylogowania uzytkownika.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Login_Click(object sender, EventArgs e)
        {
            if (securityLevel == 0)
            {
                using (Form Login = new Form3())
                {
                    Login.ShowDialog();
                }
            }
            else
            {
                uzytkownik = "Brak";
                securityLevel = 0;
                User_Label.Text = uzytkownik;
                Securitylvl_Label.Text = securityLevel.ToString();
            }
        }
        /// <summary>
        /// Przycisk odswiezenia listy uzytkowników.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pr_RefreshUsers_Click(object sender, EventArgs e)
        {
            uzytkownicyBox.DataSource = null;
            userCombo.DataSource = null;
            uzytkownicyBox.Items.Clear();
            userCombo.Items.Clear();
            List<Uzytkownicy> dane = Plik.PobierzUzytkownikow();
            try
            {
                uzytkownicyBox.DisplayMember = "Wyswietl";
                uzytkownicyBox.ValueMember = "Seclevel";
                uzytkownicyBox.DataSource = dane;
                userCombo.DisplayMember = "Nazwa";
                userCombo.DataSource = dane;
                pr_EditUser.Enabled = true;
                pr_DeleteUser.Enabled = true;
                pr_NewUser.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        /// <summary>
        /// Dodawanie nowego użytkownika
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pr_NewUser_Click(object sender, EventArgs e)
        {
            if (addUsernameBox.TextLength > 0)
            {
                if (addPassBox.TextLength > 0)
                {
                    if (addSeclevelBox.TextLength > 0)
                    {
                        int seclevel = Convert.ToInt32(addSeclevelBox.Text);
                        if (seclevel < 3 && seclevel > 0)  //poziom dostępu musi mieścić się pomiędzy 1 a 2, nie ma możliwości utworzenia drugiego konta administratora
                        {
                            bool wynik = Uzytkownik.Dodaj_Uzytkownika(addUsernameBox.Text, addPassBox.Text, seclevel);
                            if (wynik)
                            {
                                pr_RefreshUsers.PerformClick();
                                addUsernameBox.Text = "";
                                addPassBox.Text = "";
                                addSeclevelBox.Text = "";
                                MessageBox.Show("Pomyślnie dodano użytkownika");
                            }
                            else
                            {
                                MessageBox.Show("Wystapił bład - uzytkownik nie dodany");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Wpisz prawidłową wartość poziomu dostępu!\n(1 - użytkownik , 2 - technolog/automatyk)");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Podaj poziom dostępu!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Podaj hasło!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Podaj nazwę użytkownika!");
                return;
            }
        }
        /// <summary>
        /// Usuwanie użytkownika
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pr_DeleteUser_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Czy na pewno chcesz usunąć wybranego użytkownika?", "", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                int ind = userCombo.SelectedIndex; //przerzucic indeks do listy i usunac uzytkownika z tego indeksu
                if (ind == 0)
                {
                    MessageBox.Show("Nie można usunać konta Administratora!");
                    return;
                }
                else if (ind > 0)
                {
                    bool wynik = Uzytkownik.Usun_Uzytkownika(ind);
                    if (wynik)
                    {
                        pr_RefreshUsers.PerformClick();
                        MessageBox.Show("Pomyślnie usunięto użytkownika");
                    }
                    else
                    {
                        MessageBox.Show("Wystapił błąd - nie można usunąć użytkownika");
                    }
                }
                else
                {
                    MessageBox.Show("Proszę wybrać użytkownika!");
                    return;
                }
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// Przycisk zmiany hasła, badź poziomu dostępu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pr_EditUser_Click(object sender, EventArgs e)
        {

            int ind = userCombo.SelectedIndex;
            string haslo = "";
            int seclevel = -1;
            if (ind == 0)
            {
                MessageBox.Show("Brak mozliwości zmiany hasła/poziomu dla konta administratora!");
                return;
            }
            else
            {
                if (EditPasswordBox.Text.Length > 0)
                {
                    haslo = EditPasswordBox.Text;
                    EditPasswordBox.Text = "";
                }
                if (EditSeclevelBox.Text.Length > 0)
                {
                    int sec = Convert.ToInt32(EditSeclevelBox.Text);
                    if (sec < 0 || sec > 2)
                    {
                        MessageBox.Show("Podaj prawidłową wartość poziomu dostępu!\n(1 - użytkownik, 2 - technolog/automatyk)");
                        return;
                    }
                    seclevel = sec;
                    EditSeclevelBox.Text = "";
                }
                bool wynik = Uzytkownik.Edytuj_Uzytkownika(ind, haslo, seclevel);
                if (wynik)
                {
                    MessageBox.Show("Pomyślnie zmieniono dane użytkownika");
                }
                else
                {
                    MessageBox.Show("Brak danych do zmiany");
                }
            }

        }
        /// <summary>
        /// Otwiera folder, w ktorym znajduje sie program - zakładka Debug
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_OpenDirectory_Click(object sender, EventArgs e)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
        /// <summary>
        /// Przycisk wczytywania programu z pliku - gdybysmy chcieli miec jeszcze wiecej programów niż 15.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Prog_Wcz_Z_Pliku_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            ProgBox.SelectionMode = SelectionMode.One;
            try
            {
                using (OpenFileDialog fileDialog = new OpenFileDialog())
                {
                    fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                    fileDialog.Filter = "Binary files(*.bin)|*.bin";
                    fileDialog.FilterIndex = 1;
                    fileDialog.RestoreDirectory = true;
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        path = fileDialog.FileName;
                    }

                }
                LProg = Plik.Otworz_Program(path);
                ProgBox.DataSource = null;
                ProgBox.Items.Clear();
                ProgBox.DataSource = LProg;
                ProgBox.DisplayMember = "Wyswietl";
                Pr_Mod_Prog.Enabled = true;
                ProgBox.SelectionMode = SelectionMode.None;
            }
            catch (Exception)
            {
                MessageBox.Show("Wczytywanie anulowane");
            }
        }
        /// <summary>
        /// Przycisk zapisywania aktualnej listy parametrów programu do pliku .bin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Prog_Zap_DoPliku_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            try
            {
                using (SaveFileDialog fileDialog = new SaveFileDialog())
                {
                    fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                    fileDialog.Filter = "Binary files(*.bin)|*.bin";
                    fileDialog.FilterIndex = 1;
                    fileDialog.RestoreDirectory = true;
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        path = fileDialog.FileName;
                    }

                }
                Plik.ZapiszProgram(path, LProg);
            }
            catch (Exception)
            {
                MessageBox.Show("Zapis anulowany");
            }

        }

        /// <summary>
        /// Metoda wywoływana jest podczas zmiany właściwości DataSource.
        /// Aktywuje i dezaktywuje przyciski zapisywania do pliku.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgBox_DataSourceChanged(object sender, EventArgs e)
        {
            if (ProgBox.DataSource == LProg)
            {
                Pr_Prog_Zap_DoPliku.Enabled = true;
                Pr_EditProgParam.Enabled = true;
            }
            else
            {
                Pr_Prog_Zap_DoPliku.Enabled = false;
                Pr_EditProgParam.Enabled = true;
            }
        }
        /// <summary>
        /// Przycisk zapisu wszystkich parametrów programu bezpośrednio do regulatora.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_ProgZAP_REG_Click(object sender, EventArgs e)
        {
            Pr_ProgZAP_REG.Enabled = false;
            int program = comboProgram.SelectedIndex;               //wybrany program
            string urz = comboUrzadzenia.SelectedItem.ToString();   //wybrane urzadzenie
            byte adres = 0;
            ushort rejestr;
            int offsetprog = 114;
            int offset;
            switch (urz)
            {
                case "Piec1":
                    adres = 1;
                    break;
                case "Piec2":
                    adres = 2;
                    break;
                case "Piec3":
                    adres = 3;
                    break;
                case "Piec4":
                    adres = 4;
                    break;
                case "Piec5":
                    adres = 5;
                    break;
            }
            try
            {
                //mam wybrane urzadzenie, program, parametr
                offsetprog *= program;                  //114 * numer programu
                offset = offsetprog + 4170;             //4170 + przesuniecie o wartosc programu - pierwszy rejestr danego programu
                rejestr = Convert.ToUInt16(offset);     //konwersja na ushort
                byte[] ramka = ModbusRTU.FormujRamke(adres, 3, rejestr, 114);   //formowanie ramki
                ushort[] results = ModbusRTU.Odbierz_Dane(ramka, 500);          //odbiór danych
                for (int i = 0; i < LProg.Count; i++)                                   //przepisanie tylko wybranych danych z listy do wektora parametrów (reszta parametrów bez zmian)
                {
                    results[LProg[i].Indeks] = LProg[i].Wartosc;
                }
                ramka = ModbusRTU.FormujRamkeREJ(adres, rejestr, results); //formowanie ramki - wybrany adres, kod funkcji 6, wyliczony rejestr poczatkowy i parametry
                int kod = ModbusRTU.Zapisz_Rejestry(ramka);
                if (kod == 0)
                {
                    MessageBox.Show("Zapis zakończony powodzeniem");
                }
                else if (kod == 2)
                {
                    MessageBox.Show("Błąd zapisu parametrów!\nSprawdź wartości parametrów - czy nie sa poza zakresem!");
                    return;
                }
                else if (kod == 3)
                {
                    MessageBox.Show("Błąd zapisu parametrów!\nSprawdź wartości parametrów - czy nie sa poza zakresem!");
                    return;
                }
                else
                {
                    MessageBox.Show("Błąd zapisu parametrów!\nSprawdź regulator i jego połączenie!");
                    return;
                }
                //niech pobiera nazwe parametru
                Plik.ZapiszDoLog($"{urz}\r\nZmiana wszystkich parametrów programu nr {program + 1} \r\n");
                //dopisac obsluge kodu!!
                //LProg[index].Wartosc = parametr;
                ProgBox.DataSource = null;
                Thread.Sleep(10);
                ProgBox.Items.Clear();
                ProgBox.DataSource = LProg;
                ProgBox.DisplayMember = "Wyswietl";
                ProgBox.ValueMember = "Wartosc";
                Pr_ProgZAP_REG.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        /// <summary>
        /// Przycisk modyfikacji parametru bez wgrywania go do regulatora.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_EditProgParam_Click(object sender, EventArgs e)
        {
            ushort parametr;
            int index = ProgBox.SelectedIndex;                      //wybrany parametr
            if (index < 0)
            {
                MessageBox.Show("Wybierz parametr do modyfikacji!");
                return;
            }
            if (ProgParamBox.TextLength == 0)
            {
                MessageBox.Show("Wprowadź wartość parametru!");
                return;
            }
            try
            {
                parametr = Convert.ToUInt16(ProgParamBox.Text);     //sprawdzic czy to co wpisalem jest liczba
            }
            catch (Exception)
            {
                MessageBox.Show("Wprowadzona wartość musi być liczbą!");
                return;
            }
            try
            {
                LProg[index].Wartosc = parametr;
                ProgBox.DataSource = null;
                ProgBox.Items.Clear();
                ProgBox.DataSource = LProg;
                ProgBox.DisplayMember = "Wyswietl";
                ProgBox.ValueMember = "Wartosc";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Przycisk odczytu pojedynczego rejestru - z karty Ustawienia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Rej_Czytaj_Click(object sender, EventArgs e)
        {
            byte adres = Convert.ToByte(textBox3.Text);                 //konwersja adresu regulatora na format bajtowy
            ushort rejestr = Convert.ToUInt16(textBox2.Text);           //konwersja numeru rejestru na 16 bitowy int
            byte[] ramka = FormujRamke(adres, 3, rejestr, 1);           //formowanie zapytania
            UInt16[] dane = Odbierz_Dane(ramka, 50);                    //odbiór danych z regulatora
            if (dane != null)                                           //jeżeli otrzymamy jakieś dane
            {
                textBox1.Text = dane[0].ToString();                     //wpisane danych do pola tekstowego
            }
            else                                                        //wyświetlenie komunikatu w przypadku braku otrzymanych danych
            {
                MessageBox.Show("Brak danych do wyświetlenia\nSprawdź poprawność wpisanych danych i połączenie z regulatorem!", "Błąd");
            }
        }

        /// <summary>
        /// Przycisk zapisu nazwy recepty - dla listy wyboru dostępnych recept.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_RecZap_Click(object sender, EventArgs e)
        {
            try
            {
                string nazwa = ReceptaBox.Text;          //pobranie nazwy z pola tekstowego - przyjmie wszystko, co się do niego wpisze
                int index = comboProgram.SelectedIndex;  //pobranie indexu - numeru recepty do zapisu
                recepty = Plik.PobierzRecepty();         //pobranie listy nazw recept
                recepty[index].Recept = nazwa;           //przypisanie nowej nazwy do wybranej recepty
                Plik.ZapiszRecepty(recepty);             //zapisanie zaktualizowanej listy do pliku
                comboProgram.DataSource = null;          //odświeżenie interfejsu
                comboProgram.Items.Clear();
                comboProgram.DataSource = recepty;
                comboProgram.DisplayMember = "Wyswietl";
                Plik.ZapiszDoLog("Zmiana nazwy recepty\r\nProgram numer " + (index + 1) + " na " + nazwa);
            }
            catch (Exception ex)                         //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }

        /// <summary>
        /// Wywolanie przy wejsciu na kartę "Ustawienia"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPage6_Enter(object sender, EventArgs e)
        {
            pr_NewUser.Enabled = false;
            pr_EditUser.Enabled = false;
            pr_DeleteUser.Enabled = false;
        }
        /// <summary>
        /// Wywoła się, gdy użytkownik wybierze kartę "Programy"
        /// Przypisanie domyślnych wartości i właściwości do elementów interfejsu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPage7_Enter(object sender, EventArgs e)
        {
            ProgBox.SelectionMode = SelectionMode.None;
            comboUrzadzenia.Enabled = true;
            Pr_ProgZAP.Enabled = false;
            Pr_ProgZAP_REG.Enabled = false;
            comboProgram.Enabled = true;
            comboProgram.DataSource = recepty;
            comboProgram.DisplayMember = "Wyswietl";
            if (comboUrzadzenia.Items.Count > 0) { Pr_Wcz_Prog.Enabled = true; }
            else { Pr_Wcz_Prog.Enabled = false; }
            Pr_Mod_Prog.Text = "Rozpocznij edytowanie";

        }
        /// <summary>
        /// Obsługa przycisku otwierajacego okno z raportami
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_RaportOknoOtworz_Click(object sender, EventArgs e)
        {
            if (Raportowanie.Visible == false)   //jeżeli okno jest niewidoczne
            {
                Raportowanie = new Form4         //tworzenie nowego okna
                {
                    TopMost = true,              //właściwości okna - na wierzchu oraz wyświetlanie na środku ekranu
                    StartPosition = FormStartPosition.CenterScreen
                };
                Raportowanie.Show();             //wyświetlenie okna
            }
            else                                 //jeżeli okno jest widoczne - zabezpieczenie przed wielokrotnym otwarciem tego samego okna
            {
                Raportowanie.Close();            //zamknięcie okna
            }
        }
        /// <summary>
        /// Przycisk modyfikacji programu "Rozpocznij edytowanie"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Mod_Prog_Click(object sender, EventArgs e)
        {
            //aktywna lista wyboru pieca     && przynajmniej jedno urzadzenie   && wybrany piec z listy
            if (comboUrzadzenia.Enabled == true && comboUrzadzenia.Items.Count > 0 && comboUrzadzenia.SelectedIndex >= 0) //sprawdzenie, czy program nie jest aktualnie edytowany - funkcja przycisku "Rozpocznij edytowanie"
            {

                Pr_Wcz_Prog.PerformClick();                  //odświeżenie danych recepty (wczytanie z regulatora)
                ProgBox.SelectionMode = SelectionMode.One;   //aktywacja możliwości wyboru parametru z listy
                comboUrzadzenia.Enabled = false;             //dezaktywacja listy wyboru pieca (zmiana pieca podczas edytowania programu jest niemożliwa)
                comboProgram.Enabled = false;                //dezaktywacja listy wyboru recepty
                Pr_Wcz_Prog.Enabled = false;                 //dezaktywacja przycisku pobrania recepty z regulatora
                Pr_ProgZAP.Enabled = true;                   //aktywacja przycisku zapisu pojedynczego parametru do regulatora
                Pr_ProgZAP_REG.Enabled = true;               //aktywacja przycisku zapisu wszystkich parametrów do regulatora
                Pr_Mod_Prog.Text = "Zakończ edytowanie";     //ustawienie opisu przycisku i zmiana jego funkcjonalności

            }
            else                                            //jeżeli przycisk był wciśnięty i program jest w trakcie edytowania - funkcja przycisku "Zakończ edytowanie"
            {
                ProgBox.SelectionMode = SelectionMode.None;          //dezaktywacja możliwości wyboru parametrów z listy
                comboUrzadzenia.Enabled = true;                      //aktywacja listy wyboru pieca
                comboProgram.Enabled = true;                         //aktywacja listy wyboru programu
                Pr_Wcz_Prog.Enabled = true;                          //aktywacja przycisku pobrania recepty z regulatora
                Pr_ProgZAP.Enabled = false;                          //dezaktywacja przycisku zapisu pojedynczego parametru do regulatora
                Pr_ProgZAP_REG.Enabled = false;                      //dezaktywacja przycisku zapisu wszystkich parametrów do regulatora
                Pr_Mod_Prog.Text = "Rozpocznij edytowanie";          //ustawienie opisu przycisku i zmiana jego funkcjonalności
            }
        }
        /// <summary>
        /// Metody wywoływane przy rozruchu programu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.SendToBack();                  //przeniesienie okna głównego 'na drugi plan' pod okienko Startera
            if (!Plik.Katalog())                 //sprawdzenie obecności plików i katalogów niezbędnych do działania programu
            {
                this.Close();                   //jeżeli wystąpi błąd podczas sprawdzania katalogu - wyłącz program
            }
            recepty = Plik.PobierzRecepty();    //pobranie nazw recept i przypisanie ich do numerów programu
            securityLevel = 0;                  //ustawienie domyślnego poziomu dostępu programu, spowoduje to również zadziałanie metody właczajacej menusy i przyciski
            uzytkownik = "Brak";                //ustawienie domyślnego użytkownika programu
            //portCOM = "COM12";
        }
        /// <summary>
        /// EventHandler klasy Timer - co 500ms odświeża odczyty z regulatorów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Czasomierz_Tick(object sender, EventArgs e)
        {
            label_tzm_p1.Text = tzP1.ToString();            //aktualizacja temperatury zmierzonej
            label_tzad_p1.Text = tSP1.ToString();           //aktualizacja temperatury zadanej
            label_nrodc_p1.Text = odcp1.ToString();         //aktualizacja numeru odcinka
            label_todch_p1.Text = godzodcp1.ToString();     //aktualizacja czasu odcinka (godziny)
            label_todcm_p1.Text = minodcp1.ToString();      //aktualizacja czasu odcinka (minuty)
            if (status1 > 0)                                //jeżeli piec jest obecny
            {
                OB1 = true;                                 //ustawienie flagi obecności pieca
            }
            else
            {
                OB1 = false;                                //w przeciwnym wypadku wylaczenie flagi obecności pieca
            }
            label_tzm_p2.Text = tzP2.ToString();            //*******analogicznie dla pozostałych pieców********//
            label_tzad_p2.Text = tSP2.ToString();
            label_nrodc_p2.Text = odcp2.ToString();
            label_todch_p2.Text = godzodcp2.ToString();
            label_todcm_p2.Text = minodcp2.ToString();
            if (status2 > 0)
            {
                OB2 = true;
            }
            else
            {
                OB2 = false;
            }
            label_tzm_p3.Text = tzP3.ToString();
            label_tzad_p3.Text = tSP3.ToString();
            label_nrodc_p3.Text = odcp3.ToString();
            label_todch_p3.Text = godzodcp3.ToString();
            label_todcm_p3.Text = minodcp3.ToString();
            if (status3 > 0)
            {
                OB3 = true;
            }
            else
            {
                OB3 = false;
            }
            label_tzm_p4.Text = tzP4.ToString();
            label_tzad_p4.Text = tSP4.ToString();
            label_nrodc_p4.Text = odcp4.ToString();
            label_todch_p4.Text = godzodcp4.ToString();
            label_todcm_p4.Text = minodcp4.ToString();
            if (status4 > 0)
            {
                OB4 = true;
            }
            else
            {
                OB4 = false;
            }
            label_tzm_p5.Text = tzP5.ToString();
            label_tzad_p5.Text = tSP5.ToString();
            label_nrodc_p5.Text = odcp5.ToString();
            label_todch_p5.Text = godzodcp5.ToString();
            label_todcm_p5.Text = minodcp5.ToString();
            if (status5 > 0)
            {
                OB5 = true;
            }
            else
            {
                OB5 = false;
            }
        }
        /// <summary>
        /// Funkcja wywoływana z Form2 (Starter). Na podstawie otrzymanych danych uruchamia watki kontynuujace prace wybranych piecow.
        /// Sprawdzany jest plik procesy.dat, czy proces byl uruchomiony na danym piecu. Jeżeli tak:
        /// pobiera dotychczasowe dane i zapisuje je dalej do tego samego pliku, uruchamia watek pieca i wyswietla dane na wykresie, tak jak w przypadku normalnego watku.
        /// </summary>
        /// <param name="p1s"></param>
        /// <param name="p2s"></param>
        /// <param name="p3s"></param>
        /// <param name="p4s"></param>
        /// <param name="p5s"></param>
        public void SetStart(bool p1start, bool p2start, bool p3start, bool p4start, bool p5start)
        {
            if (p1start)
            {
                string plik = Plik.PobierzProces("Piec 1");                 //sprawdzam, czy w pliku jest zapisany proces
                if (plik != null)                                           //jeżeli program znajdzie proces
                {
                    //PomiarP1 = Plik.PobierzPomiar(plik);                    //pobierz dotychczasowe dane do listy                  
                    ObservableCollection<Pomiar> pomiary = Plik.PobierzPomiarWykres(plik);
                    pomiarWykres1 = new ObservableCollection<Pomiar>(pomiary);
                    WykresStart(1);
                    p1 = new Piec1(plik, 1);                                       //tworzenie nowego obiektu dla kontynuacji (nie mam numeru programu)
                    p1.SetContinue();                                       //ustawienie zmiennej pracy w pętli
                    Piec1Thread = new Thread(() => p1.PiecKontynuuj()); //praca na watku kontynuujacym proces
                    Piec1Thread.Start();                                    //start watku
                }
                else                                                        //jeżeli nie ma zapisanego procesu, a program widzi, że regulator jest w trakcie pracy
                {
                    Regulacja_V2.PotwZatrz potw = new Regulacja_V2.PotwZatrz("Piec nr 1");
                    DialogResult res = potw.ShowDialog();
                    if(res == DialogResult.Yes)
                    {
                        StartStop_Cykl(1, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                        p1s = false;
                    }
                    potw.Dispose();
                    //w przeciwnym wypadku nic nie rób
                }
            }
            if (p2start)
            {
                string plik = Plik.PobierzProces("Piec 2");                 //sprawdzam, czy w pliku jest zapisany proces
                if (plik != null)                                        //jeżeli program znajdzie proces
                {
                    ObservableCollection<Pomiar> pomiary = Plik.PobierzPomiarWykres(plik);
                    pomiarWykres2 = new ObservableCollection<Pomiar>(pomiary);
                    WykresStart(2);
                    p2 = new Piec2(plik, 2);                                       //tworzenie nowego obiektu
                    Piec2Thread = new Thread(() => p2.PiecKontynuuj()); //praca na watku kontynuujacym proces
                    p2.SetContinue();                                       //ustawienie zmiennej pracy w pętli
                    Piec2Thread.Start();                                    //start watku
                }
                else                                                        //jeżeli nie ma zapisanego procesu, a program widzi, że regulator jest w trakcie pracy
                {
                    Regulacja_V2.PotwZatrz potw = new Regulacja_V2.PotwZatrz("Piec nr 2");
                    DialogResult res = potw.ShowDialog();
                    if (res == DialogResult.Yes)
                    {
                        StartStop_Cykl(2, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                        p2s = false;
                    }
                    potw.Dispose();
                }

            }
            if (p3start)
            {
                string plik = Plik.PobierzProces("Piec 3");                 //sprawdzam, czy w pliku jest zapisany proces
                if (plik != null)                                        //jeżeli program znajdzie proces
                {
                    ObservableCollection<Pomiar> pomiary = Plik.PobierzPomiarWykres(plik);
                    pomiarWykres3 = new ObservableCollection<Pomiar>(pomiary);
                    WykresStart(3);
                    p3 = new Piec3(plik, 3);                                       //tworzenie nowego obiektu
                    Piec3Thread = new Thread(() => p3.PiecKontynuuj()); //praca na watku kontynuujacym proces
                    p3.SetContinue();                                       //ustawienie zmiennej pracy w pętli
                    Piec3Thread.Start();                                    //start watku
                }
                else                                                        //jeżeli nie ma zapisanego procesu, a program widzi, że regulator jest w trakcie pracy
                {
                    Regulacja_V2.PotwZatrz potw = new Regulacja_V2.PotwZatrz("Piec nr 3");
                    DialogResult res = potw.ShowDialog();
                    if (res == DialogResult.Yes)
                    {
                        StartStop_Cykl(3, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                        p3s = false;
                    }
                    potw.Dispose();
                }

            }
            if (p4start)
            {
                string plik = Plik.PobierzProces("Piec 4");                 //sprawdzam, czy w pliku jest zapisany proces
                if (plik != null)                                        //jeżeli program znajdzie proces
                {
                    ObservableCollection<Pomiar> pomiary = Plik.PobierzPomiarWykres(plik);
                    pomiarWykres4 = new ObservableCollection<Pomiar>(pomiary);
                    WykresStart(4);
                    p4 = new Piec4(plik, 4);                                       //tworzenie nowego obiektu
                    Piec4Thread = new Thread(() => p4.PiecKontynuuj()); //praca na watku kontynuujacym proces
                    p4.SetContinue();                                       //ustawienie zmiennej pracy w pętli
                    Piec4Thread.Start();                                    //start watku
                }
                else                                                        //jeżeli nie ma zapisanego procesu, a program widzi, że regulator jest w trakcie pracy
                {
                    Regulacja_V2.PotwZatrz potw = new Regulacja_V2.PotwZatrz("Piec nr 4");
                    DialogResult res = potw.ShowDialog();
                    if (res == DialogResult.Yes)
                    {
                        StartStop_Cykl(4, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                        p4s = false;
                    }
                    potw.Dispose();
                }

            }
            if (p5start)
            {
                string plik = Plik.PobierzProces("Piec 5");                 //sprawdzam, czy w pliku jest zapisany proces
                if (plik != null)                                           //jeżeli program znajdzie proces
                {
                    ObservableCollection<Pomiar> pomiary = Plik.PobierzPomiarWykres(plik);
                    pomiarWykres5 = new ObservableCollection<Pomiar>(pomiary);
                    WykresStart(5);
                    p5 = new Piec5(plik, 5);                                       //tworzenie nowego obiektu
                    Piec5Thread = new Thread(() => p5.PiecKontynuuj()); //praca na watku kontynuujacym proces
                    p5.SetContinue();                                       //ustawienie zmiennej pracy w pętli
                    Piec5Thread.Start();                                    //start watku
                }
                else                                                        //jeżeli nie ma zapisanego procesu, a program widzi, że regulator jest w trakcie pracy
                {
                    Regulacja_V2.PotwZatrz potw = new Regulacja_V2.PotwZatrz("Piec nr 5");
                    DialogResult res = potw.ShowDialog();
                    if (res == DialogResult.Yes)
                    {
                        StartStop_Cykl(5, 0, false);                                    //przesłanie rozkazu bezpośrednio do sterownika
                        p5s = false;
                    }
                    potw.Dispose();
                }

            }
            pomiarWykres1.CollectionChanged += this.Wykres1_Update;
            pomiarWykres2.CollectionChanged += this.Wykres2_Update;
            pomiarWykres3.CollectionChanged += this.Wykres3_Update;
            pomiarWykres4.CollectionChanged += this.Wykres4_Update;
            pomiarWykres5.CollectionChanged += this.Wykres5_Update;
        }
        /// <summary>
        /// Druga wersja funkcji, zmienia tylko obecnosc jednego urzadzenia.
        /// </summary>
        /// <param name="piec"></param>
        /// <param name="numer"></param>
        public void SetBool(bool stan, int piec)
        {
            int i = piec;
            switch (i)
            {
                case 1:
                    OB1 = stan;
                    break;
                case 2:
                    OB2 = stan;
                    break;
                case 3:
                    OB3 = stan;
                    break;
                case 4:
                    OB4 = stan;
                    break;
                case 5:
                    OB5 = stan;
                    break;
            };
        }
        /// <summary>
        /// Inicjalizacja wykresu przebiegu temperatur dla wybranego pieca
        /// <param name="piec"> Numer urzadzenia do uruchomienia</param>
        /// </summary>
        public void WykresStart(int piec)
        {
            int index = piec - 1;
            //try
            //{
            //    switch (piec)                                       //w zaleznosci od pieca aktywowany jest wykres
            //    {
            //        case 1:
            //            //pomiarWykres1.Clear();
            //            chartPiec1.DataSource = null;               //usunięcie przypisania danych do wykresu
            //            chartPiec1.Series.Clear();                  //czyszczenie wykresu
            //            chartPiec1.Series.Add(new Series("Temp Zm") //dodanie serii - temperatura zmierzona (czerwona linia)
            //            {
            //                XValueMember = "X",                     //przypisanie wyświetlanych danych z listy dla osi X 
            //                YValueMembers = "TempZm",               //przypisanie wyświetlanych danych z listy dla osi Y  
            //                XValueType = ChartValueType.String,     //typ danych osi X 
            //                YValueType = ChartValueType.Auto,       //typ danych osi Y 
            //                ChartType = SeriesChartType.FastLine,   //rodzaj wykresu (linii) 
            //                Color = Color.Red                       //kolor linii
            //            });
            //            chartPiec1.Series.Add(new Series("Temp Zad")//dodanie serii - temperatura zadana (zielona linia)
            //            {
            //                XValueMember = "X",                     //przypisanie wyświetlanych danych z listy dla osi X 
            //                YValueMembers = "TempZad",              //przypisanie wyświetlanych danych z listy dla osi Y  
            //                XValueType = ChartValueType.String,     //typ danych osi X 
            //                YValueType = ChartValueType.Auto,       //typ danych osi Y 
            //                ChartType = SeriesChartType.FastLine,   //rodzaj wykresu (linii)
            //                Color = Color.Green                     //kolor linii
            //            });
            //            chartPiec1.AlignDataPointsByAxisLabel();    //przyporzadkowanie punktów X do Y (ustawienie w jednej linii)
            //            chartPiec1.DataSource = pomiarWykres1;           //przypisanie danych z listy do wykresu
            //            break;
            //        case 2:                                         //******reszta analogicznie**********
            //            chartPiec2.DataSource = null;
            //            chartPiec2.Series.Clear();
            //            chartPiec2.Series.Add(new Series("Temp Zm")
            //            {
            //                XValueMember = "X",
            //                YValueMembers = "TempZm",
            //                XValueType = ChartValueType.String,
            //                YValueType = ChartValueType.Auto,
            //                ChartType = SeriesChartType.FastLine,
            //                Color = Color.Red
            //            });
            //            chartPiec2.Series.Add(new Series("Temp Zad")
            //            {
            //                XValueMember = "X",
            //                YValueMembers = "TempZad",
            //                XValueType = ChartValueType.String,
            //                YValueType = ChartValueType.Auto,
            //                ChartType = SeriesChartType.FastLine,
            //                Color = Color.Green
            //            });
            //            chartPiec2.AlignDataPointsByAxisLabel();
            //            chartPiec2.DataSource = PomiarP2;
            //            break;
            //        case 3:
            //            chartPiec3.DataSource = null;
            //            chartPiec3.Series.Clear();
            //            chartPiec3.Series.Add(new Series("Temp Zm")
            //            {
            //                XValueMember = "X",
            //                YValueMembers = "TempZm",
            //                XValueType = ChartValueType.String,
            //                YValueType = ChartValueType.Auto,
            //                ChartType = SeriesChartType.FastLine,
            //                Color = Color.Red
            //            });
            //            chartPiec3.Series.Add(new Series("Temp Zad")
            //            {
            //                XValueMember = "X",
            //                YValueMembers = "TempZad",
            //                XValueType = ChartValueType.String,
            //                YValueType = ChartValueType.Auto,
            //                ChartType = SeriesChartType.FastLine,
            //                Color = Color.Green
            //            });
            //            chartPiec3.AlignDataPointsByAxisLabel();
            //            chartPiec3.DataSource = PomiarP3;
            //            break;
            //        case 4:
            //            chartPiec4.DataSource = null;
            //            chartPiec4.Series.Clear();
            //            chartPiec4.Series.Add(new Series("Temp Zm")
            //            {
            //                XValueMember = "X",
            //                YValueMembers = "TempZm",
            //                XValueType = ChartValueType.String,
            //                YValueType = ChartValueType.Auto,
            //                ChartType = SeriesChartType.FastLine,
            //                Color = Color.Red
            //            });
            //            chartPiec4.Series.Add(new Series("Temp Zad")
            //            {
            //                XValueMember = "X",
            //                YValueMembers = "TempZad",
            //                XValueType = ChartValueType.String,
            //                YValueType = ChartValueType.Auto,
            //                ChartType = SeriesChartType.FastLine,
            //                Color = Color.Green
            //            });
            //            chartPiec4.AlignDataPointsByAxisLabel();
            //            chartPiec4.DataSource = PomiarP4;
            //            break;
            //        case 5:
            //            chartPiec5.DataSource = null;
            //            chartPiec5.Series.Clear();
            //            chartPiec5.Series.Add(new Series("Temp Zm")
            //            {
            //                XValueMember = "X",
            //                YValueMembers = "TempZm",
            //                XValueType = ChartValueType.String,
            //                YValueType = ChartValueType.Auto,
            //                ChartType = SeriesChartType.FastLine,
            //                Color = Color.Red
            //            });
            //            chartPiec5.Series.Add(new Series("Temp Zad")
            //            {
            //                XValueMember = "X",
            //                YValueMembers = "TempZad",
            //                XValueType = ChartValueType.String,
            //                YValueType = ChartValueType.Auto,
            //                ChartType = SeriesChartType.FastLine,
            //                Color = Color.Green
            //            });
            //            chartPiec5.AlignDataPointsByAxisLabel();
            //            chartPiec5.DataSource = PomiarP5;
            //            break;
            //    }
            //}

            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //    return;
            //}
            wykresy[index].DataSource = null;               //usunięcie przypisania danych do wykresu
            wykresy[index].Series.Clear();                  //czyszczenie wykresu
            wykresy[index].Series.Add(new Series("Temp Zm") //dodanie serii - temperatura zmierzona (czerwona linia)
            {
                XValueMember = "X",                     //przypisanie wyświetlanych danych z listy dla osi X 
                YValueMembers = "TempZm",               //przypisanie wyświetlanych danych z listy dla osi Y  
                XValueType = ChartValueType.String,     //typ danych osi X 
                YValueType = ChartValueType.Auto,       //typ danych osi Y 
                ChartType = SeriesChartType.FastLine,   //rodzaj wykresu (linii) 
                Color = Color.Red                       //kolor linii
            });
            wykresy[index].Series.Add(new Series("Temp Zad")//dodanie serii - temperatura zadana (zielona linia)
            {
                XValueMember = "X",                     //przypisanie wyświetlanych danych z listy dla osi X 
                YValueMembers = "TempZad",              //przypisanie wyświetlanych danych z listy dla osi Y  
                XValueType = ChartValueType.String,     //typ danych osi X 
                YValueType = ChartValueType.Auto,       //typ danych osi Y 
                ChartType = SeriesChartType.FastLine,   //rodzaj wykresu (linii)
                Color = Color.Green                     //kolor linii
            });
            wykresy[index].AlignDataPointsByAxisLabel();    //przyporzadkowanie punktów X do Y (ustawienie w jednej linii)
            switch (index)
            {
                case 0:
                    wykresy[index].DataSource = pomiarWykres1;
                    break;
                case 1:
                    wykresy[index].DataSource = pomiarWykres2;
                    break;
                case 2:
                    wykresy[index].DataSource = pomiarWykres3;
                    break;
                case 3:
                    wykresy[index].DataSource = pomiarWykres4;
                    break;
                case 4:
                    wykresy[index].DataSource = pomiarWykres5;
                    break;
            }


        }
        /// <summary>
        /// Aktualizacja wykresu przebiegu temperatury wewnatrz pieca.
        /// Przy wskazaniu nazwy pliku - zapisuje dane do pliku.
        /// </summary>
        /// <param name="tzm"> Temperatura zmierzona </param>
        /// <param name="SP"> Temperatura zadana </param>
        /// <param name="piec"> Numer pieca wywolujacego funkcje </param>
        /// <param name="plik">Sciezka pliku do zapisu danych </param>
        public void WykresDodaj(float tzm, float SP, int piec, string plik)
        {
            try
            {
                switch (piec)
                {
                    case 1:
                        Invoke(new Action(() => { PomiarP1.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm)); })); //dodaj dane do listy
                        Invoke(new Action(() => { chartPiec1.DataBind(); }));                                                   //aktualizuj wykres
                        //chartPiec1.DataBind();
                        Plik.ZapiszPomiar(plik, PomiarP1);                                       //zapis pomiarow do pliku (nadpisanie)
                        break;
                    case 2:                                                                      //**reszta analogicznie**//
                        PomiarP2.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm));
                        Invoke(new Action(() => { chartPiec2.DataBind(); }));
                        Plik.ZapiszPomiar(plik, PomiarP2);
                        break;
                    case 3:
                        PomiarP3.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm));
                        Invoke(new Action(() => { chartPiec3.DataBind(); }));
                        Plik.ZapiszPomiar(plik, PomiarP3);
                        break;
                    case 4:
                        PomiarP4.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm));
                        Invoke(new Action(() => { chartPiec4.DataBind(); }));
                        Plik.ZapiszPomiar(plik, PomiarP4);
                        break;
                    case 5:
                        PomiarP5.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm));
                        Invoke(new Action(() => { chartPiec5.DataBind(); }));
                        Plik.ZapiszPomiar(plik, PomiarP5);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }
        /// <summary>
        /// Funkcja zapisujaca ostatni pomiar do pliku - wywoływana na zakończenie procesu.
        /// </summary>
        /// <param name="piec">Numer pieca, z którego jest wywoływana funkcja</param>
        /// <param name="plik">Sciezka do pliku, w którym maja byc zapisane dane</param>
        public void WykresZakoncz(int piec, string plik)
        {
            switch (piec)
            {
                case 1:
                    Plik.ZapiszPomiar(plik, PomiarP1);//zapis pomiarow do pliku (nadpisanie)
                    break;
                case 2:                               //**reszta analogicznie**//
                    Plik.ZapiszPomiar(plik, PomiarP2);
                    break;
                case 3:
                    Plik.ZapiszPomiar(plik, PomiarP3);
                    break;
                case 4:
                    Plik.ZapiszPomiar(plik, PomiarP4);
                    break;
                case 5:
                    Plik.ZapiszPomiar(plik, PomiarP5);
                    break;
                default:
                    return;
            }
        }
        /// <summary>
        /// Aktualizacja wykresu przebiegu temperatury wewnatrz pieca.
        /// Wersja bez zapisu danych do pliku.
        /// </summary>
        /// <param name="tzm"> Temperatura zmierzona </param>
        /// <param name="SP"> Temperatura zadana </param>
        /// <param name="piec"> Numer pieca wywolujacego funkcje </param>
        public void WykresDodaj(float tzm, float SP, int piec)
        {
            try
            {
                switch (piec)
                {
                    case 1:
                        Invoke(new Action(() => { pomiarWykres1.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm)); }));  //dodaj dane do listy (data, temp zmierzona, temp zadana)
                        //Invoke(new Action(() => { chartPiec1.DataBind(); }));                                                    //aktualizuj wykres
                        break;                                                                                                   //**reszta analogicznie**
                    case 2:
                        Invoke(new Action(() => { pomiarWykres2.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm)); }));  //dodaj dane do listy (data, temp zmierzona, temp zadana)
                        //Invoke(new Action(() => { chartPiec2.DataBind(); }));
                        break;
                    case 3:
                        Invoke(new Action(() => { pomiarWykres3.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm)); }));  //dodaj dane do listy (data, temp zmierzona, temp zadana)
                        //Invoke(new Action(() => { chartPiec3.DataBind(); }));
                        break;
                    case 4:
                        Invoke(new Action(() => { pomiarWykres4.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm)); }));  //dodaj dane do listy (data, temp zmierzona, temp zadana)
                        //Invoke(new Action(() => { chartPiec4.DataBind(); }));
                        break;
                    case 5:
                        Invoke(new Action(() => { pomiarWykres5.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, tzm)); }));  //dodaj dane do listy (data, temp zmierzona, temp zadana)
                        //Invoke(new Action(() => { chartPiec5.DataBind(); }));
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }

        private void Wykres1_Update(object sender, NotifyCollectionChangedEventArgs e)
        {
            chartPiec1.DataBind();
        }
        private void Wykres2_Update(object sender, NotifyCollectionChangedEventArgs e)
        {
            chartPiec2.DataBind();
        }
        private void Wykres3_Update(object sender, NotifyCollectionChangedEventArgs e)
        {
            chartPiec3.DataBind();
        }
        private void Wykres4_Update(object sender, NotifyCollectionChangedEventArgs e)
        {
            chartPiec4.DataBind();
        }
        private void Wykres5_Update(object sender, NotifyCollectionChangedEventArgs e)
        {
            chartPiec5.DataBind();
        }


    }
}
