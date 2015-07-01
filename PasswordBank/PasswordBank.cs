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
        public PasswordBank()
        {
            InitializeComponent();
        }

        private void PasswordBank_Load(object sender, EventArgs e)
        {

        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login openForm = new Login();
            openForm.ShowDialog();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout openForm = new Logout();
            openForm.ShowDialog();
        }

        private void createAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateAccount openForm = new CreateAccount();
            openForm.ShowDialog();
        }
    }
}
