using BCrypt;

namespace NewsTrack.Identity.Encryption
{
    public class CryptoManager : ICryptoManager
    {
        public bool CheckPassword(string password, string cryptoPassword)
        {
            return BCryptHelper.CheckPassword(password, cryptoPassword);
        }

        public string HashPassword(string password)
        {
            return BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt());
        }
    }
}