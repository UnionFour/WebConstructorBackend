namespace WebConstructorBackend.Domain.ValueTypes
{
    public class TokenInput
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string EncryptedCode { get; set; } = string.Empty;
    }
}
