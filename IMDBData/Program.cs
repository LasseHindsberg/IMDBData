// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using IMDBData;
using IMDBData.Models;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

// --- INSERTER CLASSES ---
IInserter inserter = null;
DataLoader dataLoader = new DataLoader();
bool shouldInsert = false;

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
        shouldInsert = true;
        inserter = new NormalInserter(); // KEEP OR REMOVE \[T]/
        break;

    case "2": 
        shouldInsert = true;
        inserter = new PreparedInserter();
        break;

    case "3":
        Console.WriteLine("Enter the title of the movie you want to search for");
        string title = Console.ReadLine();
        Console.WriteLine("You've searched for movies including: \n" + title + "\nSearching...");
        reader.SearchForMovieByTitle(title);
        break;
    
    case "4":
        Console.WriteLine("Enter the name of the person you want to search for: ");
        string name = Console.ReadLine();
        Console.WriteLine("You've searched for people including: \n" + name + "\nSearching...");
        reader.SearchByName(name);
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

if (shouldInsert)
{


// load data from files
string titleFilePath = "C:/temp/tempData/title.basics.tsv/title.basics.tsv";
string personFilePath = "C:/temp/tempData/name.basics.tsv/name.basics.tsv";
string crewFilePath = "C:/temp/tempData/title.crew.tsv/title.crew.tsv";

List<Title> titles = dataLoader.LoadTitles(titleFilePath);
List<Person> persons = dataLoader.LoadPersons(personFilePath);
List<Crew> crews = dataLoader.LoadCrews(crewFilePath);

Console.WriteLine("List of titles length: " + titles.Count);
Console.WriteLine("List of persons length: " + persons.Count);
Console.WriteLine("List of crews length: " + crews.Count);

    // --- TRY CONNECTION TO DATABASE ---
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

    SqlTransaction transAction = sqlConn.BeginTransaction();

    DateTime before = DateTime.Now;

    // --- INSERT DATA INTO DATABASE ---
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

    // --- CLOSE CONNECTION ---
    sqlConn.Close();

    // --- PRINT TIME TAKEN FOR INSERTION---
    Console.WriteLine("Milliseconds passed: " + (after - before).TotalMilliseconds);


}