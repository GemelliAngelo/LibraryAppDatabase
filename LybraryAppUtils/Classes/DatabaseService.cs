using Bogus;
using LibraryApp.Classes;
using LibraryApp.Classes.Generics;
using LibraryApp.Exceptions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LybraryAppUtils.Classes
{
    public abstract class DatabaseService
    {
        private static string _connectionString = "Server=localhost;Database=Library;User Id=sa;Password=bitspa.1;TrustServerCertificate=true";

        public static SqlParameter CreateParameter(string paramName, object? value, DbType dbType)
        {
            return new SqlParameter(paramName, value)
            {
                DbType = dbType
            };
        }

        public static void ReturnBook(long isbn)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                var query = "UPDATE Books SET [Available]=@available WHERE ISBN=@isbn AND [Available] = 0";
                using (SqlCommand cmd = new(query, connection))
                {
                    cmd.Parameters.Add(CreateParameter("@isbn", isbn, DbType.Int64));
                    cmd.Parameters.Add(CreateParameter("@available", true, DbType.Boolean));
                    var affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows == 0)
                    {
                        throw new NotAvailableBook("Errore: il libro è già disponibile o non esiste.");
                    }
                    else
                    {
                        Console.WriteLine("-------------");
                        Console.WriteLine($"Hai restituito il libro [ISBN-{isbn}]");
                    }
                }
            }
        }

        public static void BorrowBook(string title)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                var query = "UPDATE Books SET [Available]=@available WHERE Title=@title";
                using (SqlCommand cmd = new(query, connection))
                {
                    cmd.Parameters.Add(CreateParameter("@title", title, DbType.String));
                    cmd.Parameters.Add(CreateParameter("@available", false, DbType.Boolean));
                    var affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows == 0)
                    {
                        throw new NotAvailableBook("Errore: il libro non è disponibile o non esiste.");
                    }
                    else
                    {
                        Console.WriteLine("-------------");
                        Console.WriteLine($"Hai preso in prestito {title}");
                    }
                }
            }
        }

        public static void RetrieveBook(long isbn)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                var query = "SELECT [ISBN],[Title],[Description],[PublishDate] FROM [Books] WHERE ISBN=@isbn";
                SqlCommand cmd = new(query, connection);
                cmd.Parameters.Add(CreateParameter("@isbn", isbn, DbType.Int64));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.HasRows)
                    {
                        throw new NotAvailableBook("Libro non trovato");
                    }
                    else
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine("-------------");
                            Console.WriteLine($"{dr.GetInt64("ISBN")} - {dr.GetString("Title")} - {dr.GetDateTime("PublishDate")}");
                        }
                    }
                }
            }
        }
        public static void RetrieveBook(string title)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                var query = "SELECT [ISBN],[Title],[Description],[PublishDate] FROM [Books] WHERE Title=@title";
                SqlCommand cmd = new(query, connection);
                cmd.Parameters.Add(CreateParameter("@title", title, DbType.String));

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.HasRows)
                    {
                        throw new NotAvailableBook("Libro non trovato");
                    }
                    else
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine("-------------");
                            Console.WriteLine($"{dr.GetInt64("ISBN")} - {dr.GetString("Title")} - {dr.GetDateTime("PublishDate")}");
                        }
                    }
                }
            }
        }

        public static List<Book> RetrieveBooksList()
        {
            using (SqlConnection connection = new(_connectionString))
            {
                if (Library<Book>.books.Any()) {
                    Library<Book>.books.Clear();
                }

                connection.Open();

                var query = "SELECT [ISBN],[Title],[PublishDate],[Description],[Available] FROM [Books]";
                SqlCommand cmd = new(query, connection);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    List<Book> booksList = new();
                    while (dr.Read())
                    {
                        var book = new Book()
                        {
                            ISBN = dr.GetInt64(nameof(Book.ISBN)),
                            Title = dr.GetString("Title"),
                            PublishDate = dr.GetDateTime("PublishDate"),
                            Description = dr.GetString("Description"),
                            Available = dr.GetBoolean("Available")
                        };

                        booksList.Add(book);
                    }
                    Library<Book>.AddBooks(booksList);
                }
            }
            return Library<Book>.books;
        }

        public static List<Book> RetrieveBooksListSp()
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();

                var query = "sp_GetBooks";
                SqlCommand cmd = new(query, connection);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    List<Book> booksList = new();
                    while (dr.Read())
                    {
                        var book = new Book()
                        {
                            ISBN = dr.GetInt64(nameof(Book.ISBN)),
                            Title = dr.GetString("Title"),
                            PublishDate = dr.GetDateTime("PublishDate"),
                            Description = dr.GetString("Description"),
                            Available = dr.GetBoolean("Available")
                        };

                        booksList.Add(book);
                    }
                    Library<Book>.AddBooks(booksList);
                }
            }
            return Library<Book>.books;
        }

        public static void AddBook(string title, string description)
        {

            Faker<Book> faker = new Faker<Book>("it")
            .RuleFor(b => b.ISBN, setter => setter.Random.Long(1000000000000, 9999999999999))
            .RuleFor(b => b.Title, setter => title)
            .RuleFor(b => b.Description, setter => description)
            .RuleFor(b => b.PublishDate, setter => DateTime.Now)
            .RuleFor(b => b.Available, setter => setter.Random.Bool());

            Book book = faker.Generate(1).First();

            using SqlConnection connection = new(_connectionString);
            connection.Open();

            var query = "INSERT INTO Books(ISBN, Title, Description, PublishDate, Available) VALUES(@isbn, @title, @description, @publishdate, @available)";

            SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddRange([
                CreateParameter("@isbn", book.ISBN, DbType.Int64),
                CreateParameter("@title", book.Title, DbType.String),
                CreateParameter("@description", book.Description, DbType.String),
                CreateParameter("@publishdate", book.PublishDate, DbType.DateTime2),
                CreateParameter("@available", book.Available, DbType.Boolean),
            ]);
            var affectedRows = cmd.ExecuteNonQuery();

            if (affectedRows == 1)
            {
                Console.WriteLine("-------------");
                Console.WriteLine($"Hai aggiunto {title} alla biblioteca");
            }
            else
            {
                throw new NotAvailableBook("Errore: il libro non è disponibile o non esiste.");
            }
        }

        public static void DeleteBook(long isbn)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            var query = "DELETE FROM Books WHERE ISBN = @isbn";
            SqlCommand cmd = new(query, connection);
            cmd.Parameters.Add(CreateParameter("@isbn", isbn, DbType.Int64));
            var affectedRows = cmd.ExecuteNonQuery();

            if (affectedRows == 1)
            {
                Console.WriteLine("-------------");
                Console.WriteLine($"Hai eliminato il libro {isbn} dalla biblioteca");
            }
            else
            {
                throw new NotAvailableBook("Errore: il libro non è disponibile o non esiste.");
            }
        }
    }
}
