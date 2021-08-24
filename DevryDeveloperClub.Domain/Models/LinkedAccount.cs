namespace DevryDeveloperClub.Domain.Models
{
    [System.Serializable]
    public class LinkedAccount
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Provider { get; set; }
    }
}