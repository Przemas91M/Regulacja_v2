using System;
using System.Collections.Generic;
using static Regulacja_v2.Form1;


namespace Regulacja_v2
{
    public class Piec
    {
        protected bool start = false, altstart = false;
        protected int status;
        protected int program, licznik = 0, numer = 0;
        protected float SP, temp1;
        protected List<Pomiar> pomiary = new List<Pomiar>();
        protected string plik;
        public Piec(string plik, int numer, int program) //konstruktor dla nowego cyklu
        {
            this.numer = numer;
            this.plik = plik;
            this.program = program;
        }

        public Piec(string plik, int numer)            //konstruktor dla kontynuacji cyklu (regulator już pracuje więc niepotrzebny jest numer programu)
        {
            this.plik = plik;
            this.numer = numer;
        }

        public void Watek()
        {
            if (status != 0)//sprawdzam, czy regulator jest obecny
            {
                if (licznik == 60)                                              //zapis danych co minutę - 480 zapisów (przy próbkowaniu sekundowym)
                {
                    pomiary.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, temp1)); //dorzucenie wyniku do listy
                    Plik.ZapiszPomiar(plik, pomiary);                                         //zapis pomiarow do pliku (nadpisanie)
                    Static.WykresDodaj(temp1, SP, numer);                                     //dopisanie wyniku do listy wykresu i aktualizacja wykresu
                    licznik = 0;                                                              //reset licznika
                }
                else                                                            //dodanie danych do listy i odswiezenie wykresu
                {
                    pomiary.Add(new Pomiar(DateTime.Now.ToString("dd-MM HH:mm"), SP, temp1));
                    Static.WykresDodaj(temp1, SP, numer);
                }
                if (status == 5)                                                //zakonczenie regulacji
                {
                    Plik.ZapiszPomiar(plik, pomiary);                           //końcowy zapis pomiaru do pliku                                         
                    SetStop();                                                  //zatrzymanie watku
                }
                licznik++;
            }
            

        }

        public void Zakoncz()
        {
            Plik.ZapiszPomiar(plik, pomiary);
            Plik.ZapiszOstatniPomiar(plik, numer);                              //zapis aktualnych danych do pliku z raportami
            SetStop();
        }

        public void SetStop()
        {
            start = false;
            altstart = false;
        }
        public void SetStart()
        {
            start = true;
            altstart = false;
        }
        public void SetContinue()
        {
            altstart = true;
            start = false;
        }



    }
}
