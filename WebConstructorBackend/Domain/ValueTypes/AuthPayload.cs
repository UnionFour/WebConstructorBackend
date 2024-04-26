namespace WebConstructorBackend.Domain.ValueTypes
{
    public class AuthPayload
    {
        public TimeSpan TimeSpan { get; set; } = TimeSpan.FromMinutes(2);

        public DateTime Expiry { get; set; } = DateTime.Now;

        public string EncryptedCode { get; set; } = String.Empty;
    }
}
