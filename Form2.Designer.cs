namespace Regulacja_v2
{
    partial class Form2
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
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Piec 1");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Piec 2");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Piec 3");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Piec 4");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Piec 5");
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Com_PB = new System.Windows.Forms.Button();
            this.Pr_ID = new System.Windows.Forms.Button();
            this.Pr_Zatwierdz = new System.Windows.Forms.Button();
            this.Pr_Form_Close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listPiece = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 31);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(78, 21);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 13;
            // 
            // Com_PB
            // 
            this.Com_PB.Location = new System.Drawing.Point(12, 65);
            this.Com_PB.Name = "Com_PB";
            this.Com_PB.Size = new System.Drawing.Size(78, 23);
            this.Com_PB.TabIndex = 14;
            this.Com_PB.Text = "Sprawdź";
            this.Com_PB.UseVisualStyleBackColor = true;
            this.Com_PB.Click += new System.EventHandler(this.Com_PB_Click);
            // 
            // Pr_ID
            // 
            this.Pr_ID.Location = new System.Drawing.Point(12, 116);
            this.Pr_ID.Name = "Pr_ID";
            this.Pr_ID.Size = new System.Drawing.Size(78, 44);
            this.Pr_ID.TabIndex = 15;
            this.Pr_ID.Text = "Wyszukaj\r\nponownie";
            this.Pr_ID.UseVisualStyleBackColor = true;
            this.Pr_ID.Click += new System.EventHandler(this.Pr_ID_Click);
            // 
            // Pr_Zatwierdz
            // 
            this.Pr_Zatwierdz.Location = new System.Drawing.Point(15, 185);
            this.Pr_Zatwierdz.Name = "Pr_Zatwierdz";
            this.Pr_Zatwierdz.Size = new System.Drawing.Size(75, 23);
            this.Pr_Zatwierdz.TabIndex = 21;
            this.Pr_Zatwierdz.Text = "Zatwierdź";
            this.Pr_Zatwierdz.UseVisualStyleBackColor = true;
            this.Pr_Zatwierdz.Click += new System.EventHandler(this.Pr_Zatwierdz_Click);
            // 
            // Pr_Form_Close
            // 
            this.Pr_Form_Close.Location = new System.Drawing.Point(149, 185);
            this.Pr_Form_Close.Name = "Pr_Form_Close";
            this.Pr_Form_Close.Size = new System.Drawing.Size(75, 23);
            this.Pr_Form_Close.TabIndex = 22;
            this.Pr_Form_Close.Text = "Zamknij";
            this.Pr_Form_Close.UseVisualStyleBackColor = true;
            this.Pr_Form_Close.Click += new System.EventHandler(this.Pr_Form_Close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Dostępne porty:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 26);
            this.label2.TabIndex = 24;
            this.label2.Text = "Dostępne \r\nurzadzenia:";
            // 
            // listPiece
            // 
            this.listPiece.CheckBoxes = true;
            this.listPiece.GridLines = true;
            this.listPiece.HideSelection = false;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.StateImageIndex = 0;
            listViewItem10.StateImageIndex = 0;
            this.listPiece.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10});
            this.listPiece.Location = new System.Drawing.Point(145, 41);
            this.listPiece.Name = "listPiece";
            this.listPiece.Size = new System.Drawing.Size(79, 97);
            this.listPiece.TabIndex = 25;
            this.listPiece.UseCompatibleStateImageBehavior = false;
            this.listPiece.View = System.Windows.Forms.View.SmallIcon;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 218);
            this.ControlBox = false;
            this.Controls.Add(this.listPiece);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Pr_Form_Close);
            this.Controls.Add(this.Pr_Zatwierdz);
            this.Controls.Add(this.Pr_ID);
            this.Controls.Add(this.Com_PB);
            this.Controls.Add(this.comboBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Starter";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Com_PB;
        private System.Windows.Forms.Button Pr_ID;
        private System.Windows.Forms.Button Pr_Zatwierdz;
        private System.Windows.Forms.Button Pr_Form_Close;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listPiece;
    }
}