using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Text;

namespace batch2app82
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            string tenantId = "48a81f39-8dfe-4a5b-9e55-fdee7afef221";
             string clientId = "2d959571-e403-41ce-97f6-afd1c41bd291";
            string clientSecret = "Iul8Q~sBykx6boZI5KGcydmi2f99Q_x2i181wbZb";


            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            string vaultUrl = "https://batch2keyvault82.vault.azure.net/";
            string keyName = "batch2key82";


            var keyClient = new KeyClient(new Uri(vaultUrl), credential);

            KeyVaultKey key;

            key = await keyClient.GetKeyAsync(keyName);

            string originalText = "Sensitive order data for CloudXeus Technology Services";
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(originalText);

            var cryptoClient = new CryptographyClient(key.Id, credential);

            EncryptResult encryptResult = await cryptoClient.EncryptAsync(
                EncryptionAlgorithm.RsaOaep,
                plaintextBytes);

            Console.WriteLine("Encrypted text (Base64):");
            Console.WriteLine(Convert.ToBase64String(encryptResult.Ciphertext));

            DecryptResult decryptResult = await cryptoClient.DecryptAsync(
                EncryptionAlgorithm.RsaOaep,
                encryptResult.Ciphertext);

            string decryptedText = Encoding.UTF8.GetString(decryptResult.Plaintext);

            Console.WriteLine("\nDecrypted text:");
            Console.WriteLine(decryptedText);

            Console.ReadLine();


        }
    }
}
