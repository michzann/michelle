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
    public partial class Transaction : UserControl
    {
        DatabaseHelper dbhelper = new DatabaseHelper();
        private int _selectedTransactionId = 0;
        private int _selectedProductId = 0;
        private int _selectedVendorId = 0;
        private int _selectedCustomerId = 0;
        private decimal _selectedQuantity = 0;
        private decimal _selectedPricePerUnit = 0;
        public Transaction()
        {
            InitializeComponent();
            // wire events
            dataGridView1.CellClick += DataGridView1_CellClick;
            dataGridView2.CellClick += DataGridView2_CellClick;
            guna2Button4.Click += Guna2Button4_Click; // Approval
            guna2Button1.Click += Guna2Button1_Click; // Decline
            guna2Button2.Click += Guna2Button2_Click; // Cancel

            LoadTransactions();
        }

        private void guna2GroupBox3_Click(object sender, EventArgs e)
        {

        }

        private void LoadTransactions()
        {
            try
            {
                using (MySqlConnection conn = dbhelper.GetConnection())
                {
                    conn.Open();
                    // History: success or failed (and completed)
                    // note: actual table in database is named `transaction` and column `custumer_id` (typo)
                    string qHistory = @"SELECT t.id, t.vendor_id, t.custumer_id AS customer_id, t.product_id, p.product_name, u.vendor_name AS vendor_name, cu.cust_name AS customer_name, t.quantity, p.price_per_unit, t.total_price, t.delivery_cost, t.status
FROM `transaction` t
LEFT JOIN products p ON t.product_id = p.id
LEFT JOIN users u ON t.vendor_id = u.id
LEFT JOIN users cu ON t.custumer_id = cu.id
WHERE t.status IN ('success','failed','completed') ORDER BY t.id DESC";

                    MySqlDataAdapter a1 = new MySqlDataAdapter(qHistory, conn);
                    DataTable dtHistory = new DataTable();
                    a1.Fill(dtHistory);
                    dataGridView1.DataSource = dtHistory;

                    // Pending
                    string qPending = @"SELECT t.id, t.vendor_id, t.custumer_id AS customer_id, t.product_id, p.product_name, u.vendor_name AS vendor_name, cu.cust_name AS customer_name, t.quantity, p.price_per_unit, t.total_price, t.delivery_cost, t.status
FROM `transaction` t
LEFT JOIN products p ON t.product_id = p.id
LEFT JOIN users u ON t.vendor_id = u.id
LEFT JOIN users cu ON t.custumer_id = cu.id
WHERE t.status = 'pending' ORDER BY t.id DESC";
                    MySqlDataAdapter a2 = new MySqlDataAdapter(qPending, conn);
                    DataTable dtPending = new DataTable();
                    a2.Fill(dtPending);
                    dataGridView2.DataSource = dtPending;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading transactions: " + ex.Message);
            }
        }

        private void PopulateDetailsFromRow(DataGridViewRow row)
        {
            try
            {
                _selectedTransactionId = row.Cells["id"].Value != null && row.Cells["id"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["id"].Value) : 0;
                _selectedVendorId = row.Cells["vendor_id"].Value != null && row.Cells["vendor_id"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["vendor_id"].Value) : 0;
                _selectedCustomerId = row.Cells["customer_id"].Value != null && row.Cells["customer_id"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["customer_id"].Value) : 0;
                _selectedProductId = row.Cells["product_id"].Value != null && row.Cells["product_id"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["product_id"].Value) : 0;

                string pname = row.Cells["product_name"]?.Value?.ToString() ?? "-";
                label7.Text = pname;

                // show vendor or customer name depending on current user role
                string vendorName = row.Cells["vendor_name"]?.Value?.ToString() ?? "-";
                string customerName = row.Cells["customer_name"]?.Value?.ToString() ?? "-";
                if (string.Equals(UserSession.UserRole, "Customer", StringComparison.OrdinalIgnoreCase))
                {
                    // show vendor
                    if (label5 != null) label5.Text = "Vendor: " + vendorName;
                }
                else if (string.Equals(UserSession.UserRole, "Vendor", StringComparison.OrdinalIgnoreCase))
                {
                    if (label5 != null) label5.Text = "Customer: " + customerName;
                }
                else
                {
                    if (label5 != null) label5.Text = "Details";
                }

                decimal qty = 0;
                if (row.Cells["quantity"]?.Value != null && row.Cells["quantity"].Value != DBNull.Value) qty = Convert.ToDecimal(row.Cells["quantity"].Value);
                _selectedQuantity = qty;
                label9.Text = qty.ToString();

                decimal price = 0;
                if (row.Cells["price_per_unit"]?.Value != null && row.Cells["price_per_unit"].Value != DBNull.Value) price = Convert.ToDecimal(row.Cells["price_per_unit"].Value);
                _selectedPricePerUnit = price;
                label12.Text = price.ToString("N0");

                decimal total = 0;
                if (row.Cells["total_price"]?.Value != null && row.Cells["total_price"].Value != DBNull.Value) total = Convert.ToDecimal(row.Cells["total_price"].Value);
                label14.Text = total.ToString("N0");

                decimal delivery = 0;
                if (row.Cells["delivery_cost"]?.Value != null && row.Cells["delivery_cost"].Value != DBNull.Value) delivery = Convert.ToDecimal(row.Cells["delivery_cost"].Value);
                label16.Text = delivery.ToString("N0");

                // enable/disable action buttons based on role and status
                string status = row.Cells["status"]?.Value?.ToString() ?? string.Empty;
                bool isCustomer = string.Equals(UserSession.UserRole, "Customer", StringComparison.OrdinalIgnoreCase);
                bool isVendor = string.Equals(UserSession.UserRole, "Vendor", StringComparison.OrdinalIgnoreCase);

                // default disable all
                try { guna2Button4.Enabled = false; } catch { }
                try { guna2Button1.Enabled = false; } catch { }
                try { guna2Button2.Enabled = false; } catch { }

                // only enable appropriate actions for pending rows
                if (string.Equals(status, "pending", StringComparison.OrdinalIgnoreCase))
                {
                    if (isCustomer)
                    {
                        try { guna2Button2.Enabled = true; } catch { }
                    }
                    else if (isVendor)
                    {
                        try { guna2Button4.Enabled = true; } catch { }
                        try { guna2Button1.Enabled = true; } catch { }
                    }
                }
            }
            catch { }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridView1.Rows[e.RowIndex];
            PopulateDetailsFromRow(row);
        }

        private void DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridView2.Rows[e.RowIndex];
            PopulateDetailsFromRow(row);
        }

        private void Guna2Button4_Click(object sender, EventArgs e)
        {
            // Approval
            if (_selectedTransactionId == 0)
            {
                MessageBox.Show("Pilih transaksi yang akan di-approve terlebih dahulu.");
                return;
            }
            try
            {
                using (MySqlConnection conn = dbhelper.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE `transaction` SET status='success' WHERE id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", _selectedTransactionId);
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Transaction approved.");
                        LoadTransactions();
                        _selectedTransactionId = 0;
                        _selectedProductId = 0;
                        _selectedQuantity = 0;
                        try { guna2Button4.Enabled = false; } catch { }
                        try { guna2Button1.Enabled = false; } catch { }
                        try { guna2Button2.Enabled = false; } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error approving: " + ex.Message);
            }
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            // Decline
            if (_selectedTransactionId == 0)
            {
                MessageBox.Show("Pilih transaksi yang akan di-decline terlebih dahulu.");
                return;
            }
            try
            {
                using (MySqlConnection conn = dbhelper.GetConnection())
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand("UPDATE `transaction` SET status='failed' WHERE id=@id", conn, tr);
                            cmd.Parameters.AddWithValue("@id", _selectedTransactionId);
                            int r = cmd.ExecuteNonQuery();

                            // restock product
                            if (_selectedProductId > 0 && _selectedQuantity > 0)
                            {
                                MySqlCommand rest = new MySqlCommand("UPDATE products SET unit_stock = unit_stock + @q WHERE id=@pid", conn, tr);
                                rest.Parameters.AddWithValue("@q", _selectedQuantity);
                                rest.Parameters.AddWithValue("@pid", _selectedProductId);
                                rest.ExecuteNonQuery();
                            }

                            tr.Commit();
                            MessageBox.Show("Transaction declined and product restocked.");
                            LoadTransactions();
                            _selectedTransactionId = 0;
                            _selectedProductId = 0;
                            _selectedQuantity = 0;
                            try { guna2Button4.Enabled = false; } catch { }
                            try { guna2Button1.Enabled = false; } catch { }
                            try { guna2Button2.Enabled = false; } catch { }
                        }
                        catch
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error declining: " + ex.Message);
            }
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            // Cancel Transaction
            if (_selectedTransactionId == 0)
            {
                MessageBox.Show("Pilih transaksi yang akan dibatalkan terlebih dahulu.");
                return;
            }
            try
            {
                using (MySqlConnection conn = dbhelper.GetConnection())
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand("UPDATE `transaction` SET status='failed' WHERE id=@id", conn, tr);
                            cmd.Parameters.AddWithValue("@id", _selectedTransactionId);
                            int r = cmd.ExecuteNonQuery();

                            // restock product
                            if (_selectedProductId > 0 && _selectedQuantity > 0)
                            {
                                MySqlCommand rest = new MySqlCommand("UPDATE products SET unit_stock = unit_stock + @q WHERE id=@pid", conn, tr);
                                rest.Parameters.AddWithValue("@q", _selectedQuantity);
                                rest.Parameters.AddWithValue("@pid", _selectedProductId);
                                rest.ExecuteNonQuery();
                            }

                            tr.Commit();
                            MessageBox.Show("Transaction cancelled and product restocked.");
                            LoadTransactions();
                            _selectedTransactionId = 0;
                            _selectedProductId = 0;
                            _selectedQuantity = 0;
                            try { guna2Button4.Enabled = false; } catch { }
                            try { guna2Button1.Enabled = false; } catch { }
                            try { guna2Button2.Enabled = false; } catch { }
                        }
                        catch
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling: " + ex.Message);
            }
        }
    }
}
