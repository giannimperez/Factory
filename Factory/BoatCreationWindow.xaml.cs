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
    /// Interaction logic for BoatCreationWindow.xaml
    /// </summary>
    public partial class BoatCreationWindow : Window
    {
        public BoatCreationWindow()
        {
            InitializeComponent();
        }

        private void SaveBoatSpecificationButton_Click(object sender, RoutedEventArgs e)
        {
            if (WpfUtils.ChildControlsFilledOut(ControlsGrid))
            {
                // Hides "Please populate all fields" message
                EmptyFieldsErrorLabel.Content = "";

                // Determines whether to overwrite boat or create new boat in db
                if (BoatIdLabel.Content != null)
                    UpdateExistingBoat();
                else
                    CreateNewBoat();
            }
            else
            {
                EmptyFieldsErrorLabel.Content = "Please populate all fields";
            }
        }

        /// <summary>
        /// Creates a new boat in db using values from window, then closes window.
        /// </summary>
        private void CreateNewBoat()
        {
            using (var context = new DataContext())
            {
                try
                {
                    Boat boat = new Boat()
                    {
                        Make = MakeTextBox.Text,
                        Model = ModelTextBox.Text,
                        Msrp = Convert.ToDecimal(MsrpTextBox.Text),
                        BoatType = (BoatType)BoatTypeComboBox.SelectedValue,
                        TotalEngineDisplacement = Convert.ToDouble(DisplacementTextBox.Text),
                        Length = Convert.ToInt32(LengthTextBox.Text)
                    };
                    context.Boats.Add(boat);
                    context.SaveChanges();
                    (Owner as BoatsWindow).Update(); // Updates parent window listview
                    Close();
                }
                catch (FormatException)
                {
                    EmptyFieldsErrorLabel.Content = "One or more values are not in the correct format";
                }
                catch (Exception ex)
                {
                    Window parentWindow = this;
                    MessageBox.Show(parentWindow, ex.ToString(), "Error creating boat", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Updates existing boat in db using values from window, then closes window.
        /// </summary>
        private void UpdateExistingBoat()
        {
            using (var context = new DataContext())
            {
                try
                {
                    var boat = context.Boats.Find(Convert.ToInt32(BoatIdLabel.Content));
                    boat.Make = MakeTextBox.Text;
                    boat.Model = ModelTextBox.Text;
                    boat.Msrp = Convert.ToDecimal(MsrpTextBox.Text);
                    boat.BoatType = (BoatType)BoatTypeComboBox.SelectedValue;
                    boat.TotalEngineDisplacement = Convert.ToDouble(DisplacementTextBox.Text);
                    boat.Length = Convert.ToInt32(LengthTextBox.Text);
                    context.SaveChanges();
                    (Owner as BoatsWindow).Update();
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