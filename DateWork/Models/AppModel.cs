using DateWork.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateWork.Models
{
    public class AppModel : EntityBase
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
                RaisePropertyChanged(() => Year);
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
                RaisePropertyChanged(() => Month);
            }
        }
        #endregion

        #region 属性 Items
        private CollectionBase<DateModel> _Items = null;
        public CollectionBase<DateModel> Items
        {
            get
            {
                if (_Items == null)
                {
                    _Items = new CollectionBase<DateModel>();
                }
                return _Items;
            }
        }
        #endregion






    }
}
