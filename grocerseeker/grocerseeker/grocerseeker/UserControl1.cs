using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace grocerseeker
{
    public partial class UserControl1 : UserControl
    {
        DatabaseHelper dbHeloper = new DatabaseHelper();
        public UserControl1()
        {
            InitializeComponent();
        }
        // Use global UserSession class (defined in UserSession.cs)

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }
        private void guna2TextBox5_TextChanged(object sender, EventArgs e) { }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e) { }

        private void label15_Click(object sender, EventArgs e) { }

        private void label16_Click(object sender, EventArgs e) { }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = dbHeloper.GetConnection())
            {
                // TAMBAHKAN BARIS INI:i9
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = "SELECT * FROM users WHERE id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Jika user belum login, jangan load profile
                if (string.IsNullOrWhiteSpace(UserSession.UserID))
                {
                    // No user session available
                    return;
                }
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(UserSession.UserID));

                MySqlDataReader reader = cmd.ExecuteReader();
                // ... sisa kode kamu ...


                if (reader.Read())
                {
                    phone_number.Text = reader["phone_number"].ToString();
                    email.Text = reader["email"].ToString();

                    // Gunakan Convert untuk menangani berbagai tipe data (int/bool/bit)
                    bool isCustActive = Convert.ToBoolean(reader["cust_active"]);
                    bool isVendorActive = Convert.ToBoolean(reader["vendor_active"]);

                    if (isCustActive)
                    {
                        checkBox1.Checked = true;
                        c_name.Text = reader["cust_name"].ToString();
                        c_addres.Text = reader["cust_addres"].ToString();
                        c_latitude.Text = reader["cust_latitude"].ToString();
                        c_longtitude.Text = reader["cust_longtitude"].ToString();
                    }

                    // Gunakan IF terpisah, jangan ELSE IF, jika user bisa jadi keduanya
                    if (isVendorActive)
                    {
                        checkBox2.Checked = true;
                        v_name.Text = reader["vendor_name"].ToString();
                        v_addres.Text = reader["vendor_addres"].ToString();
                        v_latitude.Text = reader["vendor_latitude"].ToString();
                        v_longtitude.Text = reader["vendor_longtitude"].ToString();
                    }
                }
                reader.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string role;
            if (checkBox1.Checked) role = "Customer";
            if (checkBox2.Checked) role = "Vendor";

            string query = @"UPDATE users SET phone_number=@phone_number, email=@email, cust_active=@checkBox1, vendor_active=@checkBox2, cust_name=@c_name,
               cust_addres=@c_addres, cust_latitude=@c_latitude, cust_longtitude=@c_longtitude, vendor_name=@v_name, vendor_addres=@v_addres,
               vendor_latitude=@v_latitude, vendor_longtitude=@v_longtitude
               WHERE id=@user_id";
            // Ganti baris 117 menjadi:
            using (MySqlConnection conn = dbHeloper.GetConnection()) // Sesuaikan dengan nama method di DatabaseHelper kamu
            {
                try
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (string.IsNullOrWhiteSpace(UserSession.UserID))
                    {
                        MessageBox.Show("User id tidak diset. Tidak bisa menyimpan.");
                        return;
                    }
                    cmd.Parameters.AddWithValue("@user_id", Convert.ToInt32(UserSession.UserID));
                    cmd.Parameters.AddWithValue("@phone_number", phone_number.Text);
                    cmd.Parameters.AddWithValue("@email", email.Text);
                    // Ubah @cust_active menjadi @checkBox1 sesuai permintaan error
                    cmd.Parameters.AddWithValue("@checkBox1", checkBox1.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@checkBox2", checkBox2.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@c_name", c_name.Text);
                    cmd.Parameters.AddWithValue("@c_addres", c_addres.Text);    
                    cmd.Parameters.AddWithValue("@v_name", v_name.Text);
                    cmd.Parameters.AddWithValue("@v_addres", v_addres.Text);
                    double cLat = 0, cLong = 0, vLat = 0, vLong = 0;
                    double.TryParse(c_latitude.Text, out cLat);
                    double.TryParse(c_longtitude.Text, out cLong);
                    double.TryParse(v_latitude.Text, out vLat);
                    double.TryParse(v_longtitude.Text, out vLong);

                    cmd.Parameters.AddWithValue("@c_latitude", cLat);
                    cmd.Parameters.AddWithValue("@c_longtitude", cLong);

                    cmd.Parameters.AddWithValue("@v_latitude", vLat);
                    cmd.Parameters.AddWithValue("@v_longtitude", vLong);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan!");
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                }
            }
        }

        private void v_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
