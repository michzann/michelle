using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System.Data;
namespace grocerseeker

{
    public partial class Form1 : Form
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = this.username.Text;
            string pass = this.pass.Text;

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                try
                {

                    string query = "SELECT * FROM users WHERE phone_number=@phone AND password=@pass";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@phone", username);
                    cmd.Parameters.AddWithValue("@pass", pass);

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                        int userId = Convert.ToInt32(reader["id"]);
                        UserSession.UserID = userId;
                        UserSession.PhoneNumber = reader["phone_number"].ToString();

                        double latitude = Convert.ToDouble(reader["cust_latitude"]);
                        UserSession.latitude = latitude;
                        double longitude = Convert.ToDouble(reader["cust_longtitude"]);
                        UserSession.longitude = longitude;
                        int isCust = Convert.ToInt32(reader["cust_active"]);
                        string selectedRole = role.SelectedItem.ToString();
                        
                       


                        if (selectedRole == "Customer" && isCust == 1)
                        {
                            UserSession.UserRole = "Customer";
                            UserSession.Username = reader["cust_name"].ToString();
                        }
                        else if (selectedRole == "Vendor" && isCust == 0)
                        {
                            UserSession.UserRole = "Vendor";
                            UserSession.Username = reader["vendor_name"].ToString();
                            UserSession.UserID = userId;
                            UserSession.VendorID = userId;
                        }
                        else
                        {
                            MessageBox.Show("Role tidak sesuai dengan akun!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        MessageBox.Show(UserSession.Username);
                        MessageBox.Show("Login Berhasil!", "Sukses");

                        this.Hide();
                        MainForm Form1 = new MainForm();
                        Form1.ShowDialog();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
