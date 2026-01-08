using System.IO;
using System.Net.Sockets;
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
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;
        private NetworkStream ns;
        public LoginWindow()
        {
            InitializeComponent();

            //zerbitzarira konektatuko gara lehenengo
            zerbitzariraKonektatu();
        }

        public void zerbitzariraKonektatu()
        {
            try
            {
               
                client = new TcpClient();
                client.Connect("127.0.0.1", 13000); // Zerbitzariaren IP eta portua
                ns = client.GetStream();
                writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };
                reader = new StreamReader(ns, Encoding.UTF8);

                txt_mezuak.Text += "Konektatuta zerbitzarira.\n";

                // Lehen konexioan zerbitzarira mezua bidali eta erantzuna jasotzeko.
                Thread t = new Thread(() =>
                {
                    try
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                txt_mezuak.Text += "Server-etik: " + line + "\n";
                                
                                // Process authentication responses
                                if (line.StartsWith("LOGIN_OK"))
                                {
                                    string[] parts = line.Split(':');
                                    string userType = parts.Length > 1 ? parts[1] : "user";
                                    
                                    txt_mezuak.Text = "Login arrakastatsua!";
                                    
                                    // Ireki bakoitzaren lehioa
                                    if (userType == "admin")
                                    {
                                        AdminWindow adminWin = new AdminWindow();
                                        adminWin.Show();
                                    }
                                    else
                                    {
                                        // UserWindow userWin = new UserWindow();
                                        // userWin.Show();
                                    }
                                    
                                    this.Close();
                                }
                                else if (line.StartsWith("LOGIN_FAIL"))
                                {
                                    string[] parts = line.Split(':');
                                    txt_erroreak.Text = parts.Length > 1 ? parts[1] : "Login errorea";
                                    txt_mezuak.Text = "";
                                }
                                else if (line == "SIGNUP_OK")
                                {
                                    txt_mezuak.Text = "Erabiltzailea ondo sortu da!";
                                }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            txt_mezuak.Text += "Bezeroa deskonektatua\n";
                        });
                    }
                });
                t.IsBackground = true;
                t.Start();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ezin izan da konektatu zerbitzarira: {ex.Message}");
                txt_erroreak.Text = "Ezin izan da konektatu zerbitzarira.";
            }
        }

        private void zerbitzaritikDeskonektatu()
        {
            //Galdera ikurrak null ez den egiaztatzen du
            try
            {
                reader?.Close();
                writer?.Close();
                ns?.Close();
                if (client?.Connected == true)
                    client.Close();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    txt_mezuak.Text += "Errorea deskonektatzean: " + ex.Message + "\n";
                });
            }

        }
        /*
         * Sisteman logeatuko gara hemendik logeatzeko botoia sakatzen dugunean
         * Behar dugu:
         *  - erabiltzailea
         *  - pasahitza
         * Eta ez badago erabiltzailerik, sortzeko aukera sortu
        */
        public void login(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    txt_erroreak.Text = "Erabiltzaile edo pasahitza hutsik daude.";
                    return;
                }

                // Zerbitzariari bidali log in egiteko mezua
                string message = $"LOGIN:{txtUsuario.Text}:{txtPassword.Password}";
                writer.WriteLine(message);
                txt_erroreak.Text = "";
                txt_mezuak.Text = "Autentifikazioa bidaltzen...";
            }
            catch (Exception ex)
            {
                txt_erroreak.Text = $"Errorea login egitean: {ex.Message}, ez badaukazu, sortu erabiltzaile bat";
                txtUsuario.Clear();
                txtPassword.Clear();
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
                // Pass the writer to the signup window so it can communicate with the server
                Window signup = new SingUp(writer);
                signup.ShowDialog();
            }
            catch (Exception ex)
            {
                txt_erroreak.Text = $"Ezin izan da erabiltzailerik sortu: {ex.Message}";
            }
        }
        //enter sakatuz login egin ahal izateko
        //XAML fitxategian jarri PreviewKeyDown="Window_PreviewKeyDown"
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                login(this, new RoutedEventArgs());
            }
        }
    }
}