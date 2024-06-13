using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        private IUserRepository users { get; }

        private IOrganizationRepository organizations { get; }

        public AuthService(IOptions<AuthOptions> authOptions,
            IDataProtectionProvider dataProtectionProvider,
            [FromServices] IUserRepository users,
            [FromServices] IOrganizationRepository organization)
        {
            AuthOptions = authOptions.Value;
            TimeLimitedDataProtector = TimeLimitedDataProtector = dataProtectionProvider
                .CreateProtector("auth")
                .ToTimeLimitedDataProtector();
            this.users = users;
            organizations = organization;
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
                    [JwtRegisteredClaimNames.Sub] = user.ID.ToString() ?? throw new InvalidDataException(),
                    [JwtRegisteredClaimNames.Name] = input.OrganizationName
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
                Token = accessToken,
                isOrganizator = user.IsOrganizator,
                OrganizationName = user.OrganizationName,
                IsCouch = user.IsCouch
            };
        }

        public UserPayload RegisterUser(UserAuthInput input)
        {
            var user = users.GetUserByEmail(input.Email);
            if (user == null)
            {
                if (input.IsCouch)
                    user = new Couch { IsCouch = true, IsOrganizator = false };
                else if (input.IsOrganizator)
                    user = new Organizator { IsCouch = true, IsOrganizator = true };
                else
                    user = new User { IsCouch = false, IsOrganizator = false };

                user.Email = input.Email;
                user.passHash = input.Password;
                user.OrganizationName = input.OrganizationName;
                if (organizations.GetOrganizationByName(user.OrganizationName) == null)
                    organizations.CreateOrganization(new Organization { Name = user.OrganizationName, Organizator = user as Organizator, OrganizatorID = user.ID});
                users.CreateUser(user);
            }
            else
                throw new Exception(message: "user with such Email is already exists");

            return AuthorizeUser(new UserAuthInput { Email = input.Email, Password = input.Password, OrganizationName = input.OrganizationName });
        }
    }
}
