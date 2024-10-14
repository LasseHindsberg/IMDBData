using System;
using IMDBData.Models;
using System.Data.SqlClient;

namespace IMDBData
{
    public interface IInserter
    {
       
        void Insert(List<Title> Titles, List<Person> Persons, List<Crew> Crews, SqlConnection SqlConn, SqlTransaction TransAction);
    }
}
