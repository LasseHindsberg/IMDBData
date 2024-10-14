using IMDBData.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IMDBData
{
    public class NormalInserter : IInserter
    {
        public NormalInserter() { }

        public string? CheckIntForNull(int? value)
        {
            if (value == null)
            {
                return "NULL";
            }
            else
            {
                return value.ToString();
            }
        }

        public void Insert (List<Title> Titles, List<Person> Persons, List<Crew> Crews, SqlConnection SqlConn, SqlTransaction TransAction)
        {
            // Insert Titles
            foreach (Title title in Titles)
            {
                string SQL = "INSERT INTO [Titles]([TConst]," +
                    "[PrimaryTitle],[OriginalTitle],[IsAdult],[StartYear]," +
                    "[EndYear],[RuntimeMinutes]) " +
                    "VALUES('" + title.TConst + "'" +
                    ",'" + title.PrimaryTitle.Replace("'", "''") + "'" +
                    ",'" + title.OriginalTitle.Replace("'", "''") + "'" +
                    ",'" + title.IsAdult + "'" +
                    "," + CheckIntForNull(title.StartYear) +
                    "," + CheckIntForNull(title.EndYear) +
                    "," + CheckIntForNull(title.RuntimeMinutes) + ")";


                SqlCommand sqlComm = new SqlCommand(SQL, SqlConn, TransAction);
                sqlComm.ExecuteNonQuery();
            }
            foreach (Person person in Persons)
            {
                string SQL = "INSERT INTO [Persons]([NConst],[PrimaryName],[BirthYear],[DeathYear]) " +
                    "VALUES('" + person.NConst + "'" +
                    ",'" + person.PrimaryName.Replace("'", "''") + "'" +
                    "," + CheckIntForNull(person.BirthYear) +
                    "," + CheckIntForNull(person.DeathYear) + ")";

                SqlCommand sqlComm = new SqlCommand(SQL, SqlConn, TransAction);
                sqlComm.ExecuteNonQuery();
            }
            foreach (Crew crew in Crews)
            {
                string SQL = "INSERT INTO [Crews]([TConst],[Directors],[Writers]) " +
                    "VALUES('" + crew.TConst + "'" +
                    "," + crew.Directors + "'" + "," + crew.Writers + ")";

                SqlCommand sqlComm = new SqlCommand(SQL, SqlConn, TransAction);
                sqlComm.ExecuteNonQuery();
            }


        }
    }
}
