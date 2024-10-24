﻿// See https:aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using IMDBData;
using IMDBData.Models;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

// --- INSERTER CLASSES ---
// Handles bulk insertion of data into the database
IInserter inserter = null;
DataLoader dataLoader = new DataLoader();
bool shouldInsert = false;

// --- READER CLASS ---
// Handles all functionality for searching the database
Reader reader = new Reader();

// --- USER INPUT ---
Console.WriteLine(
    "What do you want to do? \n" +
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
        Console.WriteLine("Enter the primary title of the movie you want to add:");
        string primaryTitle = Console.ReadLine();
        Console.WriteLine("Enter the original title of the movie you want to add:");
        string originalTitle = Console.ReadLine();
        Console.WriteLine("Enter the year of release, of the movie you want to add:");
        int startYear = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter the runtime of the movie you want to add (in minutes):");
        int runtimeMinutes = int.Parse(Console.ReadLine());
        reader.addMovieToDatabase(primaryTitle, originalTitle, startYear, runtimeMinutes);
        break;

    case "6":
        Console.WriteLine("Enter the name of the person you want to add: ");
        string ActorName = Console.ReadLine();
        Console.WriteLine("Enter the birth year of the person you want to add: ");
        int birthYear = int.Parse(Console.ReadLine());
        reader.addPersonToDatabase(ActorName, birthYear);
        break;

    case "7":
        Console.WriteLine("Enter the Id of the movie you want to update: ");
        string movieId = Console.ReadLine();
        //cw write out the movie information from the id provided.

        Console.WriteLine("Enter the new title of the movie: ");
        string newTitle = Console.ReadLine();
        Console.WriteLine("Enter the new original title of the movie: ");
        string newOriginalTitle = Console.ReadLine();
        Console.WriteLine("Enter the new year of release, of the movie: ");
        int newStartYear = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter the new runtime of the movie (in minutes): ");
        int newRuntimeMinutes = int.Parse(Console.ReadLine());
        reader.UpdateMovie(movieId, newTitle, newOriginalTitle, newStartYear, newRuntimeMinutes);
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

    // take data from files and put them into lists
    List<Title> titles = dataLoader.LoadTitles(titleFilePath);
    List<Person> persons = dataLoader.LoadPersons(personFilePath);
    List<Crew> crews = dataLoader.LoadCrews(crewFilePath);

    // Print the length of the lists
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

    // --- TRY TO INSERT DATA INTO DATABASE ---
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

