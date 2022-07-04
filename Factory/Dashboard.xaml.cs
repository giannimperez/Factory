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

namespace Factory
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            Startup();
        }

        public void Startup()
        {
            using (var context = new DataContext())
            {
                // Populate total data
                var bankAccount = context.BankAccounts.Find(1);
                ProfitLabel.Content = $"${bankAccount.AccountTotal}";
                UnitsLabel.Content = bankAccount.DepositCount;

                // Populate cars data
                var myCars = context.Cars;
                var carInvValue = 0.0m;
                foreach(Car car in myCars)
                {
                    carInvValue += car.Msrp;
                }
                CarsInvValueLabel.Content = $"${carInvValue}";
                CarsInvUnitsLabel.Content = myCars.Count();

                // Populate boats data
                var myBoats = context.Boats;
                var boatInvValue = 0.0m;
                foreach(Boat boat in myBoats)
                {
                    boatInvValue += boat.Msrp;
                }
                BoatsInvValueLabel.Content = $"${boatInvValue}";
                BoatsInvUnitsLabel.Content = myBoats.Count();
            }
            
        }

        private void CarsManageButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                CarsWindow carsWindow = new CarsWindow();
                carsWindow.Left = this.Left;
                carsWindow.Top = this.Top;

                this.Close();
                carsWindow.Show();
            }
            catch (Exception ex)
            {
                Window parentWindow = this;
                MessageBox.Show(parentWindow, ex.ToString(), "Error opening cars window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BoatsManageButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                BoatsWindow boatsWindow = new BoatsWindow();
                boatsWindow.Left = this.Left;
                boatsWindow.Top = this.Top;

                this.Close();
                boatsWindow.Show();
            }
            catch (Exception ex)
            {
                Window parentWindow = this;
                MessageBox.Show(parentWindow, ex.ToString(), "Error opening boats window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
}
