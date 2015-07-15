using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class LoginControls
    {
        DataAccessLayer.DBController DBConn = new DataAccessLayer.DBController();
        
        private int generateSalt()
        {
            int salt;
            int diceD20;
            int card;
            int diceD12;

            Random rnd = new Random();
            diceD20 = rnd.Next(1, 21);
            card = rnd.Next(52);
            diceD12 = rnd.Next(1, 13);

            salt = ((diceD12 * diceD20) + (card * diceD12)) * diceD20;

            return salt;
        }

        public int generateNounce()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            int diceD6;
            int diceD8;
            int diceD20;
            int random;
            int nounce;

            Random rnd = new Random();
            diceD6 = rnd.Next(1, 7);
            diceD8 = rnd.Next(1, 9);
            diceD20 = rnd.Next(1, 21);

            random = (diceD6 * diceD20) + (diceD8 * 3);

            nounce = (secondsSinceEpoch / random) * 2;

            return nounce;
        }

        public string Hash(string ToBeHashed)
        {
            SHA1 sha = SHA1.Create();
            byte[] hashData = sha.ComputeHash(Encoding.Default.GetBytes(ToBeHashed));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }

        public int RegisterUser(string username, string password)
        {
            int salt;
            string saltedPassword;
            string passwordHash;
            int result;

            salt = generateSalt();
            saltedPassword = password + salt.ToString();
            passwordHash = Hash(saltedPassword);

            result = DBConn.registerUser(username, passwordHash, salt);

            return result;
        }

        public int LoginUser(string username, string password)
        {
            int salt;
            int nounce;
            string saltedPassword;
            string passwordHash;
            int result;

            salt = DBConn.getSalt(username);
            saltedPassword = password + salt.ToString();
            passwordHash = Hash(saltedPassword);
            nounce = generateNounce();

            result = DBConn.authenticateUser(username, passwordHash, nounce);

            return result;
        }
    }
}
