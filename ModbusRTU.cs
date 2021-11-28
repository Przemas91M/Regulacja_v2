using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Regulacja_v2
{
    /// <summary>
    /// KLASA MODBUS RTU
    /// Jest odpowiedzialna za komunikację pomiędzy komputerem a regulatorami.
    /// </summary>
    public class ModbusRTU
    {
        private static readonly Mutex mut = new Mutex();
        private static string com;
        /// <summary>
        /// Funkcja tworzy ramke komunikacyjna dla Modbus RTU
        /// </summary>
        /// <param name="AdresUrz">Adres regulatora (1-5)</param>
        /// <param name="Funkcja">Funkcja FC protokołu</param>
        /// <param name="AdresPocz">Adres rejestru do którego funkcja ma się odwołać</param>
        /// <param name="Dane">Dodatkowe dane przesyłane do regulatora</param>
        /// <returns>Zwraca tabelę z 8bitowymi słowami sterujacymi</returns>
        public static byte[] FormujRamke(byte AdresUrz, byte Funkcja, ushort AdresPocz, uint Dane)
        {
            byte[] ramka = new byte[8];
            ramka[0] = AdresUrz;			        // Adres urzadzenia
            ramka[1] = Funkcja;				        // Numer funkcji             
            ramka[2] = (byte)(AdresPocz >> 8);  	// Adres rejestru HI
            ramka[3] = (byte)AdresPocz;		        // Adres rejestru LO            
            ramka[4] = (byte)(Dane >> 8);	        // Dane / liczba rejestrow do odczytania HI
            ramka[5] = (byte)Dane;		            // Dane / liczba rejestrow do odczytania LO
            byte[] crc = WyliczCRC(ramka);          // Liczenie CRC sumy kontrolnej.
            ramka[ramka.Length - 2] = crc[0];       // Error Check Low
            ramka[ramka.Length - 1] = crc[1];       // Error Check High
            return ramka;
        }
        /// <summary>
        /// Funkcja tworzaca ramkę - wersja dla zapisu wielu rejestrów.
        /// Realizuje funkcję FC16 protokołu Modbus RTU.
        /// </summary>
        /// <param name="AdresUrz">Adres regulatora (1-5)</param>
        /// <param name="RejPocz">Adres rejestru do którego funkcja ma się odwołać</param>
        /// <param name="Dane">Tabela z danymi do zapisu</param>
        /// <returns>Zwraca gotowa ramkę</returns>
        public static byte[] FormujRamkeREJ(byte AdresUrz, ushort RejPocz, ushort[] Dane)
        {
            byte[] ramka = new byte[9 + Dane.Length * 2];
            ushort rejestry = Convert.ToUInt16(Dane.Length);
            ushort bajty = (byte)(rejestry * 2);
            ramka[0] = AdresUrz;			        // Adres urzadzenia
            ramka[1] = 16;				            // Numer funkcji             
            ramka[2] = (byte)(RejPocz >> 8);  	    // Adres rejestru HI
            ramka[3] = (byte)RejPocz;		        // Adres rejestru LO            
            ramka[4] = (byte)(rejestry >> 8);	    // Dane / liczba rejestrow do odczytania HI
            ramka[5] = (byte)rejestry;		        // Dane / liczba rejestrow do odczytania LO
            ramka[6] = (byte)bajty;                 // Liczba bajtów
            for (int i = 0; i < Dane.Length; i++)
            {
                ramka[7 + i * 2] = (byte)(Dane[i] >> 8);
                ramka[8 + i * 2] = (byte)Dane[i];
            }
            byte[] crc = WyliczCRC(ramka);          // Liczenie CRC sumy kontrolnej.
            ramka[ramka.Length - 2] = crc[0];       // Error Check Low
            ramka[ramka.Length - 1] = crc[1];       // Error Check High
            return ramka;
        }
        /// <summary>
        /// Funkcja wyliczajaca sumę kontrolna CRC, wywoływana z funkcji tworzacych ramki.
        /// </summary>
        /// <param name="data">Jako dane przyjmuje dotychczasowo utworzona ramkę</param>
        /// <returns>Zwraca wartość CRC do umieszczenia w ramce</returns>
        private static byte[] WyliczCRC(byte[] data)
        {
            ushort CRCFull = 0xFFFF; 
            char CRCLSB;
            byte[] CRC = new byte[2];
            for (int i = 0; i < (data.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ data[i]); // 

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = (byte)(CRCFull & 0xFF);
            return CRC;
        }
        /// <summary>
        /// Funkcja przesyła zapytanie z wcześniej utworzonej ramki do regulatora.
        /// </summary>
        /// <param name="ramka">Wektor bajtowy ramki komunikacyjnej</param>
        /// <param name="delay">Opóźnienie odczytu danych. W zależności od wielkości danych, trzeba odczekać na koniec transmisji.</param>
        /// <returns>Zwraca wywoływane przez zapytanie dane w formacie UInt16</returns>
        public static UInt16[] Odbierz_Dane(byte[] ramka, int delay)
        {
            try
            {
                mut.WaitOne();                                                                //uruchomienie mutexu 
                com = Form1.Static.portCOM;                                                   //pobranie adresu portu COM 
                SerialPort port = new SerialPort(com, 57600, Parity.None, 8, StopBits.Two);   //tworzenie obiektu SerialPort
                port.Open();                                                                  //otwarcie portu
                port.Write(ramka, 0, ramka.Length);                                           //wysłanie zapytania do regulatora
                Thread.Sleep(delay);                                                          //oczekiwanie na przesłanie danych - zależne od ilości rejestrów
                byte[] bufor_odbioru = new byte[port.BytesToRead];                            //deklaracja buforu odbiorczego
                port.Read(bufor_odbioru, 0, port.BytesToRead);                                //odczyt danych z bufora
                port.DiscardInBuffer();                                                       //czyszczenie bufora
                port.Close();                                                                 //zamknięcie portu
                mut.ReleaseMutex();                                                           //zamknięcie mutexu - zwolnienie portu 
                if(bufor_odbioru == null)                                                     //sprawdzam, czy otrzymałem dane
                {
                    return null;
                }
                byte[] dane = new byte[bufor_odbioru.Length - 5];                             //filtrowanie otrzymanych danych - usuwanie 5 ostatnich bajtów (CRC)
                Array.Copy(bufor_odbioru, 3, dane, 0, dane.Length);                           //kopiowanie danych z buforu do wektora, usunięcie 3 pierwszych bajtów (adres i numer rozkazu)
                UInt16[] result = Word.ByteToUInt16(dane);                                    //konwersja otrzymanych danych na Int16
                return result;                                                                //zwracam dane
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Funkcja działa bardzo podobnie do "Odbierz_Dane".
        /// Różnica polega na zwracanych wartościach - tutaj tabela bajtowa. 
        /// </summary>
        /// <param name="ramka">Wektor bajtowy ramki komunikacyjnej</param>
        /// <param name="delay">Opóźnienie odczytu danych. W zależności od wielkości danych, trzeba odczekać na koniec transmisji.</param>
        /// <returns>Zwraca wywoływane przez zapytanie dane w formacie byte</returns>
        public static byte[] Odbierz_DaneRAW(byte[] ramka, int delay)
        {
            try
            {
                mut.WaitOne();                                                                //uruchomienie mutexu 
                com = Form1.Static.portCOM;                                                   //pobranie adresu portu COM 
                SerialPort port = new SerialPort(com, 57600, Parity.None, 8, StopBits.Two);   //tworzenie obiektu SerialPort
                port.Open();                                                                  //otwarcie portu
                port.Write(ramka, 0, ramka.Length);                                           //wysłanie zapytania do regulatora
                Thread.Sleep(delay);                                                          //oczekiwanie na przesłanie danych - zależne od ilości rejestrów
                byte[] bufor_odbioru = new byte[port.BytesToRead];                            //deklaracja buforu odbiorczego
                port.Read(bufor_odbioru, 0, port.BytesToRead);                                //odczyt danych z bufora
                port.DiscardInBuffer();                                                       //czyszczenie bufora
                port.Close();                                                                 //zamknięcie portu
                mut.ReleaseMutex();                                                           //zamknięcie mutexu - zwolnienie portu 
                if (bufor_odbioru == null)                                                    //sprawdzam, czy otrzymałem dane
                {
                    return null;
                }
                byte[] dane = new byte[bufor_odbioru.Length - 5];                             //filtrowanie otrzymanych danych - usuwanie 5 ostatnich bajtów (CRC)
                Array.Copy(bufor_odbioru, 3, dane, 0, dane.Length);                           //kopiowanie danych z buforu do wektora, usunięcie 3 pierwszych bajtów (adres i numer rozkazu)
                return dane;                                                                  //zwracam dane w formacie bajtowym
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Funkcja sprawdzajaca obecność regulatora w sieci. Funkcja FC17.
        /// </summary>
        /// <param name="adres">Adres wywoływanego regulatora</param>
        /// <returns>Zwraca przesłane przez regulator dane</returns>
        public byte[] ID(byte adres)
        {
            try
            {
                byte[] ramka = new byte[4];
                ramka[0] = adres;
                ramka[1] = 17;
                byte[] crc = WyliczCRC(ramka);
                ramka[ramka.Length - 2] = crc[0];
                ramka[ramka.Length - 1] = crc[1];
                mut.WaitOne();
                com = Form1.Static.portCOM;
                SerialPort port = new SerialPort(com, 57600, Parity.None, 8, StopBits.Two);
                port.Open();
                port.Write(ramka, 0, ramka.Length);
                Thread.Sleep(50);
                byte[] bufor_odbioru = new byte[port.BytesToRead];
                port.Read(bufor_odbioru, 0, port.BytesToRead);
                port.DiscardInBuffer();
                port.Close();
                mut.ReleaseMutex();
                return bufor_odbioru;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Funkcja służaca do zapisu danych do pojedynczego rejestru.
        /// </summary>
        /// <param name="ramka">Ramka - wektor bajtowy z rozkazem</param>
        /// <returns>Zwraca liczbę w zależności od statusu wykonania wywołania.
        /// Zwróci zero - wszystko okey, jeżeli wystapi bład - pobiera jego numer i przekazuje dalej</returns>
        public int Zapisz_Rejestr(byte[] ramka)
        {
            try
            {
                mut.WaitOne();
                com = Form1.Static.portCOM;
                SerialPort port = new SerialPort(com, 57600, Parity.None, 8, StopBits.Two);
                port.Open();
                port.Write(ramka, 0, ramka.Length);
                Thread.Sleep(50);
                byte[] bufor_odbioru = new byte[port.BytesToRead];
                port.Read(bufor_odbioru, 0, port.BytesToRead);
                port.DiscardInBuffer();
                port.Close();
                mut.ReleaseMutex();
                if (bufor_odbioru.SequenceEqual(ramka))
                {
                    return 0;                                       //zwroci zero - jezeli wszystko jest w porzadku
                }
                else
                {
                    //obsługa błędu
                    byte dane = bufor_odbioru[2];       //kopiuje jeden bajt z 3 pozycji
                    return Convert.ToInt32(dane);                  //zwraca kod bledu
                }
            }
            catch (Exception)
            {
                return 4;
            }
        }
        /// <summary>
        /// Funkcja do zapisu wielu rejestrów.
        /// </summary>
        /// <param name="ramka">Ramka - wektor bajtowy z rozkazem</param>
        /// <returns>Zwraca liczbę w zależności od statusu wykonania wywołania.
        /// Zwróci zero - wszystko okey, jeżeli wystapi bład - pobiera jego numer i przekazuje dalej</returns>
        public static int Zapisz_Rejestry(byte[] ramka)
        {
            try
            {
                mut.WaitOne();
                com = Form1.Static.portCOM;
                SerialPort port = new SerialPort(com, 57600, Parity.None, 8, StopBits.Two);
                port.Open();
                port.Write(ramka, 0, ramka.Length);
                Thread.Sleep(300);
                byte[] bufor_odbioru = new byte[port.BytesToRead];
                port.Read(bufor_odbioru, 0, port.BytesToRead);
                port.DiscardInBuffer();
                port.Close();
                mut.ReleaseMutex();
                if (bufor_odbioru.Length > 5)
                {
                    ushort rejestryRamka = (UInt16)(ramka[4] * 256 + ramka[5]);
                    ushort rejestryOdbior = Convert.ToUInt16(bufor_odbioru[5]);
                    if (rejestryRamka == rejestryOdbior)
                    {
                        return 0;                                       //zwroci zero - jezeli wszystko jest w porzadku
                    }
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    //obsługa błędu
                    byte dane = bufor_odbioru[2];       //kopiuje jeden bajt z 3 pozycji
                    return Convert.ToInt32(dane);  //zwraca kod bledu
                }
            }
            catch (Exception)
            {
                return 4;
            }

        }
        /// <summary>
        /// Funkcja uruchamiajaca lub zatrzymujaca proces regulacji automatycznej.
        /// </summary>
        /// <param name="piec">Adres pieca</param>
        /// <param name="prog">Numer programu do realizacji</param>
        /// <param name="startStop">Zmienna ustalajaca, czy program ma byc uruchomiony, czy zatrzymany (1 - start, 0 - stop)</param>
        /// <returns></returns>
        public static int StartStop_Cykl(int piec, int prog, bool startStop)
        {
            ushort[] dane;
            if (startStop)       //jeżeli jest 1 - start programu
            {
                dane = new ushort[4] { (ushort)prog, 1, 0, 0 };     //tworzenie odpowiedniego wektora z rozkazem
            }
            else                //jeżeli jest 0 - stop programu
            {
                dane = new ushort[4] { 0, 0, 0, 0 };                //tworzenie wektora z rozkazem
            }
            byte[] ramka = FormujRamkeREJ((byte)piec, 4150, dane);  //tworzenie ramki z rozkazem
            return Zapisz_Rejestry(ramka);                          //przekazanie statusu wykonania rozkazu

        }
        ~ModbusRTU()
        {
            mut.Dispose();
            mut.Close();
        }

    }
}
