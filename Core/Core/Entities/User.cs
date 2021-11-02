using System.Collections.Generic;

namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public ICollection<Token> tokens { get; set; }
    }
}
