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
        public List<decimal> Discounts { get; set; }
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
                    Discounts = dealElement.Element("discounts").Elements("discount").Select(d => Convert.ToDecimal(d.Value)).ToList()
                };
                dealsList.Add(deal);
            }


            DealsDataGrid.ItemsSource = dealsList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddDealDialog dialog = new AddDealDialog();
            if (dialog.ShowDialog() == true)
            {
                Deals newDeal = new Deals
                {
                    Id = dialog.Id,
                    ClientId = dialog.ClientId,
                    ServiceIds = dialog.ServiceIds,
                    Sum = dialog.Sum,
                    Commissions = dialog.Commissions,
                    Description = dialog.Description,
                    Discounts = dialog.Discounts
                };
                dealsList.Add(newDeal);

                AddDealToXml(newDeal);
                RefreshDataGrid();
            }
        }

        private void AddDealToXml(Deals deal)
        {
            dealsXml.Root.Add(new XElement("Deal",
                new XElement("id", deal.Id),
                new XElement("client_id", deal.ClientId),
                new XElement("sum", deal.Sum),
                new XElement("comissions", deal.Commissions),
                new XElement("description", deal.Description),
                new XElement("services", deal.ServiceIds.Select(id => new XElement("service_id", id))),
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
