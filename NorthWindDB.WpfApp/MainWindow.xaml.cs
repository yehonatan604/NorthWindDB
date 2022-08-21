using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
using NorthWindDB.Entities.Models;

namespace NorthWindDB.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NorthwindContext nw = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            var query = from region in nw.Regions select region;
            
            Dgrid_Update(query.ToList());
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            var query = from region in nw.Regions
                        select new { region = region.RegionDescription };
            
            Dgrid_Update(query.ToList());
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            var query = from territory in nw.Territories select territory;
            
            Dgrid_Update(query.ToList());
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            var query = from territory in nw.Territories 
                        select new { description = territory.TerritoryDescription};
            
            Dgrid_Update(query.ToList());
        }

        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            var query = from region in nw.Regions
                        orderby region.RegionDescription descending
                        join territory in nw.Territories
                        on region.RegionId equals territory.RegionId 
                        select new { Region = region.RegionDescription, territory = territory.TerritoryDescription  };

            Dgrid_Update(query.ToList());
        }
        private void Dgrid_Update(IEnumerable<Object> query)
        {
            Dgrid.ItemsSource = query;
        }

        private void Btn6_Click(object sender, RoutedEventArgs e)
        {
            int order = nw.Orders.Skip(nw.Orders.Count() - 1).FirstOrDefault().OrderId;

            OrderDetail ord1 = new() { ProductId = 11, OrderId = order, UnitPrice = 95, Quantity = 3, Discount = 0 };
            OrderDetail ord2 = new() { ProductId = 56, OrderId = order, UnitPrice = 47, Quantity = 6, Discount = 0 };
            OrderDetail ord3 = new() { ProductId = 74, OrderId = order, UnitPrice = 120, Quantity = 5, Discount = 0 };

            nw.Orders.Add(new Order() { CustomerId = "FRANK",
                EmployeeId = 6,
                OrderDate = DateTime.Now,
                ShipAddress = "7 Piccadilly Rd.",
                ShipCity = "New York",
                ShipCountry = "New York",
            });

            nw.SaveChanges();

            nw.OrderDetails.Add(ord1);
            nw.OrderDetails.Add(ord2);
            nw.OrderDetails.Add(ord3);

            nw.SaveChanges();

            var query = from ord in nw.OrderDetails 
                        select new { ord.OrderId, ord.ProductId, ord.UnitPrice, ord.Quantity, ord.Discount };

            Dgrid_Update(query.ToList());
        }

        private void Btn7_Click(object sender, RoutedEventArgs e)
        {
            var query = from order in nw.Orders 
                        join detail in nw.OrderDetails
                        on order.OrderId equals detail.OrderId
                        join product in nw.Products
                        on detail.ProductId equals product.ProductId
                        join employee in nw.Employees
                        on order.EmployeeId equals employee.EmployeeId
                        select new { orderId = order.OrderId, product = product.ProductName, employee = employee.FirstName };

            Dgrid_Update(query.ToList());
        }

        private void Btn8_Click(object sender, RoutedEventArgs e)
        {
            int order = nw.Orders.Skip(nw.Orders.Count() - 1).FirstOrDefault().OrderId;
            var record = nw.Orders.SingleOrDefault(x => x.OrderId == order);
            record.EmployeeId = 5;

            var query = from ord in nw.Orders
                        where ord.OrderId == order
                        select ord;

            Dgrid_Update(query.ToList());
        }

        private void Btn9_Click(object sender, RoutedEventArgs e)
        {
            int order = nw.OrderDetails.Skip(nw.OrderDetails.Count() - 1).FirstOrDefault().OrderId;
            nw.OrderDetails.Remove(nw.OrderDetails.Single(x => x.ProductId == 56 && x.OrderId == order));

            var query = from item in nw.OrderDetails select item;

            Dgrid_Update(query.ToList());
        }
    }
}
