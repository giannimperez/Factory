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
            //(ComboBoxItem)cboType.SelectedItem
            Car car = new Car()
            {
                //CarType = CarType.Unknown,

/*                Make = MakeTextBox.Text,
                Model = ModelTextBox.Text,
                Msrp = (decimal)MsrpTextBox.Text,
                CarType = (CarType)CarTypeComboBox.SelectedItem,
                TotalEngineDisplacement = (double)DisplacementTextBox.Text,
                NumWheels = (int)NumWheelsTextBox.Text*/



            };

            
        }
    }
}
