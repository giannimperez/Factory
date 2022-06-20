using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class Car : Vehicle
    {
        public int NumWheels { get; set; } = 4;
        public CarType CarType { get; set; }

        public override string Drive()
        {
            return $"The {Make} {Model} drives down the road.";
        }
    }

    enum CarType
    {
        Unknown,
        Coupe,
        Sedan,
        Hatchback,
        Crossover,
        Van,
        SUV,
        Pickup,
        Bus
    }
}
