namespace API.Models.Tokens
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}