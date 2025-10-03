using Bogus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using LibraryApp.Classes.Generics;
using LibraryApp.Classes;

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
                        Console.WriteLine("Errore: il libro è già disponibile o non esiste.");
                    }
                    else
                    {
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

                // Se è disponibile, aggiorno
                var query = "UPDATE Books SET [Available]=@available WHERE Title=@title";
                using (SqlCommand cmd = new(query, connection))
                {
                    cmd.Parameters.Add(CreateParameter("@title", title, DbType.String));
                    cmd.Parameters.Add(CreateParameter("@available", false, DbType.Boolean));
                    var affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows == 1)
                    {
                        Console.WriteLine("Errore: il libro non è disponibile o non esiste.");
                    }
                    else
                    {
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
                    while (dr.Read())
                    {
                        Console.WriteLine($"{dr.GetInt64("ISBN")} - {dr.GetString("Title")} - {dr.GetDateTime("PublishDate")}");
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
                    while (dr.Read())
                    {
                        Console.WriteLine($"{dr.GetInt64("ISBN")} - {dr.GetString("Title")} - {dr.GetDateTime("PublishDate")}");
                    }
                }
            }
        }

        public static List<Book> RetrieveBooksList()
        {
            using (SqlConnection connection = new(_connectionString))
            {
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

        public static void AddBook(Book book)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            var query = "INSERT INTO Books(ISBN, Title, Description, PublishDate) VALUES(@isbn, @title, @description, @publishdate)";

            SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddRange([
                CreateParameter("@isbn", book.ISBN, DbType.Int64),
                CreateParameter("@title", book.Title, DbType.String),
                CreateParameter("@description", book.Description, DbType.String),
                CreateParameter("@publishdate", book.PublishDate, DbType.DateTime2),
            ]);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteBook(long isbn)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            var query = "DELETE FROM Books WHERE ISBN = @isbn";
            SqlCommand cmd = new(query, connection);
            cmd.Parameters.Add(CreateParameter("@isbn", isbn, DbType.Int64));
            cmd.ExecuteNonQuery();
        }
    }
}
