using Core.DTO;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface ITokenRepository
    {
        public Task<bool> CheckExistToken(string Token);
        public Task<int> AddToken(int UserId, TokenResponseDTO ResponseToken);
        public Task<Token> FindRefreshToken(string RefreshToken);
        public Task DeleteToken(int UserId);
    }
}
