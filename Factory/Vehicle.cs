using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal abstract class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public DateTime ManufactureDate { get; set; } = DateTime.UtcNow;
        public decimal Msrp { get; set; }
        public double TotalEngineDisplacement { get; set; }

        public abstract string Drive();

    }
}
