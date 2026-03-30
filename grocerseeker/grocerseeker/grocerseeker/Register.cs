using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace grocerseeker
{
    public partial class Register : Form
    {
        DatabaseHelper dbHeloper = new DatabaseHelper();
        int CbCustomervalue;
        int CbVendorvalue;


        public Register()
        {
            InitializeComponent();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string phone = this.phone.Text.Trim();
            string email = this.email.Text.Trim();
            string pass = password.Text.Trim();
            string confirmPass = confirm.Text.Trim();

            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Nomor telpon, email, password. WAJIB DI ISI!");
                return;
            }

            if (pass != confirmPass)
            {
                MessageBox.Show("Password dan konfirmasi password tidak cocok!");
                return;
            }

            using (MySqlConnection conn = dbHeloper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO users (phone_numbers, email, password, cust_active, vendor_active, cust_name, cust_address, cust_latitude, cust_longtitude, vendor_name, vendor_addres, vendor_latitude, vendor_longtitude, created_at, update_at ) VALUES (@phone, @email, @password, @c_acvtive, @v_active, @c_name, @c_addres, @c_latitude, @c_longtitude, @v_name , @v_addres, @v_latitude, @v_longtitude)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", pass);
                        if (checkBox1.Checked)
                        {
                            cmd.Parameters.AddWithValue("@c_active", 1);
                            cmd.Parameters.AddWithValue("@v_active", 0);
                            cmd.Parameters.AddWithValue("@c_name", c_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@c_addres", c_addres.Text.Trim());

                            decimal cLat = 0, Clong = 0;
                            decimal.TryParse(c_latitude.Text.Trim(), out cLat);
                            decimal.TryParse(c_longtitude.Text.Trim(), out Clong);

                            cmd.Parameters.AddWithValue("@v_name", "");
                            cmd.Parameters.AddWithValue("@v_addres", "");
                            cmd.Parameters.AddWithValue("@v_latitude", 0);  
                            cmd.Parameters.AddWithValue("@v_longtitude", 0);    
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@c_active", 0);
                            cmd.Parameters.AddWithValue("@v_active", 1);
                            cmd.Parameters.AddWithValue("@c_name" , v_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@c_addres", v_addres.Text.Trim());

                            decimal vLat = 0, vLong = 0;
                            decimal.TryParse(v_latitude.Text.Trim(), out vLat);
                            decimal.TryParse(v_longtitude.Text.Trim(), out vLong);
                            cmd.Parameters.AddWithValue("@v_latitude", vLat);
                            cmd.Parameters.AddWithValue("@v_longtitude", vLong);

                            cmd.Parameters.AddWithValue("@v_name", "");
                            cmd.Parameters.AddWithValue("@v_addres", "");
                            cmd.Parameters.AddWithValue("@c_latitude", 0);
                            cmd.Parameters.AddWithValue("@c_longtitude", 0);

                        }
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Registrasi berhasil!");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Registrasi gagal. Silakan coba lagi.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            } 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = false;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = true;
            }
        }
    }
}
