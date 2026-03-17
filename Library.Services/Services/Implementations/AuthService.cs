using Library.DataAccess.Units;
using Library.Models.Models;
using Library.Services.Helpers;
using Library.Services.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility = Library.Services.Helpers.Utility;

namespace Library.Services.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _myUnit;
        private readonly IConfiguration _configuration;
        public AuthService(IUnitOfWork myUnit, IConfiguration configuration)
        {
            _myUnit = myUnit;
            _configuration = configuration;
        }

        public async Task<TransactionResult<string>> LoginAsync(string email, string password)
        {
            var user = await _myUnit.Users.GetAsync(u => u.Email ==  email);
            if (user == null)
            {
                return Utility.GetTransactionResult(false, "invalid credentials", "");
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isValidPassword)
            {
                return Utility.GetTransactionResult(false, "invalid credentials", "");

            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: _configuration["Jwt:Issuer"],     
               audience: _configuration["Jwt:Audience"], 
               claims: claims,
               expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:DurationInDays"])),
               signingCredentials: creds
            );
            string result = new JwtSecurityTokenHandler().WriteToken(token);
            return Utility.GetTransactionResult(true, "Success!", result);
        }

        public async Task<TransactionResult> RegisterAsync(User user, string plainPassword)
        {
            
           

            if (await _myUnit.Users.ExistsAsync(u => u.Email == user.Email))
            {
                return Utility.GetTransactionResult(false, "can't add user with existing email");
            }
            string libraryCard = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
            user.LibraryCardNumber = libraryCard;
            if (await _myUnit.Users.ExistsAsync(u => u.LibraryCardNumber == user.LibraryCardNumber))
            {
                return Utility.GetTransactionResult(false, "can't add user with existing card number");
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            await _myUnit.Users.AddAsync(user);
            await _myUnit.CommitChanges();

            return Utility.GetTransactionResult(true, "User added successfully");


        }
    }
}
