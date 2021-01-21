namespace NewsTrack.Identity.Encryption
{
    public class CryptoManager : ICryptoManager
    {
        public bool CheckPassword(string password, string cryptoPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, cryptoPassword);
        }

        public string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}