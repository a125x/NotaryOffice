using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace NotaryOffice
{
    public partial class MainWindow : Window
    {
        //Paths to the files
        private const string UsersFile = "..\\XMLDB\\users.xml";
        private const string DealsFile = "..\\XMLDB\\deals.xml";

        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (tbxLogin.Text != "" && txbPassword.Text != "")
            {
                // Get the user information entered by the user
                var userInfo = new XElement("User",
                                            new XElement("Login", tbxLogin.Text),
                                            new XElement("Password", txbPassword.Text));

                // Add the user information to the XML file
                XDocument doc = XDocument.Load(UsersFile);
                doc.Element("Users").Add(userInfo);
                doc.Save(UsersFile);

                // Clear the login and password boxes
                tbxLogin.Text = "";
                txbPassword.Text = "";

                // Allow the user to pass to the content
                MessageBox.Show("welcome");
                var newPage = new NotaryOfficeMain();
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.Content = newPage;
            }
            else
            {
                tbPlaceholderLogin.Text = "add your login";
                tbPlaceholderPassword.Text = "add your password";
            }
        }
        private void btnSingIn_Click(object sender, RoutedEventArgs e)
        {
            XDocument doc = XDocument.Load(UsersFile);

            // Get the user information based on the login entered by the user
            var userInfo = doc.Descendants("User")
                               .Where(x => x.Element("Login").Value == tbxLogin.Text)
                               .FirstOrDefault();

            // Check if the user exists in the XML file
            if (userInfo != null)
            {
                // Check if the password entered by the user matches the password in the XML file
                if (txbPassword.Text == userInfo.Element("Password").Value)
                {
                    MessageBox.Show("welcome");
                    var newPage = new NotaryOfficeMain();
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.Content = newPage;
                }
                else
                {
                    txbPassword.Text = "";
                    tbPlaceholderPassword.Text = "invalid password";
                }
            }
            else
            {
                tbxLogin.Text = "";
                tbPlaceholderLogin.Text = "user not found";
            }
        }
        

            private void txbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txbPassword.Text))
            {
                tbPlaceholderPassword.Visibility = Visibility.Visible;
            }
            else
            {
                tbPlaceholderPassword.Visibility = Visibility.Hidden;
            }
        }

        private void tbxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbxLogin.Text))
            {
                tbPlaceholderLogin.Visibility = Visibility.Visible;
            }
            else
            {
                tbPlaceholderLogin.Visibility = Visibility.Hidden;
            }
        }
    }
}
