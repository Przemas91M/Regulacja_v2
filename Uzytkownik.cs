using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Regulacja_v2.Form1;

namespace Regulacja_v2
{
    /// <summary>
    /// Klasa odpowiadajaca za logowanie danego uzytkownika, przechowujaca dane aktualnych uzytkownikow i mozliwosc dodania/usuniecia uzytkownikow 
    /// Dane logowania zapisywac do encrypted file - w folderze programu (metoda sprawdzajaca istnienie pliku, w przypadku braku takiego pliku utworzyc domyslne konta i hasła)
    /// Logowanie kazdego uzytkownika zapisywac do logu
    /// Zalogowanie uzytkownika powoduje zmiane security level - zmiennej w glownym programie
    /// </summary>
    class Uzytkownik
    {
        Plik Plik = new Plik();
        /// <summary>
        /// Funkcja umozliwiajaca zalogowanie sie uzytkownika, zwraca security level, aby glowna funkcja mogla uruchomic elementy interfejsu.
        /// </summary>
        /// <param name="uzytkownik"></param>
        /// <param name="haslo"></param>
        /// <returns> Zwraca wartosc liczbowa 1 - logowanie sie powiodlo, 2 - nieprawidlowe haslo, 3 - brak uzytkownika na liscie</returns>
        public int Zaloguj(string uzytkownik, string haslo)
        {
            List<Uzytkownicy> dane = Plik.PobierzUzytkownikow();
            //string[] users = new string[dane.Length];
            //string[] passwords = new string[dane.Length];
            //string[] seclevels = new string[dane.Length];
            int securityLevel = 0;
            int index = 0, wynik = 0;
            bool usr = false, pas = false;
            //Przeszukuje tabele w poszukiwaniu nazwy uzytkownika i hasła
            for (int i = 0; i < dane.Count; i++)
            {
                if (dane[i].Nazwa == uzytkownik)         //jezeli znalazlem uzytkownika
                {
                    usr = true;                          //ustawianie boola
                    if (dane[i].Haslo == haslo)          //sprawdzam, czy zgadza sie haslo
                    {
                        pas = true;                      //ustawiam boola
                        index = i;                       //jezeli haslo tez sie zgadza to loguje uzytkownika
                        break;
                    }
                }
            }
            if (usr && pas)                              //jezeli uzytkownik i haslo sie zgadzaja
            {
                securityLevel = dane[index].Seclevel;    //pobieram poziom dostepu uzytkownika
                Static.Login(uzytkownik, securityLevel); //logowanie uzytkownika
                Plik.ZapiszDoLog("Zalogowano: " + uzytkownik);
                wynik = 1;                            //zwrocenie wartosci pomyslnego zalogowania
                return wynik;
            }
            else if (usr)                                //jezeli tylko uzytkownik sie zgadza - wyswiet
            {
                wynik = 2;
                return wynik;
            }
            else                                         //jezeli nie znaleziono uzytkownika
            {
                wynik = 3;
                return wynik;
            }
        }
        /// <summary>
        /// Dodawanie nowego uzytkownika
        /// </summary>
        /// <param name="nazwa"> Wyswietlana nazwa - login nowego uzytkownika </param>
        /// <param name="haslo"> Hasło nowego użytkownika </param>
        /// <param name="seclevel"> Nadany poziom dostępu </param>
        /// <returns> Zwraca boola - jezeli zapis sie powiódł</returns>
        public bool Dodaj_Uzytkownika(string nazwa, string haslo, int seclevel)
        {
            bool wynik = false;
            try
            {
                List<Uzytkownicy> dane = Plik.PobierzUzytkownikow();    //pobiera z pliku listę aktualnie zapisanych użytkowników
                dane.Add(new Uzytkownicy(nazwa, haslo, seclevel));      //dodaje do listy nowego użytkownika
                Plik.ZapiszPlikUsers(dane);                             //zapis danych do pliku
                wynik = true;                                           //zwrocenie sygnalizacji pozytywnego wykonania funkcji
                return wynik;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return wynik;
            }
        }
        /// <summary>
        /// Usuwanie uzytkownika
        /// </summary>
        /// <param name="ind"></param>
        /// <returns></returns>
        public bool Usun_Uzytkownika(int ind)
        {
            bool wynik = false;
            try
            {
                List<Uzytkownicy> dane = Plik.PobierzUzytkownikow();    //pobranie z pliku aktualnej listy uzytkownikow
                dane.Remove(dane[ind]);                                 //usuniecie wybranej pozycji z listy
                Plik.ZapiszPlikUsers(dane);                             //zapisanie zaktualizowanej listy uzytkownikow do pliku
                wynik = true;                                           //sygnalizacja pozytywnego wykonania funkcji
                return wynik;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return wynik;
            }
        }
        /// <summary>
        /// Edycja danych uzytkownika
        /// </summary>
        /// <param name="ind"></param>
        /// <param name="haslo"></param>
        /// <param name="seclevel"></param>
        /// <returns></returns>
        public bool Edytuj_Uzytkownika(int ind, string haslo, int seclevel)
        {
            bool wynik = false;
            List<Uzytkownicy> dane = Plik.PobierzUzytkownikow();    //pobranie aktualnej listy uzytkownikow z pliku
            if (haslo != "")                     //jezeli zostalo wpisane haslo
            {
                dane[ind].Haslo = haslo;         //zmiana hasła na wybrane w formularzu
                wynik = true;                    //sygnalizator na true
            }
            if(seclevel >= 0 && seclevel < 4)    //jezeli zostalo wybrany poziom dostepu
            {
                dane[ind].Seclevel = seclevel;   //zmiana poziomu dostepu na wybrany w formularzu
                wynik = true;                    //sygnalizator na true
            }
            if(wynik)                            //jezeli zostala dokonana jakas zmiana
            {
                Plik.ZapiszPlikUsers(dane);      //zapis danych do pliku
            }
            return wynik;

        }
    }
}
