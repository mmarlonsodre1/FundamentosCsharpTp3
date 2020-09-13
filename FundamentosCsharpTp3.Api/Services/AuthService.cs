using FundamentosCsharpTp3.WebApplication.Repository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FundamentosCsharpTp3.Models;

namespace FundamentosCsharpTp3.Api.Services
{
    public class AuthService : IAuthService
    {
        public UserRepository UserRepository { get; set; }

        public JwtSecurityTokenHandler TokenHandler { get; set; }

        public CommonSettings Settings { get; set; }

        public string key;

        public AuthService(UserRepository userRepository, IConfiguration config)
        {
            UserRepository = userRepository;
            Settings = new CommonSettings(config);
            key = Settings.GetJwtSecret();
            TokenHandler = new JwtSecurityTokenHandler();
        }

        private string GenerateToken(Guid Id)
        {
            SecurityTokenDescriptor TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken Token = TokenHandler.CreateToken(TokenDescriptor);
            return TokenHandler.WriteToken(Token);
        }

        public AuthenticationReturn Authenticate(User model)
        {
            var user = UserRepository.GetByUsername(model.Username);
            if (user == null)
                return new AuthenticationReturn { Status = false };

            string token = GenerateToken(user.Id);

            return new AuthenticationReturn { Status = true, Token = token, Id = user.Id };
        }
    }

    public class CommonSettings
    {
        private readonly IConfiguration Configuration;

        public CommonSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GetJwtSecret()
        {
            return Configuration["JWT:Key"];
        }
    }
}
