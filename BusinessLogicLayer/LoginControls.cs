using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Aes;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    class LoginControls
    {
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
    }
}
