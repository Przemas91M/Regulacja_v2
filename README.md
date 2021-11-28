# Regulacja_v2
Program do obsługi pięciu pieców do wygrzewania okładzin, poprzez komunikację za pomocą protokołu MODBUS-RTU z regulatorem LUMEL RE82.  
Do połączenia z regulatorem wykorzystywany jest konwerter USB <-> RS485.  
  
Możliwości programu:
- komunikacja poprzez protoków MODBUS-RTU z poszczególnymi regulatorami LUMEL RE82 (ModbusRTU.cs),
- odczytywanie i zmiana parametrów konfiguracyjnych regulatora,
- odczytywanie, zmiana parametrów regulacji PID,
- odczytywanie, zmiana oraz zapis do pliku recept wygrzewania poszczególnych produktów (Plik.cs, Recepta.cs),
- prosta obsługa kont użytkowników (Uzytkownik.cs, Uzytkownicy.cs),
- automatyczne odnajdywanie połączonych urządzeń w sieci Modbus,
- praca każdego pieca w osobnym wątku,
- cykliczne odczytywanie aktualnej temperatury w piecu i wyświetlanie jej w interfejsie,
- tworzenie raportów w postaci plików PDF po zakończonym cyklu wygrzewania okładzin. Każdy raport zawiera wykresy temperatury, nazwę użytkownika rozpoczynającego proces, numery realizowanych zleceń, receptę produktu,  
- możliwość przegladania raportów z wygrzewania okładzin,
- diagnostyka regulatora.
