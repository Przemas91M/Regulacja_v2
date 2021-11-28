using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Regulacja_v2
{
    public partial class Form5 : Form
    {
        public Form5(List<Pomiar> pomiary, string title, string start, string stop, string program, string piec, string[] zlecenia, string piecowy)
        {
            InitializeComponent();
            this.TopMost = true;        //wyswietlenie okna na wierzchu
            graf = pomiary;             //przepisanie danych do zmiennych prywatnych
            tytul = title;
            rapstart = start;
            rapstop = stop;
            rapprogram = program;
            rappiec = piec;
            rappiecowy = piecowy;
            rapzlecenia = zlecenia;
        }
        private List<Pomiar> graf;    //tworzenie listy
        private string tytul,rapstart,rapstop,rapprogram,rappiec,rappiecowy;         //zmienna przechowujaca tytul grafu
        private string[] rapzlecenia;
        private Series S1, S2;
        string path = Directory.GetCurrentDirectory();
        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                S1 = new Series("Temp Zm")
                {
                    XValueMember = "X",                      //pobiera wartosci X z przypisanej tabeli
                    YValueMembers = "TempZm",                //pobiera wartosci Y z przypisanej tabeli
                    XValueType = ChartValueType.String,      //rodzaj zmiennych X - tutaj string
                    YValueType = ChartValueType.Auto,      //rodzaj zmiennych Y - auto przypisanie
                    ChartType = SeriesChartType.FastPoint,    //rodzaj wykresu - fastline  dla duzej ilosci danych
                    //ChartType = SeriesChartType.FastLine,    //rodzaj wykresu - fastline  dla duzej ilosci danych
                    Color = Color.Red,                       //kolor linii
                    BorderWidth = 1,                         //grubosc linii
                    MarkerSize = 3,
                    MarkerStep = 1,
                    MarkerStyle = MarkerStyle.Circle
                }; //**** analogicznie dla temp zadanej przez regulator ***//
                S2 = new Series("Temp Zad")
                {
                    XValueMember = "X",
                    YValueMembers = "TempZad",
                    XValueType = ChartValueType.String,
                    YValueType = ChartValueType.Auto,
                    ChartType = SeriesChartType.FastPoint,
                    //ChartType = SeriesChartType.FastLine,    //rodzaj wykresu - fastline  dla duzej ilosci danych
                    Color = Color.Green,
                    BorderWidth = 1,
                    MarkerSize = 2,
                    MarkerStep = 1,
                    MarkerStyle = MarkerStyle.Circle
            }; 
                Wykres.Series.Add(S1);
                Wykres.Series.Add(S2);
                Wykres.Titles.Add(tytul);                   //dopisanie tytułu do grafu
                Wykres.Dock = DockStyle.Fill;               //zadokowanie grafu - zmienia rozmiar z oknem
                Wykres.Titles[0].Docking = Docking.Top;     //zadokowanie tytułu grafu na górze
                ChartArea CA = Wykres.ChartAreas[0];        //tworzenie pola wykresu
                CA.AxisY.MajorGrid.Interval = 20; //styl osi Y na kropka - kreska
                CA.AxisX.LabelStyle.Interval = 400;
                CA.AxisY.LabelStyle.Interval = 5;
                CA.AxisX.LabelStyle.Angle = -45;
                CA.AxisX.MajorGrid.Interval = 400;
                CA.AxisY.MinorGrid.Enabled = true;
                CA.AxisY.MinorGrid.Interval = 5;
                CA.AxisY.MinorGrid.IntervalOffset = 10;
                CA.AxisX.ScaleView.Zoomable = true;         //ustawienie mozliwosci powiekszania grafu
                CA.AxisY.ScaleView.Zoomable = true;
                CA.CursorX.AutoScroll = true;               //autoscroll do powiekszanego pola
                CA.CursorY.AutoScroll = true;
                CA.CursorX.IsUserSelectionEnabled = true;   //odblokowanie zaznaczania pola
                Wykres.Titles[0].Font = new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold);    //czcionka tytułu
                CA.AxisY.Title = "Temperatura [ᵒC]";        //opis osi Y
                CA.AxisY.TitleFont = new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold); //czcionka opisu osi Y
                CA.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
                CA.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid; //styl osi Y na kropka - kreska
                CA.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDot; //styl osi X na kropka - kreska
                Wykres.AlignDataPointsByAxisLabel();        //ustawienie punktów w linii X-Y
                Wykres.DataSource = graf;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Pr_ZapiszRaport_Click(object sender, EventArgs e)
        {
            try
            {
                Series[] Serie = new Series[2];
                ChartArea[] CA = new ChartArea[1];
                Chart Wykres2 = new Chart();
                //Wykres2.Visible = false;
                Wykres.Series.CopyTo(Serie, 0);
                Wykres.ChartAreas.CopyTo(CA, 0);
                CA[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
                CA[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid; //styl osi Y na kropka - kreska
                CA[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDot; //styl osi X na kropka - kreska
                CA[0].AxisY.MajorGrid.Interval = 20; //styl osi Y na kropka - kreska
                CA[0].AxisX.LabelStyle.Interval = 400;
                CA[0].AxisY.LabelStyle.Interval = 5;
                CA[0].AxisX.LabelStyle.Angle = -45;
                CA[0].AxisX.MajorGrid.Interval = 400;
                CA[0].AxisY.MinorGrid.Enabled = true;
                CA[0].AxisY.MinorGrid.Interval = 5;
                CA[0].AxisY.MinorGrid.IntervalOffset = 10;
                CA[0].AxisX.ScaleView.ZoomReset();         
                CA[0].AxisY.TitleFont = new System.Drawing.Font("Times New Roman", 13, FontStyle.Bold); //czcionka opisu osi Y
                Wykres2.Series.Add(Serie[0]);
                Wykres2.Series.Add(Serie[1]);
                Wykres2.ChartAreas.Add(CA[0]);
                Wykres2.Titles.Add(tytul);
                Wykres2.Titles[0].Font = new System.Drawing.Font("Times New Roman", 13, FontStyle.Bold);    //czcionka tytułu
                Wykres2.Series[0].BorderWidth = 1;
                Wykres2.Series[1].BorderWidth = 1;
                Wykres2.Width = 2100;
                Wykres2.Height = 1400;
                Wykres2.SaveImage(path + "\\Temp\\Wykres.tif", ChartImageFormat.Tiff);
                Document doc = new Document();
                doc.SetPageSize(PageSize.A4);
                PdfWriter.GetInstance(doc, new FileStream(@"C:\Raporty\" + rappiec + @"\" + tytul + ".pdf", FileMode.Create));
                doc.Open();
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(path + "\\Temp\\Wykres.tif");
                jpg.Rotation = (float)Math.PI / 2;
                jpg.RotationDegrees = -90f;
                jpg.ScaleToFit(560f, 806f);
                doc.Add(jpg);
                Phrase p1 = new Phrase();
                Chunk c0 = new Chunk("Urzadzenie: " + rappiec +"\r\n");
                Chunk c1 = new Chunk("Poczatek procesu: " + rapstart + "\r\n");
                Chunk c2 = new Chunk("Koniec procesu: " + rapstop + "\r\n");
                Chunk c3 = new Chunk("Realizowany program: " + rapprogram + "\r\n");
                Chunk c4 = new Chunk("Operator: " + rappiecowy + "\r\n");
                Chunk c5 = new Chunk("Realizowane zlecenia:\r\n");
                Chunk c6;
                p1.Add(c0); p1.Add(c1); p1.Add(c2); p1.Add(c3); p1.Add(c4); p1.Add(c5);
                for (int i = 0; i < rapzlecenia.Length; i++)
                {
                    c6 = new Chunk(rapzlecenia[i] + "\r\n");
                    p1.Add(c6);
                }
                Paragraph p = new Paragraph();
                p.Add(p1);
                doc.Add(p);
                doc.Close();
                using (var file = File.CreateText(@"C:\Raporty\" + rappiec + @"\" + tytul + ".csv"))
                {
                    file.WriteLine("sep=;");
                    file.WriteLine("Data;Temperatura Zadana;Temperatura Zmierzona");
                    for (int i = 0; i < graf.Count; i++)
                    {
                        file.WriteLine(graf[i].X + ";" + graf[i].TempZad.ToString() + ";" + graf[i].TempZm.ToString());
                    }
                }
                MessageBox.Show("Zapis raportu zakończony powodzeniem.");
                Wykres2.Dispose();
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }
        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            S1.Dispose();
            S2.Dispose();
            Wykres.Dispose();
        }
    }
}
