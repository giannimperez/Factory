using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Factory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Car> myCars;

        DataTable dt = new DataTable();
        DataRow dr;


        public MainWindow()
        {
            InitializeComponent();
            Startup();
            Update();
        }
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

        private void Update()
        {
            dt.Clear();

            using (var context = new DataContext())
            {
                myCars = context.Cars.ToList();

                // Populate car listview
                for (int i = 0; i < myCars.Count(); i++)
                {
                    dr = dt.NewRow();
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
            }
        }

        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Load Edit/Create page but populate with selected car info
            var selectedItem = CarListView.SelectedItem as DataRowView;
            var car = GetCarFromSelection(selectedItem);
            if(car != null)
            {
                CarCreationWindow window = new CarCreationWindow();
                window.Owner = this;
                window.ShowDialog();
            }
        }

        private void DeleteCarButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = CarListView.SelectedItem as DataRowView;
            DeleteSelectedCar(GetCarFromSelection(selectedItem));
        }

        private void CreateCarButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Make "Create car" Pop up

            // Placeholder
            Car newCar = new Car
            {
                Make = "Nissan",
                Model = "Frontier",
                CarType = CarType.Pickup,
                Msrp = 25000.00m,
                TotalEngineDisplacement = 3.0
            };

            using (var context = new DataContext())
            {
                context.Cars.Add(newCar);
                context.SaveChanges();
                this.DataContext = null;
                Update();
            }
            CarTextBox.Text = "Building new car...";
        }

        private void DriveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = CarListView.SelectedItem as DataRowView;

            var car = GetCarFromSelection(selectedItem);
            if(car != null)
                CarTextBox.Text = car.Drive();
  
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = CarListView.SelectedItem as DataRowView;

            var car = GetCarFromSelection(selectedItem);
            if(car != null)
            {
                DeleteSelectedCar(car);
                //BankAccount.AddFunds(car.Msrp);
                CarTextBox.Text = $"{car.Make} {car.Model} sold for ${car.Msrp}";
                //ProfitLabel.Content = $"Total Profit: ${BankAccount.GetFunds()}";
            }
                
        }

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
                CarTextBox.Text = "Please select item";
                return null;
            }
            
        }

        private void DeleteSelectedCar(Car car)
        {
            if (car != null)
            {   
                // Remove item from list view
                CarListView.ItemsSource = null;
                CarListView.Items.Remove(CarListView.SelectedItem);

                // Delete car from db
                using (var context = new DataContext())
                {
                    context.Cars.Remove(car);
                    context.SaveChanges();
                }
                CarTextBox.Text = $"Deleted car with Id: {car.Id}";
                Update();
            }
        }
    }
}
