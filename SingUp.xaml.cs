using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace txuribeltz
{
    /// <summary>
    /// Interaction logic for SingUp.xaml
    /// </summary>
    public partial class SingUp : Window
    {
        public SingUp()
        {
            InitializeComponent();
        }

        private void btnRegistratu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // zerbitzarira konektatu eta erabiltzaile hau sortu
                if (string.IsNullOrWhiteSpace(txtErabiltzailea.Text) || string.IsNullOrWhiteSpace(txtPasahitza.Password))
                {
                    txt_erroreak.Text = "Erabiltzaile edo pasahitza hutsik daude.";
                    return;
                }
                if (txtPasahitza.Password != txtPasahitza2.Password)
                {
                    txt_erroreak.Text = "Pasahitzak ez datoz bat.";
                    return;
                }
                // erabiltzailea sortu
                MessageBox.Show("Erabiltzailea ongi erregistratu da.");
                Close();
            }
            catch (Exception ex)
            {
                // errorea erakutsi
                txt_erroreak.Text = $"Errorea erregistratzean: {ex.Message}";
            }
        }

        private void btnAtzera_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
