using DateWork.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateWork.Models
{
    public class DateModel : EntityBase 
    {
        #region 属性 Day
        private int _Day = 0;
        public int Day
        {
            get
            {
                return _Day;
            }
            set
            {
                _Day = value;
                RaisePropertyChanged(() => Day);
            }
        }
        #endregion 

        #region 属性 MoonDay
        private int _MoonDay = 0;
        public int MoonDay
        {
            get
            {
                return _MoonDay;
            }
            set
            {
                _MoonDay = value;
                RaisePropertyChanged(() => MoonDay);
            }
        }
        #endregion

        #region 属性 MoonDayName
        private string _MoonDayName = string.Empty;
        public string MoonDayName
        {
            get
            {
                return _MoonDayName;
            }
            set
            {
                _MoonDayName = value;
                RaisePropertyChanged(() => MoonDayName);
            }
        }
        #endregion


    }
}
