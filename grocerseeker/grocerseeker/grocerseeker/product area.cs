using grocerseeker;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace grocerseeker
{

    public partial class product_area : UserControl
    {
        double price = 0;
        double stock = 0;
        private int selectedVendorId = 0;

        private int selectedID = 0;
        DatabaseHelper dbhelper = new DatabaseHelper();
        private DataGridView guna2DataGridView1 => this.dataGridView1;

        private Guna2TextBox guna2TextBox1;
        private Guna2NumericUpDown guna2NumericUpDown1;
        private Guna2NumericUpDown guna2NumericUpDown2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private Guna2RadioButton guna2RadioButton1;
        private Guna2RadioButton guna2RadioButton2;
        private Guna2HtmlLabel guna2HtmlLabel12;
        private Guna2HtmlLabel guna2HtmlLabel11;
        private System.Windows.Forms.Button guna2Button3;
        private Guna2Button guna2Button7;
        private Panel guna2Panel1;
        private Guna2GroupBox guna2GroupBox2;
        // guard to avoid re-entrant ValueChanged handling and duplicate messageboxes
        private bool _suppressNumericUpDown3Events = false;
        // only show stock warning once until user sets value back in range
        private bool _stockWarningShown = false;

        public product_area()
        {
            InitializeComponent();

            LoadCategories();
            LoadData(); ;


            ApplyRoleVisibility();
        }

        private void ApplyRoleVisibility()
        {
            try
            {
                bool isCustomer = string.Equals(UserSession.UserRole, "Customer", StringComparison.OrdinalIgnoreCase);
                bool isVendor = string.Equals(UserSession.UserRole, "Vendor", StringComparison.OrdinalIgnoreCase);

                if (guna2GroupBox3 != null)
                {
                    // show transaction area for customers or default
                    guna2GroupBox3.Visible = isCustomer || (!isCustomer && !isVendor);
                }

                if (guna2GroupBox2 != null)
                {
                    // show details area for vendors or default
                    guna2GroupBox2.Visible = isVendor || (!isCustomer && !isVendor);
                }
            }
            catch
            {
                if (guna2GroupBox3 != null) guna2GroupBox3.Visible = true;
                if (guna2GroupBox2 != null) guna2GroupBox2.Visible = true;
            }
        }

        private void LoadCategories()
        {
            if (comboBox1 == null) return;

            using (MySqlConnection conn = dbhelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT id, name FROM categories WHERE is_active = 1 ORDER BY name";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBox1.DisplayMember = "name";
                    comboBox1.ValueMember = "id";
                    comboBox1.DataSource = dt;
                    comboBox1.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Load Categories: " + ex.Message);
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserSession.UserID))
            {
                MessageBox.Show("Sesi login habis, silahkan login ulang.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (guna2TextBox1 == null || guna2NumericUpDown1 == null || guna2NumericUpDown2 == null || comboBox1 == null || comboBox2 == null)
            {
                MessageBox.Show("Kontrol UI tidak tersedia. Pastikan form sudah di-desain dengan benar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(guna2TextBox1.Text))
            {
                MessageBox.Show("Nama produk tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int statusValue = (comboBox2.Text == "active") ? 1 : 0;

            using (MySqlConnection conn = dbhelper.GetConnection())
            {
                try
                {
                    string query = "INSERT INTO products (products_name, price_per_unit, unit_stock, vendor_id, is_active, category_id, unit_type) " +
                                   "VALUES (@name, @price, @stock, @vendorId, @active, @categoryId, @type)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", guna2TextBox1.Text);
                    cmd.Parameters.AddWithValue("@price", guna2NumericUpDown1.Value);
                    cmd.Parameters.AddWithValue("@stock", guna2NumericUpDown2.Value);
                    cmd.Parameters.AddWithValue("@vendorId", UserSession.UserID);


                    cmd.Parameters.AddWithValue("@active", statusValue);


                    if (comboBox1.SelectedValue != null)
                    {
                        cmd.Parameters.AddWithValue("@categoryId", comboBox1.SelectedValue);
                    }
                    else
                    {
                        MessageBox.Show("Silahkan pilih kategori yang valid!");
                        return;
                    }

                    string unitType = null;
                    if (guna2RadioButton1.Checked) unitType = "countable";
                    else if (guna2RadioButton2.Checked) unitType = "measureable";
                    cmd.Parameters.AddWithValue("@type", (object)unitType ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Produk berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Simpan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadData()
        {
            using (MySqlConnection conn = dbhelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query;
                    if (string.Equals(UserSession.UserRole, "Customer", StringComparison.OrdinalIgnoreCase))
                    {
                        query = "SELECT p.id, p.vendor_id, u.vendor_name, p.product_name AS product_name, c.id AS category, c.name AS category_name, p.unit_type, p.price_per_unit, p.unit_stock " +
                                "FROM products p " +
                                "LEFT JOIN categories c ON p.category = c.id " +
                                "LEFT JOIN users u ON p.vendor_id = u.id " +
                                "WHERE p.delete_at IS NULL";

                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;
                    }
                    else if (string.Equals(UserSession.UserRole, "Vendor", StringComparison.OrdinalIgnoreCase))
                    {
                        query = "SELECT p.id, p.vendor_id, p.product_name AS product_name, c.id AS category, c.name AS category_name, p.unit_type, p.price_per_unit, p.unit_stock, p.is_active " +
                                "FROM products p " +
                                "LEFT JOIN categories c ON p.category = c.id " +
                                "WHERE p.delete_at IS NULL";

                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;
                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Load: " + ex.Message);
                }
            }
        }

        private void ClearForm()
        {
            if (guna2TextBox1 != null) guna2TextBox1.Clear();
            if (comboBox1 != null) comboBox1.SelectedIndex = -1;
            if (guna2NumericUpDown1 != null) guna2NumericUpDown1.Value = 0;
            if (guna2NumericUpDown2 != null) guna2NumericUpDown2.Value = 0;
            if (guna2RadioButton1 != null) guna2RadioButton1.Checked = false;
            if (guna2RadioButton2 != null) guna2RadioButton2.Checked = false;
            if (comboBox2 != null) comboBox2.SelectedIndex = -1;
            selectedID = 0;
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];
            if (row.Cells["id"] != null && row.Cells["id"].Value != DBNull.Value)
            {
                selectedID = Convert.ToInt32(row.Cells["id"].Value);
            }
            if (row.Cells["vendor_id"] != null && row.Cells["vendor_id"].Value != DBNull.Value)
            {
                selectedVendorId = Convert.ToInt32(row.Cells["vendor_id"].Value);
            }

            if (guna2TextBox1 != null && guna2NumericUpDown1 != null && guna2NumericUpDown2 != null)
            {
                guna2TextBox1.Text = row.Cells["product_name"].Value?.ToString() ?? string.Empty;
                if (row.Cells["price_per_unit"].Value != null)
                    guna2NumericUpDown1.Value = Convert.ToDecimal(row.Cells["price_per_unit"].Value);
                if (row.Cells["unit_stock"].Value != null)
                    guna2NumericUpDown2.Value = Convert.ToDecimal(row.Cells["unit_stock"].Value);
            }
            try
            {
                var isActiveVal = row.Cells["is_active"].Value;
                if (comboBox2 != null)
                {
                    if (isActiveVal != null && isActiveVal != DBNull.Value)
                    {
                        int ia = Convert.ToInt32(isActiveVal);
                        comboBox2.Text = (ia == 1) ? "active" : "inactive";
                    }
                    else
                    {
                        comboBox2.SelectedIndex = -1;
                    }
                }
            }
            catch
            {
                if (comboBox2 != null) comboBox2.SelectedIndex = -1;
            }
            try
            {
                var catVal = row.Cells["category_id"].Value;
                if (comboBox1 != null)
                {
                    if (catVal != null && catVal != DBNull.Value)
                    {
                        comboBox1.SelectedValue = Convert.ToInt32(catVal);
                    }
                    else
                    {
                        comboBox1.SelectedIndex = -1;
                    }
                }
            }
            catch
            {
                if (comboBox1 != null)
                    comboBox1.SelectedIndex = -1;
            }

            try
            {
                var ut = row.Cells["unit_type"].Value;
                if (ut != null && ut != DBNull.Value)
                {
                    string uts = ut.ToString().ToLower();
                    if (uts.Contains("count"))
                    {
                        if (guna2RadioButton1 != null) guna2RadioButton1.Checked = true;
                        if (guna2RadioButton2 != null) guna2RadioButton2.Checked = false;
                    }
                    else if (uts.Contains("measure"))
                    {
                        if (guna2RadioButton2 != null) guna2RadioButton2.Checked = true;
                        if (guna2RadioButton1 != null) guna2RadioButton1.Checked = false;
                    }
                    else
                    {
                        if (guna2RadioButton1 != null) guna2RadioButton1.Checked = false;
                        if (guna2RadioButton2 != null) guna2RadioButton2.Checked = false;
                    }
                }
                else
                {
                    guna2RadioButton1.Checked = false;
                    guna2RadioButton2.Checked = false;
                }
            }
            catch
            {
                guna2RadioButton1.Checked = false;
                guna2RadioButton2.Checked = false;
            }


            if (row.Cells["price_per_unit"].Value != null)
                price = Convert.ToDouble(row.Cells["price_per_unit"].Value);
            if (row.Cells["unit_stock"].Value != null)
                stock = Convert.ToDouble(row.Cells["unit_stock"].Value);

            if (guna2NumericUpDown3 != null)
            {
                guna2NumericUpDown3.Value = 1;
                HitungTotal();
            }


        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (selectedID == 0)
            {
                MessageBox.Show("Pilih data di tabel dulu!", "Peringatan");
                return;
            }

            using (MySqlConnection conn = dbhelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = @"UPDATE products SET 
                            products_name = @name,
                            price_per_unit = @price,
                            unit_stock = @stock,
                            is_active = @active,
                            category_id = @categoryId,
                            unit_type = @type,
                            updated_at = NOW()
                            WHERE id = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);


                    cmd.Parameters.AddWithValue("@name", guna2TextBox1.Text);
                    cmd.Parameters.AddWithValue("@price", guna2NumericUpDown1.Value);
                    cmd.Parameters.AddWithValue("@stock", guna2NumericUpDown2.Value);
                    int statusValue = (comboBox2.Text == "active") ? 1 : 0;
                    cmd.Parameters.AddWithValue("@active", statusValue);
                    object categoryVal = comboBox1.SelectedValue ?? (object)DBNull.Value;
                    cmd.Parameters.AddWithValue("@categoryId", categoryVal);

                    string unitType = null;
                    if (guna2RadioButton1.Checked) unitType = "countable";
                    else if (guna2RadioButton2.Checked) unitType = "measureable";
                    cmd.Parameters.AddWithValue("@type", (object)unitType ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@id", selectedID);


                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil diupdate!", "Sukses");

                    LoadData();
                    ClearForm();
                    selectedID = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Update: " + ex.Message);
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (selectedID == 0)
            {
                MessageBox.Show("Silahkan pilih data di tabel terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult dialogResult = MessageBox.Show($"Apakah Anda yakin ingin menghapus data dengan ID: {selectedID}?",
                "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                using (MySqlConnection conn = dbhelper.GetConnection())
                {
                    try
                    {

                        string query = "UPDATE products SET delete_at = NOW() WHERE id=@id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selectedID);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            selectedID = 0;
                        }
                        else
                        {
                            MessageBox.Show("Tidak ada data yang terhapus. ID mungkin tidak ditemukan.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saat menghapus: " + ex.Message);
                    }
                }
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // handler for pending grid row click added by designer
        private void dataGridViewPending_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                DataGridView grid = sender as DataGridView;
                DataGridViewRow row = grid.Rows[e.RowIndex];
                if (row.Cells["price_per_unit"]?.Value != null)
                    price = Convert.ToDouble(row.Cells["price_per_unit"].Value);
                if (row.Cells["unit_stock"]?.Value != null)
                    stock = Convert.ToDouble(row.Cells["unit_stock"].Value);
                if (guna2NumericUpDown3 != null)
                {
                    guna2NumericUpDown3.Value = 1;
                    HitungTotal();
                }
            }
            catch { }
        }


        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void productcontrol_Load(object sender, EventArgs e)
        {

        }

        private void product_area_Load(object sender, EventArgs e)
        {
            productcontrol_Load(sender, e);
        }

        private void guna2HtmlLabel12_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel12_TextChanged(object sender, EventArgs e)
        {
            if (selectedID == 0)
            {
                MessageBox.Show("Silahkan pilih product di tabel terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = dbhelper.GetConnection())
            {
            }
        }

        private void guna2NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (_suppressNumericUpDown3Events) return;
            HitungTotal();
        }

        // Designer referenced handler - keep for compatibility with generated Designer file
        private void guna2NumericUpDown3_ValueChanged_1(object sender, EventArgs e)
        {
            if (_suppressNumericUpDown3Events) return;
            HitungTotal();
        }

        private void HitungTotal()
        {
            try
            {
                double qty = Convert.ToDouble(guna2NumericUpDown3.Value);

                if (qty > stock)
                {
                    // ensure price/stock are read from selected row if still zero
                    TryRefreshPriceAndStockFromGrid();

                    // show warning only once until user changes the value again
                    if (!_stockWarningShown)
                    {
                        _stockWarningShown = true;
                        MessageBox.Show("Stock tidak cukup!");
                    }

                    // set the quantity to available stock without re-entering handler
                    _suppressNumericUpDown3Events = true;
                    try
                    {
                        if (guna2NumericUpDown3 != null)
                        {
                            guna2NumericUpDown3.Value = Math.Max(0, (decimal)stock);
                        }
                        qty = stock;
                    }
                    finally
                    {
                        _suppressNumericUpDown3Events = false;
                    }
                }

                double total = price * qty;

                double deliveryCost = 0;
                // compute delivery cost based on distance between customer and vendor
                // default delivery per 100 KM = 15000
                double per100km = 15000;

                try
                {
                    // read customer lat/lng from UserSession if available
                    double custLat = 0;
                    double custLng = 0;
                    // UserSession may not expose Latitude/Longitude; try properties if present
                    try
                    {
                        var latProp = typeof(UserSession).GetProperty("Latitude");
                        var lngProp = typeof(UserSession).GetProperty("Longitude");
                        if (latProp != null && lngProp != null)
                        {
                            var latVal = latProp.GetValue(null);
                            var lngVal = lngProp.GetValue(null);
                            if (latVal != null) custLat = Convert.ToDouble(latVal);
                            if (lngVal != null) custLng = Convert.ToDouble(latVal);
                        }
                    }
                    catch { }

                    // read vendor lat/lng from DB using selectedVendorId
                    if (selectedVendorId > 0)
                    {
                        using (MySqlConnection conn = dbhelper.GetConnection())
                        {
                            conn.Open();
                            using (MySqlCommand cmd = new MySqlCommand("SELECT latitude, longitude FROM users WHERE id = @id", conn))
                            {
                                cmd.Parameters.AddWithValue("@id", selectedVendorId);
                                using (var r = cmd.ExecuteReader())
                                {
                                    if (r.Read())
                                    {
                                        var vLatObj = r[0];
                                        var vLngObj = r[1];
                                        if (vLatObj != DBNull.Value && vLngObj != DBNull.Value)
                                        {
                                            double vendLat = Convert.ToDouble(vLatObj);
                                            double vendLng = Convert.ToDouble(vLngObj);
                                            double distKm = HaversineDistance(custLat, custLng, vendLat, vendLng);
                                            deliveryCost = Math.Ceiling(distKm / 100.0) * per100km;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // fallback fixed cost
                    deliveryCost = 15000;
                }

                double grandTotal = total + deliveryCost;

                string totalText = "Rp " + total.ToString("N0");
                string deliveryText = "Rp " + deliveryCost.ToString("N0");
                if (guna2HtmlLabel12 != null) guna2HtmlLabel12.Text = totalText;
                else if (this.GetType().GetField("label13", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic) != null) label13.Text = totalText;

                if (guna2HtmlLabel11 != null) guna2HtmlLabel11.Text = deliveryText;
                else if (this.GetType().GetField("label15", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic) != null) label15.Text = deliveryText;
            }
            catch
            {
                if (guna2HtmlLabel12 != null) guna2HtmlLabel12.Text = "Rp 0";
                else label13.Text = "Rp 0";
                if (guna2HtmlLabel11 != null) guna2HtmlLabel11.Text = "Rp 0";
                else label15.Text = "Rp 0";
            }
        }

        // Try to refresh price and stock values from the currently selected grid row
        private void TryRefreshPriceAndStockFromGrid()
        {
            try
            {
                if ((price == 0 || stock == 0) && guna2DataGridView1 != null && guna2DataGridView1.CurrentRow != null)
                {
                    var row = guna2DataGridView1.CurrentRow;
                    try { if (price == 0 && row.Cells["price_per_unit"]?.Value != null && row.Cells["price_per_unit"].Value != DBNull.Value) price = Convert.ToDouble(row.Cells["price_per_unit"].Value); } catch { }
                    try { if (stock == 0 && row.Cells["unit_stock"]?.Value != null && row.Cells["unit_stock"].Value != DBNull.Value) stock = Convert.ToDouble(row.Cells["unit_stock"].Value); } catch { }
                }
            }
            catch { }
        }



        // Haversine distance in kilometers between two lat/lon points
        private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius km
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            string name = (guna2TextBox1 != null) ? guna2TextBox1.Text : (guna2DataGridView1.CurrentRow != null ? guna2DataGridView1.CurrentRow.Cells["products_name"].Value?.ToString() : "(unknown)");
            MessageBox.Show("Purchase successful for " + name);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}