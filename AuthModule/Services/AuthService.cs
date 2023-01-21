using CostEventManegement.AuthModule.Models;
using CostEventManegement.DatabaseModule;
using CostEventManegement.DatabaseModule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.AuthModule.Services
{
    public class AuthService: IAuthService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoggedUser> Login(LoginDTO userLoginModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == userLoginModel.Email.ToLower());
            if (user == null)
                return null;
            if (!VerifyPasswordHash(userLoginModel.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Tokens:Key").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration.GetSection("Tokens:Issuer").Value,
                _configuration.GetSection("Tokens:Issuer").Value,
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
                );


            var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.CreateToken(token12);
            return new LoggedUser() { Username = user.Email, Token = tokenHandler.WriteToken(token), UserId = user.Id };
        }

        public Task<bool> LogOff(string userName)
        {
            return null;
        }

        public async Task<LoggedUser> Register(UserDTO userRegistrationModel)
        {
            User newUser = new User();
            newUser.Surname = userRegistrationModel.Surname;
            newUser.Name = userRegistrationModel.Name;  
            newUser.Email = userRegistrationModel.Email;    

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userRegistrationModel.Password, out passwordHash, out passwordSalt);
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return await Login(new LoginDTO() { Email = newUser.Email, Password = userRegistrationModel.Password });

        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                    if (computedHash[i] != passwordHash[i]) return false;

            }
            return true;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            int count = await _context.Users.CountAsync(x => x.Email == userName);
            if (count == 0) return false;
            return true;
        }
    }
}
