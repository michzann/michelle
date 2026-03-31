using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace grocerseeker
{
    public partial class Product_Area_Vendor : UserControl
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        private int selectedProductId = -1;
        public Product_Area_Vendor()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        private void guna2RadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void LoadData()
        {
            string query = @"SELECT 
                        id,
                        product_name, 
                        category, 
                        unit_type, 
                        price_per_unit, 
                        unit_stock, 
                        is_active 
                     FROM products";

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Contains("id"))
                {
                    dataGridView1.Columns["id"].Visible = false;
                }

            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Product_Area_Vendor_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.CellClick += dataGridView1_CellClick;
            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                string query = "SELECT * FROM categories";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                category.DataSource = dt;
                category.DisplayMember = "name";
                category.ValueMember = "id";
            }
        }

        private void Add_items_Click(object sender, EventArgs e)
        {
            string nama = this.Nama.Text.Trim();

            object sel = this.category.SelectedValue;
            if (sel == null)
            {
                MessageBox.Show("Pilih kategori terlebih dahulu!");
                return;
            }
            int categoryId = Convert.ToInt32(sel);

            string unitType = string.Empty;
            if (this.Count_table.Checked)
            {
                unitType = this.Count_table.Text.Trim();
            }
            else if (this.measurable.Checked)
            {
                unitType = this.measurable.Text.Trim();
            }

            decimal pricePerUnit = this.Price.Value;
            int unitStock = Convert.ToInt32(this.stock.Value);
            int isActive = (this.status.Text.Trim().ToLower() == "active") ? 1 : 0;

            if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(unitType))
            {
                MessageBox.Show("Semua field harus diisi!");
                return;
            }

            string query = @"INSERT INTO products (product_name, category, unit_type, price_per_unit, unit_stock, is_active) 
                                VALUES (@nama, @category, @unit_type, @price_per_unit, @unit_stock, @is_active)";

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                conn.Open();

               
                int newId = -1;
                using (MySqlCommand idCmd = new MySqlCommand("SELECT COALESCE(MAX(id), 0) + 1 FROM products", conn))
                {
                    object res = idCmd.ExecuteScalar();
                    if (res != null && res != DBNull.Value)
                    {
                        newId = Convert.ToInt32(res);
                    }
                    else
                    {
                        newId = 1;
                    }
                }

               
                object vendorIdParam = DBNull.Value;
                DateTime createAt = DateTime.Now;

                string insertQuery = @"INSERT INTO products (id, product_name, category, unit_type, price_per_unit, unit_stock, is_active, vendor_id, create_at) 
                                VALUES (@id, @nama, @category, @unit_type, @price_per_unit, @unit_stock, @is_active, @vendor_id, @create_at)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id", newId);
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@category", categoryId);
                    cmd.Parameters.AddWithValue("@unit_type", unitType);
                    cmd.Parameters.AddWithValue("@price_per_unit", pricePerUnit);
                    cmd.Parameters.AddWithValue("@unit_stock", unitStock);
                    cmd.Parameters.AddWithValue("@is_active", isActive);
                    cmd.Parameters.AddWithValue("@vendor_id", vendorIdParam);
                    cmd.Parameters.AddWithValue("@create_at", createAt);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadData(); 

        }

        private void Edit_items_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih baris terlebih dahulu untuk melihat detail.");
                return;
            }
            int rowIndex = dataGridView1.CurrentRow.Index;
            dataGridView1_CellClick(this, new DataGridViewCellEventArgs(0, rowIndex));
        }

        private void SaveEdit_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0)
            {
                MessageBox.Show("Tidak ada item yang dipilih untuk disimpan.");
                return;
            }

            string nama = this.Nama.Text.Trim();
            object sel = this.category.SelectedValue;
            if (sel == null)
            {
                MessageBox.Show("Pilih kategori terlebih dahulu!");
                return;
            }
            int categoryId = Convert.ToInt32(sel);

            string unitType = string.Empty;
            if (this.Count_table.Checked)
                unitType = this.Count_table.Text.Trim();
            else if (this.measurable.Checked)
                unitType = this.measurable.Text.Trim();

            decimal pricePerUnit = this.Price.Value;
            int unitStock = Convert.ToInt32(this.stock.Value);
            int isActive = (this.status.Text.Trim().ToLower() == "active") ? 1 : 0;

            if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(unitType))
            {
                MessageBox.Show("Semua field harus diisi!");
                return;
            }

            string updateQuery = @"UPDATE products SET product_name=@nama, category=@category, unit_type=@unit_type,
                                    price_per_unit=@price_per_unit, unit_stock=@unit_stock, is_active=@is_active
                                    WHERE id=@id";

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@category", categoryId);
                    cmd.Parameters.AddWithValue("@unit_type", unitType);
                    cmd.Parameters.AddWithValue("@price_per_unit", pricePerUnit);
                    cmd.Parameters.AddWithValue("@unit_stock", unitStock);
                    cmd.Parameters.AddWithValue("@is_active", isActive);
                    cmd.Parameters.AddWithValue("@id", selectedProductId);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadData();
            MessageBox.Show("Perubahan berhasil disimpan.");
            
            selectedProductId = -1;
        }

        private void CancelEdit_Click(object sender, EventArgs e)
        {
            
            selectedProductId = -1;
            Nama.Text = string.Empty;
            category.SelectedIndex = -1;
            Count_table.Checked = false;
            measurable.Checked = false;
            Price.Value = 0;
            stock.Value = 0;
            status.SelectedIndex = -1;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (row.Cells["id"] != null && row.Cells["id"].Value != DBNull.Value)
            {
                int.TryParse(row.Cells["id"].Value.ToString(), out selectedProductId);
            }

            Nama.Text = row.Cells["product_name"].Value?.ToString() ?? string.Empty;

            if (row.Cells["category"].Value != null)
            {
                try
                {
                    category.SelectedValue = Convert.ToInt32(row.Cells["category"].Value);
                }
                catch { }
            }

            
            var ut = row.Cells["unit_type"].Value?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(ut))
            {
                if (ut.IndexOf("Count", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Count_table.Checked = true;
                    measurable.Checked = false;
                }
                else if (ut.IndexOf("Measurable", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    measurable.Checked = true;
                    Count_table.Checked = false;
                }
                else
                {
                    Count_table.Checked = false;
                    measurable.Checked = false;
                }
            }

            decimal p = 0;
            int s = 0;
            if (row.Cells["price_per_unit"].Value != null && decimal.TryParse(row.Cells["price_per_unit"].Value.ToString(), out p))
            {
                Price.Value = p;
            }
            if (row.Cells["unit_stock"].Value != null && int.TryParse(row.Cells["unit_stock"].Value.ToString(), out s))
            {
                stock.Value = s;
            }

            if (row.Cells["is_active"].Value != null)
            {
                var val = row.Cells["is_active"].Value.ToString();
                if (val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase))
                    status.SelectedItem = "active";
                else
                    status.SelectedItem = "inactive";
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0)
            {
                MessageBox.Show("Pilih item terlebih dahulu yang akan dihapus.", "Hapus Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Yakin ingin menghapus item ini?","Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM products WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedProductId);
                        int affected = cmd.ExecuteNonQuery();
                        if (affected > 0)
                        {
                            MessageBox.Show("Item berhasil dihapus.", "Hapus Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Item tidak ditemukan atau sudah dihapus.", "Hapus Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                LoadData();
                selectedProductId = -1;
                Nama.Text = string.Empty;
                category.SelectedIndex = -1;
                Count_table.Checked = false;
                measurable.Checked = false;
                Price.Value = 0;
                stock.Value = 0;
                status.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menghapus: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0)
            {
                MessageBox.Show("Tidak ada item yang dipilih untuk disimpan.");
                return;
            }

            string nama = this.Nama.Text.Trim();
            object sel = this.category.SelectedValue;
            if (sel == null)
            {
                MessageBox.Show("Pilih kategori terlebih dahulu!");
                return;
            }
            int categoryId = Convert.ToInt32(sel);

            string unitType = string.Empty;
            if (this.Count_table.Checked)
                unitType = this.Count_table.Text.Trim();
            else if (this.measurable.Checked)
                unitType = this.measurable.Text.Trim();

            decimal pricePerUnit = this.Price.Value;
            int unitStock = Convert.ToInt32(this.stock.Value);
            int isActive = (this.status.Text.Trim().ToLower() == "active") ? 1 : 0;

            if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(unitType))
            {
                MessageBox.Show("Semua field harus diisi!");
                return;
            }

            string updateQuery = @"UPDATE products SET product_name=@nama, category=@category, unit_type=@unit_type,
                                    price_per_unit=@price_per_unit, unit_stock=@unit_stock, is_active=@is_active
                                    WHERE id=@id";

            using (MySqlConnection conn = dbHelper.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@category", categoryId);
                    cmd.Parameters.AddWithValue("@unit_type", unitType);
                    cmd.Parameters.AddWithValue("@price_per_unit", pricePerUnit);
                    cmd.Parameters.AddWithValue("@unit_stock", unitStock);
                    cmd.Parameters.AddWithValue("@is_active", isActive);
                    cmd.Parameters.AddWithValue("@id", selectedProductId);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadData();
            MessageBox.Show("Perubahan berhasil disimpan.");
            
            selectedProductId = -1;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            selectedProductId = -1;
            Nama.Text = string.Empty;
            category.SelectedIndex = -1;
            Count_table.Checked = false;
            measurable.Checked = false;
            Price.Value = 0;
            stock.Value = 0;
            status.SelectedIndex = -1;
        }
    }
}
