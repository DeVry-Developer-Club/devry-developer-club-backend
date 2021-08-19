namespace DevryDeveloperClub.Infrastructure.Options
{
    /// <summary>
    /// Contractual obligation for database settings
    /// </summary>
    public interface IDatabaseOptions
    {
        string Host { get; set; }
        string DatabaseName { get; set; }
    }
    
    /// <summary>
    /// Data structure that holds our database options
    /// </summary>
    public class DatabaseOptions : IDatabaseOptions
    {
        public string Host { get; set; }
        public string DatabaseName { get; set; }
    }
}