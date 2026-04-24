
Azure key Vault : 
------------------
  It is a managed servcie that can be used to store secret information ,encryption keys and creating certificates 

  
First create an Azure key vault in azure portal and then you will see there three things keys,secrets and certificates 
so after creeating key vault and one key and create a dummy app go to console application and create an project with the same name of app 


Add these packages in console app

Azure.Identity 1.21.0 stable version add it 

Azure.Security.KeyVault.Keys  4.9.0 version 

add this much basic template into the console application 

 string tenantId = ""
 string clientId = "";
 string clientSecret = "";


 var credential = new ClientSecretCredential(tenantId,clientId,clientSecret);

 string vaultUrl = "https://key-vault-cg.vault.azure.net/";
 string keyName = "key-cg";


 var keyClient = new KeyClient(new Uri(vaultUrl), credential);


after writing above code properly write below lines 

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

convert your main method like this 

     static async Task Main(string[] args)


  Now i want to encrypt and decrypt a blob image of container 

  so use this prompt and get the code for this in local visual studio program 

  Lets forget now what i was asking now i will give u the task in proper order
so Now i am having a storage account raghustoragecg and data as container as shown in s1 image and now in side data container i want to add the image but i want to encrypt the image and add it and when i want to download or want to see the image i should decrypt the image and want to see for this in second image s2 I had created a key-vault with key key-cg so now i want to create an app in console visual studio 2026 to encrypt and decrypt the image which is to be kept in data container so in app registration i will create a new app and want to work with it as shown in image 3
I am having a sample program for just encrypting text as showin in image 4 but i want a simillar program like this where i can encrypt and decrypt image
so for this give me in detail all the steps in detail step by step with complete souce code of image encryption and decryption and what packages need to be added etc and what role to be assingned blob usage also mention


https://learn.microsoft.com/en-us/azure/storage/blobs/storage-encrypt-decrypt-blobs-key-vault?tabs=roles-azure-portal%2Cpackages-dotnetcli  //check this url also 

Code for encrypting an image before sending into blob container and decrypting the image after downloading and using key vault the code is written 
----------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Storage.Blobs;

namespace ImageEncryptDecrypt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string tenantId = "<TENANT_ID>";
            string clientId = "<CLIENT_ID>";
            string clientSecret = "<CLIENT_SECRET>";

            string vaultUrl = "https://<your-keyvault-name>.vault.azure.net/";
            string keyName = "<your-key-name>";

            string storageUrl = "https://<your-storage-account>.blob.core.windows.net/";
            string containerName = "<your-container-name>";

            string inputImagePath = @"C:\path\to\input.jpg";
            string outputImagePath = @"C:\path\to\output.jpg";

            string encryptedBlobName = "image.enc";
            string encryptedKeyBlobName = "key.enc";
            string ivBlobName = "iv.bin";

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var keyClient = new KeyClient(new Uri(vaultUrl), credential);
            KeyVaultKey key = (await keyClient.GetKeyAsync(keyName)).Value;

            var cryptoClient = new CryptographyClient(key.Id, credential);

            byte[] imageBytes = File.ReadAllBytes(inputImagePath);

            using Aes aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();

            byte[] encryptedImage;
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(imageBytes, 0, imageBytes.Length);
                cs.Close();
                encryptedImage = ms.ToArray();
            }

            EncryptResult encryptedKey = await cryptoClient.EncryptAsync(
                EncryptionAlgorithm.RsaOaep,
                aes.Key);

            var container = new BlobContainerClient(new Uri(storageUrl + containerName), credential);

            await container.GetBlobClient(encryptedBlobName).UploadAsync(new MemoryStream(encryptedImage), overwrite: true);
            await container.GetBlobClient(encryptedKeyBlobName).UploadAsync(new MemoryStream(encryptedKey.Ciphertext), overwrite: true);
            await container.GetBlobClient(ivBlobName).UploadAsync(new MemoryStream(aes.IV), overwrite: true);

            Console.WriteLine("Encrypted and uploaded.");

            byte[] downloadedImage = (await container.GetBlobClient(encryptedBlobName).DownloadContentAsync()).Value.Content.ToArray();
            byte[] downloadedKey = (await container.GetBlobClient(encryptedKeyBlobName).DownloadContentAsync()).Value.Content.ToArray();
            byte[] downloadedIV = (await container.GetBlobClient(ivBlobName).DownloadContentAsync()).Value.Content.ToArray();

            DecryptResult decryptedKey = await cryptoClient.DecryptAsync(
                EncryptionAlgorithm.RsaOaep,
                downloadedKey);

            using Aes aesDecrypt = Aes.Create();
            aesDecrypt.Key = decryptedKey.Plaintext;
            aesDecrypt.IV = downloadedIV;

            byte[] decryptedImage;
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aesDecrypt.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(downloadedImage, 0, downloadedImage.Length);
                cs.Close();
                decryptedImage = ms.ToArray();
            }

            File.WriteAllBytes(outputImagePath, decryptedImage);

            Console.WriteLine("Decrypted image saved.");
        }
    }
}

some settings to be done 
----------------------------
Storage Account Access
Storage Account → Access control (IAM)
→ Add role assignment
→ Role: Storage Blob Data Contributor
→ Assign to: BlobEncryptDecrypt (App Registration)
→ Review + Assign
🔹 Key Vault Access (RBAC)
Key Vault → Access control (IAM)
→ Add role assignment
→ Role: Key Vault Crypto User
→ Assign to: BlobEncryptDecrypt (App Registration)
→ Review + Assign

