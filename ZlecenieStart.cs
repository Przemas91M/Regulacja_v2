using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Regulacja_v2
{
    public partial class ZlecenieStart : Form
    {
        public ZlecenieStart(string usr, string urz)
        {
            InitializeComponent();
            user = usr;
            piec = urz;
        }
        readonly Plik plik = new Plik();
        private readonly string user, piec;
        private int index;
        private readonly List<TextBox> textBoxy = new List<TextBox>();
        private List<Recepta> rec;
        private readonly List<string> zlec = new List<string>();
        private string prog, file;
        private void button1_Click(object sender, EventArgs e)
        {
            textBoxy.Add(textBox1);                                     //tworzę listę z textboxami, z których będę pobierać numery zleceń
            textBoxy.Add(textBox2);
            textBoxy.Add(textBox3);
            textBoxy.Add(textBox4);
            textBoxy.Add(textBox5);
            textBoxy.Add(textBox6);
            textBoxy.Add(textBox7);
            textBoxy.Add(textBox8);
            textBoxy.Add(textBox9);
            textBoxy.Add(textBox10);
            foreach(TextBox poleTekstowe in textBoxy)                   //sprawdzam każdego textboxa, czy jest coś do niego wpisane
            {
                if(poleTekstowe.Text !=string.Empty)
                {
                    zlec.Add(poleTekstowe.Text);
                }
            }
            if (zlec.Count > 0)                                         //gdy zostało wypełnione przynajmniej jedno pole z receptą - wszystko prawidłowo
            {
                index = comboRecepty.SelectedIndex;                     //sprawdzam, która recepta została wybrana
                prog = rec[index].Recept;                               //pobranie nazwy recepty z listy
                file = Plik.UtworzPlikPomiar(piec, user, prog, zlec);   //tworzenie pliku, do którego będą zapisywane pomiary
                Form1.Static.Filename_Prog(file, index);                //kopiowanie nazwy pliku i wybranego programu do klasy głównej w celu uruchomienia wątku
                DialogResult = DialogResult.OK;                         //zamknięcie okna
            }
            else                                                        //w przypadku, gdy operator zapomni wpisać numer zlecenia, program wyświetli komunikat o błędzie
            {
                MessageBox.Show("Proszę wpisać numer zlecenia.");
                return;
            }
        }
        private void ZlecenieStart_Load(object sender, EventArgs e)     
        {
            rec = plik.PobierzRecepty();                                //pobranie nazw recept do listy
            comboRecepty.DataSource = rec;                              //wpisanie nazw recept do listy wyboru
            comboRecepty.DisplayMember = "Wyswietl";                    //ustawienie jaka zmienna ma być wyświetlana
            comboRecepty.SelectedIndex = 0;                             //automatyczny wybór pierwszego elementu z listy
            labelOperator.Text = user;                                  //pobranie i wyświetlenie nazwy operatora w oknie
            labelPiec.Text = piec;                                      //pobranie i wyświetlenie nazwy wybranego pieca w oknie
        }
    }
}
