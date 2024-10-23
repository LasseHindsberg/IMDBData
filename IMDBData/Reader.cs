using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBData
{
    public class Reader
    {
        private string ConnString = "server=localhost,1433;database=imdbDatabase;user id=User;password=fiskmedkiks22;TrustServerCertificate=true";
        public void SearchForMovieByTitle(string title)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnString))
            try
            {
                sqlConn.Open();

                    using (SqlCommand cmd = new SqlCommand("searchByTitle", sqlConn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Title", title));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string? primaryTitle = reader["primaryTitle"].ToString();
                                    string? year = reader["startYear"].ToString();
                                    // add more fields possibly?

                                    Console.WriteLine($"Title: {primaryTitle}, Year: {year}");
                                }
                            } 
                            else
                            {
                                Console.WriteLine("No movies found with the title.");
                            }
                        }
                    }
            }
                catch (SqlException ex)
                {
                    Console.WriteLine("an SQL error occured: " +ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("an error occured: " + ex.Message);
                }
        }



    }
}
