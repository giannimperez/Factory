using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class BankAccount
    {
        [Key]
        public int Id { get; set; }
        public decimal AccountTotal { get; set; } = 0.00m;
        public int DepositCount { get; set; } = 0;

        public void AddFunds(decimal amount)
        {
            AccountTotal += amount;
            DepositCount++;
        }
    }
}
