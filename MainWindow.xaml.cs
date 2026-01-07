using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace txuribeltz
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        /*
         * Sisteman logeatuko gara hemendik
         * Behar dugu:
         *  - erabiltzailea
         *  - pasahitza
         * Eta ez badago erabiltzailerik, sortzeko aukera sortu
        */
        public void login(object sender, RoutedEventArgs e)
        {
            try
            {
                // zerbitzarira konektatu eta erabiltzaile honekin logeatu

                if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    txt_erroreak.Text = "Erabiltzaile edo pasahitza hutsik daude.";
                    return;
                }
                
            }
            catch (Exception ex)
            {
                // erabiltzailea sortzeko menua eman

                txt_erroreak.Text = $"Sortu erabiltzailea, ez dago {txtUsuario} erabiltzailerik";
            }
        }

        /*
         * Sisteman erabiltzaile berri bat sortuko dugu hemendik
         * Behar dugu:
         *  - erabiltzailea
         *  - pasahitza
        */
        private void newUser(object sender, RoutedEventArgs e)
        {
            try
            {
                // zerbitzarira konektatu eta erabiltzaile hau sortu
                Window signup = new SingUp();
                signup.ShowDialog();
            }
            catch (Exception ex)
            {
                txt_erroreak.Text = $"Ezin izan da erabiltzailerik sortu";
            }
        }
    }
}