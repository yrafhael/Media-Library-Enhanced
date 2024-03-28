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
}