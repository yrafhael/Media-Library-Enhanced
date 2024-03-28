using NLog;

class Program
{
    private static NLog.Logger logger = LogManager.LoadConfiguration(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

    static void Main(string[] args)
    {
        try
        {
            string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
            logger.Info(scrubbedFile);

            // creating new movie file object
            MovieFile movieFile = new MovieFile(scrubbedFile);

            // prompt for user
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Movie Display:");
                Console.WriteLine("Add Movie:");
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display All Movies");
                Console.WriteLine("3) Find movie");
                Console.WriteLine("Enter to quit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddMovie(movieFile);
                        break;
                    case "2":
                        DisplayAllMovies(movieFile);
                        break;
                    case "3":
                        FindMovie(movieFile);
                        break;
                    case "":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
        finally
        {
            logger.Info("Program ended");
        }
    }

    static void AddMovie(MovieFile movieFile)
    {
        // log the user's choice
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "|INFO|MediaLibrary.Program|User choice: \"1\"");

        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();

        List<string> genres = new List<string>();
        while (true)
        {
            Console.Write("Enter genre (or done to quit): ");
            string genre = Console.ReadLine();
            if (genre.ToLower() == "done")
                break;
            genres.Add(genre);
        }

        Console.Write("Enter movie director: ");
        string director = Console.ReadLine();

        Console.Write("Enter running time (h:m:s): ");
        string runtimeInput = Console.ReadLine();
        TimeSpan runtime = TimeSpan.Parse(runtimeInput);

        // Create a new movie object
        Movie movie = new Movie
        {
            title = title,
            director = director,
            runningTime = runtime,
            genres = genres
        };

        // Add the movie to the movie file
        movieFile.AddMovie(movie);

        // Log the addition of the movie
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "|INFO|MediaLibrary.MovieFile|Media id " + movie.mediaId + " added");
    }

    static void DisplayAllMovies(MovieFile movieFile)
    {
        // Display all movies in the movie file
        Console.WriteLine("All Movies:");
        foreach (var movie in movieFile.Movies)
        {
            Console.WriteLine(movie.Display());
        }
    }

    static void FindMovie(MovieFile movieFile)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        // LINQ - Where filter operator & Select projection operator & Contains quantifier operator
        var titles = movieFile.Movies.Where(m => m.title.Contains("Walking")).Select(m => m.title);
        // LINQ - Count aggregation method
        Console.WriteLine($"There are {titles.Count()} movies with \"Walking\" in the title:");
        foreach (string t in titles)
        {
            Console.WriteLine($"  {t}");
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}