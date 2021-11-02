using System;

namespace Core.Entities
{
    public class Token
    {
        public int Id { get; set; }
        public string TokenHash { get; set; }
        public DateTime TokenExp { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExp { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
