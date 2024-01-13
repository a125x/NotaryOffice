using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for NotaryOfficeMain.xaml
    /// </summary>

    public class Deals
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public List<int> ServiceIds { get; set; }
        public decimal Sum { get; set; }
        public decimal Commissions { get; set; }
        public string Description { get; set; }
        public List<int> Discounts { get; set; }
    }

    public partial class NotaryOfficeMain : Page
    {
        private XDocument dealsXml;
        private List<Deals> dealsList;
        private string dealsPath = "..\\XMLDB\\deals.xml";

        public NotaryOfficeMain()
        {
            InitializeComponent();
            LoadDeals();
        }

        private void LoadDeals()
        {
            dealsList = new List<Deals>();
            dealsXml = XDocument.Load(dealsPath);

            foreach (XElement dealElement in dealsXml.Descendants("Deal"))
            {
                Deals deal = new Deals
                {
                    Id = Convert.ToInt32(dealElement.Element("id").Value),
                    ClientId = Convert.ToInt32(dealElement.Element("client_id").Value),
                    Sum = Convert.ToDecimal(dealElement.Element("sum").Value),
                    Commissions = Convert.ToDecimal(dealElement.Element("commissions").Value),
                    Description = dealElement.Element("description").Value,
                    ServiceIds = dealElement.Element("services").Elements("service_id").Select(s => Convert.ToInt32(s.Value)).ToList(),
                    Discounts = dealElement.Element("discounts").Elements("discount").Select(d => Convert.ToInt32(d.Value)).ToList()
                };
                dealsList.Add(deal);
            }

            DealsDataGrid.ItemsSource = dealsList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txbId.Text != "" && 
                txbClientId.Text != "" && 
                txbServiceIds.Text != "" && 
                txbSum.Text != "" && 
                txbCommissions.Text != "" &&
                txbDiscount.Text != "")
            {
                try
                {
                    Deals newDeal = new Deals
                    {
                        Id = Int32.Parse(txbId.Text),
                        ClientId = Int32.Parse(txbClientId.Text),
                        ServiceIds = txbServiceIds.Text.Split(' ')?.Select(Int32.Parse)?.ToList(),
                        Sum = Int32.Parse(txbSum.Text),
                        Commissions = Int32.Parse(txbCommissions.Text),
                        Description = txbDescription.Text,
                        Discounts = txbDiscount.Text.Split(' ')?.Select(Int32.Parse)?.ToList()
                    };
                    dealsList.Add(newDeal);

                    AddDealToXml(newDeal);
                    RefreshDataGrid();

                    MessageBox.Show("deal with id " + txbId.Text + " has been added successfully");
                }
                catch
                {
                    MessageBox.Show("incorrect data, please try again");
                }
            }
            else
                MessageBox.Show("incorrect data, please try again");
        }

        private void AddDealToXml(Deals deal)
        {
            dealsXml.Root.Add(new XElement("Deal",
                new XElement("id", deal.Id),
                new XElement("client_id", deal.ClientId),
                new XElement("services", deal.ServiceIds.Select(id => new XElement("service_id", id))),
                new XElement("sum", deal.Sum),
                new XElement("commissions", deal.Commissions),
                new XElement("description", deal.Description),
                new XElement("discounts", deal.Discounts.Select(discount => new XElement("discount", discount)))
            ));
            dealsXml.Save(dealsPath);
        }

        private void RefreshDataGrid()
        {
            // Refresh the DataGrid to display the updated information
            DealsDataGrid.ItemsSource = null;
            DealsDataGrid.ItemsSource = dealsList;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int dealIdToDelete = Int32.Parse(txbId.Text);

                XDocument dealsXml = XDocument.Load(dealsPath);
                XElement dealToDelete = dealsXml.Root.Elements("Deal").FirstOrDefault(d => (int)d.Element("id") == dealIdToDelete);

                if (dealToDelete != null)
                {
                    dealToDelete.Remove();
                    dealsXml.Save(dealsPath);
                    LoadDeals();
                    RefreshDataGrid();

                    MessageBox.Show("deal with id " + dealIdToDelete + " has been successfully deleted");
                }
                else
                {
                    MessageBox.Show("deal with id " + dealIdToDelete + " was not found");
                }
            }
            catch
            {
                MessageBox.Show("deal with id " + txbId.Text + " was not found");
            }
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            var newPage = new Clients();
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
