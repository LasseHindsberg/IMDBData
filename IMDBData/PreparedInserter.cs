using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IMDBData.Models;
using System.Data.SqlClient;

namespace IMDBData
{
    public class PreparedInserter : IInserter
    {
        public void Insert(List<Title> titles , List<Person> persons , List<Crew> crews , SqlConnection sqlConn, SqlTransaction transAction)
        {
            Console.WriteLine("starting insertion");
            string TitleSQL = "INSERT INTO [Title]([tconst]," +
                "[primarytitle],[originaltitle],[isadult],[startyear],"+
                "[endyear],[runtimeminutes])" +
                "VALUES(@tconst,"+
                "@primarytitle,"+
                "@originaltitle,"+
                "@isadult,"+
                "@startyear,"+
                "@endyear,"+
                "@runtimeminutes)";



            SqlCommand TitleSqlComm = new SqlCommand(TitleSQL, sqlConn, transAction);

            SqlParameter tconstPar = new SqlParameter("@tconst",
                SqlDbType.VarChar, 50);
            TitleSqlComm.Parameters.Add(tconstPar);

            SqlParameter primaryTitlePar = new SqlParameter("@primarytitle",
                SqlDbType.VarChar, 255);
            TitleSqlComm.Parameters.Add(primaryTitlePar);

            SqlParameter originalTitlePar = new SqlParameter("@originaltitle",
                SqlDbType.VarChar, 255);
            TitleSqlComm.Parameters.Add(originalTitlePar);

            SqlParameter isAdultPar = new SqlParameter("@isadult",
                SqlDbType.Bit);
            TitleSqlComm.Parameters.Add(isAdultPar);

            SqlParameter startYearPar = new SqlParameter("@startyear",
                SqlDbType.Int);
            TitleSqlComm.Parameters.Add(startYearPar);
            
            SqlParameter endYearPar = new SqlParameter("@endyear",
                SqlDbType.Int);
            TitleSqlComm.Parameters.Add(endYearPar);

            SqlParameter runtimeMinutesPar = new SqlParameter("@runtimeminutes",
                SqlDbType.Int);
            TitleSqlComm.Parameters.Add(runtimeMinutesPar);

            TitleSqlComm.Prepare();
            Console.WriteLine("Title Sql prepared..");


            foreach (Title title in titles)
            {
                tconstPar.Value = title.TConst;
                primaryTitlePar.Value = checkObjectForNull(title.PrimaryTitle);
                originalTitlePar.Value = checkObjectForNull(title.OriginalTitle);
                isAdultPar.Value = title.IsAdult;
                startYearPar.Value = checkObjectForNull(title.StartYear);
                endYearPar.Value = checkObjectForNull(title.EndYear);
                runtimeMinutesPar.Value = checkObjectForNull(title.RuntimeMinutes);

                TitleSqlComm.ExecuteNonQuery();
            }
            Console.WriteLine("Title sql command executed..");

            string PersonSQL = "INSERT INTO [Person]([nconst],[primaryname],[birthyear],[deathyear])" +
                "VALUES(@nconst,@primaryname," +
                "@birthyear,@deathyear)";

            SqlCommand PersonSqlComm = new SqlCommand(PersonSQL, sqlConn, transAction);

            SqlParameter nconstPar = new SqlParameter("@nconst",
                SqlDbType.VarChar, 50);
            PersonSqlComm.Parameters.Add(nconstPar);

            SqlParameter primaryNamePar = new SqlParameter("@primaryname",
                SqlDbType.VarChar, 255);
            PersonSqlComm.Parameters.Add(primaryNamePar);

            SqlParameter birthYearPar = new SqlParameter("@birthyear",
                SqlDbType.Int);
            PersonSqlComm.Parameters.Add(birthYearPar);

            SqlParameter deathYearPar = new SqlParameter("@deathyear",
                SqlDbType.Int);
            PersonSqlComm.Parameters.Add(deathYearPar);

            PersonSqlComm.Prepare();
            Console.WriteLine("Person Sql prepared..");

            foreach (Person person in persons)
            {
                nconstPar.Value = person.NConst;
                primaryNamePar.Value = checkObjectForNull(person.PrimaryName);
                birthYearPar.Value = checkObjectForNull(person.BirthYear);
                deathYearPar.Value = checkObjectForNull(person.DeathYear);

                PersonSqlComm.ExecuteNonQuery();
            }
            Console.WriteLine("Person sql command executed..");

            string CrewSQL = "INSERT INTO [Crew]([tconst],[directors],[writers])" +
                "VALUES(@tconst,@directors,@writers)";

            SqlCommand CrewSqlComm = new SqlCommand(CrewSQL, sqlConn, transAction);

            SqlParameter tconstCrewPar = new SqlParameter("@tconst",
                               SqlDbType.VarChar, 50);
            CrewSqlComm.Parameters.Add(tconstCrewPar);

            SqlParameter directorsPar = new SqlParameter("@directors",
                               SqlDbType.VarChar, 255);
            CrewSqlComm.Parameters.Add(directorsPar);

            SqlParameter writersPar = new SqlParameter("@writers",
                                              SqlDbType.VarChar, 255);
            CrewSqlComm.Parameters.Add(writersPar);

            CrewSqlComm.Prepare();
            Console.WriteLine("Crew sql command prepared..");

            foreach (Crew crew in crews)
            {
                tconstCrewPar.Value = crew.TConst;
                directorsPar.Value = checkObjectForNull(crew.Directors);
                writersPar.Value = checkObjectForNull(crew.Writers);

                CrewSqlComm.ExecuteNonQuery();

            }
            Console.WriteLine("Crew sql command executed..");
        }
        public object checkObjectForNull(Object? value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }
    }
}
