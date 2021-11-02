using Core.Entities;
using Core.ViewModel;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(int Id);
        public Task<User> GetUserByName(string UserName);
        public Task<User> GetUserByInfo(string Name, string HashedPassword);
        public Task<User> ValidateUser(RegisterViewModel user);
        public Task<User> AddUser(RegisterViewModel NewUser);
    }
}
