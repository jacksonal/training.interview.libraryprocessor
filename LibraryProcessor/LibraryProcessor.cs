using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LibraryProcessor
{
    /// <summary>
    /// This class processes book objects in order. This code is pretty sloppy though, 
    /// and is nearly impossible to unit test. Please refactor it as much as you feel is necessary
    /// </summary>
    public class LibraryProcessor
    {
        private readonly Queue<Book> _inQueue = new Queue<Book>();

        public LibraryProcessor()
        {
            var sqlConnection = new SqlConnection("ConnectionString");
            /*
             * Here would be code to populate the _inQueue from the database if necessary
             */
        }

        private Book GetNext()
        {
            if (_inQueue.Count == 0)
            {
                return null;
            }
            else
            {
                return _inQueue.Dequeue();
            }
        }

        public void ProcessBooks()
        {
            var book = GetNext();
            while (book != null)
            {
                switch (book.Type)
                {
                    case 1: //print
                        if(DateTime.Now - book.CheckoutDate > TimeSpan.FromDays(14))
                        {
                            book.Status = "Overdue";
                        }
                        else
                        {
                            book.Status = "Not Overdue";
                        }
                        break;
                    case 2: //ebook
                        if (DateTime.Now - book.CheckoutDate > TimeSpan.FromDays(20))
                        {
                            book.Status = "Overdue";
                        }
                        else
                        {
                            book.Status = "Not Overdue";
                        }
                        break;
                    case 3: //audiobook
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
                book = GetNext();
            }
        }
    }
    public class Book
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public DateTime CheckoutDate { get; set; }
        public string Status { get; set; }
    }
}