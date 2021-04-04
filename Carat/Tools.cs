using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

        static public bool isEqual(double value1, double value2)
        {
            return Math.Abs(value1 - value2) < accuracy;
        }

        static public string GetSubjectNameFromCell(string cellText)
        {
            if (cellText == null)
            {
                throw new Exception("Invalid cell text!");
            }

            return cellText.Substring(0, cellText.IndexOf(';'));
        }

        static public uint GetCurriculumItemCourse(string cellText)
        {
            if (cellText == null)
            {
                throw new Exception("Invalid cell text!");
            }

            var reg = new Regex(@";\W\dк\.;\W");
            var courseInString = reg.Match(cellText).Groups[0].Value.ToString()[2];

            return Convert.ToUInt32(courseInString.ToString());
        }

        static public string GetEducLevel(string cellText)
        {
            string result = "";

            if (cellText == "Бакалаври")
                result = "Бакалаври";

            if (cellText == "Магістри")
                result = "Магістр";

            if (cellText == "Доктори філософії")
                result = "PhD";

            return result;
        }
    }
}
