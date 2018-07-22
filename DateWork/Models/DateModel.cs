using DateWork.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateWork.Models
{
    public class DayModel : EntityBase
    {
        #region 属性 Day
        private DateTime _Day = DateTime.Now;
        public DateTime Day
        {
            get
            {
                return _Day;
            }
            set
            {
                _Day = value;
                RaisePropertyChanged(() => Day);
                Init();
            }
        }

        private void Init()
        {
            DayName = Day.Day.ToString();
        }
        #endregion

        #region 属性 DayName
        private string _DayName = string.Empty;
        public string DayName
        {
            get
            {
                return _DayName;
            }
            set
            {
                _DayName = value;
                RaisePropertyChanged(() => DayName);
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
