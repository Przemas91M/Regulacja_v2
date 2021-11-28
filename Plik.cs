using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Regulacja_v2
{
    /// <summary>
    /// KLASA PLIK
    /// Klasa odpowiedzialna za obsługę plików całego programu.
    /// </summary>
    public class Plik
    {
        private static readonly string path = System.IO.Directory.GetCurrentDirectory(); //pobierz aktualna lokalizacje programu (przy uruchomieniu, program nie bedzie ruszany podczas dzialania)
        /// <summary>
        ///Funkcja podczas uruchamiania programu sprawdza i w razie potrzeby tworzy lokalizacje i pliki z domyślnymi wartościami, niezbędnymi do działania programu.
        /// </summary>
        public bool Katalog()
        {
            try
            {
                string sciezka = path + "\\log.txt";        //sprawdzam plik log
                if (!File.Exists(sciezka))                  //jezeli plik nie został odnaleziony, zostaje on utworzony i zapisany
                {
                    StreamWriter streamWriter = File.CreateText(sciezka);
                    streamWriter.Dispose();
                    streamWriter.Close();
                }

                sciezka = path + "\\Data\\";                    //sprawdzam folder Data
                if (!Directory.Exists(sciezka))                 //jezeli folder nie istnieje - tworzy go
                {
                    Directory.CreateDirectory(sciezka);
                    FileStream fileStream = File.Create(sciezka + "rapindex.bin");
                    fileStream.Dispose();
                }
                if (!File.Exists(sciezka + "rapindex.bin"))     //sprawdzam, czy w folderze jest zapisany plik z listą raportów 
                {
                    FileStream fileStream = File.Create(sciezka + "rapindex.bin");
                    fileStream.Dispose();
                }
                if (!File.Exists(sciezka + "procesy.dat"))      //sprawdzam, czy istnieje plik przechowujacy dane aktualnie uruchomionych procesów (w razie awarii programu)
                {
                    StreamWriter streamWriter = File.CreateText(sciezka + "procesy.dat");
                    streamWriter.Dispose();
                }

                sciezka = path + "\\Users\\";                   //szukam folderu Users
                if (!Directory.Exists(sciezka))                 //analogicznie - tworzy folder i zapisuje plik z domyslnymi uzytkownikami
                {
                    Directory.CreateDirectory(sciezka);
                    List<Uzytkownicy> users = new List<Uzytkownicy>
                    {
                        new Uzytkownicy("Admin", "admin123", 3),
                        new Uzytkownicy("Piecowy", "1234", 1),
                        new Uzytkownicy("Automatyk", "a1538", 2)
                    };      
                    /*Blok kodu zapisuje liste do pliku binarnego - domyslni uzytkownicy*/
                    using (Stream stream = File.Open(sciezka + "users.bin", FileMode.Create))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        bformatter.Serialize(stream, users);
                    }
                }
                else if (!File.Exists(sciezka + "users.bin"))   //tworzenie samego pliku, gdyby ktoś usunał sam plik
                {
                    List<Uzytkownicy> users = new List<Uzytkownicy>
                    {
                        new Uzytkownicy("Admin", "admin123", 3),
                        new Uzytkownicy("Piecowy", "1234", 1),
                        new Uzytkownicy("Automatyk", "a1538", 2)
                    };
                    using (Stream stream = File.Open(sciezka + "users.bin", FileMode.Create))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        bformatter.Serialize(stream, users);
                    }
                }

                sciezka = path + "\\Recepty\\";                 //szukam folderu z receptami
                if (!Directory.Exists(sciezka))                 //tworzy folder i zapisuje plik z pustymi receptami
                {
                    Directory.CreateDirectory(sciezka);
                    List<Recepta> rec = Listy.L.GetRecepty;
                    /*Blok kodu zapisuje liste do pliku binarnego - pusta lista recept*/
                    using (Stream stream = File.Open(sciezka + "recipies.bin", FileMode.Create))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        bformatter.Serialize(stream, rec);
                    }
                }
                else if (!File.Exists(sciezka + "recipies.bin"))   //tworzenie samego pliku
                {
                    List<Recepta> rec = Listy.L.GetRecepty;
                    /*Blok kodu zapisuje liste do pliku binarnego - pusta lista recept*/
                    using (Stream stream = File.Open(sciezka + "recipies.bin", FileMode.Create))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        bformatter.Serialize(stream, rec);
                    }
                }

                sciezka = path + "\\Temp\\";                    //szukam folderu temp - do zapisu screenshotu grafu
                if (!Directory.Exists(sciezka))                 //tworzenie ścieżki
                {
                    Directory.CreateDirectory(sciezka);
                }

                sciezka = @"C:\Raporty\";                       //szukam folderu na dysku C: - do którego zapisywane są raporty .pdf
                if (!Directory.Exists(sciezka))
                {
                    Directory.CreateDirectory(sciezka + @"Piec 1\");
                    Directory.CreateDirectory(sciezka + @"Piec 2\");
                    Directory.CreateDirectory(sciezka + @"Piec 3\");
                    Directory.CreateDirectory(sciezka + @"Piec 4\");
                    Directory.CreateDirectory(sciezka + @"Piec 5\");
                }
                return true;                                    //potwierdzenie zakończenia sprawdzania katalogów
            }

            catch (Exception ex)                                //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// Funkcja zwraca tabele z danymi uzytkownikow zapisanymi w pliku users.bin
        /// Jezeli program jest uruchamiany po raz pierwszy - zapisani sa tylko domyślni uzytkownicy
        /// </summary>
        /// <returns> Zwraca liste w formacie Uzytkownicy </returns>
        public List<Uzytkownicy> PobierzUzytkownikow()
        {
            try
            {
                List<Uzytkownicy> uzytkownicy;                                                  //inicjalizacja listy
                using (Stream stream = File.Open(path + "\\Users\\users.bin", FileMode.Open))   //pobranie danych uzytkownikow z pliku
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    uzytkownicy = (List<Uzytkownicy>)binaryFormatter.Deserialize(stream);
                    stream.Close();
                    return uzytkownicy;                                                         //zwracana lista
                }
            }
            catch (Exception ex)                                                                 //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Pobiera listę parametrów programu zapisanych w pliku .bin
        /// </summary>
        /// <param name="path"> Ścieżka do pliku (wybierana w osobnej funkcji) </param>
        /// <returns> Zwraca listę w formacie Programy z odczytanymi parametrami programu. </returns>
        public List<Programy> Otworz_Program(string path)
        {
            List<Programy> LProg;                                       //inicjalizacja listy
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))  //pobranie listy parametrów programu z pliku 
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    LProg = (List<Programy>)binaryFormatter.Deserialize(stream);
                    stream.Close();
                    return LProg;
                }
            }
            catch (Exception ex)                                        //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Zapis listy parametrów programu do pliku .bin
        /// Uzytkownik wybiera nazwe i lokalizacje pliku.
        /// </summary>
        /// <param name="path">Wybrana sciezka pliku. </param>
        /// <param name="program">Lista wybranych parametrów do zapisu. </param>
        /// <returns>Zwraca bool - potwierdzenie zapisu</returns>
        public bool ZapiszProgram(string path, List<Programy> program)
        {
            try
            {
                using (Stream stream = File.Open(path, FileMode.Create))        //zapis listy parametrów do pliku
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(stream, program);
                    stream.Close();
                }
                return true;                                                    //potwierdzenie zapisu pliku
            }
            catch (Exception ex)                                                //obsługa błędu
            {
                MessageBox.Show("Bład zapisu pliku: " + ex.Message.ToString());
                return false;
            }

        }
        /// <summary>
        /// Nazwa mowi sama za siebie
        /// </summary>
        /// <param name="users">Zaktualizowana lista uzytkowników.</param>
        /// <returns>Zwraca bool - potwierdzenie zapisu</returns>
        public bool ZapiszPlikUsers(List<Uzytkownicy> users)
        {
            try
            {
                string sciezka = path + "\\Users\\users.bin";                 //ustawiam sciezke na plik uzytkowników              
                //File.Delete(sciezka);
                using (Stream stream = File.Open(sciezka, FileMode.Create))   //zapisuję listę użytkowników do pliku
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(stream, users);
                    stream.Close();
                }

                return true;                                                  //potwierdzenie zapisu pliku
            }
            catch (Exception ex)                                              //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// Funkcja tworzy plik, do któego będą zapisane pomiary z procesu wygrzewania.
        /// Dodatkowo zapisuje proces do pliku rapindex - lista wszystkich procesów.
        /// Ostatnim etapem jest zapis aktualnego zlecenia do pliku procesy. Jeżeli w trakcie trwania procesu wygrzewania zostanie wyłączony komputer, 
        /// to po ponownym uruchomieniu będzie możliwa dalsza rejestracja procesu.
        /// </summary>
        /// <param name="piec"></param>
        /// <param name="user"></param>
        /// <param name="prog"></param>
        /// <param name="zlecenia"></param>
        /// <returns></returns>
        public static string UtworzPlikPomiar(string piec, string user, string prog, List<string> zlecenia)//dorzucic nazwe zlecenia i pieca - przekazywane z okienka startu pieca
        {
            string nazwa = DateTime.Now.ToString("yyyyMMdd_HHmmss");                         //pobierz date i czas z systemu, sformatuj wg wzoru: 20200118_113725 (rrrrmmdd_ggmmss)
            DateTime data = DateTime.Now;                                                    //pobieranie daty i czasu do zapisu do pliku
            string plik = path + "\\Data\\rapindex.bin";                                     //sciezka do pliku raportów
            List<Pomiar> p1 = new List<Pomiar>();                                            //inicjalizacja listy z pomiarami
            List<Raporty> ind;                                                               //inicjalizacja listy z raportami
            ind = PobierzRaporty();                                                          //pobranie raportów do listy
            ind.Add(new Raporty(data, piec, user, nazwa, prog, zlecenia, data, data));       //dodanie wpisu o obecnym cyklu pracy
            try
            {
                using (Stream stream = File.Create(plik))                                    //zapis pliku z raportami
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(stream, ind);
                    stream.Close();
                }
                plik = path + "\\Data\\" + nazwa + ".bin";
                using (Stream stream = File.Create(plik))                                    //utworzenie pliku z pomiarami i zapis jednego pomiaru (zera)
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(stream, p1);
                    stream.Close();
                }
                string plikproc = string.Concat(path, "\\Data\\procesy.dat");                //zapis pliku z aktywnymi procesami - w razie gdyby program lub komputer padł, można uruchomić program bez konieczności odpalania procesu od nowa
                var lista = File.ReadAllLines(plikproc).Where(l => !l.StartsWith(piec));     //aktualizacja procesów - usunięcie zakończonego (gdyby w pliku z procesami nadal znajdował się dany proces)
                File.WriteAllLines(plikproc, lista);                                         //zapis listy aktualnych procesów
                using (StreamWriter file = new StreamWriter(plikproc, true))                 //zapis nowego procesu - aktualnie rozpoczynanego
                {
                    file.WriteLine($"{piec},{plik}");
                }
            }
            catch (Exception ex)                                                             //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return plik;
        }
        /// <summary>
        /// Zapisuje nazwy recept do pliku
        /// </summary>
        /// <param name="recepty">Lista z numerami i nazwami recept</param>
        /// <returns>Zwraca bool oznaczajacy powodzenie zapisu danych do pliku</returns>
        public bool ZapiszRecepty(List<Recepta> recepty)
        {
            try
            {
                string sciezka = path + "\\Recepty\\recipies.bin";              //ustawiam scieżkę na plik recept
                using (Stream stream = File.Open(sciezka, FileMode.Create))     //zapis listy do pliku
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(stream, recepty);
                    stream.Close();
                }
                return true;                                                    //potwierdzenie zapisu pliku
            }
            catch (Exception ex)                                                //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }

        }
        /// <summary>
        /// Funkcja sprawdza, czy proces danego pieca jest aktywny i zwraca nazwę pliku, do którego zapisywane są dane.
        /// </summary>
        /// <param name="piec"></param>
        /// <returns></returns>
        public string PobierzProces(string piec)
        {
            try
            {
                string[] lista = File.ReadAllLines(path + "\\Data\\procesy.dat").Where(l => l.StartsWith(piec)).ToArray(); //pobranie rekordu w któym jest szukany piec
                if (lista.Length > 0)                //sprawdzam, czy istnieje taki rekord
                {
                    lista = lista[0].Split(',');     //oddzielam nazwę pieca od nazwy pliku
                    return lista[1];                 //zwracam nazwę pliku
                }
                else                                 //jeżeli nie ma takiego wpisu w pliku
                {
                    return null;
                }
            }
            catch (Exception ex)                      //obsługa błędu
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Pobiera recepty zapisane w pliku recipies.bin
        /// Jezeli plik nie istnieje w folderze programu - zostana pobrane puste nazwy!
        /// </summary>
        /// <returns>Zwraca liste z zapisanymi nazwami recept</returns>
        public List<Recepta> PobierzRecepty()
        {
            List<Recepta> recepty;
            string sciezka = path + "\\Recepty\\recipies.bin";
            try
            {
                using (Stream stream = File.Open(sciezka, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    recepty = (List<Recepta>)binaryFormatter.Deserialize(stream);
                    stream.Close();
                    return recepty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Funkcja pobiera zapisane dane raportów procesów wygrzewania okładzin
        /// </summary>
        /// <returns> Zwraca listę z danymi raportów</returns>
        public static List<Raporty> PobierzRaporty()
        {
            List<Raporty> raporty = new List<Raporty>();
            string sciezka = path + "\\Data\\rapindex.bin";
            try
            {
                using (Stream stream = File.Open(sciezka, FileMode.Open))
                {
                    if (stream.Length > 0)
                    {
                        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        raporty = (List<Raporty>)binaryFormatter.Deserialize(stream);
                        stream.Close();
                    }
                    return raporty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }

        }
        /// <summary>
        /// Funkcja obslugujaca zapis danych do pliku podczas pracy pieca
        /// </summary>
        /// <param name="plik"> Wskazany plik, do ktorego zapisane beda pomiary </param>
        /// <param name="pomiary"> Lista z pomiarami </param>
        public static void ZapiszPomiar(string plik, List<Pomiar> pomiary)
        {
            try
            {
                using (Stream stream = File.Open(plik, FileMode.Open, FileAccess.Write))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(stream, pomiary);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }
        /// <summary>
        /// Funkcja pobiera listę z pomiarami z danego cyklu wygrzewania
        /// </summary>
        /// <param name="plik"> Plik, z ktorego maja byc odczytane pomiary</param>
        /// <returns> Zwraca liste z pomiarami </returns>
        public static List<Pomiar> PobierzPomiar(string plik)
        {
            List<Pomiar> wynik = new List<Pomiar>();                                             //tworze pusta liste   
            try
            {
                using (Stream stream = File.Open(plik, FileMode.Open))          //otwieram wskazany plik
                {                                                               //tworzenie formatera
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    wynik = (List<Pomiar>)binaryFormatter.Deserialize(stream);  //pobranie danych do listy
                    stream.Close();                                             //zamkniecie bufora
                    return wynik;                                               //zwrocenie listy
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return wynik;
            }
        }

        public ObservableCollection<Pomiar> PobierzPomiarWykres(string plik)
        {
            string file = plik;
            ObservableCollection<Pomiar> returnList = new ObservableCollection<Pomiar>();
            List<Pomiar> wynik = new List<Pomiar>();
            try
            {
                wynik = PobierzPomiar(plik);
                wynik.ForEach(x => returnList.Add(x));
                return returnList;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return new ObservableCollection<Pomiar>();
            }
        }
        /// <summary>
        /// Funkcja dorzuca do pliku raportu date zakonczenia cyklu
        /// </summary>
        /// <param name="plik"> Nazwa pliku, w ktorym byly zapisywane pomiary. </param>
        public static void ZapiszOstatniPomiar(string plik, int numer)
        {
            if(numer < 1 || numer > 5)
            {
                MessageBox.Show("Nieprawidłowy numer pieca!");
                return;
            }
            string piec;
            switch (numer)
            {
                case 1:
                    piec = "Piec 1";
                    break;
                case 2:
                    piec = "Piec 2";
                    break;
                case 3:
                    piec = "Piec 3";
                    break;
                case 4:
                    piec = "Piec 4";
                    break;
                case 5:
                    piec = "Piec 5";
                    break;
                default:
                    piec = "Błąd";
                    break;

            }
            DateTime dataczas = DateTime.Now;                       //pobiera aktualna date i godzine (zakonczenia cyklu)
            List<Raporty> rap = PobierzRaporty();                   //pobiera raporty do listy
            string name = plik.Substring(plik.Length - 19, 15);     //ekstrakcja samej nazwy pliku ze ścieżki
            int index = rap.FindIndex(x => x.Nazwapliku == name);   //szukanie pozycji raportu
            rap[index].Stop = dataczas;                             //podmiana daty zakonczenia cyklu
            plik = path + "\\Data\\rapindex.bin";                   //przejscie do pliku raportów
            try
            {
                using (Stream stream = File.Create(plik))           //zapis aktualnej tablicy
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bformatter.Serialize(stream, rap);
                    stream.Close();
                }
                plik = path + "\\Data\\procesy.dat";                                 //przejscie do pliku aktualnych procesów
                var lista = File.ReadAllLines(plik).Where(l => !l.StartsWith(piec)); //aktualizacja procesów - usunięcie zakończonego
                File.WriteAllLines(plik, lista);                                     //zapis informacji do pliku
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Funkcja zapisujaca dane do pliku log.txt - znajdujacy sie w folderze programu
        /// </summary>
        /// <param name="message">String zawierajacy wiadomosc do zapisania w pliku</param>
        public void ZapiszDoLog(string message)
        {
            try
            {
                long length = new System.IO.FileInfo("log.txt").Length; //pobranie infomacji o pliku
                if (length > 100000)                                    //jezeli plik ma wiecej jak 100kb                
                {
                    File.WriteAllText("log.txt", string.Empty);         //kasowanie zawartosci pliku
                }
                using (StreamWriter w = File.AppendText("log.txt"))     //zapis informacji do loga
                {
                    Log(message, w);                                    //wpisanie tresci 
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        private void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nZapis : ");
            w.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}");
            w.WriteLine("  :");
            w.WriteLine($"{logMessage}");
            w.WriteLine("-------------------------------");
        }
    }
}
