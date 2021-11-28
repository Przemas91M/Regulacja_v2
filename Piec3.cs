using System;
using System.Threading;
using static Regulacja_v2.Form1;
using static Regulacja_v2.ModbusRTU;

namespace Regulacja_v2
{
    /// <summary>
    /// KLASA WATKU PIECA 1
    /// W czasie swojego dzialania pobiera z interfejsu odczytane wartosci temperatury, statusu regulacji.
    /// Pobrane dane sa wyswietlane na wykresie i nastepnie zapisywane co 60 cykli do pliku.
    /// Gdy regulator zakonczy swoje dzialanie - watek jest zatrzymywany.
    /// </summary>
    class Piec3 : Piec
    {
        public Piec3(string file, int numer, int program) : base(file, numer, program)
        { }
        public Piec3(string file, int numer) : base(file, numer)
        { }
        public void PiecWatek()
        {
            Static.p3s = true;
            pomiary.Clear();
            int wynik = StartStop_Cykl(numer, program, true);
            if (wynik == 0)
            {
                Static.status3 = 1;
            }
            else
            {
                start = false;
                System.Windows.Forms.MessageBox.Show("Nieudany start procesu!/r/nProces zostaje przerwany");
            }
            while (start) //modyfikuj ja przyciskiem albo program - gdy skonczy wygrzewanie(flaga)
            {
                try
                {
                    ////pobierz dane z regulatora
                    status = Static.status3;
                    temp1 = Static.tzP3;
                    SP = Static.tSP3;
                    Watek();
                }
                catch (ThreadAbortException e)
                {
                    System.Windows.Forms.MessageBox.Show(e.ToString());
                }
                catch (Exception ex)
                {
                    //pokaz co sie dzieje w razie bledu i zamknij port oraz mutex, zeby inny watek mogl skorzystac
                    System.Windows.Forms.MessageBox.Show(ex.ToString());            //wyswietlenie bledu
                    Zakoncz();
                    Thread.CurrentThread.Abort();                                   //zabicie awaryjne watku
                    return;                                                         //wyjscie z watku
                }
                Thread.Sleep(2000);
            }
            Zakoncz();
            Static.p3s = false;                                                     //ustawienie interfejsu
            return;                                                                 //wyjscie z watku
        }
        /// <summary>
        /// Pobrać nazwę pliku, wczytać z pliku dane, wrzucić do wykresu (Wykres start), reszta watku taka sama.
        /// Piec jest w trakcie pracy, ma wszystko ustawione, program wykrył go jako pracujacy - ustawiony przycisk stopu.
        /// </summary>
        /// <param name="file"></param>
        public void PiecKontynuuj()
        {
            pomiary.Clear();
            try
            {
                pomiary = Plik.PobierzPomiar(plik);
                //Static.Invoke(new Action(() => { Static.WykresStart(1); }));               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message.ToString());
                Thread.CurrentThread.Abort();                                   //zabicie awaryjne watku
                return;
            }
            while (altstart) //modyfikuj ja przyciskiem albo program - gdy skonczy wygrzewanie(flaga)
            {
                try
                {
                    //pobierz dane z regulatora//
                    status = Static.status3;
                    temp1 = Static.tzP3;
                    SP = Static.tSP3;
                    Watek();
                }
                catch (Exception ex)
                {
                    //pokaz co sie dzieje w razie bledu i zamknij port oraz mutex, zeby inny watek mogl skorzystac
                    System.Windows.Forms.MessageBox.Show(ex.ToString());            //wyswietlenie bledu
                    Zakoncz();
                    Thread.CurrentThread.Abort();                                   //zabicie awaryjne watku
                    return;                                                         //wyjscie z watku
                }
                Thread.Sleep(2000);
            }
            Zakoncz();
            Static.p3s = false;                                                     //ustawienie interfejsu
            return;                                                                 //wyjscie z watku

        }

    }
}