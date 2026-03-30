using MySql.Data.MySqlClient;
using System.Data;
namespace grocerseeker

{
    public partial class Form1 : Form
    {
        DatabaseHelper dbHeloper = new DatabaseHelper();

        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            using (MySqlConnection conn = dbHeloper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT role FROM users WHERE username = @username AND password = @password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string role = result.ToString();
                            MessageBox.Show("Login successful! Role: " + role);
                            MainForm Form1 = new MainForm();
                            Form1.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Register Form1 = new Register();
            Form1.ShowDialog();
        }
    }
}
