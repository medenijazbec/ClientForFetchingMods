using System;
using System.Windows.Forms;

namespace MinecraftServerHelper
{
    public partial class CredentialsForm : Form
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public CredentialsForm(string defaultUser, string defaultPass)
        {
            InitializeComponent();
            txtUsername.Text = defaultUser;
            txtPassword.Text = defaultPass;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Username = txtUsername.Text.Trim();
            Password = txtPassword.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
