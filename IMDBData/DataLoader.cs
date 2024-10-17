using IMDBData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBData
{
    public class DataLoader
    {
        int lineCount = 0;
        public List<Title> LoadTitles(string filePath)
        {
            List<Title> titles = new List<Title>();
            foreach (string line in File.ReadLines(filePath).Skip(1))
            {
                if (lineCount == 50000)
                {
                    break;
                }
                string[] splitLine = line.Split("\t");
                if (splitLine.Length != 9)
                {
                    throw new Exception("Invalid line:" + line);
                }

                string tconst = splitLine[0];
                string primaryTitle = splitLine[2];
                string originalTitle = splitLine[3];
                bool isAdult = splitLine[4] == "1";
                int? startYear = ParseInt(splitLine[5]);
                int? endYear = ParseInt(splitLine[6]);
                int? runtimeMinutes = ParseInt(splitLine[7]);


                titles.Add(new()
                {
                    TConst = tconst,
                    PrimaryTitle = primaryTitle,
                    OriginalTitle = originalTitle,
                    IsAdult = isAdult,
                    StartYear = startYear,
                    EndYear = endYear,
                    RuntimeMinutes = runtimeMinutes
                });

                lineCount++;
            }
            return titles;
        }
        public List<Person> LoadPersons(string filePath)
        {
        lineCount = 0;
            List<Person> persons = new List<Person>();
            foreach (string line in File.ReadLines(filePath).Skip(1))
            {


                if (lineCount == 50000)
                {
                    break;
                }
                string[] splitLine = line.Split("\t");
                if (splitLine.Length != 6)
                {
                    throw new Exception("Invalid line:" + line);
                }

                string nconst = splitLine[0];
                string primaryName = splitLine[1];
                int? birthYear = ParseInt(splitLine[2]);
                int? deathYear = ParseInt(splitLine[3]);

                persons.Add(new()
                {
                    NConst = nconst,
                    PrimaryName = primaryName,
                    BirthYear = birthYear,
                    DeathYear = deathYear
                });

                lineCount++;
            }
            return persons;
        }
        public List<Crew> LoadCrews(string filePath)
        {
            lineCount = 0;
            List<Crew> crews = new List<Crew>();
            foreach (string line in File.ReadLines(filePath).Skip(1))
            {
                if (lineCount == 50000)
                {
                    break;
                }
                string[] splitLine = line.Split("\t");
                if (splitLine.Length != 3)
                {
                    throw new Exception("Invalid line:" + line);
                }

                string tconst = splitLine[0];
                string directors = splitLine[1];
                string writers = splitLine[2];

                crews.Add(new()
                {
                    TConst = tconst,
                    Directors = directors,
                    Writers = writers
                });

                lineCount++;
            }
            return crews;
        }

        int? ParseInt(string value)
        {
            if (value.ToLower() == "\\n") // checks if it is \n
                {
                    return null;
                }
                return int.Parse(value);

        }
    }
}
