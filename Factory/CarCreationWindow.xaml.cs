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
    /// Interaction logic for CarCreationWindow.xaml
    /// </summary>
    public partial class CarCreationWindow : Window
    {
        public CarCreationWindow()
        {
            InitializeComponent();
        }

        private void SaveCarSpecificationButton_Click(object sender, RoutedEventArgs e)
        {
            if (WpfUtils.ChildControlsFilledOut(ControlsGrid))
            {
                // Hides "Please populate all fields" message
                EmptyFieldsErrorLabel.Visibility = Visibility.Hidden;

                // Determines whether to overwrite car or create new car in db
                if (CarIdLabel.Content != null)
                    UpdateExistingCar();
                else
                    CreateNewCar();
            }
            else
            {
                // Displays "Please populate all fields" message
                EmptyFieldsErrorLabel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Creates a new car in db using values from window, then closes window.
        /// </summary>
        private void CreateNewCar()
        {
            using (var context = new DataContext())
            {
                try
                {
                    Car car = new Car()
                    {
                        Make = MakeTextBox.Text,
                        Model = ModelTextBox.Text,
                        Msrp = Convert.ToDecimal(MsrpTextBox.Text),
                        CarType = (CarType)CarTypeComboBox.SelectedValue,
                        TotalEngineDisplacement = Convert.ToDouble(DisplacementTextBox.Text),
                        NumWheels = Convert.ToInt32(NumWheelsTextBox.Text)
                    };
                    context.Cars.Add(car);
                    context.SaveChanges();
                    (Owner as CarsWindow).Update(); // Updates parent window listview
                    Close();
                }
                catch (Exception ex)
                {
                    Window parentWindow = this;
                    MessageBox.Show(parentWindow, ex.ToString(), "Error creating car", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Updates existing car in db using values from window, then closes window.
        /// </summary>
        private void UpdateExistingCar()
        {
            using (var context = new DataContext())
            {
                try
                {
                    var car = context.Cars.Find(Convert.ToInt32(CarIdLabel.Content));
                    car.Make = MakeTextBox.Text;
                    car.Model = ModelTextBox.Text;
                    car.Msrp = Convert.ToDecimal(MsrpTextBox.Text);
                    car.CarType = (CarType)CarTypeComboBox.SelectedValue;
                    car.TotalEngineDisplacement = Convert.ToDouble(DisplacementTextBox.Text);
                    car.NumWheels = Convert.ToInt32(NumWheelsTextBox.Text);
                    context.SaveChanges();
                    (Owner as CarsWindow).Update();
                    Close();
                }
                catch (Exception ex)
                {
                    Window parentWindow = this;
                    MessageBox.Show(parentWindow, "Please populate all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}