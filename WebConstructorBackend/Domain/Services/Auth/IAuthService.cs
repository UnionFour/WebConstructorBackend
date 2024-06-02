using WebConstructorBackend.Domain.ValueTypes;

namespace WebConstructorBackend.Domain.Services.Auth
{
    public interface IAuthService
    {
        public UserPayload RegisterUser(UserAuthInput input);

        public UserPayload AuthorizeUser(UserAuthInput input);
    }
}
