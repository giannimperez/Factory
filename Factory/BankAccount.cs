using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal static class BankAccount
    {
        private static decimal _accountTotal = 0.00m;

        public static decimal GetFunds()
        {
            return _accountTotal;
        }

        public static void AddFunds(decimal amount)
        {
            _accountTotal += amount;
        }
    }
}
