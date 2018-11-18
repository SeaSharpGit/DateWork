using DateWork.Helpers;
using DateWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateWork.Windows
{
    public class NoteViewModel : EntityBase
    {
        public Action CloseWindow { get; set; } = null;

        public NoteViewModel()
        {

        }

        #region 属性 Notes
        private Notes _Notes = null;
        public Notes Notes
        {
            get
            {
                if (_Notes == null)
                {
                    _Notes = Notes.LoadXml();
                }
                return _Notes;
            }
            set
            {
                _Notes = value;
                RaisePropertyChanged(() => Notes);
            }
        }
        #endregion



    }
}
