﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Factory
{
    internal static class VehicleUtils
    {

        /// <summary>
        /// Deletes a vehicle from database.
        /// </summary>
        /// <param name="vehicle">Vehicle to be deleted from database.</param>
        public static void DeleteVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                using (var context = new DataContext())
                {
                    try
                    {
                        if (vehicle is Car)
                            context.Cars.Remove((Car)vehicle);
                        else if (vehicle is Boat)
                            context.Boats.Remove((Boat)vehicle);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error deleting vehicle from database", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a vehicle from database and adds car's Msrp to AccountTotal.
        /// </summary>
        /// <param name="vehicle">Vehicle to be sold and deleted from database.</param>
        public static void SellVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                BankAccount bankAccount;
                using (var context = new DataContext())
                {
                    try
                    {
                        bankAccount = context.BankAccounts.Find(1);
                        bankAccount.AddFunds(vehicle.Msrp);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error Selling vehicle and adding funds", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                DeleteVehicle(vehicle);
            }
        }
    }
}
