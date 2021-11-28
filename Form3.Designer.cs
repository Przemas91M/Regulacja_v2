namespace Regulacja_v2
{
    partial class Form3
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
            this.Password_Box = new System.Windows.Forms.TextBox();
            this.User_Box = new System.Windows.Forms.TextBox();
            this.Pr_Login = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Password_Box
            // 
            this.Password_Box.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Password_Box.Location = new System.Drawing.Point(52, 77);
            this.Password_Box.Name = "Password_Box";
            this.Password_Box.PasswordChar = '*';
            this.Password_Box.Size = new System.Drawing.Size(100, 22);
            this.Password_Box.TabIndex = 19;
            this.Password_Box.UseSystemPasswordChar = true;
            this.Password_Box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Password_Box_KeyDown);
            // 
            // User_Box
            // 
            this.User_Box.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.User_Box.Location = new System.Drawing.Point(52, 30);
            this.User_Box.Name = "User_Box";
            this.User_Box.Size = new System.Drawing.Size(100, 22);
            this.User_Box.TabIndex = 18;
            // 
            // Pr_Login
            // 
            this.Pr_Login.Location = new System.Drawing.Point(64, 129);
            this.Pr_Login.Name = "Pr_Login";
            this.Pr_Login.Size = new System.Drawing.Size(75, 23);
            this.Pr_Login.TabIndex = 17;
            this.Pr_Login.Text = "Zaloguj";
            this.Pr_Login.UseVisualStyleBackColor = true;
            this.Pr_Login.Click += new System.EventHandler(this.Pr_Login_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(80, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 20;
            this.label1.Text = "Login:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(77, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 18);
            this.label2.TabIndex = 21;
            this.label2.Text = "Hasło:";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 166);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Password_Box);
            this.Controls.Add(this.User_Box);
            this.Controls.Add(this.Pr_Login);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zaloguj";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Password_Box;
        private System.Windows.Forms.TextBox User_Box;
        private System.Windows.Forms.Button Pr_Login;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}