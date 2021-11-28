namespace Regulacja_v2
{
    partial class Form5
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.Wykres = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Pr_ZapiszRaport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Wykres)).BeginInit();
            this.SuspendLayout();
            // 
            // Wykres
            // 
            this.Wykres.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.Wykres.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            legend1.Position.Auto = false;
            legend1.Position.Height = 4.469274F;
            legend1.Position.Width = 9.555069F;
            legend1.Position.Y = 3F;
            this.Wykres.Legends.Add(legend1);
            this.Wykres.Location = new System.Drawing.Point(0, 0);
            this.Wykres.Name = "Wykres";
            this.Wykres.Size = new System.Drawing.Size(1372, 538);
            this.Wykres.TabIndex = 0;
            this.Wykres.Text = "chart1";
            // 
            // Pr_ZapiszRaport
            // 
            this.Pr_ZapiszRaport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pr_ZapiszRaport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Pr_ZapiszRaport.Location = new System.Drawing.Point(1378, 12);
            this.Pr_ZapiszRaport.Name = "Pr_ZapiszRaport";
            this.Pr_ZapiszRaport.Size = new System.Drawing.Size(88, 42);
            this.Pr_ZapiszRaport.TabIndex = 1;
            this.Pr_ZapiszRaport.Text = "Zapisz raport";
            this.Pr_ZapiszRaport.UseVisualStyleBackColor = true;
            this.Pr_ZapiszRaport.Click += new System.EventHandler(this.Pr_ZapiszRaport_Click);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1469, 538);
            this.Controls.Add(this.Pr_ZapiszRaport);
            this.Controls.Add(this.Wykres);
            this.Name = "Form5";
            this.Text = "Raport Wykres";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form5_FormClosing);
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Wykres)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Wykres;
        private System.Windows.Forms.Button Pr_ZapiszRaport;
    }
}