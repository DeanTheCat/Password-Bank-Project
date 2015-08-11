using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordBank
{
    public partial class PasswordBank : Form
    {
        public static string currentUser;
        
        public PasswordBank()
        {
            InitializeComponent();
            btn_passwordManager.Visible = false;
        }

        private void PasswordBank_Load(object sender, EventArgs e)
        {
            if (currentUser == null)
            {
                lbl_currentUser.Text = "Guest";
            }
        }

        public string LabelText
        {
            get
            {
                return this.lbl_currentUser.Text;
            }
            set
            {
                this.lbl_currentUser.Text = value;
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            Login openForm = new Login();
            dr = openForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                lbl_currentUser.Text = currentUser;

                if (currentUser != null)
                {
                    btn_passwordManager.Visible = true;
                }
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            Logout openForm = new Logout();
            dr = openForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                btn_passwordManager.Visible = false;
                lbl_currentUser.Text = "Guest";
            }
        }

        private void createAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateAccount openForm = new CreateAccount();
            openForm.ShowDialog();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_passwordManager_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            ManagePassword openForm = new ManagePassword();
            dr = openForm.ShowDialog();
        }

    }
}
