namespace Services.Interface
{
    public interface IEncryption
    {
        string HashHmac(string secret, string password);
    }
}
