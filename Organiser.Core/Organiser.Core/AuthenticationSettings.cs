namespace Organiser.Cores
{
    public class AuthenticationSettings
    {
        public string? JWTKey { get; set; }
        public int JWTExpiredDays { get; set; }
        public string? JWTIssuer { get; set; }
    }
}
