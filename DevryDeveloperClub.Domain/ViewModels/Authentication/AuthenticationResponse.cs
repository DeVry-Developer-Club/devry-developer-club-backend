namespace DevryDeveloperClub.Domain.ViewModels.Authentication
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
    }
}