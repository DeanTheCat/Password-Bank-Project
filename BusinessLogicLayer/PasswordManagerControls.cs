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
    }
}
