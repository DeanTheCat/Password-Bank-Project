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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void lbl_user_Click(object sender, EventArgs e)
        {

        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            int result = 0;
            
            string username;
            string password;

            username = tb_user.Text.ToString();
            password = tb_pass.Text.ToString();

            BusinessLogicLayer.LoginControls reg = new BusinessLogicLayer.LoginControls();
            result = reg.LoginUser(username, password);

            if (result == 1)
            {
                lbl_error.Text = "Login Successful!";
                PasswordBank.currentUser = username;
                PasswordBank PB = new PasswordBank();
                PB.LabelText = username;
            }
            else
            {
                lbl_error.Text = "Login Unsuccessful!";
            }
        }
        }
    }
