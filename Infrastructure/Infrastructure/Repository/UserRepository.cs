using Core.Entities;
using Core.RepositoryInterface;
using Core.Tools;
using Core.ViewModel;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int Id)
        {
            var user = await _context.users.Include(p => p.tokens).FirstOrDefaultAsync(p => p.Id == Id);
            return user;
        }

        public async Task<User> GetUserByName(string UserName)
        {
            var user = await _context.users.Include(p => p.tokens).FirstOrDefaultAsync(p => p.UserName == UserName);
            return user;
        }
        public async Task<User> GetUserByInfo(string Name, string HashedPassword)
        {
            var User = await _context.users.FirstOrDefaultAsync(p => p.UserName == Name && p.HashedPassword == HashedPassword);
            return User;
        }

        public async Task<User> ValidateUser(RegisterViewModel user)
        {
            var helper = new SecurityHelper();
            var HashPass = helper.Getsha256Hash(user.Password);
            var Person = await _context.users.Include(p => p.tokens).FirstOrDefaultAsync(x => x.UserName == user.UserName && x.HashedPassword == HashPass);
            if (Person != null)
            {
                return Person;
            }
            return null;
        }

        public async Task<User> AddUser(RegisterViewModel NewUser)
        {
            var helper = new SecurityHelper();
            var User = new User()
            {
                UserName = NewUser.UserName,
                HashedPassword = helper.Getsha256Hash(NewUser.Password),
            };
            await _context.users.AddAsync(User);
            var result = await _context.SaveChangesAsync();
            return User;
        }
    }
}
