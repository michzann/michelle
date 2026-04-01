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
                        query = "SELECT p.id, p.vendor_id, u.vendor_name , p.product_name AS product_name, c.name AS category, c.name AS category_name, p.unit_type, p.price_per_unit, p.unit_stock " +
                                "FROM products p " +
                                "LEFT JOIN categories c ON p.category = c.id " +
                                "LEFT JOIN users u ON p.vendor_id = u.id " +
                                "WHERE p.delete_at IS NULL AND p.unit_stock >= 0";

                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;
                        // Sembunyikan kolom yang tidak ingin dilihat customer/vendor
                        guna2DataGridView1.Columns["id"].Visible = false;
                        guna2DataGridView1.Columns["vendor_id"].Visible = false;


                    }
                    else if (string.Equals(UserSession.UserRole, "Vendor", StringComparison.OrdinalIgnoreCase))
                    {
                        query = "SELECT p.id, p.vendor_id, u.vendor_name, p.product_name AS product_name, c.name AS category, c.name AS category_name, p.unit_type, p.price_per_unit, p.unit_stock, p.is_active " +
                                "FROM products p " +
                                "LEFT JOIN categories c ON p.category = c.id " +
                                "WHERE p.delete_at IS NULL AND p.unit_stock >= 0";

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
            // 1. Validasi klik row
            if (e.RowIndex < 0 || dataGridView1.Rows.Count == 0)
                return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            try
            {
                // 2. Ambil ID utama
                selectedID = GetInt(row, "id");
                selectedVendorId = GetInt(row, "vendor_id");

                // 3. Ambil data utama
                string productName = GetString(row, "product_name");
                double priceVal = GetDouble(row, "price_per_unit");
                double stockVal = GetDouble(row, "unit_stock");
                string unitType = GetString(row, "unit_type");

                // Simpan ke global
                price = priceVal;
                stock = stockVal;

                // 4. Tampilkan ke UI (SAFE)
                if (guna2TextBox1 != null)
                    guna2TextBox1.Text = productName;

                if (guna2NumericUpDown1 != null)
                    guna2NumericUpDown1.Value = (decimal)priceVal;

                if (guna2NumericUpDown2 != null)
                    guna2NumericUpDown2.Value = (decimal)stockVal;

                // 5. Set category
                if (comboBox1 != null && row.Cells["category_id"] != null)
                {
                    comboBox1.SelectedValue = GetInt(row, "category_id");
                }

                // 6. Set status (vendor mode)
                if (comboBox2 != null && row.Cells["is_active"] != null)
                {
                    int status = GetInt(row, "is_active");
                    comboBox2.Text = (status == 1) ? "active" : "inactive";
                }

                // 7. Set unit type
                if (guna2RadioButton1 != null && guna2RadioButton2 != null)
                {
                    guna2RadioButton1.Checked = unitType.Contains("count");
                    guna2RadioButton2.Checked = unitType.Contains("measure");
                }

                // 8. Reset quantity
                if (guna2NumericUpDown3 != null)
                {
                    guna2NumericUpDown3.Value = 1;
                }

                // 9. Hitung total
                HitungTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat memilih data: " + ex.Message);
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


        private int GetInt(DataGridViewRow row, string col)
        {
            if (row.Cells[col] == null || row.Cells[col].Value == DBNull.Value)
                return 0;

            return Convert.ToInt32(row.Cells[col].Value);
        }

        private double GetDouble(DataGridViewRow row, string col)
        {
            if (row.Cells[col] == null || row.Cells[col].Value == DBNull.Value)
                return 0;

            return Convert.ToDouble(row.Cells[col].Value);
        }

        private string GetString(DataGridViewRow row, string col)
        {
            if (row.Cells[col] == null || row.Cells[col].Value == DBNull.Value)
                return "";

            return row.Cells[col].Value.ToString();
        }

        private static double ToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
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
            if (selectedID == 0)
            {
                MessageBox.Show("Pilih produk terlebih dahulu!");
                return;
            }
            

            double qty = (double)guna2NumericUpDown3.Value;

            // VALIDASI
            if (qty <= 0)
            {
                MessageBox.Show("Quantity harus lebih dari 0!");
                return;
            }

            if (qty > stock)
            {
                MessageBox.Show("Stock tidak mencukupi!");
                return;
            }

            using (MySqlConnection conn = dbhelper.GetConnection())
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 🔹 1. CEK LIMIT PENDING (MAX 10)
                    string checkQuery = "SELECT COUNT(*) FROM transaction WHERE custumer_id = @cust AND status = 'pending'";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn, transaction);
                    checkCmd.Parameters.AddWithValue("@cust", UserSession.UserID);

                    int pendingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (pendingCount >= 10)
                    {

                        MessageBox.Show("Maksimal 10 pending transaksi!");
                        transaction.Rollback();
                        return;
                    }

                    // 🔹 2. HITUNG TOTAL
                    double total = price * qty;

                    // 🔹 3. HITUNG DELIVERY COST (SUDAH ADA DI METHOD KAMU)
                    double delivery = GetDeliveryCost();

                    // 🔹 4. INSERT TRANSACTION
                    string insertQuery = @"INSERT INTO transaction
                        (vendor_id, custumer_id, product_id, quantity, total_price, delivery_cost, status, create_at,update_at)
                        VALUES (@vendor, @cust, @product, @qty, @total, @delivery, 'pending', NOW(),NOW()) ";

                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn, transaction);
                    insertCmd.Parameters.AddWithValue("@vendor", selectedVendorId);
                    insertCmd.Parameters.AddWithValue("@cust", UserSession.UserID);
                    insertCmd.Parameters.AddWithValue("@product", selectedID);
                    insertCmd.Parameters.AddWithValue("@qty", qty);
                    insertCmd.Parameters.AddWithValue("@total", total);
                    insertCmd.Parameters.AddWithValue("@delivery", delivery);

                    insertCmd.ExecuteNonQuery();

                    // 🔹 5. UPDATE STOCK & AUTO SOFT-DELETE (SATU LANGKAH OPTIMAL)
                    // Kita taruh delete_at di awal agar dia menghitung berdasarkan stok saat ini
                    string updateStock = @"
                                        UPDATE products 
                                        SET 
                                        delete_at = IF(unit_stock - @qty <= 0, NOW(), delete_at),
                                          unit_stock = unit_stock - @qty
                                        WHERE id = @id AND unit_stock >= @qty";

                    MySqlCommand stockCmd = new MySqlCommand(updateStock, conn, transaction);
                    stockCmd.Parameters.AddWithValue("@qty", qty);
                    stockCmd.Parameters.AddWithValue("@id", selectedID);

                    // Mengeksekusi CUKUP 1 KALI SAJA
                    int rowsAffected = stockCmd.ExecuteNonQuery();

                    // 🔹 SAFETY GUARD (Penting agar stok tidak minus!)
                    // Jika baris yang terpengaruh = 0, berarti stok di database sudah tidak cukup
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Transaksi Batal: Stok di database tidak mencukupi atau sudah habis dibeli orang lain!");
                        transaction.Rollback();
                        return;
                    }

                    // Catatan: LANGKAH 6 (autoDeleteCmd) SEKARANG DIHAPUS TOTAL KARENA SUDAH DIGABUNG DI ATAS

                    // 🔹 COMMIT
                    transaction.Commit();

                    MessageBox.Show("Pembelian berhasil (Pending)!");

                    // 🔹 RESET ID PENTING
                    // Reset ID agar pembeli tidak tidak sengaja membeli barang yang sama dua kali jika asal klik tombol Buy
                    selectedID = 0;
                    LoadData();



                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Error Buy: " + ex.Message);
                }
            }
        }

        private double GetDeliveryCost()
        {
            try
            {
                double per100km = 15000;

                double custLat = Convert.ToDouble(UserSession.latitude);
                double custLng = Convert.ToDouble(UserSession.longitude);

                using (MySqlConnection conn = dbhelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT vendor_latitude, vendor_longitude FROM users WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedVendorId);

                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            double vLat = Convert.ToDouble(r["vendor_latitude"]);
                            double vLng = Convert.ToDouble(r["vendor_longitude"]);

                            double dist = HaversineDistance(custLat, custLng, vLat, vLng);

                            return Math.Ceiling(dist / 100.0) * per100km;
                        }
                    }
                }
            }
            catch { }

            return 15000; // fallback
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}