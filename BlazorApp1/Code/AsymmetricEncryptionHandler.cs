using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BlazorApp1.Code
{
    public class AsymmetricEncryptionHandler
    {
        private readonly string _privateKey;
        private readonly string _publicKey;
        private readonly HttpClient _httpClient;

        public AsymmetricEncryptionHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;

            using (RSACryptoServiceProvider rsa = new())
            {
                _privateKey = rsa.ToXmlString(true);
                _publicKey = rsa.ToXmlString(false);
            }
        }

        public async Task<byte[]> AsymmetricEncrypt(string textToEncrypt) //TODO Send as byte[] instead of string
        {
            var payload = new
            {
                Data = textToEncrypt,
                PublicKey = _publicKey
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7040/api/Encryptor", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<string> AsymmetricDecrypt(string textToDecrypt) //TODO Send as byte[] instead of string
        {
            using (RSACryptoServiceProvider rsa = new())
            {
                rsa.FromXmlString(_privateKey);
                byte[] dataToDecrypt = Convert.FromBase64String(textToDecrypt);
                byte[] decryptedData = rsa.Decrypt(dataToDecrypt, false); //False cause API is not run on ancient Windows OS
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
    }
}
