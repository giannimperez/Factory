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
    /// <summary>
    /// Interaction logic for BoatsWindow.xaml
    /// </summary>
    public partial class BoatsWindow : Window
    {
        DataTable dt = new DataTable();

        public BoatsWindow()
        {
            InitializeComponent();
            Startup();
            Update();
        }

        /// <summary>
        /// Executes necessary startup actions. 
        /// Should only be called once.
        /// </summary>
        private void Startup()
        {
            // Creates listview columns
            dt.Columns.Add("Id");
            dt.Columns.Add("Make");
            dt.Columns.Add("Model");
            dt.Columns.Add("Msrp");
            dt.Columns.Add("BoatType");
            dt.Columns.Add("TotalEngineDisplacement");
            dt.Columns.Add("ManufactureDate");
            dt.Columns.Add("Length");
        }

        /// <summary>
        /// Updates the data of various components. 
        /// Can be called as needed.
        /// </summary>
        public void Update()
        {
            // dt must be cleared to update BoatListView
            dt.Clear();
            using (var context = new DataContext())
            {
                var myBoats = context.Boats.ToList();

                // Populate boat listview
                for (int i = 0; i < myBoats.Count(); i++)
                {
                    try
                    {
                        DataRow dr = dt.NewRow();
                        dr["Id"] = myBoats[i].Id;
                        dr["Make"] = myBoats[i].Make;
                        dr["Model"] = myBoats[i].Model;
                        dr["Msrp"] = myBoats[i].Msrp;
                        dr["BoatType"] = myBoats[i].BoatType.ToString();
                        dr["TotalEngineDisplacement"] = myBoats[i].TotalEngineDisplacement;
                        dr["ManufactureDate"] = myBoats[i].ManufactureDate;
                        dr["Length"] = myBoats[i].Length;
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                        BoatListView.ItemsSource = dt.DefaultView;
                    }
                    catch (Exception ex)
                    {
                        Window parentWindow = this;
                        MessageBox.Show(parentWindow, ex.ToString(), "Error populating listview", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Populate total profit label
                try
                {
                    var bankAccount = context.BankAccounts.Find(1);
                    if (bankAccount == null)
                    {
                        bankAccount = new BankAccount();
                        context.BankAccounts.Add(bankAccount);
                    }
                    ProfitLabel.Content = $"Total Profit: ${bankAccount.AccountTotal}";
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Window parentWindow = this;
                    MessageBox.Show(parentWindow, ex.ToString(), "Error populating total profit label", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Opens BoatCreationWindow with fields populated for selected boat using data from db
        private void EditBoatButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = BoatListView.SelectedItem as DataRowView;
            var boat = GetBoatFromSelection(selectedItem);
            if (boat != null)
            {
                try
                {
                    BoatCreationWindow boatCreationWindow = new BoatCreationWindow();
                    boatCreationWindow.Owner = this;
                    boatCreationWindow.MakeTextBox.Text = boat.Make;
                    boatCreationWindow.ModelTextBox.Text = boat.Model;
                    boatCreationWindow.MsrpTextBox.Text = boat.Msrp.ToString();
                    boatCreationWindow.BoatTypeComboBox.SelectedIndex = (int)boat.BoatType;
                    boatCreationWindow.DisplacementTextBox.Text = boat.TotalEngineDisplacement.ToString();
                    boatCreationWindow.LengthTextBox.Text = boat.Length.ToString();
                    boatCreationWindow.EditOrCreateLabel.Content = "Editing boat:";
                    boatCreationWindow.BoatIdLabel.Content = boat.Id.ToString();
                    boatCreationWindow.SaveBoatSpecificationButton.Content = "Save Changes";
                    boatCreationWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    Window parentWindow = this;
                    MessageBox.Show(parentWindow, ex.ToString(), "Error opening edit window", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Deletes selected boat
        private void DeleteBoatButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = BoatListView.SelectedItem as DataRowView;
                var boat = GetBoatFromSelection(selectedItem);
                VehicleUtils.DeleteVehicle(boat);
                if (boat != null)
                    BoatTextBox.Text = $"Deleted vehicle with Id: {boat.Id}";
            }
            catch (Exception ex)
            {
                Window parentWindow = this;
                MessageBox.Show(parentWindow, ex.ToString(), "Error deleting vehicle", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Update();
        }

        // Opens BoatCreationWindow with blank fields
        private void CreateBoatButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BoatCreationWindow boatCreationWindow = new BoatCreationWindow();
                boatCreationWindow.Owner = this;
                boatCreationWindow.EditOrCreateLabel.Content = "Creating boat";
                boatCreationWindow.SaveBoatSpecificationButton.Content = "Create";
                boatCreationWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Window parentWindow = this;
                MessageBox.Show(parentWindow, ex.ToString(), "Error opening create window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Displays driving message
        private void DriveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = BoatListView.SelectedItem as DataRowView;
                var boat = GetBoatFromSelection(selectedItem);
                if (boat != null)
                    BoatTextBox.Text = boat.Drive();
            }
            catch (Exception ex)
            {
                Window parentWindow = this;
                MessageBox.Show(parentWindow, ex.ToString(), "Error driving vehicle", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Deletes boat and adds Msrp to AccountTotal
        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = BoatListView.SelectedItem as DataRowView;
                var boat = GetBoatFromSelection(selectedItem);
                VehicleUtils.SellVehicle(boat);
                if (boat != null)
                    BoatTextBox.Text = $"{boat.Make} {boat.Model} sold for ${boat.Msrp}";
            }
            catch (Exception ex)
            {
                Window parentWindow = this;
                MessageBox.Show(parentWindow, ex.ToString(), "Error selling vehicle", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Update();
        }

        /// <summary>
        /// Gets a boat from database using Id acquired from a listview selection.
        /// </summary>
        /// <param name="selectedItem">Listview item with Id that correlates to a boat in database.</param>
        /// <returns>Boat acquired from db.</returns>
        private Boat GetBoatFromSelection(DataRowView selectedItem)
        {
            if (selectedItem != null)
            {
                try
                {
                    var selectedItemId = Convert.ToInt32(selectedItem["Id"]);
                    using (var context = new DataContext())
                    {
                        return context.Boats.Find(selectedItemId);
                    }
                }
                catch (Exception ex)
                {
                    Window parentWindow = this;
                    MessageBox.Show(parentWindow, ex.ToString(), "Error retrieving selected boat from database", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            else
            {
                BoatTextBox.Text = "No boat selected";
                return null;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Left = this.Left;
                dashboard.Top = this.Top;

                this.Close();
                dashboard.Show();
            }
            catch (Exception ex)
            {
                Window parentWindow = this;
                MessageBox.Show(parentWindow, ex.ToString(), "Error opening dashboard window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
