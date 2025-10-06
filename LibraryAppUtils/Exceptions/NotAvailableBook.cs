namespace LibraryApp.Exceptions
{
    public class NotAvailableBook : Exception
    {
        // Exception personalizzata per i messaggi di errore
        public NotAvailableBook(string message) : base(message) { }

    }
}
