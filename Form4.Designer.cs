namespace Regulacja_v2
{
    partial class Form4
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.zleceniaBox = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelProcesStop = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelProcesStart = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelOperator = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelProg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Kalendarz = new System.Windows.Forms.MonthCalendar();
            this.procesyBox = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkYear = new System.Windows.Forms.CheckBox();
            this.checkMonth = new System.Windows.Forms.CheckBox();
            this.checkDay = new System.Windows.Forms.CheckBox();
            this.checkAll = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Pr_Szukaj_Raporty = new System.Windows.Forms.Button();
            this.textBoxSzukaj = new System.Windows.Forms.TextBox();
            this.comboSortuj = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboRegulatory = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.zleceniaBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelProcesStop);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.labelProcesStart);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelOperator);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelProg);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 459);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dane procesu";
            // 
            // zleceniaBox
            // 
            this.zleceniaBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.zleceniaBox.FormattingEnabled = true;
            this.zleceniaBox.ItemHeight = 16;
            this.zleceniaBox.Location = new System.Drawing.Point(10, 231);
            this.zleceniaBox.Name = "zleceniaBox";
            this.zleceniaBox.Size = new System.Drawing.Size(160, 212);
            this.zleceniaBox.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(7, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Realizowane zlecenia:";
            // 
            // labelProcesStop
            // 
            this.labelProcesStop.AutoSize = true;
            this.labelProcesStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelProcesStop.Location = new System.Drawing.Point(7, 163);
            this.labelProcesStop.Name = "labelProcesStop";
            this.labelProcesStop.Size = new System.Drawing.Size(109, 16);
            this.labelProcesStop.TabIndex = 7;
            this.labelProcesStop.Text = "labelProcesStop";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(6, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Koniec procesu:";
            // 
            // labelProcesStart
            // 
            this.labelProcesStart.AutoSize = true;
            this.labelProcesStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelProcesStart.Location = new System.Drawing.Point(6, 119);
            this.labelProcesStart.Name = "labelProcesStart";
            this.labelProcesStart.Size = new System.Drawing.Size(108, 16);
            this.labelProcesStart.TabIndex = 5;
            this.labelProcesStart.Text = "labelProcesStart";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(6, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Start procesu:";
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelOperator.Location = new System.Drawing.Point(6, 78);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(91, 16);
            this.labelOperator.TabIndex = 3;
            this.labelOperator.Text = "labelOperator";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(6, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Operator:";
            // 
            // labelProg
            // 
            this.labelProg.AutoSize = true;
            this.labelProg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelProg.Location = new System.Drawing.Point(6, 36);
            this.labelProg.Name = "labelProg";
            this.labelProg.Size = new System.Drawing.Size(67, 16);
            this.labelProg.TabIndex = 1;
            this.labelProg.Text = "labelProg";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Program wygrzewania:";
            // 
            // Kalendarz
            // 
            this.Kalendarz.Location = new System.Drawing.Point(9, 20);
            this.Kalendarz.MaxSelectionCount = 1;
            this.Kalendarz.MinDate = new System.DateTime(2020, 3, 11, 0, 0, 0, 0);
            this.Kalendarz.Name = "Kalendarz";
            this.Kalendarz.TabIndex = 2;
            // 
            // procesyBox
            // 
            this.procesyBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.procesyBox.FormattingEnabled = true;
            this.procesyBox.ItemHeight = 15;
            this.procesyBox.Location = new System.Drawing.Point(220, 244);
            this.procesyBox.Name = "procesyBox";
            this.procesyBox.Size = new System.Drawing.Size(242, 319);
            this.procesyBox.TabIndex = 3;
            this.procesyBox.SelectedIndexChanged += new System.EventHandler(this.procesyBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(274, 225);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Wybierz proces:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(533, 405);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 51);
            this.button1.TabIndex = 5;
            this.button1.Text = "Wyświetl wykres";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(6, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 16);
            this.label8.TabIndex = 7;
            this.label8.Text = "- z wybranego dnia: ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(5, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(155, 16);
            this.label9.TabIndex = 8;
            this.label9.Text = "- z wybranego miesiaca: ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label10.Location = new System.Drawing.Point(5, 123);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 16);
            this.label10.TabIndex = 9;
            this.label10.Text = "- z wybranego roku: ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label11.Location = new System.Drawing.Point(6, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 16);
            this.label11.TabIndex = 10;
            this.label11.Text = "- wszystkie: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkYear);
            this.groupBox2.Controls.Add(this.checkMonth);
            this.groupBox2.Controls.Add(this.checkDay);
            this.groupBox2.Controls.Add(this.checkAll);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox2.Location = new System.Drawing.Point(468, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(239, 162);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wyświetl procesy:";
            // 
            // checkYear
            // 
            this.checkYear.AutoSize = true;
            this.checkYear.Location = new System.Drawing.Point(129, 125);
            this.checkYear.Name = "checkYear";
            this.checkYear.Size = new System.Drawing.Size(15, 14);
            this.checkYear.TabIndex = 14;
            this.checkYear.UseVisualStyleBackColor = true;
            this.checkYear.CheckedChanged += new System.EventHandler(this.checkYear_CheckedChanged);
            // 
            // checkMonth
            // 
            this.checkMonth.AutoSize = true;
            this.checkMonth.Location = new System.Drawing.Point(156, 94);
            this.checkMonth.Name = "checkMonth";
            this.checkMonth.Size = new System.Drawing.Size(15, 14);
            this.checkMonth.TabIndex = 13;
            this.checkMonth.UseVisualStyleBackColor = true;
            this.checkMonth.CheckedChanged += new System.EventHandler(this.checkMonth_CheckedChanged);
            // 
            // checkDay
            // 
            this.checkDay.AutoSize = true;
            this.checkDay.Location = new System.Drawing.Point(129, 63);
            this.checkDay.Name = "checkDay";
            this.checkDay.Size = new System.Drawing.Size(15, 14);
            this.checkDay.TabIndex = 12;
            this.checkDay.UseVisualStyleBackColor = true;
            this.checkDay.CheckedChanged += new System.EventHandler(this.checkDay_CheckedChanged);
            // 
            // checkAll
            // 
            this.checkAll.AutoSize = true;
            this.checkAll.Location = new System.Drawing.Point(78, 33);
            this.checkAll.Name = "checkAll";
            this.checkAll.Size = new System.Drawing.Size(15, 14);
            this.checkAll.TabIndex = 11;
            this.checkAll.UseVisualStyleBackColor = true;
            this.checkAll.CheckedChanged += new System.EventHandler(this.checkAll_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Pr_Szukaj_Raporty);
            this.groupBox3.Controls.Add(this.textBoxSzukaj);
            this.groupBox3.Controls.Add(this.comboSortuj);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.comboRegulatory);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox3.Location = new System.Drawing.Point(468, 180);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(222, 214);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sortowanie:";
            // 
            // Pr_Szukaj_Raporty
            // 
            this.Pr_Szukaj_Raporty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Pr_Szukaj_Raporty.Location = new System.Drawing.Point(65, 141);
            this.Pr_Szukaj_Raporty.Name = "Pr_Szukaj_Raporty";
            this.Pr_Szukaj_Raporty.Size = new System.Drawing.Size(87, 41);
            this.Pr_Szukaj_Raporty.TabIndex = 16;
            this.Pr_Szukaj_Raporty.Text = "Wyszukaj";
            this.Pr_Szukaj_Raporty.UseVisualStyleBackColor = true;
            this.Pr_Szukaj_Raporty.Click += new System.EventHandler(this.Pr_Szukaj_Raporty_Click);
            // 
            // textBoxSzukaj
            // 
            this.textBoxSzukaj.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxSzukaj.Location = new System.Drawing.Point(33, 104);
            this.textBoxSzukaj.Name = "textBoxSzukaj";
            this.textBoxSzukaj.Size = new System.Drawing.Size(160, 22);
            this.textBoxSzukaj.TabIndex = 15;
            // 
            // comboSortuj
            // 
            this.comboSortuj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSortuj.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboSortuj.FormattingEnabled = true;
            this.comboSortuj.Items.AddRange(new object[] {
            "Numer zlecenia",
            "Nazwa programu",
            "Nazwa operatora"});
            this.comboSortuj.Location = new System.Drawing.Point(88, 61);
            this.comboSortuj.Name = "comboSortuj";
            this.comboSortuj.Size = new System.Drawing.Size(121, 24);
            this.comboSortuj.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label12.Location = new System.Drawing.Point(12, 64);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 16);
            this.label12.TabIndex = 13;
            this.label12.Text = "Szukaj po: ";
            // 
            // comboRegulatory
            // 
            this.comboRegulatory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRegulatory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboRegulatory.FormattingEnabled = true;
            this.comboRegulatory.Items.AddRange(new object[] {
            "Wszystkie",
            "Piec 1",
            "Piec 2",
            "Piec 3",
            "Piec 4",
            "Piec 5"});
            this.comboRegulatory.Location = new System.Drawing.Point(89, 25);
            this.comboRegulatory.Name = "comboRegulatory";
            this.comboRegulatory.Size = new System.Drawing.Size(102, 24);
            this.comboRegulatory.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(4, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 16);
            this.label7.TabIndex = 11;
            this.label7.Text = "Urzadzenie:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Kalendarz);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox4.Location = new System.Drawing.Point(248, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(178, 194);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Wybierz datę:";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 570);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.procesyBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form4";
            this.Text = "LUMAG Raport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form4_FormClosing);
            this.Load += new System.EventHandler(this.Form4_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelOperator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelProg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelProcesStop;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelProcesStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox zleceniaBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MonthCalendar Kalendarz;
        private System.Windows.Forms.ListBox procesyBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboRegulatory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button Pr_Szukaj_Raporty;
        private System.Windows.Forms.TextBox textBoxSzukaj;
        private System.Windows.Forms.ComboBox comboSortuj;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkYear;
        private System.Windows.Forms.CheckBox checkMonth;
        private System.Windows.Forms.CheckBox checkDay;
        private System.Windows.Forms.CheckBox checkAll;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}