using DateWork.Helpers;
using DateWork.Heplers;
using DateWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateWork.Models
{
    public class AppModel : BaseViewModel
    {
        #region 属性 Current
        private static AppModel _Current = null;
        public static AppModel Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new AppModel();
                }
                return _Current;
            }
        }

        public static void Clear()
        {
            _Current = null;
        }
        #endregion

        public AppModel()
        {
            Init();
        }

        private void Init()
        {
            var now = DateTime.Now;
            Year = now.Year;
            Month = now.Month;
            RefreshDays();
        }

        public void RefreshDays()
        {
            try
            {
                var days = new List<DateTime>();
                var day_first = new DateTime(Year, Month, 1);
                var week = day_first.DayOfWeek;
                var count = 0;
                switch (week)
                {
                    case DayOfWeek.Monday:
                        break;
                    case DayOfWeek.Tuesday:
                        days.Add(day_first.AddDays(-1));
                        count += 1;
                        break;
                    case DayOfWeek.Wednesday:
                        days.Add(day_first.AddDays(-2));
                        days.Add(day_first.AddDays(-1));
                        count += 2;
                        break;
                    case DayOfWeek.Thursday:
                        days.Add(day_first.AddDays(-3));
                        days.Add(day_first.AddDays(-2));
                        days.Add(day_first.AddDays(-1));
                        count += 3;
                        break;
                    case DayOfWeek.Friday:
                        days.Add(day_first.AddDays(-4));
                        days.Add(day_first.AddDays(-3));
                        days.Add(day_first.AddDays(-2));
                        days.Add(day_first.AddDays(-1));
                        count += 4;
                        break;
                    case DayOfWeek.Saturday:
                        days.Add(day_first.AddDays(-5));
                        days.Add(day_first.AddDays(-4));
                        days.Add(day_first.AddDays(-3));
                        days.Add(day_first.AddDays(-2));
                        days.Add(day_first.AddDays(-1));
                        count += 5;
                        break;
                    case DayOfWeek.Sunday:
                        days.Add(day_first.AddDays(-6));
                        days.Add(day_first.AddDays(-5));
                        days.Add(day_first.AddDays(-4));
                        days.Add(day_first.AddDays(-3));
                        days.Add(day_first.AddDays(-2));
                        days.Add(day_first.AddDays(-1));
                        count += 6;
                        break;
                }

                var day_last = day_first;
                var monthDayCount = DateHelper.GetMonthDayCount(Year, Month);
                for (int i = 1; i <= monthDayCount; i++)
                {
                    day_last = new DateTime(Year, Month, i);
                    days.Add(day_last);
                }
                var week_last = day_last.DayOfWeek;
                switch (week_last)
                {
                    case DayOfWeek.Monday:
                        days.Add(day_last.AddDays(1));
                        days.Add(day_last.AddDays(2));
                        days.Add(day_last.AddDays(3));
                        days.Add(day_last.AddDays(4));
                        days.Add(day_last.AddDays(5));
                        days.Add(day_last.AddDays(6));
                        break;
                    case DayOfWeek.Tuesday:
                        days.Add(day_last.AddDays(1));
                        days.Add(day_last.AddDays(2));
                        days.Add(day_last.AddDays(3));
                        days.Add(day_last.AddDays(4));
                        days.Add(day_last.AddDays(5));
                        break;
                    case DayOfWeek.Wednesday:
                        days.Add(day_last.AddDays(1));
                        days.Add(day_last.AddDays(2));
                        days.Add(day_last.AddDays(3));
                        days.Add(day_last.AddDays(4));
                        break;
                    case DayOfWeek.Thursday:
                        days.Add(day_last.AddDays(1));
                        days.Add(day_last.AddDays(2));
                        days.Add(day_last.AddDays(3));
                        break;
                    case DayOfWeek.Friday:
                        days.Add(day_last.AddDays(1));
                        days.Add(day_last.AddDays(2));
                        break;
                    case DayOfWeek.Saturday:
                        days.Add(day_last.AddDays(1));
                        break;
                    case DayOfWeek.Sunday:
                        break;
                }
                Days = days;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region 属性 Year
        private int _Year = 0;
        public int Year
        {
            get
            {
                return _Year;
            }
            set
            {
                _Year = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 属性 Month
        private int _Month = 0;
        public int Month
        {
            get
            {
                return _Month;
            }
            set
            {
                _Month = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 属性 Days
        private List<DateTime> _Days = null;
        public List<DateTime> Days
        {
            get
            {
                if (_Days == null)
                {
                    _Days = new List<DateTime>();
                }
                return _Days;
            }
            set
            {
                _Days = value;
                OnPropertyChanged();
            }
        }
        #endregion

        





    }
}
