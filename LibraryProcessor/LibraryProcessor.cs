using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LibraryProcessor
{
    /// <summary>
    /// POCO object that represents a book in the system.
    /// </summary>
    public class Book
    {
        public int Id { get; set; }
        /// <summary>
        /// 1 - print
        /// 2 - ebook
        /// 3 - audio
        /// </summary>
        public int Type { get; set; }
        public DateTime CheckoutDate { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// This class processes book objects in order. This code is pretty sloppy though, 
    /// and is nearly impossible to unit test. Please refactor it as much as you feel is necessary
    /// </summary>
    public class LibraryProcessor
    {
        private readonly BookProcessorQueue _inQueue;

        public LibraryProcessor()
        {
            var sqlConnection = new SqlConnection("Data Source = database.server.com; Initial Catalog = DB; user id = admin; password = 12345;");
            _inQueue = new BookProcessorQueue(sqlConnection);
        }
        /// <summary>
        /// Update the statuses of books in the queue
        /// </summary>
        public void ProcessBooks()
        {
            var book = _inQueue.GetNext();
            while (book != null)
            {
                switch (book.Type)
                {
                    case 1: //print books are overdue after 14 days
                        if(DateTime.Now - book.CheckoutDate > TimeSpan.FromDays(14))
                        {
                            book.Status = "Overdue";
                        }
                        else
                        {
                            book.Status = "Not Overdue";
                        }
                        break;
                    case 2: //ebooks are overdue after 20 days
                        if (DateTime.Now - book.CheckoutDate > TimeSpan.FromDays(20))
                        {
                            book.Status = "Overdue";
                        }
                        else
                        {
                            book.Status = "Not Overdue";
                        }
                        break;
                    case 3: //audiobooks are overdue after 10 days
                        if (DateTime.Now - book.CheckoutDate > TimeSpan.FromDays(10))
                        {
                            book.Status = "Overdue";
                        }
                        else
                        {
                            book.Status = "Not Overdue";
                        }
                        break;
                }
                book = _inQueue.GetNext();
            }
        }
    }

}