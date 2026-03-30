namespace grocerseeker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            username = new TextBox();
            pass = new TextBox();
            button1 = new Button();
            label2 = new Label();
            role = new ComboBox();
            label3 = new Label();
            linkLabel1 = new LinkLabel();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            label1 = new Label();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // username
            // 
            username.Font = new Font("Times New Roman", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            username.Location = new Point(30, 131);
            username.Name = "username";
            username.Size = new Size(237, 22);
            username.TabIndex = 0;
            username.TextChanged += textBox1_TextChanged;
            // 
            // pass
            // 
            pass.Font = new Font("Times New Roman", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pass.Location = new Point(30, 183);
            pass.Name = "pass";
            pass.Size = new Size(237, 22);
            pass.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(97, 274);
            button1.Name = "button1";
            button1.Size = new Size(96, 30);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 165);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 4;
            label2.Text = "Password";
            // 
            // role
            // 
            role.Font = new Font("Times New Roman", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            role.FormattingEnabled = true;
            role.Items.AddRange(new object[] { "Vendor", "Customer" });
            role.Location = new Point(30, 235);
            role.Name = "role";
            role.Size = new Size(237, 23);
            role.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(30, 217);
            label3.Name = "label3";
            label3.Size = new Size(30, 15);
            label3.TabIndex = 6;
            label3.Text = "Role";
            label3.Click += label3_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel1.LinkColor = Color.Black;
            linkLabel1.Location = new Point(218, 314);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(48, 15);
            linkLabel1.TabIndex = 7;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Sign Up";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // guna2Panel1
            // 
            guna2Panel1.BorderRadius = 50;
            guna2Panel1.BorderThickness = 50;
            guna2Panel1.Controls.Add(label1);
            guna2Panel1.Controls.Add(linkLabel1);
            guna2Panel1.Controls.Add(label3);
            guna2Panel1.Controls.Add(role);
            guna2Panel1.Controls.Add(label2);
            guna2Panel1.Controls.Add(button1);
            guna2Panel1.Controls.Add(pass);
            guna2Panel1.Controls.Add(username);
            guna2Panel1.CustomBorderColor = Color.FromArgb(192, 255, 255);
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.FillColor = Color.FromArgb(192, 255, 255);
            guna2Panel1.Location = new Point(235, 44);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(295, 352);
            guna2Panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 113);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 8;
            label1.Text = "Username";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(guna2Panel1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox username;
        private TextBox pass;
        private Button button1;
        private Label label2;
        private ComboBox role;
        private Label label3;
        private LinkLabel linkLabel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Label label1;
    }
}
