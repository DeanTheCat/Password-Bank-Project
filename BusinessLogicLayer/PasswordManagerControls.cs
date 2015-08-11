using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class PasswordManagerControls
    {
        DataAccessLayer.DBController DBConn = new DataAccessLayer.DBController();
        
        public int storePassword(string user, string url, string username, string password)
        {
            byte[] iv;
            byte[] key;
            byte[] encrypted;
            string encryptedPass;
            string ivString;
            string keyString;
            int result;
            
            using (Aes myAes = Aes.Create())
            {
                iv = myAes.IV;
                key = myAes.Key;
                
                encrypted = EncryptStringToBytes_Aes(password, key, iv);
            }

            encryptedPass = Convert.ToBase64String(encrypted);
            ivString = Convert.ToBase64String(iv);
            keyString = Convert.ToBase64String(key);

            result = DBConn.storePasswordDetails(url, username, encryptedPass, ivString, keyString, user);

            return result;
        }

        public string getDecryptedPassword(string user, string url)
        {
            byte[] ivRetrieved;
            byte[] keyRetrieved;
            string decrypted;
            string encryptedPass;
            byte[] encryptedPassByte;
            string ivString;
            string keyString;

            encryptedPass = DBConn.getPassword(url, user);
            ivString = DBConn.getIniValue(url, user);
            keyString = DBConn.getKey(url, user);

            ivRetrieved = Convert.FromBase64String(ivString);
            keyRetrieved = Convert.FromBase64String(keyString);
            encryptedPassByte = Convert.FromBase64String(encryptedPass);

            
            decrypted = DecryptStringFromBytes_Aes(encryptedPassByte, keyRetrieved, ivRetrieved);

            return decrypted;
            
        }

        public string getUsername(string user, string url)
        {
            string username = DBConn.getUsername(url, user);
            return username;
        }

        public int checkURL(string user, string url)
        {
            int check = DBConn.checkURL(url, user);
            return check;
        }


        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
                { 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
 
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
 
            string plaintext = null;
 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}
