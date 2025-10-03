using Bogus;
using LibraryApp.Classes;
using LibraryApp.Classes.Generics;
using LibraryApp.Exceptions;
using LybraryAppUtils.Classes;


namespace LibraryApp
{
    internal class Program
    {

        static void Main(string[] args)
        {
            /* 
            Creare una app che che gestisce una biblioteca rispettando i step sotto elencati:
            uso di generics
            implementazione della ricerca dei libri usando Linq(con e senza metodi)
            gestione prestiti libri(libro disponibile si / no)
            gestione errori quando il libro non è disponibile quando si tenta di fare un prestito.
            Il libro deve contenere almeno queste proprietà(ISBN, Nome, Descrizione, DataPubblicazione)
            */

            string? menuInput;

            do
            {
                // cicla finchè l'input non è valido
                Console.WriteLine("------------------------------");
                Console.WriteLine("Cosa vuoi fare");
                Console.WriteLine("1.Visualizza tutti i libri");
                Console.WriteLine("2.Cerca libro");
                Console.WriteLine("3.Restituisci un libro");
                Console.WriteLine("4.Prendi in prestito un libro");
                Console.WriteLine("5.Esci");
                menuInput = Console.ReadLine();

                switch (menuInput)
                {
                    case "1":
                        Console.WriteLine("Ecco tutti i libri");
                        DatabaseService.RetrieveBooksList();
                        Library<Book>.PrintBooks();
                        break;
                    case "2":
                        Console.WriteLine("Inserisci titolo o ISBN");
                        Console.Write("Cerca: ");
                        string? searchInput = Console.ReadLine();

                        if (long.TryParse(searchInput, out long anno))
                        {
                            // Se l'input è parsabile, allora cerca per isbn, lanciando un errore nel caso non trovasse risultati
                            try
                            {
                                // stampa ogni libro con separatori
                                Console.WriteLine("---------------------------");
                                DatabaseService.RetrieveBook(Convert.ToInt64(searchInput));
                                Console.WriteLine("---------------------------");

                            }
                            catch (NotAvailableBook ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else if (searchInput != null)
                        {
                            // Se l'input non è nè parsabile nè null, allora cerca per titolo, lanciando un errore nel caso non trovasse nessun risultato
                            try
                            {
                                Console.WriteLine("---------------");
                                DatabaseService.RetrieveBook(searchInput);
                                Console.WriteLine("---------------");
                            }
                            catch (NotAvailableBook ex)
                            {

                                Console.WriteLine(ex.Message);
                            }

                        }
                        else
                        {
                            // Se l'input è null allora stampa un messaggio di errore
                            Console.WriteLine("Inserisci un valore valido");
                        }

                        break;
                    case "3":
                        Console.Write("Inserisci ISBN: ");
                        string? returnInput = Console.ReadLine();
                        if (returnInput != null)
                        {
                            // Rende un libro scelto dall'utente tramite ISBN di nuovo disponibile da prestato
                            try
                            {
                                DatabaseService.ReturnBook(Convert.ToInt64(returnInput));
                            }
                            catch (NotAvailableBook ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            // Se l'input è null stampa un messaggio di errore
                            Console.WriteLine("Inserisci un valore valido");
                        }
                        break;
                    case "4":
                        Console.Write("Inserisci Titolo: ");
                        string? borrowInput = Console.ReadLine();
                        if (borrowInput != null)
                        {
                            try
                            {
                                DatabaseService.BorrowBook(borrowInput);
                            }
                            catch (NotAvailableBook ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Inserisci un valore valido");
                        }

                        break;
                    default:
                        Console.WriteLine("Inserisci un valore valido");
                        break;
                }
                // L'app ripete il programma finchè l'utente non sceglie di uscire dal ciclo con l'opzione dedicata
            } while (menuInput != "5");
        }
    }
}
