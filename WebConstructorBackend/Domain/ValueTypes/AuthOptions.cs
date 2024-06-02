using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebConstructorBackend.Domain.ValueTypes
{
    public class AuthOptions
    {
        public string Issuer { get; set; } = "Issuer";

        public string Audience { get; set; } = "Audience";

        public string Key { get; set; } = "Key";

        public SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new(Encoding.UTF8.GetBytes(Key));
    }
}
