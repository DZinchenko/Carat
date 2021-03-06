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

        static public bool isLessThanZero(double value)
        {
            return (value + accuracy) < 0;
        }
    }
}
