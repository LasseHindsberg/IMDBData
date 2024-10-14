// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using IMDBData;
using IMDBData.Models;
using System.Data.SqlClient;


IInserter inserter;

Console.WriteLine("DO stuff :D");
string input = Console.ReadLine();

switch (input)
{
    case "1":
        inserter = new NormalInserter();
        break;

    default:
        throw new Exception("Invalid input");
}

// Read title file
int lineCount = 0;
List<Title> Titles = new List<Title>();
string titleFilePath = "C:\\Users\\hurli\\Desktop\\School shit\\4th semester\\tempData\\title.basics.tsv";

foreach (string line in File.ReadLines(titleFilePath).Skip(1))
{
    if (lineCount == 50000)
    {
        break;
    }
    string[] splitLine = line.Split("\t");
    if (splitLine.Length != 9)
    {
        throw new Exception("upser :(" + line);
    }


    string tconst = splitLine[0];
    string primaryTitle = splitLine[2];
    string originalTitle = splitLine[3];
    bool isAdult = splitLine[4] == "1";
    int? startYear = ParseInt(splitLine[5]);
    int? endYear = ParseInt(splitLine[6]);
    int? runtimeMinutes = ParseInt(splitLine[7]);

    Title newTitle = new Title()
    {
        TConst = tconst,
        PrimaryTitle = primaryTitle,
        OriginalTitle = originalTitle,
        IsAdult = isAdult,
        StartYear = (int)startYear,
        EndYear = endYear,
        RuntimeMinutes = (int)runtimeMinutes
    };

    Titles.Add(newTitle);

    lineCount++;
}

Console.WriteLine("List of titles length: " + Titles.Count);

// Read person file
lineCount = 0;
List<Person> Persons = new List<Person>();
string personFilePath = "C:\\Users\\hurli\\Desktop\\School shit\\4th semester\\tempData\\name.basics.tsv";

foreach (string line in File.ReadLines(personFilePath).Skip(1))
{
    if (lineCount == 50000)
    {
        break;
    }
    string[] splitLine = line.Split("\t");
    if (splitLine.Length != 6)
    {
        throw new Exception("upser :(" + line);
    }

    string nconst = splitLine[0];
    string primaryName = splitLine[1];
    int? birthYear = ParseInt(splitLine[2]);
    int? deathYear = ParseInt(splitLine[3]);

    Person newPerson = new Person()
    {
        NConst = nconst,
        PrimaryName = primaryName,
        BirthYear = (int)birthYear,
        DeathYear = deathYear
    };

    Persons.Add(newPerson);

    lineCount++;
}

Console.WriteLine("List of persons length: " + Persons.Count);

// Read crew file
lineCount = 0;
List<Crew> Crews = new List<Crew>();
string crewFilePath = "C:\\Users\\hurli\\Desktop\\School shit\\4th semester\\tempData\\title.crew.tsv";

 foreach(string line in File.ReadLines(crewFilePath).Skip(1))
{
    if (lineCount == 50000)
    {
        break;
    }
    string[] splitLine = line.Split("\t");
    if (splitLine.Length != 3)
    {
        throw new Exception("upser :(" + line);
    }

    string tconst = splitLine[0];
    string directors = splitLine[1];
    string writers = splitLine[2];

    Crew newCrew = new Crew()
    {
        TConst = tconst,
        Directors = directors,
        Writers = writers
    };

    Crews.Add(newCrew);

    lineCount++;
}

SqlConnection sqlConn = new SqlConnection("server=localhost;database=imdbDatabase;user id=user;password=fiskmedkiks22;TrustServerCertificate=true");

sqlConn.Open();
SqlTransaction transAction = sqlConn.BeginTransaction();

DateTime before = DateTime.Now;

try
{ 
    inserter.Insert(Titles, Persons, Crews, sqlConn, transAction);
    transAction.Rollback();

}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    transAction.Rollback();
}

DateTime after = DateTime.Now;

sqlConn.Close();

Console.WriteLine("Milliseconds passed: " + (after - before).TotalMilliseconds);

int? ParseInt(string value)
{
    if (value == "\\N")
    {
        return null;
    }
    else
    {
        return int.Parse(value);
    }
}