using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class CarsWindow : Window
    {
        DataTable dt = new DataTable();

        public CarsWindow()
        {
            InitializeComponent();
            Startup();
            Update();
        }

        /// <summary>
        /// Executes necessary startup actions.
        /// </summary>
        private void Startup()
        {
            // Creates listview columns
            dt.Columns.Add("Id");
            dt.Columns.Add("Make");
            dt.Columns.Add("Model");
            dt.Columns.Add("Msrp");
            dt.Columns.Add("CarType");
            dt.Columns.Add("TotalEngineDisplacement");
            dt.Columns.Add("ManufactureDate");
            dt.Columns.Add("NumWheels");
        }

        /// <summary>
        /// Updates the data of various components.
        /// </summary>
        public void Update()
        {
            // dt must be cleared to update CarListView
            dt.Clear();
            using (var context = new DataContext())
            {
                var myCars = context.Cars.ToList();

                // Populate car listview
                for (int i = 0; i < myCars.Count(); i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = myCars[i].Id;
                    dr["Make"] = myCars[i].Make;
                    dr["Model"] = myCars[i].Model;
                    dr["Msrp"] = myCars[i].Msrp;
                    dr["CarType"] = myCars[i].CarType.ToString();
                    dr["TotalEngineDisplacement"] = myCars[i].TotalEngineDisplacement;
                    dr["ManufactureDate"] = myCars[i].ManufactureDate;
                    dr["NumWheels"] = myCars[i].NumWheels;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    CarListView.ItemsSource = dt.DefaultView;
                }

                // Populate total profit label
                var bankAccount = context.BankAccounts.Find(1);
                if (bankAccount == null)
                {
                    bankAccount = new BankAccount();
                    context.BankAccounts.Add(bankAccount);
                }
                ProfitLabel.Content = $"Total Profit: ${bankAccount.AccountTotal}";
                context.SaveChanges();
            }
        }

        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            // Opens CarCreationWindow with fields populated for selected car using data from db
            var selectedItem = CarListView.SelectedItem as DataRowView;
            var car = GetCarFromSelection(selectedItem);
            if (car != null)
            {
                CarCreationWindow carCreationWindow = new CarCreationWindow();
                carCreationWindow.Owner = this;
                carCreationWindow.MakeTextBox.Text = car.Make;
                carCreationWindow.ModelTextBox.Text = car.Model;
                carCreationWindow.MsrpTextBox.Text = car.Msrp.ToString();
                carCreationWindow.CarTypeComboBox.SelectedIndex = (int)car.CarType;
                carCreationWindow.DisplacementTextBox.Text = car.TotalEngineDisplacement.ToString();
                carCreationWindow.NumWheelsTextBox.Text = car.NumWheels.ToString();
                carCreationWindow.EditOrCreateLabel.Content = "Editing car:";
                carCreationWindow.CarIdLabel.Content = car.Id.ToString();
                carCreationWindow.SaveCarSpecificationButton.Content = "Save Changes";
                carCreationWindow.ShowDialog();
            }
        }

        private void DeleteCarButton_Click(object sender, RoutedEventArgs e)
        {
            // Deletes selected car
            var selectedItem = CarListView.SelectedItem as DataRowView;
            DeleteSelectedCar(GetCarFromSelection(selectedItem));
        }

        private void CreateCarButton_Click(object sender, RoutedEventArgs e)
        {
            // Opens CarCreationWindow with blank fields
            CarCreationWindow carCreationWindow = new CarCreationWindow();
            carCreationWindow.Owner = this;

            carCreationWindow.EditOrCreateLabel.Content = "Creating car";
            carCreationWindow.SaveCarSpecificationButton.Content = "Create";
            carCreationWindow.ShowDialog();
        }

        private void DriveButton_Click(object sender, RoutedEventArgs e)
        {
            // Displays driving message
            var selectedItem = CarListView.SelectedItem as DataRowView;
            var car = GetCarFromSelection(selectedItem);
            if (car != null)
                CarTextBox.Text = car.Drive();
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            // Deletes car and adds Msrp to AccountTotal
            var selectedItem = CarListView.SelectedItem as DataRowView;
            SellSelectedCar(GetCarFromSelection(selectedItem));
        }

        /// <summary>
        /// Gets a car from database using Id acquired from a listview selection.
        /// </summary>
        /// <param name="selectedItem">Listview item with Id that correlates to a car in database.</param>
        /// <returns>Car acquired from db.</returns>
        private Car GetCarFromSelection(DataRowView selectedItem)
        {
            if (selectedItem != null)
            {
                var selectedItemId = Convert.ToInt32(selectedItem["Id"]);
                using (var context = new DataContext())
                {
                    return context.Cars.Find(selectedItemId);
                }
            }
            else
            {
                CarTextBox.Text = "Please select a car";
                return null;
            }
        }

        /// <summary>
        /// Deletes a car from database and updates CarTextBox message.
        /// </summary>
        /// <param name="car">Car to be deleted from database.</param>
        private void DeleteSelectedCar(Car car)
        {
            if (car != null)
            {
                using (var context = new DataContext())
                {
                    context.Cars.Remove(car);
                    context.SaveChanges();
                }
                CarTextBox.Text = $"Deleted car with Id: {car.Id}";
                Update();
            }
        }

        /// <summary>
        /// Deletes a car from database, updates CarTextBox message and adds car's Msrp to AccountTotal.
        /// </summary>
        /// <param name="car">Car to be sold and deleted from database.</param>
        private void SellSelectedCar(Car car)
        {
            if(car != null)
            {
                BankAccount bankAccount;
                using (var context = new DataContext())
                {
                    bankAccount = context.BankAccounts.Find(1);
                    bankAccount.AddFunds(car.Msrp);
                    context.SaveChanges();
                }
                CarTextBox.Text = $"{car.Make} {car.Model} sold for ${car.Msrp}";
                ProfitLabel.Content = $"Total Profit: ${bankAccount.AccountTotal}";
                DeleteSelectedCar(car);
            }
        }
    }
}