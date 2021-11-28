using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Regulacja_V2
{
    public partial class PotwZatrz : Form
    {
        private readonly string _piec;
        public PotwZatrz(string piec)
        {
            InitializeComponent();
            _piec = piec;
        }

        /// <summary>
        /// Przy otwarciu okna ustawiana jest nazwa pieca w miejscu label_piec.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PotwZatrz_Load(object sender, EventArgs e)
        {
            label_piec.Text = _piec;
        }

        /// <summary>
        /// Przycisk otwierajacy plik z aktualnymi procesami.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_check_Click(object sender, EventArgs e)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            path = string.Concat(path, "\\Data\\procesy.dat");
            System.Diagnostics.Process.Start(path);

        }
        /// <summary>
        /// Przycisk wyboru opcji 'NIE'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_no_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
        /// <summary>
        /// Przycisk wyboru opcji 'TAK'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_yes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }
        /// <summary>
        /// Okienko otwierane po kliknięciu przycisku pomocy. 
        /// Wyświetla tekst wyjaśniający przyczynę niezatrzymania regulatora po zakończonym procesie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PotwZatrz_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            MessageBox.Show("Program wykrył, że regulator jest w stanie cyklu pracy, \n" +
                "lecz w systemie nie ma zapisanego zlecenia dla tego pieca. \n" +
                "Czasami tak się dzieje, gdy piec zakończy cykl wygrzewania ale sam regulator nie wychodzi ze stanu pracy. \n" +
                "Regulator potrafi również przejść w stan pracy po załączeniu napięcia zasilajacego. \n" +
                "Jeżeli piec pracuje a nie uruchamiałeś cyklu wygrzewania zatrzymaj go, klikając przycisk 'TAK'. \n" +
                "Klikając przycisk 'NIE' regulator będzie pracować dalej, lecz wykres nie zostanie zapisany w programie.");
        }
    }
}
