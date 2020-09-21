using System;
using System.Data.SqlClient;

namespace LibraryProcessor
{
    internal class BookProcessorQueue
    {
        private SqlConnection sqlConnection;

        public BookProcessorQueue(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        internal Book GetNext()
        {
            throw new NotImplementedException();
        }
    }
}