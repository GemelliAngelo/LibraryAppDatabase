using LibraryApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Classes.Generics
{
    // Classe generica perchè non specifica il tipo di elemento ma usa un carattere incognito che può essere sostituito
    public class Library<T> where T : Book
    {
        // Dichiara una lista di libri privata poichè non serve essere modificabile
        public static readonly List<T> books = new();

        public static void AddBooks(List<T> book)
        {
            // Aggiunge un libro alla lista privata
            books.AddRange(book);
        }

        public static void PrintBooks()
        {
            foreach (Book book in books)
            {
                // Cicla e stampa ogni elemento della lista privata tramite il metodo ToString
                Console.WriteLine("--------------------------");
                Console.WriteLine(book);
            }
        }
    }
}
