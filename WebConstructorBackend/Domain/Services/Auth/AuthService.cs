using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;
using WebConstructorBackend.Domain.Services.Repositories;
using WebConstructorBackend.Domain.ValueTypes;

namespace WebConstructorBackend.Domain.Services.Auth
{
    public class AuthService : IAuthService
    {
        private AuthOptions AuthOptions { get; }
        private ITimeLimitedDataProtector TimeLimitedDataProtector { get; }
        private IUserRepository users;

        public AuthService(AuthOptions authOptions, ITimeLimitedDataProtector timeLimitedDataProtector, [FromServices] IUserRepository users)
        {
            AuthOptions = authOptions;
            TimeLimitedDataProtector = timeLimitedDataProtector;
            this.users = users;
        }

        public UserPayload AuthorizeUser(UserAuthInput input)
        {
            var user = users.GetUserByEmail(input.Email);

            if (user == null) throw new Exception(message: "User is not registrated");
            if (user.passHash != input.Password) throw new Exception(message: "wrong Password or Email");

            var handler = new JsonWebTokenHandler();
            var accessToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                {
                    [JwtRegisteredClaimNames.Email] = input.Email,
                    [JwtRegisteredClaimNames.Sub] = user.ID.ToString() ?? throw new InvalidDataException()
                },
                Issuer = AuthOptions.Issuer,
                Audience = AuthOptions.Audience,
                Expires = DateTime.Now.AddMinutes(15),
                TokenType = "Bearer",
                SigningCredentials = new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256)
            });

            return new UserPayload
            {
                Id = user.ID.ToString(),
                Login = user.Email,
                Token = accessToken
            };
        }

        public UserPayload RegisterUser(UserAuthInput input)
        {
            var user = users.GetUserByEmail(input.Email);
            if (user == null)
            {
                if (input.IsCouch)
                    user = new Couch { Email = input.Email, passHash = input.Password };
                else if (input.IsOrganizator)
                    user = new Organizator { Email = input.Email, passHash = input.Password };
                else
                    user = new User { Email = input.Email, passHash = input.Password };

                users.CreateUser(user);
            }
            else
                throw new Exception(message: "user with such Email is already exists");

            return AuthorizeUser(new UserAuthInput { Email = input.Email, Password = input.Password });
        }
    }
}
