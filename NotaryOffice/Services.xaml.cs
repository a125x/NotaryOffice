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
using static NotaryOffice.Clients;
using System.Xml.Linq;

namespace NotaryOffice
{
    /// <summary>
    /// Interaction logic for Services.xaml
    /// </summary>
    public partial class Services : Page
    {
        public class ServicesData
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public Services()
        {
            InitializeComponent();
            LoadServices();
        }

        private XDocument servicesXml;
        private List<ServicesData> servicesList;
        private string servicesPath = "..\\XMLDB\\services.xml";

        private void LoadServices()
        {
            servicesList = new List<ServicesData>();
            servicesXml = XDocument.Load(servicesPath);

            foreach (XElement serviceElement in servicesXml.Descendants("service"))
            {
                ServicesData data = new ServicesData
                {
                    Id = Convert.ToInt32(serviceElement.Element("id").Value),
                    Title = serviceElement.Element("title").Value,
                    Description = serviceElement.Element("description").Value
                };
                servicesList.Add(data);
            }

            ServicesDataGrid.ItemsSource = servicesList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txbId.Text != "" &&
                txbTitle.Text != "" &&
                txbDescription.Text != "")
            {
                try
                {
                    ServicesData newData = new ServicesData
                    {
                        Id = Int32.Parse(txbId.Text),
                        Title = txbTitle.Text,
                        Description = txbDescription.Text
                    };
                    servicesList.Add(newData);

                    AddDataToXml(newData);
                    RefreshDataGrid();

                    MessageBox.Show("service with id " + txbId.Text + " has been added successfully");
                }
                catch
                {
                    MessageBox.Show("incorrect data, please try again");
                }
            }
            else
                MessageBox.Show("incorrect data, please try again");
        }

        private void AddDataToXml(ServicesData data)
        {
            servicesXml.Root.Add(new XElement("service",
                new XElement("id", data.Id),
                new XElement("title", data.Title),
                new XElement("description", data.Description)
            ));
            servicesXml.Save(servicesPath);
        }

        private void RefreshDataGrid()
        {
            // Refresh the DataGrid to display the updated information
            ServicesDataGrid.ItemsSource = null;
            ServicesDataGrid.ItemsSource = servicesList;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int serviceIdToDelete = Int32.Parse(txbId.Text);

                XDocument servicesXml = XDocument.Load(servicesPath);
                XElement servicesToDelete = servicesXml.Root.Elements("service").FirstOrDefault(d => (int)d.Element("id") == serviceIdToDelete);

                if (servicesToDelete != null)
                {
                    servicesToDelete.Remove();
                    servicesXml.Save(servicesPath);
                    LoadServices();
                    RefreshDataGrid();

                    MessageBox.Show("service with id " + serviceIdToDelete + " has been successfully deleted");
                }
                else
                {
                    MessageBox.Show("service with id " + serviceIdToDelete + " was not found");
                }
            }
            catch
            {
                MessageBox.Show("service with id " + txbId.Text + " was not found");
            }
        }

        private void btnDeals_Click(object sender, RoutedEventArgs e)
        {
            var newPage = new NotaryOfficeMain();
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Content = newPage;
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            var newPage = new Clients();
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Content = newPage;
        }
    }
}
