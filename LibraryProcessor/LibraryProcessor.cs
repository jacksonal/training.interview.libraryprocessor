using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// a possible solution to the library processor refactoring problem.
/// </summary>
namespace LibraryProcessor
{
    /// <summary>
    /// We want to use interfaces so we can utilize an IOC container and mock objects for testing
    /// </summary>
    public interface IBookRepository
    {
        Book GetNext();
    }

    public class BookRepository : IBookRepository
    {
        private readonly Queue<Book> _inQueue = new Queue<Book>();
        private object _lockObj = new object();

        public BookRepository(IDbConnection conn) //inject the sql connection to remove dependency
        {
           /*
            * Here would be code to populate the _inQueue from the database if necessary
            */
        }

        public Book GetNext()
        {
            lock (_lockObj) //makes the book repository thread safe
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
        }
    }
    public class LibraryProcessor
    {
        private IBookRepository _bookRepo;

        public LibraryProcessor(IBookRepository bookRepo) //decouple the book queue from the actual processor
        {
            _bookRepo = bookRepo;
        }

        public void ProcessBooks()
        {
            var book = _bookRepo.GetNext();
            while (book != null)
            {
                //refactor the different book types into distinct class implementations
                //each implementation encapsulates the update status logic.
                book.UpdateStatus(); 
                
                book = _bookRepo.GetNext();
            }
        }
    }
    public abstract class Book
    {
        protected readonly string _overdueStatus = "Overdue";
        protected readonly string _notOverdueStatus = "Not Overdue";
        public int Id { get; set; }
        public DateTime CheckoutDate { get; set; }
        public string Status { get; set; }

        public abstract void UpdateStatus();
    }

    class AudioBook : Book
    {
        public override void UpdateStatus()
        {
            if (DateTime.Now - CheckoutDate > TimeSpan.FromDays(10))
            {
                Status = _overdueStatus;
            }
            else
            {
                Status = _notOverdueStatus;
            }
        }
    }

    class EBook : Book
    {
        public override void UpdateStatus()
        {
            if (DateTime.Now - CheckoutDate > TimeSpan.FromDays(20))
            {
                Status = _overdueStatus;
            }
            else
            {
                Status = _notOverdueStatus;
            }
        }
    }

    public class PrintBook : Book
    {
        public override void UpdateStatus()
        {
            if (DateTime.Now - CheckoutDate > TimeSpan.FromDays(14))
            {
                Status = _overdueStatus;
            }
            else
            {
                Status = _notOverdueStatus;
            }
        }
    }
}