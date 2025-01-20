using Jobify.BL.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobifyTests.Services
{
    public class JwtTokenProviderTests
    {
        private readonly JwtTokenProvider _jwtTokenProvider;
        private readonly IConfiguration _configuration;

        public JwtTokenProviderTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "JwtSettings:Secret", "1xvawozgzh78q2m9xpdlshegaqaspkpe" } 
                })
                .Build();
            _jwtTokenProvider = JwtTokenProvider.GetInstance(_configuration);
        }

        [Fact]
        public void GetInstance_ShouldReturnSingletonInstance()
        {
            // Act
            var instance1 = JwtTokenProvider.GetInstance(_configuration);
            var instance2 = JwtTokenProvider.GetInstance(_configuration);

            // Assert
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void GenerateEmailVerificationToken_ShouldReturnValidToken()
        {
            // Arrange
            string email = "minerva.mcgonagall@example.com";
            string role = "Student";

            // Act
            var token = _jwtTokenProvider.GenerateEmailVerificationToken(email, role);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public void ValidateToken_ShouldReturnEmail_ForValidToken()
        {
            // Arrange
            string email = "minerva.mcgonagall@example.com";
            string role = "Student";
            var token = _jwtTokenProvider.GenerateEmailVerificationToken(email, role);

            // Act
            var result = _jwtTokenProvider.ValidateToken(token);

            // Assert
            Assert.Equal(email, result);
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_ForExpiredToken()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, "expired@example.com"),
                    new Claim(ClaimTypes.Role, "User")
                }),
                NotBefore = DateTime.UtcNow.AddMinutes(-5), // Token is valid 5 minutes in the past
                Expires = DateTime.UtcNow.AddSeconds(-1), // Token already expired
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var expiredToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            // Act & Assert
            var exception = Assert.Throws<SecurityTokenException>(() =>
                _jwtTokenProvider.ValidateToken(expiredToken)
            );

            Assert.Contains("expired", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ValidateToken_ShouldThrowException_ForTokenMissingEmailClaim()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "User") }), 
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            // Act & Assert
            var exception = Assert.Throws<SecurityTokenException>(() =>
                _jwtTokenProvider.ValidateToken(token)
            );

            Assert.Contains("Sequence contains no matching element", exception.Message);
        }

    }
}

