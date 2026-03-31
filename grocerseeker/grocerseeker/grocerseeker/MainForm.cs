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

            label1.Text = $"Welcome, {UserSession.Username}";


            Login.Text = "Login As, " + UserSession.UserRole;
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
           LoadControl(new Transaction());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // Match role case-insensitively and fix typo "Custumer" -> "Customer"
            if (string.Equals(UserSession.UserRole, "Customer", StringComparison.OrdinalIgnoreCase))
            {
                LoadControl(new product_area());
            }
            else if (string.Equals(UserSession.UserRole, "Vendor", StringComparison.OrdinalIgnoreCase))
            {
                LoadControl(new Product_Area_Vendor());
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            LoadControl(new UserControl1());


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void LoadControl(UserControl uc)
        {
            guna2Panel1.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            guna2Panel1.Controls.Add(uc);
        }

        private void Login_Click(object sender, EventArgs e)
        {

        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 MainForm = new Form1();
            MainForm.ShowDialog();
        }
    }
}
