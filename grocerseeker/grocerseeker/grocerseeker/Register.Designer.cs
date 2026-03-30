namespace grocerseeker
{
    partial class Register
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            label16 = new Label();
            button1 = new Button();
            groupBox2 = new GroupBox();
            v_longtitude = new TextBox();
            label11 = new Label();
            v_latitude = new TextBox();
            label12 = new Label();
            label13 = new Label();
            v_addres = new TextBox();
            label14 = new Label();
            v_name = new TextBox();
            label15 = new Label();
            groupBox1 = new GroupBox();
            c_longtitude = new TextBox();
            label10 = new Label();
            c_latitude = new TextBox();
            label9 = new Label();
            label8 = new Label();
            c_addres = new TextBox();
            cust_addres = new Label();
            c_name = new TextBox();
            label6 = new Label();
            checkBox2 = new CheckBox();
            label5 = new Label();
            checkBox1 = new CheckBox();
            confirm = new TextBox();
            label4 = new Label();
            password = new TextBox();
            label3 = new Label();
            email = new TextBox();
            label2 = new Label();
            phone = new TextBox();
            label1 = new Label();
            guna2Panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BorderColor = Color.FromArgb(192, 255, 255);
            guna2Panel1.BorderRadius = 50;
            guna2Panel1.Controls.Add(label16);
            guna2Panel1.Controls.Add(button1);
            guna2Panel1.Controls.Add(groupBox2);
            guna2Panel1.Controls.Add(groupBox1);
            guna2Panel1.Controls.Add(checkBox2);
            guna2Panel1.Controls.Add(label5);
            guna2Panel1.Controls.Add(checkBox1);
            guna2Panel1.Controls.Add(confirm);
            guna2Panel1.Controls.Add(label4);
            guna2Panel1.Controls.Add(password);
            guna2Panel1.Controls.Add(label3);
            guna2Panel1.Controls.Add(email);
            guna2Panel1.Controls.Add(label2);
            guna2Panel1.Controls.Add(phone);
            guna2Panel1.Controls.Add(label1);
            guna2Panel1.CustomBorderColor = Color.FromArgb(192, 255, 255);
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.FillColor = Color.FromArgb(192, 255, 255);
            guna2Panel1.Location = new Point(49, 44);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(706, 426);
            guna2Panel1.TabIndex = 0;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.BackColor = Color.Transparent;
            label16.ForeColor = Color.Red;
            label16.Location = new Point(179, 362);
            label16.Name = "label16";
            label16.Size = new Size(361, 15);
            label16.TabIndex = 14;
            label16.Text = "Password confirmation Doesn't Match With The Inputted Password";
            // 
            // button1
            // 
            button1.Location = new Point(222, 385);
            button1.Name = "button1";
            button1.Size = new Size(271, 29);
            button1.TabIndex = 13;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(v_longtitude);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(v_latitude);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(v_addres);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(v_name);
            groupBox2.Controls.Add(label15);
            groupBox2.Location = new Point(378, 191);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(299, 157);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Vendor Details";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // v_longtitude
            // 
            v_longtitude.Location = new Point(208, 125);
            v_longtitude.Name = "v_longtitude";
            v_longtitude.Size = new Size(71, 23);
            v_longtitude.TabIndex = 8;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(140, 128);
            label11.Name = "label11";
            label11.Size = new Size(65, 15);
            label11.TabIndex = 7;
            label11.Text = "Longtitude";
            // 
            // v_latitude
            // 
            v_latitude.Location = new Point(67, 125);
            v_latitude.Name = "v_latitude";
            v_latitude.Size = new Size(67, 23);
            v_latitude.TabIndex = 6;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(12, 129);
            label12.Name = "label12";
            label12.Size = new Size(50, 15);
            label12.TabIndex = 5;
            label12.Text = "Latitude";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(12, 107);
            label13.Name = "label13";
            label13.Size = new Size(59, 15);
            label13.TabIndex = 4;
            label13.Text = "Location: ";
            // 
            // v_addres
            // 
            v_addres.Location = new Point(67, 56);
            v_addres.Multiline = true;
            v_addres.Name = "v_addres";
            v_addres.Size = new Size(212, 48);
            v_addres.TabIndex = 3;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(12, 59);
            label14.Name = "label14";
            label14.Size = new Size(44, 15);
            label14.TabIndex = 2;
            label14.Text = "Addres";
            // 
            // v_name
            // 
            v_name.Location = new Point(67, 22);
            v_name.Name = "v_name";
            v_name.Size = new Size(212, 23);
            v_name.TabIndex = 1;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(9, 25);
            label15.Name = "label15";
            label15.Size = new Size(39, 15);
            label15.TabIndex = 0;
            label15.Text = "Name";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(c_longtitude);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(c_latitude);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(c_addres);
            groupBox1.Controls.Add(cust_addres);
            groupBox1.Controls.Add(c_name);
            groupBox1.Controls.Add(label6);
            groupBox1.Location = new Point(39, 191);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(299, 157);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Custumer Details";
            // 
            // c_longtitude
            // 
            c_longtitude.Location = new Point(208, 125);
            c_longtitude.Name = "c_longtitude";
            c_longtitude.Size = new Size(71, 23);
            c_longtitude.TabIndex = 8;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(140, 128);
            label10.Name = "label10";
            label10.Size = new Size(65, 15);
            label10.TabIndex = 7;
            label10.Text = "Longtitude";
            // 
            // c_latitude
            // 
            c_latitude.Location = new Point(67, 125);
            c_latitude.Name = "c_latitude";
            c_latitude.Size = new Size(67, 23);
            c_latitude.TabIndex = 6;
            c_latitude.TextChanged += textBox7_TextChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 129);
            label9.Name = "label9";
            label9.Size = new Size(50, 15);
            label9.TabIndex = 5;
            label9.Text = "Latitude";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 107);
            label8.Name = "label8";
            label8.Size = new Size(59, 15);
            label8.TabIndex = 4;
            label8.Text = "Location: ";
            // 
            // c_addres
            // 
            c_addres.Location = new Point(67, 56);
            c_addres.Multiline = true;
            c_addres.Name = "c_addres";
            c_addres.Size = new Size(212, 48);
            c_addres.TabIndex = 3;
            // 
            // cust_addres
            // 
            cust_addres.AutoSize = true;
            cust_addres.Location = new Point(12, 59);
            cust_addres.Name = "cust_addres";
            cust_addres.Size = new Size(44, 15);
            cust_addres.TabIndex = 2;
            cust_addres.Text = "Addres";
            // 
            // c_name
            // 
            c_name.Location = new Point(67, 22);
            c_name.Name = "c_name";
            c_name.Size = new Size(212, 23);
            c_name.TabIndex = 1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(9, 25);
            label6.Name = "label6";
            label6.Size = new Size(39, 15);
            label6.TabIndex = 0;
            label6.Text = "Name";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.BackColor = Color.Transparent;
            checkBox2.Location = new Point(240, 148);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(63, 19);
            checkBox2.TabIndex = 10;
            checkBox2.Text = "Vendor";
            checkBox2.UseVisualStyleBackColor = false;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(39, 149);
            label5.Name = "label5";
            label5.Size = new Size(78, 15);
            label5.TabIndex = 9;
            label5.Text = "Role Creation";
            label5.Click += label5_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.Transparent;
            checkBox1.Location = new Point(134, 148);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(78, 19);
            checkBox1.TabIndex = 8;
            checkBox1.Text = "Custumer\r\n";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // confirm
            // 
            confirm.Location = new Point(398, 104);
            confirm.Name = "confirm";
            confirm.Size = new Size(279, 23);
            confirm.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Location = new Point(398, 86);
            label4.Name = "label4";
            label4.Size = new Size(104, 15);
            label4.TabIndex = 6;
            label4.Text = "Confirm Password";
            // 
            // password
            // 
            password.Location = new Point(398, 54);
            password.Name = "password";
            password.Size = new Size(279, 23);
            password.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(398, 36);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 4;
            label3.Text = "Password";
            // 
            // email
            // 
            email.Location = new Point(39, 104);
            email.Name = "email";
            email.Size = new Size(279, 23);
            email.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(39, 86);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 2;
            label2.Text = "Email";
            // 
            // phone
            // 
            phone.Location = new Point(39, 54);
            phone.Name = "phone";
            phone.Size = new Size(279, 23);
            phone.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(39, 36);
            label1.Name = "label1";
            label1.Size = new Size(88, 15);
            label1.TabIndex = 0;
            label1.Text = "Phone Number";
            // 
            // Register
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 507);
            Controls.Add(guna2Panel1);
            Name = "Register";
            Text = "Register";
            Load += Register_Load;
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Label label1;
        private TextBox confirm;
        private Label label4;
        private TextBox password;
        private Label label3;
        private TextBox email;
        private Label label2;
        private TextBox phone;
        private CheckBox checkBox2;
        private Label label5;
        private CheckBox checkBox1;
        private GroupBox groupBox1;
        private TextBox c_addres;
        private Label cust_addres;
        private TextBox c_name;
        private Label label6;
        private TextBox c_longtitude;
        private Label label10;
        private TextBox c_latitude;
        private Label label9;
        private Label label8;
        private GroupBox groupBox2;
        private TextBox v_longtitude;
        private Label label11;
        private TextBox v_latitude;
        private Label label12;
        private Label label13;
        private TextBox v_addres;
        private Label label14;
        private TextBox v_name;
        private Label label15;
        private Button button1;
        private Label label16;
    }
}