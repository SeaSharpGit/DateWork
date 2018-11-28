using DateWork.Helpers;
using DateWork.Heplers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DateWork.Models
{
    [XmlRoot("Notes")]
    public class Notes : BaseViewModel
    {
        private const string _Path = @"C:\Users\dahai\Desktop\Projects\DateWork\DateWork\datework.xml";

        public static Notes Current { get; set; } = LoadXml();

        #region 属性 Items
        private CollectionBase<Note> _Items = null;
        [XmlElement("Note", typeof(Note))]
        public CollectionBase<Note> Items
        {
            get
            {
                if (_Items == null)
                {
                    _Items = new CollectionBase<Note>();
                }
                return _Items;
            }
            set
            {
                _Items = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public static Notes LoadXml()
        {
            return _Path.XmlToObject<Notes>();
        }

        public void Save()
        {
            this.ObjectToXml(_Path);
            Current = LoadXml();
        }
    }

    public class Note : BaseViewModel
    {
        #region 属性 Name
        private string _Name = null;
        [XmlAttribute("Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 属性 IsMonthDay
        private bool _IsMonthDay = false;
        [XmlAttribute("IsMonthDay")]
        public bool IsMonthDay
        {
            get
            {
                return _IsMonthDay;
            }
            set
            {
                _IsMonthDay = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 属性 Date
        private string _Date = string.Empty;
        [XmlAttribute("Date")]
        public string Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                OnPropertyChanged();
            }
        }
        #endregion


    }

}
