using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using static Regulacja_v2.Form1;

namespace Regulacja_v2
{
    public partial class Form2 : Form
    {
        /// <summary>
        /// Okienko ustawiania portu COM konwertera, szukania urzadzen.
        /// Wszystko jest robione manualnie - mozna to zrobic automatycznie na stalym porcie, w razie problemu mozna go zmienic
        /// Gdyby nie znalazło wszystkich urzadzen wyswietlanie monitu i ewentualne wyswietlenie tego okienka - koncepcja
        /// </summary>
        public Form2()
        {
            InitializeComponent();
        }
        private int ilosc = 0;                  
        private bool OB1, OB2, OB3, OB4, OB5;
        private bool p1s, p2s, p3s, p4s, p5s;
        readonly ModbusRTU Modbus = new ModbusRTU();
        /// <summary>
        /// Przycisk wyszukiwania portów COM na danym komputerze.
        /// Automatycznie wybierany jest port o najwyższym numerze.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Com_PB_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();                            //czyszczenie listy
            Array COM_Ports = SerialPort.GetPortNames();        //pobranie portów z systemu
            if (COM_Ports.Length == 0)                          //gdy nie znajdzie zadnego portu
            {
                MessageBox.Show("Nie znaleziono żadnego portu szeregowego!\nSprawdź ustawienia systemu Windows.");
                return;
            }
            foreach (string port in COM_Ports)                  //dla kazdego znalezionego portu:
            {
                comboBox1.Items.Add(port);                      //dodanie portu do listy                             
            }
            int ind = comboBox1.Items.Count;                    //pobranie ilości znalezionych portów
            comboBox1.SelectedIndex = ind - 1;                  //wybranie ostatniego portu z listy
            Pr_ID.Enabled = true;                               //aktywacja przycisku szukania regulatorów
        }
        /// <summary>
        /// Funkcja uruchamiana podczas inicjalizacji okna - wyszukuje port, zaznacza port o najwiekszym numerze i wyszukuje regulatory.
        /// </summary>
        public void Szukaj()
        {
            Com_PB.PerformClick();  //pobranie listy portów z systemu
            if (Pr_ID.Enabled == true)
            {
                Pr_ID.PerformClick();   //wyszukiwanie urzadzeń
            }
            /*while(!connected) //zablokuj tok programu, dopóki nie będzie ustanowione połaczenie
            {

            }*/
        }
        /// <summary>
        /// Przycisk wyszukiwania urzadzen, sprawdzac podczas szukania albo po szukaniu, czy aktualnie pracuja!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_ID_Click(object sender, EventArgs e)
        {
            //bool status;
            Static.portCOM = comboBox1.SelectedItem.ToString(); //pobranie nazwy portu COM z listy
            Com_PB.Enabled = false;                             //zablokowanie przycisku wyszukiwania portów COM
            comboBox1.Enabled = false;                          //zablokowanie listy wyboru portów
            ilosc = 0;                                          //inicjalizacja zmiennej - ilosc regulatorów
            Pr_ID.Enabled = false;                              //zablokowanie przycisku szukania
            OB1 = OB2 = OB3 = OB4 = OB5 = false;                //inicjalizacja sygnalizatorów obecności regulatorów
            Static.comboUrzadzenia.Items.Clear();               //czyszczenie listy wyboru regulatora - karta "Programy"
            Static.comboUrzadzenia2.Items.Clear();              //czyszczenie listy wyboru regulatora - karta "Parametry regulatora"
            listPiece.Enabled = true;                           //odblokowanie listy z piecami (w ktorej zaznaczane sa kratki obecnosci poszczegolnych urzadzen)
            for (int i = 0; i < 5; i++)
            {
                listPiece.Items[i].Checked = false;             //usuniecie 'ptaszków' z kratek obecności
            }
            byte adres = 1;                                     //adres poczatkowy
            try
            {
                for (int i = 0; i <= 4; i++)
                {
                    byte[] bufor = Modbus.ID(adres);                                //sprawdzam obecnosc urzadzenia pod danym adresem (i)
                    if (bufor.Length > 0)                                           //jezeli otrzymalem odpowiedz
                    {
                        if (adres == bufor[0] && bufor[1] == 17 && bufor[2] == 17)  //interpretacja komunikatu
                        {
                            switch (adres)                                          //w zaleznosci od adresu odpalam kontrolke i ustawiam zmienne
                            {
                                case 1:
                                    listPiece.Items[0].Checked = true;              //zaznaczenie "ptaszka" w kratke obok pieca
                                    OB1 = true;                                     //flaga obecności
                                    ilosc++;                                        //inkrementacja zmiennej ilości urzadzeń
                                    break;
                                case 2:                                             /****Reszta analogicznie****/
                                    listPiece.Items[1].Checked = true;
                                    OB2 = true;
                                    ilosc++;
                                    break;
                                case 3:
                                    listPiece.Items[2].Checked = true;
                                    OB3 = true;
                                    ilosc++;
                                    break;
                                case 4:
                                    listPiece.Items[3].Checked = true;
                                    OB4 = true;
                                    ilosc++;
                                    break;
                                case 5:
                                    listPiece.Items[4].Checked = true;
                                    OB5 = true;
                                    ilosc++;
                                    break;
                            }
                        }
                    }
                    adres++;                                                        //przechodze do kolejnego adresu
                }
            }
            catch (Exception ex)                                                    //jezeli cos pojdzie nie tak - wyswietli komunikat
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            if (ilosc == 0)                                                         //jezeli nie wykryje zadnego urzadzenia  - decyzje podejmuje uzytkownik
            {
                DialogResult res = MessageBox.Show("Nie znaleziono żadnego urządzenia! \nSprawdź połączenie portu COM! \n\nPonowić wyszukiwanie?", "Błąd", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)                                        //ponowne wyszukanie 
                {
                    Pr_ID.Enabled = true;        //aktywacja przycisku szukania
                    Pr_ID.PerformClick();        //odpalenie obecnej funkcji jeszcze raz
                }
                else                                                                //nie chce wyszukiwac ponownie
                {
                    Com_PB.Enabled = true;       //aktywacja przycisku szukania portów
                    comboBox1.Enabled = true;    //aktywacja listy z portami
                    TopMost = true;         //powrót okienka na wierzch
                }
            }
            /*******Zostawiam to na wszelki wypadek - gdyby wymagane było potwierdzenie znalezienia pojedynczych pieców************
            else if (ilosc > 0 && ilosc < 5)                                        //nie ma wszystkich urzadzeń
            {
                DialogResult res = MessageBox.Show("Znaleziono " + ilosc + " obiekty.\nCzy chcesz ponowić wyszukiwanie?", "", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)                                        //szukam ponownie - jezeli znalazlo za malo urzadzen
                {
                    Pr_ID.Enabled = true;        //aktywacja przycisku szukania
                    Pr_ID.PerformClick();        //odpalenie obecnej funkcji jeszcze raz
                }
                else if (res == DialogResult.No)                                    //nie szukam - uaktywnia wszystkie kontrolki, mozliwa zmiana portu i ponowne szukanie na nowych parametrach
                {
                    status = SprawdzStatus();
                    if (status)
                    {
                        Pr_ID.Enabled = true;                       //aktywacja przycisku szukania
                        Com_PB.Enabled = true;                      //aktywacja przycisku portów
                        comboBox1.Enabled = true;                   //aktywacja listy portów
                        Static.comboUrzadzenia.SelectedIndex = 0;   //wybór pierwszego elementu z listy urzadzeń - karta "Programy"
                        Static.comboProgram.SelectedIndex = 0;      //wybór pierwszego elementu z listy programów - karta "Programy"
                        this.TopMost = true;                        //powrót okienka na wierzch
                    }
                    else
                    {
                        MessageBox.Show("Blad podczas sprawdzania statusu regulatora.");
                    }
                }
            }*************/
            else                                                    //jeżeli program znajdzie przynajmniej jeden regulator
            {
                //status = SprawdzStatus();
                if (SprawdzStatus())
                {
                    Pr_ID.Enabled = true;                           //aktywacja przycisku szukania
                    Com_PB.Enabled = true;                          //aktywacja przycisku portów
                    comboBox1.Enabled = true;                       //aktywacja listy portów
                    TopMost = true;                            //powrót okienka na wierzch
                }
                else
                {
                    MessageBox.Show("Błąd podczas sprawdzania statusu regulatora.");
                    return;
                }
            }
        }
        /// <summary>
        /// Funkcja sprawdza, które regulatory sa w trakcie pracy.
        /// </summary>
        private bool SprawdzStatus()
        {
            byte adr = 1;
            bool[] ob = {OB1, OB2, OB3, OB4, OB5};              //pobieranie wektora obecnych regulatorów
            bool[] ps = { false, false, false, false, false };  //tworzenie wektora 
            byte[] ramka;
            UInt16[] dane;
            try
            {
                for (int i = 0; i < ob.Length; i++)                         //dla każdego obecnego regulatora
                {
                    if (ob[i] == true)          
                    {
                        ramka = ModbusRTU.FormujRamke(adr, 3, 4154, 1);     //tworzę ramkę do odczytu stanu pracy regulatora
                        dane = ModbusRTU.Odbierz_Dane(ramka, 50);           //odbieram dane
                        if (dane[0] == 0 || dane[0] == 4)                   //regulator zatrzymany
                        {
                            listPiece.Items[i].BackColor = Color.Green;     //zaznaczenie regulatora na zielono
                            ps[i] = false;
                        }
                        else                                                //regulator w trakcie pracy
                        {
                            listPiece.Items[i].BackColor = Color.Red;       //zaznaczenie regulatora na czerwono
                            ps[i] = true;
                        }
                    }
                    adr++;
                }
                p1s = ps[0];                                                //przypisanie stanu pracy do odpowiednich zmiennych globalnych
                p2s = ps[1];
                p3s = ps[2];
                p4s = ps[3];
                p5s = ps[4];                                                           
                return true;                                               //ustawienie statusu wykonania funkcji
            }
            catch (Exception e)                                            //obsługa błędu
            {
                MessageBox.Show(e.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// Przycisk zamykajacy program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Form_Close_Click(object sender, EventArgs e)
        {
            Close();
            Static.Close();
        }
        /// <summary>
        /// Przycisk zatwierdzajacy wybrane opcje, chowajacy okienko
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Zatwierdz_Click(object sender, EventArgs e)
        {
            try
            {
                if (ilosc == 0)         //gdy nie przeszukano lub nie znaleziono zadnych urzadzen
                {
                    DialogResult res = MessageBox.Show("Nie znaleziono żadnych obiektów!\nCzy chcesz kontynuować?", "Uwaga", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)    //mozna uruchomic program zeby np podejrzec raporty, wszystko inne niemozliwe
                    {
                        Static.p1s = false;         //przypisanie zmiennych stanu pracy w głównym oknie - uruchomi to metody przypisujace zachowanie przyciskow start/stop
                        Static.p2s = false;
                        Static.p3s = false;
                        Static.p4s = false;
                        Static.p5s = false;
                        //Static.SetBool(OB1, OB2, OB3, OB4, OB5);   //ustawia boole obecnosci urzadzen
                        Static.securityLevel = 0;  //przypisanie poziomu dostepu - uruchamia to metode aktywujaca przyciski start/stop
                        MessageBox.Show("Możliwe jest przeglądanie raportów,\nwszelkie inne funkcje programu zablokowane!", "Uwaga");
                        this.Hide();               //ukrycie okna startera
                        Static.Enabled = true;     //aktywacja glownego okna programu
                    }
                    else if (res == DialogResult.No)//zamyka okienko i pozostawia okienko wyszukiwania urzadzen
                    {
                        return;
                    }
                }
                else                   //gdy znaleziono jakies urzadzenia - uzytkownik zatwierdza wybor (awaria ktoregos pieca itp)
                {
                    Static.p1s = p1s;                           //przypisanie zmiennych stanu pracy w głównym oknie - uruchomi to metody przypisujace zachowanie przyciskow start/stop
                    Static.p2s = p2s;
                    Static.p3s = p3s;
                    Static.p4s = p4s;
                    Static.p5s = p5s;
                    Static.securityLevel = 0;                   //przypisanie poziomu dostepu - uruchamia to metode aktywujaca przyciski start/stop
                    Hide();                                     //wylaczenie okienka
                    Static.Start_Zegara();                      //uruchomienie glownego watku zczytujacego dane z regulatorów
                    Static.Enabled = true;                      //odblokowanie glownego okna programu
                    Static.SetStart(p1s, p2s, p3s, p4s, p5s);   //uruchomienie watkow kontynuujacych prace pieca po wylaczeniu programu
                }
            }
            catch(Exception ex)                                 //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
                this.Close();
                Static.Close();
            }
        }
        /// <summary>
        /// Funkcja blokuje okno główne podczas otwierania programu do czasu znalezienia regulatora.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
            Static.Enabled = false;
        }
    }
}

