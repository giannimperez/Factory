using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Factory
{
    public static class WpfUtils
    {
        /// <summary>
        /// Checks if all child controls of a Grid are populated with values, 
        /// given they are of type TextBox or ComboBox.
        /// </summary>
        /// <param name="parentGrid">Grid which contains child controls to be checked.</param>
        /// <returns>True if all child TextBoxes and/or ComboBoxes are populated with values.</returns>
            public static bool ChildControlsFilledOut(Grid parentGrid)
            {
                foreach (var control in parentGrid.Children.OfType<Control>())
                {
                    if (control.GetType() == typeof(TextBox))
                    {
                        var textBox = (TextBox)control;
                        if (string.IsNullOrEmpty(textBox.Text))
                        {
                            return false;
                        }
                    }

                    if (control.GetType() == typeof(ComboBox))
                    {
                        var comboBox = (ComboBox)control;
                        if (comboBox.SelectedIndex < 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
    }
}
