using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tinygubackend.Contexts;
using Tinygubackend.Core.Exceptions;
using Tinygubackend.Models;

namespace Tinygubackend.Services
{
    public interface IAuthService
    {
        string Token { get; set; }
        Task Authorize(string username, string password);
        void CheckJwt(string token);
    }
    public class AuthService : IAuthService
    {
        private readonly TinyguContext _context;
        private readonly IConfiguration _config;
        public string Token { get; set; }

        public AuthService(IConfiguration configuration, TinyguContext context)
        {
            _context = context;
            _config = configuration;
        }

        public async Task Authorize(string username, string password)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            User user = await _context.Users.FirstOrDefaultAsync(_ => _.Name == username);

            if (user == null)
            {
                throw new EntityNotFoundException("Could not find user!");
            }

            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Typ, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    expires : DateTime.Now.AddMinutes(30),
                    signingCredentials : creds);
                Token = new JwtSecurityTokenHandler().WriteToken(token);
                return;
            }
            throw new UnauthorizedAccessException("Credentials incorrect!");
        }

        public void CheckJwt(string token)
        {
            // try
            // {
            //     IJsonSerializer serializer = new JsonNetSerializer();
            //     IDateTimeProvider provider = new UtcDateTimeProvider();
            //     IJwtValidator validator = new JwtValidator(serializer, provider);
            //     IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //     IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            //     // var json = decoder.Decode(token, _secret, verify : true);
            //     // User = JsonConvert.DeserializeObject<User>(json);
            //     // Console.WriteLine("Authorized user: " + User);
            // }
            // catch (TokenExpiredException)
            // {
            //     throw new UnauthorizedAccessException("Token expired!");
            // }
            // catch (SignatureVerificationException)
            // {
            //     throw new UnauthorizedAccessException("Signature invalid!");
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e.ToString());
            //     throw;
            // }
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 11);
        }

        public void CheckAuthentication(int id = -1, string role = "admin")
        {
            // if (User.Id != id && User.Role != "admin" && User.Role != role)
            // {
            //     throw new UnauthorizedAccessException();
            // }
        }

        public static string GenerateRandomToken()
        {
            Guid g = Guid.NewGuid();
            return Convert.ToBase64String(g.ToByteArray()).Replace('/', '_');
        }
    }
}