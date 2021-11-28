using System;
using static Regulacja_v2.ModbusRTU;
using static Regulacja_v2.Form1;
using System.Threading;
using System.Windows.Forms;

namespace Regulacja_v2
{
    /// <summary>
    /// KLASA WATKU GLOWNEGO PROGRAMU
    /// Ma on za zadanie odpytywac regulatory co okreslony czas o temperatury i status regulacji.
    /// W przypadku braku otrzymania danych, jest on oflagowany jako nieobecny.
    /// Jeżeli dane zaczna splywac - zostaje oflagowany jako obecny.
    /// </summary>
    class WatekGlowny
    {
        readonly byte[] ramkap1_1 = FormujRamke(1, 3, 4006, 3); //odczyt temperatury z czujnika, wejscia dodatkowego, wartosci zadanej
        readonly byte[] ramkap1_2 = FormujRamke(1, 3, 4153, 9); //odczyt danych odcinka, regulacji
        readonly byte[] ramkap2_1 = FormujRamke(2, 3, 4006, 3); //odczyt temperatury z czujnika, wejscia dodatkowego, wartosci zadanej
        readonly byte[] ramkap2_2 = FormujRamke(2, 3, 4153, 9); //odczyt danych odcinka, regulacji
        readonly byte[] ramkap3_1 = FormujRamke(3, 3, 4006, 3); //odczyt temperatury z czujnika, wejscia dodatkowego, wartosci zadanej
        readonly byte[] ramkap3_2 = FormujRamke(3, 3, 4153, 9); //odczyt danych odcinka, regulacji
        readonly byte[] ramkap4_1 = FormujRamke(4, 3, 4006, 3); //odczyt temperatury z czujnika, wejscia dodatkowego, wartosci zadanej
        readonly byte[] ramkap4_2 = FormujRamke(4, 3, 4153, 9); //odczyt danych odcinka, regulacji
        readonly byte[] ramkap5_1 = FormujRamke(5, 3, 4006, 3); //odczyt temperatury z czujnika, wejscia dodatkowego, wartosci zadanej
        readonly byte[] ramkap5_2 = FormujRamke(5, 3, 4153, 9); //odczyt danych odcinka, regulacji         
        private UInt16[] wynikp1_1, wynikp1_2, wynikp2_1, wynikp2_2, wynikp3_1, wynikp3_2, wynikp4_1, wynikp4_2, wynikp5_1, wynikp5_2; //wektory otrzymanych wyników
        bool start;
        private int t1, tSP, czas2, czasodc, godz, min, odc, status;
        float  SP, temp1;
        public void WatekStart()
        {
            while (start)
            {
                try
                {
                    //******PIEC 1 *******//
                    wynikp1_1 = Odbierz_Dane(ramkap1_1, 50);                               //pobiera temperature z czujnika
                    wynikp1_2 = Odbierz_Dane(ramkap1_2, 60);                               //dane regulacji
                    if (wynikp1_1 != null && wynikp1_2 != null)
                    {
                        t1 = Convert.ToInt16(wynikp1_1[0]);                                //pobranie temperatury mierzonej
                        tSP = Convert.ToInt16(wynikp1_1[2]);                               //pobranie temperatury zadanej
                        czas2 = Convert.ToInt32(wynikp1_2[7]);                             //pobranie czasu trwania programu
                        czasodc = Convert.ToInt32(wynikp1_2[3]);                           //pobranie czasu biezacego odcinka
                        status = Convert.ToInt16(wynikp1_2[1]);                            //pobranie statusu regulacji
                        odc = Convert.ToInt16(wynikp1_2[0]);                               //pobranie numeru realizowanego odcinka
                        temp1 = (float)t1 / 10;                                            //obliczenie temperatury zmierzonej
                        SP = (float)tSP / 10;                                              //obliczenie temperatury zadanej
                                                                                           //procent = (((float)czas - (float)czas2) / (float)czas) * 100;    //przeliczenie ile % programu zostało już wykonane
                        czas2 = czasodc / 60;                                              //przeliczenie ile minut minelo w biezacym odcinku
                        min = (czasodc / 60) % 60;                                         //  - | | -
                        godz = czas2 / 60;                                                 //przeliczenie ile minelo godzin danego odcinka
                                                                                           //Static.progP1 = Convert.ToInt32(procent);                        //przekazanie danych do progressbaru
                        Static.odcp1 = odc + 1;                                            //numer wykonywanego odcinka - do interfejsu
                        Static.godzodcp1 = godz;                                           //przekazanie liczby godzin wykonywanego odcinka
                        Static.minodcp1 = min;                                             //przekazanie minut wykonywanego odcinka
                        Static.tzP1 = temp1;                                               //przekazanie temperatury zmierzonej do interfejsu
                        Static.tSP1 = SP;                                                  //przekazanie temperatury zadanej do interfejsu
                        Static.status1 = status + 1;
                    }
                    else
                    {
                        Static.status1 = 0;
                    }
                    Thread.Sleep(100);

                    //******PIEC 2 *******//
                    wynikp2_1 = Odbierz_Dane(ramkap2_1, 50);                              //pobiera temperature z czujnika
                    wynikp2_2 = Odbierz_Dane(ramkap2_2, 60);                              //dane regulacji
                    if (wynikp2_1 != null && wynikp2_2 != null)
                    {
                        t1 = Convert.ToInt16(wynikp2_1[0]);                                //pobranie temperatury mierzonej
                        tSP = Convert.ToInt16(wynikp2_1[2]);                               //pobranie temperatury zadanej
                        czas2 = Convert.ToInt32(wynikp2_2[7]);                             //pobranie czasu trwania programu
                        czasodc = Convert.ToInt32(wynikp2_2[3]);                           //pobranie czasu biezacego odcinka
                        status = Convert.ToInt16(wynikp2_2[1]);                            //pobranie statusu regulacji
                        odc = Convert.ToInt16(wynikp2_2[0]);                               //pobranie numeru realizowanego odcinka
                        temp1 = (float)t1 / 10;                                         //obliczenie temperatury zmierzonej
                        SP = (float)tSP / 10;                                           //obliczenie temperatury zadanej
                                                                                        //procent = (((float)czas - (float)czas2) / (float)czas) * 100;   //przeliczenie ile % programu zostało już wykonane
                        czas2 = czasodc / 60;                                           //przeliczenie ile minut minelo w biezacym odcinku
                        min = (czasodc / 60) % 60;                                      //  - | | -
                        godz = czas2 / 60;                                              //przeliczenie ile minelo godzin danego odcinka
                                                                                        //Static.progP2 = Convert.ToInt32(procent);                       //przekazanie danych do progressbaru
                        Static.odcp2 = odc + 1;                                         //numer wykonywanego odcinka - do interfejsu
                        Static.godzodcp2 = godz;                                        //przekazanie liczby godzin wykonywanego odcinka
                        Static.minodcp2 = min;                                          //przekazanie minut wykonywanego odcinka
                        Static.tzP2 = temp1;                                            //przekazanie temperatury zmierzonej do interfejsu
                        Static.tSP2 = SP;                                               //przekazanie temperatury zadanej do interfejsu
                        Static.status2 = status + 1;
                    }
                    else
                    {
                        Static.status2 = 0;
                    }
                    Thread.Sleep(100);

                    //******PIEC 3 *******//
                    wynikp3_1 = Odbierz_Dane(ramkap3_1, 50);                              //pobiera temperature z czujnika
                    wynikp3_2 = Odbierz_Dane(ramkap3_2, 60);                              //dane regulacji
                    if (wynikp3_1 != null && wynikp3_2 != null)
                    {
                        t1 = Convert.ToInt16(wynikp3_1[0]);                                //pobranie temperatury mierzonej
                        tSP = Convert.ToInt16(wynikp3_1[2]);                               //pobranie temperatury zadanej
                        czas2 = Convert.ToInt32(wynikp3_2[7]);                             //pobranie czasu trwania programu
                        czasodc = Convert.ToInt32(wynikp3_2[3]);                           //pobranie czasu biezacego odcinka
                        status = Convert.ToInt16(wynikp3_2[1]);                            //pobranie statusu regulacji
                        odc = Convert.ToInt16(wynikp3_2[0]);                               //pobranie numeru realizowanego odcinka
                        temp1 = (float)t1 / 10;                                         //obliczenie temperatury zmierzonej
                        SP = (float)tSP / 10;                                           //obliczenie temperatury zadanej
                                                                                        //procent = (((float)czas - (float)czas2) / (float)czas) * 100;   //przeliczenie ile % programu zostało już wykonane
                        czas2 = czasodc / 60;                                           //przeliczenie ile minut minelo w biezacym odcinku
                        min = (czasodc / 60) % 60;                                      //  - | | -
                        godz = czas2 / 60;                                              //przeliczenie ile minelo godzin danego odcinka
                                                                                        //Static.progP3 = Convert.ToInt32(procent);                       //przekazanie danych do progressbaru
                        Static.odcp3 = odc + 1;                                         //numer wykonywanego odcinka - do interfejsu
                        Static.godzodcp3 = godz;                                        //przekazanie liczby godzin wykonywanego odcinka
                        Static.minodcp3 = min;                                          //przekazanie minut wykonywanego odcinka
                        Static.tzP3 = temp1;                                            //przekazanie temperatury zmierzonej do interfejsu
                        Static.tSP3 = SP;                                               //przekazanie temperatury zadanej do interfejsu
                        Static.status3 = status + 1;
                    }
                    else
                    {
                        Static.status3 = 0;
                    }
                    Thread.Sleep(100);

                    //******PIEC 4 *******//
                    wynikp4_1 = Odbierz_Dane(ramkap4_1, 50);                              //pobiera temperature z czujnika
                    wynikp4_2 = Odbierz_Dane(ramkap4_2, 60);                              //dane regulacji
                    if (wynikp4_1 != null && wynikp4_2 != null)
                    {
                        t1 = Convert.ToInt16(wynikp4_1[0]);                                //pobranie temperatury mierzonej
                        tSP = Convert.ToInt16(wynikp4_1[2]);                               //pobranie temperatury zadanej
                        czas2 = Convert.ToInt32(wynikp4_2[7]);                             //pobranie czasu trwania programu
                        czasodc = Convert.ToInt32(wynikp4_2[3]);                           //pobranie czasu biezacego odcinka
                        status = Convert.ToInt16(wynikp4_2[1]);                            //pobranie statusu regulacji
                        odc = Convert.ToInt16(wynikp4_2[0]);                               //pobranie numeru realizowanego odcinka
                        temp1 = (float)t1 / 10;                                         //obliczenie temperatury zmierzonej
                        SP = (float)tSP / 10;                                           //obliczenie temperatury zadanej
                                                                                        //procent = (((float)czas - (float)czas2) / (float)czas) * 100;   //przeliczenie ile % programu zostało już wykonane
                        czas2 = czasodc / 60;                                           //przeliczenie ile minut minelo w biezacym odcinku
                        min = (czasodc / 60) % 60;                                      //  - | | -
                        godz = czas2 / 60;                                              //przeliczenie ile minelo godzin danego odcinka
                                                                                        //Static.progP4 = Convert.ToInt32(procent);                       //przekazanie danych do progressbaru
                        Static.odcp4 = odc + 1;                                         //numer wykonywanego odcinka - do interfejsu
                        Static.godzodcp4 = godz;                                        //przekazanie liczby godzin wykonywanego odcinka
                        Static.minodcp4 = min;                                          //przekazanie minut wykonywanego odcinka
                        Static.tzP4 = temp1;                                            //przekazanie temperatury zmierzonej do interfejsu
                        Static.tSP4 = SP;                                               //przekazanie temperatury zadanej do interfejsu
                        Static.status4 = status + 1;
                    }
                    else
                    {
                        Static.status4 = 0;
                    }
                    Thread.Sleep(100);

                    //******PIEC 5 *******//
                    wynikp5_1 = Odbierz_Dane(ramkap5_1, 50);                              //pobiera temperature z czujnika
                    wynikp5_2 = Odbierz_Dane(ramkap5_2, 60);                              //dane regulacji
                    if (wynikp5_1 != null && wynikp5_2 != null)
                    {
                        t1 = Convert.ToInt16(wynikp5_1[0]);                                //pobranie temperatury mierzonej
                        tSP = Convert.ToInt16(wynikp5_1[2]);                               //pobranie temperatury zadanej
                        czas2 = Convert.ToInt32(wynikp5_2[7]);                             //pobranie czasu trwania programu
                        czasodc = Convert.ToInt32(wynikp5_2[3]);                           //pobranie czasu biezacego odcinka
                        status = Convert.ToInt16(wynikp5_2[1]);                            //pobranie statusu regulacji
                        odc = Convert.ToInt16(wynikp5_2[0]);                               //pobranie numeru realizowanego odcinka
                        temp1 = (float)t1 / 10;                                            //obliczenie temperatury zmierzonej
                        SP = (float)tSP / 10;                                              //obliczenie temperatury zadanej
                                                                                           //procent = (((float)czas - (float)czas2) / (float)czas) * 100;    //przeliczenie ile % programu zostało już wykonane
                        czas2 = czasodc / 60;                                              //przeliczenie ile minut minelo w biezacym odcinku
                        min = (czasodc / 60) % 60;                                         //  - | | -
                        godz = czas2 / 60;                                                 //przeliczenie ile minelo godzin danego odcinka
                                                                                           //Static.progP5 = Convert.ToInt32(procent);                        //przekazanie danych do progressbaru
                        Static.odcp5 = odc + 1;                                            //numer wykonywanego odcinka - do interfejsu
                        Static.godzodcp5 = godz;                                           //przekazanie liczby godzin wykonywanego odcinka
                        Static.minodcp5 = min;                                             //przekazanie minut wykonywanego odcinka
                        Static.tzP5 = temp1;                                               //przekazanie temperatury zmierzonej do interfejsu
                        Static.tSP5 = SP;                                                  //przekazanie temperatury zadanej do interfejsu
                        Static.status5 = status + 1;
                    }
                    else
                    {
                        Static.status5 = 0;
                    }
                    Thread.Sleep(1500);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            return;
        }
        //Funkcje uruchamiajace i zatrzymujace watek
        public void SetStart()
        {
            start = true;
        }
        public void SetStop()
        {
            start = false;
        }
    }
}
