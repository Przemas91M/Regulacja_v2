using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Regulacja_v2
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        readonly Plik Plik = new Plik();
        List<Pomiar> wyniki;//napisac metode pobierajaca dane - w kliknieciu przycisku (oczywiscie sprawdzic, czy jest co zaladowac)
        List<Raporty> raporty = new List<Raporty>();
        bool wszystkie, rok, miesiac, dzien;
        /// <summary>
        /// Przycisk wyświetlajacy wykres wybranego procesu wygrzewania.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int index = procesyBox.SelectedIndex;
                if (index >= 0)
                {
                    string path = System.IO.Directory.GetCurrentDirectory();
                    string nazwa = raporty[index].Nazwapliku;
                    string start = raporty[index].Start.ToString();
                    string stop = raporty[index].Stop.ToString();
                    string piec = raporty[index].Piec;
                    string program = raporty[index].Program;
                    string piecowy = raporty[index].User;
                    string[] zlecenia = raporty[index].Zlecenia.ToArray();
                    path += "\\Data\\" + nazwa + ".bin";
                    wyniki = Plik.PobierzPomiar(path);
                    using (Form Wykres = new Form5(wyniki, nazwa, start, stop, program, piec, zlecenia, piecowy))
                    {
                        Wykres.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Wybierz proces!", "UWAGA");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Metoda wywoływana podczas otwierania okna.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form4_Load(object sender, EventArgs e)
        {
            //raporty = Plik.PobierzRaporty();
            //zleceniaBox.DataSource = null;
            //procesyBox.DisplayMember = "Wyswietl_Raport_Krotki";
            //procesyBox.DataSource = raporty;
            comboRegulatory.SelectedIndex = 0;
            comboSortuj.SelectedIndex = 0;
            checkAll.Checked = true;
        }
        /// <summary>
        /// Metoda wywoływana, gdy użytkownik wybierze element z listy dostępnych raportów.
        /// Wyświetla parametry procesu w oknie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void procesyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = procesyBox.SelectedIndex;                       //pobranie indeksu wybranego elementu
            labelProg.Text = raporty[index].Program;                    //Wyświetlenie nazwy wykonywanego programu
            labelOperator.Text = raporty[index].User;                   //Wyświetlenie nazwy użytkownika wykonywujacego proces 
            labelProcesStart.Text = raporty[index].Start.ToString();    //Wyświetlenie daty i godziny rozpoczecia procesu
            labelProcesStop.Text = raporty[index].Stop.ToString();      //Wyświetlenie daty i godziny zakończenia procesu
            zleceniaBox.DataSource = raporty[index].Zlecenia;           //Przekazanie listy zlecen do listBoxa
        }
        /// <summary>
        /// Poniższe 4 funkcje odpowiadaja za zaznaczanie i odznaczanie kratek wyboru zakresu czasu.
        /// Zaznaczajac jedna opcje, odznaczaja sie pozostale.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAll.Checked == true)
            {
                wszystkie = true;
                checkDay.Checked = false;
                checkMonth.Checked = false;
                checkYear.Checked = false;
            }
            else
            {
                wszystkie = false;
            }
        }
        private void checkDay_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDay.Checked == true)
            {
                dzien = true;
                checkMonth.Checked = false;
                checkYear.Checked = false;
                checkAll.Checked = false;
            }
            else
            {
                dzien = false;
            }
        }
        private void checkMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (checkMonth.Checked == true)
            {
                miesiac = true;
                checkDay.Checked = false;
                checkYear.Checked = false;
                checkAll.Checked = false;
            }
            else
            {
                miesiac = false;
            }
        }
        private void checkYear_CheckedChanged(object sender, EventArgs e)
        {
            if (checkYear.Checked == true)
            {
                rok = true;
                checkDay.Checked = false;
                checkAll.Checked = false;
                checkMonth.Checked = false;
            }
            else
            {
                rok = false;
            }
        }
        /// <summary>
        /// Metoda wywoływana przy zamykaniu okna.
        /// Czyszczenie zasobów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }
        /// <summary>
        /// Funkcja filtrujaca wyniki wyszukiwania.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pr_Szukaj_Raporty_Click(object sender, EventArgs e)
        {
            try
            {
                bool pominszukanie = false;
                string urz = "", szukaj = "";
                bool all = false;
                DateTime zakresDat;
                List<Raporty> listafiltrowana = new List<Raporty>();
                if (textBoxSzukaj.TextLength > 0)       //sprawdzanie, czy jest cos wpisane w textbox
                {
                    szukaj = textBoxSzukaj.Text;        //zapis wyszukiwanego tekstu do zmiennej
                }
                else
                {
                    pominszukanie = true;               //brak informacji w textboxie - pomijamy szukanie
                }
                switch (comboRegulatory.SelectedIndex)  //przypisuje wybrane urzadzenie lub sprawdza po wszystkich
                {
                    case 0:
                        all = true;
                        break;
                    case 1:
                        urz = "Piec 1";
                        break;
                    case 2:
                        urz = "Piec 2";
                        break;
                    case 3:
                        urz = "Piec 3";
                        break;
                    case 4:
                        urz = "Piec 4";
                        break;
                    case 5:
                        urz = "Piec 5";
                        break;
                }
                if (wszystkie || rok || miesiac || dzien)       //sprawdzam, czy jest zaznaczony kwadrat z zakresem czasu
                {
                    zakresDat = Kalendarz.SelectionStart.Date;  //jezeli tak, pobieram zaznaczona date z kalendarza
                    raporty = Plik.PobierzRaporty();            //pobieram dane raportów

                    //***sortowanie po dacie***//
                    if (wszystkie)                              //brak zakresu daty - wyswietl wszystko
                    {
                        listafiltrowana = raporty;
                    }
                    else if (rok)                               //filtrowanie wyników z wybranego roku
                    {
                        listafiltrowana = raporty.FindAll(x => x.Start.Year == zakresDat.Year); //wyszukuje wszystkie procesy z zaznaczonego roku
                    }
                    else if (miesiac)                           //filtrowanie wyników z wybranego miesiąca
                    {
                        listafiltrowana = raporty.FindAll(x => x.Start.Month == zakresDat.Month && x.Start.Year == zakresDat.Year); //wyszukuje wszystkie procesy z danego miesiaca
                    }
                    else if (dzien)                             //filtrowanie wyników z wybranego dnia
                    {
                        listafiltrowana = raporty.FindAll(x => x.Start.Date == zakresDat.Date); //wyszukuje wszystkie procesy z danego dnia
                    }
                    //***wyszukiwanie specyficznego urzadzenia***//
                    if (!all)                                   //filtrowanie wyników z wybranego urządzenia
                    {
                        listafiltrowana = listafiltrowana.FindAll(x => x.Piec == urz);          //wyszukuje procesy konkretnego pieca
                    }
                    //***wyszukiwanie po filtrze***//
                    if (!pominszukanie)                         //jezeli nie szukam czegos specyficznego, mam wybrane tylko urządzenie i datę
                    {
                        if (comboSortuj.SelectedIndex == 0)     //szukam po numerze zlecenia
                        {
                            Raporty wynik = listafiltrowana.FirstOrDefault(x => x.Zlecenia.Any(y => y.Contains(szukaj)));   //szukam konkretnego zlecenia
                            listafiltrowana.Clear();            //czyszczenie listy
                            if (wynik != null)                  //jezeli zostaly znalezione jakies wyniki
                            {
                                listafiltrowana.Add(wynik);     //zapis znalezionych wynikow do listy
                            }
                        }
                        else if (comboSortuj.SelectedIndex == 1)//szukam po nazwie recepty
                        {
                            listafiltrowana = listafiltrowana.FindAll(x => x.Program == szukaj);    //szukam procesow wedlug danego programu wygrzewania
                        }
                        else if (comboSortuj.SelectedIndex == 2)//szukam po operatorze
                        {
                            listafiltrowana = listafiltrowana.FindAll(x => x.User == szukaj);       //szukam procesow, ktore wykonywal wybrany operator
                        }
                    }
                    raporty = listafiltrowana;                              //przypisanie znalezionych wynikow do koncowej listy
                    procesyBox.DataSource = raporty;                        //przesłanie wyników do okna wyświetlajacego listę
                    procesyBox.DisplayMember = "Wyswietl_Raport_Krotki";    //przypisanie nazwy wyswietlanej z klasy typu listy
                    zleceniaBox.DataSource = null;                          //czyszczenie okienka wyświetlajacego zlecenia  
                    if (raporty.Count == 0)     //gdy lista nie zawiera zadnych wynikow
                    {
                        MessageBox.Show("Nie znaleziono żadnych wyników pasujacych\r\ndo wybranego kryterium wyszukiwania!", "Błąd");   //wyswietlenie komunikatu
                    }
                }
                else    //gdy nie zostal wybrany zakres czasu wyszukiwania
                {
                    MessageBox.Show("Wybierz zakres czasu wyszukiwania\r\n(Rok, miesiac, dzień)","Błąd");  //wyswietlenie komunikatu
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
