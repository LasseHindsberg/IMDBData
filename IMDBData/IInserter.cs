using System;
using IMDBData.Models;
using System.Data.SqlClient;

namespace IMDBData
{
    public interface IInserter
    {
       
        void Insert(List<Title> titles, List<Person> persons, List<Crew> crews, SqlConnection sqlConn, SqlTransaction transAction);
    }
}
