using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace BlazorApp1.Code
{
    public class HashingHandler
    {
        //TODO Find up to date hashing algorithm
        public string MD5Hashing(string textToHash) //TODO REMOVE BEFORE HANDING IN
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(textToHash);
            byte[] hashedValue = md5.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashedValue);
        }

        public string SHA256Hashing(string textToHash) //TODO REMOVE BEFORE HANDING IN
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(textToHash);
            byte[] hashedValue = SHA256.HashData(inputBytes);
            return Convert.ToBase64String(hashedValue);
        }

        private string HMACHashing(string textToHash) //TODO Can be used? Probably shouldn't be used
        {
            byte[] myKey = Encoding.ASCII.GetBytes("NielsErMinFavoritLærer");
            byte[] inputBytes = Encoding.ASCII.GetBytes(textToHash);

            HMACSHA256 hmac = new() { Key = myKey, };
            byte[] hashedValue = hmac.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashedValue);
        }

        private string PBKDF2Hashing(string textToHash) //TODO Can be used?
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(textToHash);
            byte[] salt = Encoding.ASCII.GetBytes("SaltyBoii");
            var hashAlgo = new HashAlgorithmName("SHA256");
            var hashedValue = Rfc2898DeriveBytes.Pbkdf2(inputBytes, salt, 11, hashAlgo, 32);

            return Convert.ToBase64String(hashedValue);
        }

        public string BCryptHashing(string textToHash) //TODO Most likely the best option, cannot be used to compare if CPR matches in same way
        {
            //return BCrypt.Net.BCrypt.HashPassword(textToHash);
            int workFactor = 11;
            string salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);
            bool enhancedEntropy = true;
            HashType hashType = HashType.SHA512;
            return BCrypt.Net.BCrypt.HashPassword(textToHash, salt, enhancedEntropy);
        }

        public bool BCryptVerify(string textToVerify, string hash) //TODO Use this to verify if CPR matches
        {
            //return BCrypt.Net.BCrypt.Verify(textToVerify, hash);
            return BCrypt.Net.BCrypt.Verify(textToVerify, hash, true);
        }
    }
}