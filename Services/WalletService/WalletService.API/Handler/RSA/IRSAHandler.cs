namespace WalletService.API.Handler.RSA
{
    public interface IRSAHandler
    {
        string Encrypt(string text);
        string Decrypt(string encrypted);
    }
}
