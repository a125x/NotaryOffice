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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace NotaryOffice
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : Page
    {
        public class ClientsData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Occupation { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
        }

        public Clients()
        {
            InitializeComponent();
            LoadClients();
        }

        private XDocument clientsXml;
        private List<ClientsData> clientsList;
        private string clientsPath = "..\\XMLDB\\clients.xml";

        private void LoadClients()
        {
            clientsList = new List<ClientsData>();
            clientsXml = XDocument.Load(clientsPath);

            foreach (XElement clientElement in clientsXml.Descendants("client"))
            {
                ClientsData data = new ClientsData
                {
                    Id = Convert.ToInt32(clientElement.Element("id").Value),
                    Name = clientElement.Element("name").Value,
                    Occupation = clientElement.Element("occupation").Value,
                    Address = clientElement.Element("address").Value,
                    PhoneNumber = clientElement.Element("phoneNumber").Value
                };
                clientsList.Add(data);
            }

            ClientsDataGrid.ItemsSource = clientsList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txbId.Text != "" &&
                txbName.Text != "" &&
                txbOccupation.Text != "" &&
                txbAddress.Text != "" &&
                txbPhoneNumber.Text != "")
            {
                try
                {
                    ClientsData newData = new ClientsData
                    {
                        Id = Int32.Parse(txbId.Text),
                        Name = txbName.Text,
                        Occupation = txbOccupation.Text,
                        Address = txbAddress.Text,
                        PhoneNumber = txbPhoneNumber.Text,
                    };
                    clientsList.Add(newData);

                    AddDataToXml(newData);
                    RefreshDataGrid();

                    MessageBox.Show("client with id " + txbId.Text + " has been added successfully");
                }
                catch
                {
                    MessageBox.Show("incorrect data, please try again");
                }
            }
            else
                MessageBox.Show("incorrect data, please try again");
        }

        private void AddDataToXml(ClientsData data)
        {
            clientsXml.Root.Add(new XElement("client",
                new XElement("id", data.Id),
                new XElement("name", data.Name),
                new XElement("occupation", data.Occupation),
                new XElement("address", data.Address),
                new XElement("phoneNumber", data.PhoneNumber)
            ));
            clientsXml.Save(clientsPath);
        }
        
        private void RefreshDataGrid()
        {
            // Refresh the DataGrid to display the updated information
            ClientsDataGrid.ItemsSource = null;
            ClientsDataGrid.ItemsSource = clientsList;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int clientIdToDelete = Int32.Parse(txbId.Text);

                XDocument clientsXml = XDocument.Load(clientsPath);
                XElement clientsToDelete = clientsXml.Root.Elements("client").FirstOrDefault(d => (int)d.Element("id") == clientIdToDelete);

                if (clientsToDelete != null)
                {
                    clientsToDelete.Remove();
                    clientsXml.Save(clientsPath);
                    LoadClients();
                    RefreshDataGrid();

                    MessageBox.Show("client with id " + clientIdToDelete + " has been successfully deleted");
                }
                else
                {
                    MessageBox.Show("client with id " + clientIdToDelete + " was not found");
                }
            }
            catch
            {
                MessageBox.Show("client with id " + txbId.Text + " was not found");
            }
        }

        private void btnDeals_Click(object sender, RoutedEventArgs e)
        {
            var newPage = new NotaryOfficeMain();
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Content = newPage;
        }

        private void btnServices_Click(object sender, RoutedEventArgs e)
        {
            var newPage = new Services();
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Content = newPage;
        }
    }
}