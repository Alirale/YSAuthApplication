namespace Core.DTO
{
    public class TokenModel
    {
        public string Key { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
        public int expires { get; set; }
        public int RefreshTokenExpireshours { get; set; }
    }
}
