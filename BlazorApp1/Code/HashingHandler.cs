using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace BlazorApp1.Code
{
    public class HashingHandler
    {
        public string BCryptHashing(string textToHash) =>
            BCrypt.Net.BCrypt.HashPassword(textToHash);

        public bool BCryptVerify(string textToVerify, string hash) =>
            BCrypt.Net.BCrypt.Verify(textToVerify, hash);
    }
}