using Microsoft.AspNetCore.DataProtection;

namespace BlazorApp1.Code
{
    public class SymmetricEncryptionHandler(IDataProtectionProvider dataProtectionProvider)
    {
        private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector("SuperSecretKey");

        public string Protect(string textToEncrypt) =>
            _dataProtector.Protect(textToEncrypt);

        public string Unprotect(string textToDecrypt) =>
            _dataProtector.Unprotect(textToDecrypt);
    }
}