namespace NewsTrack.Identity.Encryption
{
    public interface ICryptoManager
    {
        bool CheckPassword(string password, string cryptoPassword);
        string HashPassword(string password);
    }
}