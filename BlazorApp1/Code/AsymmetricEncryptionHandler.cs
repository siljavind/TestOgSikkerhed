using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;

namespace BlazorApp1.Code
{
    public class AsymmetricEncryptionHandler
    {
        private string _privateKey;
        private string _publicKey;
        private readonly HttpClient _httpClient;

        //private const string PublicKeyPath = "publicKey.xml";
        //private const string PrivateKeyPath = "privateKey.xml";

        public AsymmetricEncryptionHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
            LoadKeysFromXmlFiles();
        }

        public async Task<string> Encrypt(string textToEncrypt)
        {
            string[] payload = [textToEncrypt, _publicKey];
            string serializedPayload = JsonConvert.SerializeObject(payload);
            StringContent stringContent = new StringContent(serializedPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7040/api/Encryptor", stringContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Decrypt(string textToDecrypt)
        {
            using (RSACryptoServiceProvider rsa = new())
            {
                rsa.FromXmlString(_privateKey);
                byte[] dataToDecrypt = Convert.FromBase64String(textToDecrypt);
                byte[] decryptedData = rsa.Decrypt(dataToDecrypt, true);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public void LoadKeysFromXmlFiles()
        {
            string privateKeyPath = "privateKey.xml";
            string publicKeyPath = "publicKey.xml";

            if (File.Exists(privateKeyPath) && File.Exists(publicKeyPath))
            {
                _privateKey = File.ReadAllText(privateKeyPath);
                _publicKey = File.ReadAllText(publicKeyPath);
            }
            else
            {
                SaveKeysToXmlFiles();
            }
        }

        public void SaveKeysToXmlFiles()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Generate keys if they don't exist
                string privateKeyPath = "privateKey.xml";
                string publicKeyPath = "publicKey.xml";

                if (!File.Exists(privateKeyPath) || !File.Exists(publicKeyPath))
                {
                    // Save the private key to a file
                    string privateKeyXml = rsa.ToXmlString(true);
                    File.WriteAllText(privateKeyPath, privateKeyXml);

                    // Save the public key to a file
                    string publicKeyXml = rsa.ToXmlString(false);
                    File.WriteAllText(publicKeyPath, publicKeyXml);
                    LoadKeysFromXmlFiles();
                }
            }
        }
    }
}
