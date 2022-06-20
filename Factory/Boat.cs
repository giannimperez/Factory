using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class Boat : Vehicle
    {
        public double Length { get; set; }
        public BoatType BoatType { get; set; }
        

        public override string Drive()
        {
            string drivingAdjective = "";

            switch (BoatType)
            {
                case BoatType.Unknown:
                    drivingAdjective = "floats";
                    break;
                case BoatType.Paddler:
                    drivingAdjective = "paddles";
                    break;
                case BoatType.Sail:
                    drivingAdjective = "sails";
                    break;
                case BoatType.MotorFishing:
                    drivingAdjective = "rushes";
                    break;
                case BoatType.MotorSpeed:
                    drivingAdjective = "speeds";
                    break;
                case BoatType.Yacht:
                    drivingAdjective = "cruises";
                    break;
                default:
                    drivingAdjective = "floats";
                    break;
            }
            return $"The {Make} {Model} {drivingAdjective} through the water.";
        }
    }

    enum BoatType
    {
        Unknown,
        Paddler,
        Sail,
        MotorFishing,
        MotorSpeed,
        Yacht
    }
}
