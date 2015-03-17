
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace OpenTokSDK
{
    public class ArchiveCryptoUtils
    {
       /**
         * Decrypts the input byte array using the base64password and the certificate.
         * returns a decrypted array.
         * Note the certificate must have a private key to perform the decryption
         */
        public static void DecryptBlob(X509Certificate2 cert, String base64password, byte[] input, out byte[] output)
        {
            ArchivePassword sp = ArchivePassword.FromEncryptedBase64String(cert, base64password);
            Aes256Decrypt(input, out output, sp.Key, sp.IV);
        }
       /**
         * Encrypts the input byte array using the certificate to protect the password.
         * returns a protected password and the encrypted array.
         * Note the certificate must have a public key to perform the encryption
         */
        public static void EncryptBlob(X509Certificate2 cert, byte[] input, out String base64password, out byte[] output)
        {
            ArchivePassword sp = new ArchivePassword();
            Aes256Encrypt(input, out output, sp.Key, sp.IV);
            base64password = sp.ToEncryptedBase64String(cert);
        }

        private class ArchivePassword
        {
            public byte Version;
            public byte Alg;
            public byte Mode;
            public byte[] Key;
            public byte[] IV;

            public ArchivePassword()
            {
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                Version = 1; // Version=1
                Alg = 1;    // AES256
                Mode = 1;   // Mode CBC/PKCS#7 Padding
                Key = new byte[32];
                IV = new byte[16];
                rng.GetBytes(Key);
                rng.GetBytes(IV);
            }
            /**
              * Encrypts the password with the public key and encodes it as base64
              */
            public string ToEncryptedBase64String(X509Certificate2 certWithPublicKey)
            {
                byte[] raw = new byte[3 + Key.Length + IV.Length];
                raw[0] = Version;
                raw[1] = Alg;
                raw[2] = Mode;
                Buffer.BlockCopy(Key, 0, raw, 3, Key.Length);
                Buffer.BlockCopy(IV, 0, raw, 3 + Key.Length, IV.Length);
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certWithPublicKey.PublicKey.Key;
                // use OAEP padding
                byte[] secured = rsa.Encrypt(raw, true);
                return Convert.ToBase64String(secured);
            }
            /**
              * Decrypts the password using the private key
              */
            public static ArchivePassword FromEncryptedBase64String(X509Certificate2 certWithPrivKey, string base64password)
            {
              RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certWithPrivKey.PrivateKey;
              byte[] password = Convert.FromBase64String(base64password);
              // use OAEP padding
              byte[] data = rsa.Decrypt(password, true);
              if (data.Length < 3 || data[0] != 1 || data[1] != 1 || data[2] != 1)
              {
                  throw new InvalidDataException("invalid format");
              }

              ArchivePassword res = new ArchivePassword();
                res.Version = data[0];
                res.Alg = data[1];
                res.Mode = data[2];
                Buffer.BlockCopy(data, 3, res.Key, 0, res.Key.Length);
                Buffer.BlockCopy(data, 3 + res.Key.Length, res.IV, 0, res.IV.Length);
                return res;
            }
        };
        
        private static void Aes256Decrypt(byte[] input, out byte[] output, byte[] key, byte[] iv)
        {
            // Use this method carefully as it loads everything in memory
            RijndaelManaged rijndaelCSP = new RijndaelManaged();
            rijndaelCSP.Key = key;
            rijndaelCSP.IV = iv;
            rijndaelCSP.Padding = PaddingMode.PKCS7;
            rijndaelCSP.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = rijndaelCSP.CreateDecryptor();

            MemoryStream inputFileStream = new MemoryStream(input);
            CryptoStream decryptStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read);
            output = new byte[(int)inputFileStream.Length];
            int bytesRead = decryptStream.Read(output, 0, output.Length);
            Array.Resize(ref output, bytesRead);
            rijndaelCSP.Clear();
            decryptStream.Close();
            inputFileStream.Close();
        }

        private static void Aes256Encrypt(byte[] input, out byte[] output, byte[] key, byte[] iv)
        {
            // Use this method carefully as it loads everything in memory
            RijndaelManaged rijndaelCSP = new RijndaelManaged();
            rijndaelCSP.Key = key;
            rijndaelCSP.IV = iv;
            rijndaelCSP.Padding = PaddingMode.PKCS7;
            rijndaelCSP.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = rijndaelCSP.CreateEncryptor();

            MemoryStream outputStream = new MemoryStream();
            CryptoStream encryptStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);
            encryptStream.Write(input, 0, input.Length);
            encryptStream.FlushFinalBlock();
            output = outputStream.ToArray();
            rijndaelCSP.Clear();
            encryptStream.Close();
            outputStream.Close();
        }
    }
}

