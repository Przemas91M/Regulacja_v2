namespace Regulacja_V2
{
    partial class PotwZatrz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PotwZatrz));
            this.label1 = new System.Windows.Forms.Label();
            this.button_yes = new System.Windows.Forms.Button();
            this.button_no = new System.Windows.Forms.Button();
            this.button_check = new System.Windows.Forms.Button();
            this.label_piec = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(6, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(498, 80);
            this.label1.TabIndex = 0;
            this.label1.Text = "Program wykrył, że piec jest w trakcie cyklu pracy.\r\nNie znaleziono natomiast żad" +
    "nych zleceń przypisanych do tego pieca.\r\nCzy chcesz zresetować regulator?\r\n(Więc" +
    "ej info w pomocy)\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_yes
            // 
            this.button_yes.Location = new System.Drawing.Point(10, 154);
            this.button_yes.Name = "button_yes";
            this.button_yes.Size = new System.Drawing.Size(75, 23);
            this.button_yes.TabIndex = 1;
            this.button_yes.Text = "TAK";
            this.button_yes.UseVisualStyleBackColor = true;
            this.button_yes.Click += new System.EventHandler(this.button_yes_Click);
            // 
            // button_no
            // 
            this.button_no.Location = new System.Drawing.Point(216, 154);
            this.button_no.Name = "button_no";
            this.button_no.Size = new System.Drawing.Size(75, 23);
            this.button_no.TabIndex = 2;
            this.button_no.Text = "NIE";
            this.button_no.UseVisualStyleBackColor = true;
            this.button_no.Click += new System.EventHandler(this.button_no_Click);
            // 
            // button_check
            // 
            this.button_check.Location = new System.Drawing.Point(413, 146);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(75, 38);
            this.button_check.TabIndex = 3;
            this.button_check.Text = "Sprawdź plik procesów";
            this.button_check.UseVisualStyleBackColor = true;
            this.button_check.Click += new System.EventHandler(this.button_check_Click);
            // 
            // label_piec
            // 
            this.label_piec.AutoSize = true;
            this.label_piec.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_piec.Location = new System.Drawing.Point(229, 9);
            this.label_piec.Name = "label_piec";
            this.label_piec.Size = new System.Drawing.Size(57, 20);
            this.label_piec.TabIndex = 4;
            this.label_piec.Text = "label2";
            this.label_piec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PotwZatrz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 196);
            this.Controls.Add(this.label_piec);
            this.Controls.Add(this.button_check);
            this.Controls.Add(this.button_no);
            this.Controls.Add(this.button_yes);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PotwZatrz";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UWAGA!";
            this.TopMost = true;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.PotwZatrz_HelpButtonClicked);
            this.Load += new System.EventHandler(this.PotwZatrz_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_yes;
        private System.Windows.Forms.Button button_no;
        private System.Windows.Forms.Button button_check;
        private System.Windows.Forms.Label label_piec;
    }
}