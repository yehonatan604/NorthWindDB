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
            OrderDetail ord1 = new() { ProductId = 11, UnitPrice = 95, Quantity = 3 };
            OrderDetail ord2 = new() { ProductId = 56, UnitPrice = 47, Quantity = 6 };
            OrderDetail ord3 = new() { ProductId = 74, UnitPrice = 9120, Quantity = 5 };

            nw.OrderDetails.Add(ord1);
            nw.OrderDetails.Add(ord2);
            nw.OrderDetails.Add(ord3);

            nw.Orders.Add(new Order() { CustomerId = "FRANK",
                EmployeeId = 6,
                OrderDate = DateTime.Now,
                ShipAddress = "7 Piccadilly Rd.",
                ShipCity = "New York",
                ShipCountry = "New York",
                OrderDetails = new List<OrderDetail>() { ord1, ord2, ord3 } 
            });

            nw.SaveChanges();

            var query = from order in nw.Orders select order;

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
            var record = nw.Orders.SingleOrDefault(x => x.OrderId == 11085);
            record.EmployeeId = 5;

            var query = from order in nw.Orders
                        where order.OrderId == 11085
                        select order;

            Dgrid_Update(query.ToList());
        }

        private void Btn9_Click(object sender, RoutedEventArgs e)
        {
            nw.Orders.Remove(nw.Orders.SingleOrDefault(x => x.OrderId == 11084));

            var query = from order in nw.Orders select order;

            Dgrid_Update(query.ToList());
        }
    }
}
