using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace grocerseeker
{
    public partial class MainForm : Form
    {
        private string username = UserSession.Username;
        private string userRole = UserSession.UserRole;
        public MainForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            label1.Text = "Welcome, " + UserSession.Username;


            Login.Text = "Login As, " + UserSession.UserRole;
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
