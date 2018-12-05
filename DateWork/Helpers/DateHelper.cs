using System;

namespace DateWork.Helpers
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
