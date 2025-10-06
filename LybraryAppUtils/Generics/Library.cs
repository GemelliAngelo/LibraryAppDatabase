using LibraryApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Classes.Generics
{
    // Classe generica perchè non specifica il tipo di elemento ma usa un carattere incognito che può essere sostituito
    public class Library<T>
    {
        // Dichiara una lista di libri readonly
        public static readonly List<T> books = new();

        public static void AddBook(T book)
        {
            // Aggiunge un libro alla lista
            books.Add(book);
        }

        public static void AddBooks(List<T> book)
        {
            // Aggiunge un libro alla lista
            books.AddRange(book);
        }

        public static void PrintBooks()
        {
            foreach (T book in books)
            {
                // Cicla e stampa ogni elemento della lista
                Console.WriteLine("--------------------------");
                Console.WriteLine(book);
            }
        }
    }
}
