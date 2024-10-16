// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using IMDBData;
using IMDBData.Models;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

// --- INSERTER CLASSES ---
IInserter inserter = null;


// --- USER INPUT ---
Console.WriteLine(
    "What do you want to do? \n " +
    " 1. Use normal inserter \n" +
    " 2. Use prepared inserter \n" +
    " 3. search for a movie by title \n" +
    " 4. search for a person by name \n" +
    " 5. add a movie to the database \n" +
    " 6. Add a person to the database \n" +
    " 7. Update Movie information. \n" +
    " ---------------------------------");


string? input = Console.ReadLine();


// --- SWITCH STATEMENT ---
switch (input)
{
    case "1":
        inserter = new NormalInserter(); // KEEP OR REMOVE \[T]/
        break;

    case "2": 
        inserter = new PreparedInserter();
        break;

    case "3":
        Console.WriteLine("Enter the title of the movie you want to search for");
        // cw("Enter the title of the movie you want to search for: ");
        // string title = Console.ReadLine();
        // searchForMovieByTitle(title);
        break;
    
    case "4":
        //cw("Enter the name of the person you want to search for: ");
        //string name = Console.ReadLine();
        //searchForPersonByName(name);
        break;

    case "5":
        //cw("Enter the title of the movie you want to add: ");
        //string title = Console.ReadLine();
        //addMovieToDatabase(title);
        break;

    case "6":
        //cw("Enter the name of the person you want to add: ");
        //string name = Console.ReadLine();
        //addPersonToDatabase(name);
        break;

    case "7": 
        //cw("Enter the title of the movie you want to update: ");
        //string title = Console.ReadLine();
        //updateMovieInformation(title);
        break;

    default:
        throw new Exception("Invalid input");
}


// --- FUNCTIONALITY OF INSERTION OF DATA INTO DATABASE ---

// Read title file
int lineCount = 0;
List<Title> titles = new List<Title>();
string titleFilePath = "C:/temp/tempData/title.basics.tsv/title.basics.tsv";

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
        StartYear = startYear,
        EndYear = endYear,
        RuntimeMinutes = runtimeMinutes
    };

    titles.Add(newTitle);

    lineCount++;
}

Console.WriteLine("List of titles length: " + titles.Count);

// Read person file
lineCount = 0;
List<Person> persons = new List<Person>();
string personFilePath = "C:/temp/tempData/name.basics.tsv/name.basics.tsv";

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
        BirthYear = birthYear,
        DeathYear = deathYear
    };

    persons.Add(newPerson);

    lineCount++;
}

Console.WriteLine("List of persons length: " + persons.Count);

// Read crew file
lineCount = 0;
List<Crew> crews = new List<Crew>();
string crewFilePath = "C:/temp/tempData/title.crew.tsv/title.crew.tsv";

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

    crews.Add(newCrew);

    lineCount++;
}

Console.WriteLine("List of Crew length: " + crews.Count);

SqlConnection sqlConn = new SqlConnection("server=localhost,1433;database=imdbDatabase;user id=User;password=fiskmedkiks22;TrustServerCertificate=true");
try
{
    sqlConn.Open();
}
catch (SqlException ex)
{
    Console.WriteLine("SQL Exception: " + ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine("General Exception: " + ex.Message);
}

// sqlConn.Open();
SqlTransaction transAction = sqlConn.BeginTransaction();

DateTime before = DateTime.Now;

try
{ 
    inserter.Insert(titles, persons, crews, sqlConn, transAction);
    transAction.Commit();
    Console.WriteLine("Insertion finished.");
    // transAction.Rollback();

}
catch (Exception e)
{
    Console.WriteLine("inserter failed: " + e.Message);
    transAction.Rollback();
}

DateTime after = DateTime.Now;

sqlConn.Close();

Console.WriteLine("Milliseconds passed: " + (after - before).TotalMilliseconds);

int? ParseInt(string value)
{
    if (value.ToLower() == "\\n") // checks if it is \n
    {
        return null;
    }
    return int.Parse(value);

}