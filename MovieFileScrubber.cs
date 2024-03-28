using NLog;
public static class FileScrubber
{
    private static NLog.Logger logger = LogManager.LoadConfiguration(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
    public static string ScrubMovies(string readFile)
    {
        try
        {
            string ext = readFile.Split('.').Last();
            string writeFile = readFile.Replace(ext, $"scrubbed.{ext}");

            if (File.Exists(writeFile))
            {
                logger.Info("File already scrubbed");
            }
            else
            {
                logger.Info("File scrub started");

                StreamWriter sw = new StreamWriter(writeFile);
                StreamReader sr = new StreamReader(readFile);
                sr.ReadLine(); // skip header
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    sw.WriteLine(line);
                }
                sw.Close();
                sr.Close();

                logger.Info("File scrub ended");
            }
            return writeFile;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
        return "";
    }
}
