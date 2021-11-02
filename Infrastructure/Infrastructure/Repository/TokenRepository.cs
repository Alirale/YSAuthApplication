using Core.DTO;
using Core.Entities;
using Core.RepositoryInterface;
using Core.Tools;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class TokenRepository: ITokenRepository
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public TokenRepository(DatabaseContext context, IConfiguration configuration, IUserRepository userRepository)
        {
            _context = context;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task<bool> CheckExistToken(string Token)
        {
            var helper = new SecurityHelper();
            string tokenHash = helper.Getsha256Hash(Token);
            var userToken = await _context.tokens.Where(p => p.TokenHash == tokenHash).FirstOrDefaultAsync();
            return userToken == null ? false : true;
        }

        public async Task<int> AddToken(int UserId, TokenResponseDTO ResponseToken)
        {
            var helper = new SecurityHelper();
            var token = new Token()
            {
                UserId = UserId,
                RefreshToken = helper.Getsha256Hash(ResponseToken.RefreshToken),
                RefreshTokenExp = DateTime.Now.AddMinutes(int.Parse(_configuration["JWtConfig:RefreshTokenExpireshours"])),
                TokenExp = DateTime.Now.AddMinutes(int.Parse(_configuration["JWtConfig:expires"])),
                TokenHash = helper.Getsha256Hash(ResponseToken.Token),
            };
            await _context.tokens.AddAsync(token);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Token> FindRefreshToken(string RefreshToken)
        {
            var helper = new SecurityHelper();
            string RefreshTokenHash = helper.Getsha256Hash(RefreshToken);
            var PersonToken = await _context.tokens.Include(p => p.User)
                .SingleOrDefaultAsync(p => p.RefreshToken == RefreshTokenHash);
            return PersonToken;
        }

        public async Task DeleteToken(int UserId)
        {
            var Person = await _userRepository.GetUserById(UserId);
            if (Person != null)
            {
                _context.tokens.RemoveRange(Person.tokens);
                await _context.SaveChangesAsync();
            }
        }
    }
}
