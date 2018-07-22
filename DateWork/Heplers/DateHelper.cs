using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateWork.Heplers
{
    public static class DateHelper
    {
        public static int GetMonthDayCount(int year, int month)
        {
            DateTime dt = DateTime.Now;
            var count = DateTime.DaysInMonth(year, month);
            return count;
        }


    }
}
