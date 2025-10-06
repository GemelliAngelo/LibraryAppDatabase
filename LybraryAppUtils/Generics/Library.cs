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
        private static readonly List<T> _books = new();

        public static void Clear()
        {
            if (_books.Any())
            {
                _books.Clear();
            }
            else return;
        }

        public static void AddBook(T book)
        {
            // Aggiunge un libro alla lista
            _books.Add(book);
        }

        public static void AddBooks(List<T> book)
        {
            // Aggiunge un libro alla lista
            _books.AddRange(book);
        }

        public static List<T> GetBooks()
        {
            return _books;
        }

        public static void PrintBooks()
        {
            foreach (T book in _books)
            {
                // Cicla e stampa ogni elemento della lista
                Console.WriteLine("------------------------------");
                Console.WriteLine(book);
            }
        }
    }
}
