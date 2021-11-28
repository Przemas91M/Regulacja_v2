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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        Uzytkownik Uzytkownik = new Uzytkownik();
        private void Form3_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Pr_Login_Click(object sender, EventArgs e)
        {
            int login = Uzytkownik.Zaloguj(User_Box.Text, Password_Box.Text);         //zaloguj uzytkownika
            if (login == 1)                                                           //jezeli logowanie powiodlo sie 
            {
                MessageBox.Show("Zalogowano użytkownika: " + User_Box.Text);
                this.Close();
            }
            else if(login == 2)
            {
                MessageBox.Show("Logowanie użytkownika nie powiodło się\r\nNieprawidłowe hasło!","Błąd");
                return;
            }
            else if(login == 3)
            {
                MessageBox.Show("Logowanie użytkownika nie powiodło się\r\nNie znaleziono takiego użytkownika!", "Błąd");
                return;
            }
        }
        private void Password_Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Pr_Login.PerformClick();
            }
        }
    }
}
