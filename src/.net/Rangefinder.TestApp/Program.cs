using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Harvbot.Rangefinder.Driver;

namespace Harvbot.Rangefinder.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var laser = new RangefinderDriver("COM3"))
            {
                laser.Measurement += Laser_Measurement;
                laser.Open();

                Console.WriteLine("Press any key to finish measurement and turn laser off");
                laser.StartContinuousMeasurement();

                Console.ReadKey();
            }
        }

        private static void Laser_Measurement(object sender, RangefinderMeasurementArgs e)
        {
            if (e.Success)
            {
                Console.WriteLine(string.Format("Measurement: {0}m", e.Range));
            }
            else
            {
                Console.WriteLine("Measurement: error");
            }
        }
    }
}
