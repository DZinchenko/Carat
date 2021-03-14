using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carat
{
    public class Tools
    {
        static private double accuracy = 0.000001;

        static public string MessageBoxErrorTitle() {
            return "Data error";
        }

        static public bool isLessThanZero(double value)
        {
            return (value + accuracy) < 0;
        }

        static public bool isGreaterThanZero(double value)
        {
            return value > (0 + accuracy);
        }

        static public bool isGreaterThan(double value1, double value2)
        {
            return value1 > (value2 + accuracy);
        }
    }
}
