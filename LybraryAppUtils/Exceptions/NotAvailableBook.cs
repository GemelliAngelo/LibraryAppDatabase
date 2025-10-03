using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Exceptions
{
    public class NotAvailableBook : Exception
    {
        // Exception personalizzata per i messaggi di errore
        public NotAvailableBook(string message) : base(message) { }

    }
}
