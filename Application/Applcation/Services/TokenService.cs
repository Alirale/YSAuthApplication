using Core.DTO;
using Core.RepositoryInterface;
using Core.Tools;
using Core.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Applcation.Services
{
    public interface ITokenService 
    {
        public Task<TokenResponseDTO> Register(RegisterViewModel dto);
        public Task<TokenResponseDTO> LoginwithUserPass(RegisterViewModel Person);
        public Task<TokenResponseDTO> LoginwithRefreshToken(string RefreshToken);
        public Task<TokenResponseDTO> GenerateToken(int PersonId);
        public string RefreshTokenGenerator();
    }

    public class TokenService: ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;

        public TokenService(IUserRepository userRepository , ITokenRepository tokenRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        public async Task<TokenResponseDTO> Register(RegisterViewModel dto)
        {
            var memberdoesntexist = await _userRepository.GetUserByName(dto.UserName) == null;
            if (memberdoesntexist)
            {
                var helper = new SecurityHelper();
                var Person = await _userRepository.AddUser(dto);
                var PersonGenerated = await _userRepository.GetUserByInfo(Person.UserName, Person.HashedPassword);
                var token = await GenerateToken(PersonGenerated.Id);
                var personTokens = await _tokenRepository.AddToken(PersonGenerated.Id, token);
                return personTokens > 0 ? token : null;
            }return null;
        }

        public async Task<TokenResponseDTO> LoginwithUserPass(RegisterViewModel Person)
        {
            var person = await _userRepository.ValidateUser(Person);
            if (person != null)
            {
                await _tokenRepository.DeleteToken(person.Id);
                var token = await GenerateToken(person.Id);
                var personTokens = await _tokenRepository.AddToken(person.Id, token);
                return personTokens > 0 ? token : null;
            }return null;
        }

        public async Task<TokenResponseDTO> LoginwithRefreshToken(string RefreshToken)
        {
            var PersonToken = await _tokenRepository.FindRefreshToken(RefreshToken);
            if (PersonToken != null)
            {
                await _tokenRepository.DeleteToken(PersonToken.User.Id);
                var token = await GenerateToken(PersonToken.User.Id);
                var personTokens = await _tokenRepository.AddToken(PersonToken.User.Id, token);
                return personTokens > 0 ? token : null;
            }return null;
        }

        public async Task<TokenResponseDTO> GenerateToken(int PersonId)
        {
            var Person = await _userRepository.GetUserById(PersonId);

            var claims = new List<Claim>
            {
                new Claim("Id", Person.Id.ToString() ?? throw new InvalidOperationException()),
                new Claim(ClaimTypes.Name, Person.UserName ?? throw new InvalidOperationException()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(1)).ToUnixTimeSeconds().ToString()),
            };
            string key = _configuration["JWtConfig:Key"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenexp = DateTime.Now.AddMinutes(int.Parse(_configuration["JWtConfig:expires"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWtConfig:issuer"],
                audience: _configuration["JWtConfig:audience"],
                expires: tokenexp,
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: credentials
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponseDTO
            {
                Token = jwtToken,
                RefreshToken = RefreshTokenGenerator()
            };
        }

        public string RefreshTokenGenerator()
        {
            var random = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);
            var Output = Convert.ToBase64String(random);
            return Output;
        }
    }
}
