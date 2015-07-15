using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PasswordBank
{
    public partial class CreateAccount : Form
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void btn_createAccount_Click(object sender, EventArgs e)
        {
            int result = 0;
            
            string username;
            string password;

            username = tb_user.Text.ToString();
            password = tb_pass.Text.ToString();

            BusinessLogicLayer.LoginControls reg = new BusinessLogicLayer.LoginControls();
            result = reg.RegisterUser(username, password);

            if (result == 1)
            {
                lbl_error.Text = "Registration Successful!";
            }
            else
            {
                lbl_error.Text = "Registration Unsuccessful!";
            }
        }
    }
}
