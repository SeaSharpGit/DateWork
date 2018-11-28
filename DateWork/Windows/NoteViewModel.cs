using DateWork.Controls;
using DateWork.Helpers;
using DateWork.Heplers;
using DateWork.Models;
using DateWork.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateWork.Windows
{
    public class NoteViewModel : BaseViewModel
    {
        public Action CloseWindow { get; set; } = null;

        public NoteViewModel()
        {

        }

        #region 属性 Name
        private string _Name = string.Empty;
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

        #region 属性 IsSelected
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 属性 Date
        private string _Date = string.Empty;
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

        #region 属性 IsMonthDay
        private bool _IsMonthDay = false;
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

        #region 属性 SelectedNote
        private Note _SelectedNote = null;
        public Note SelectedNote
        {
            get
            {
                return _SelectedNote;
            }
            set
            {
                _SelectedNote = value;
                OnPropertyChanged();
                if (_SelectedNote != null)
                {
                    Name = _SelectedNote.Name;
                    Date = _SelectedNote.Date;
                    IsMonthDay = _SelectedNote.IsMonthDay;
                    IsSelected = true;
                }
            }
        }
        #endregion

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
                OnPropertyChanged();
            }
        }
        #endregion



        #region 新增
        private RelayCommand _AddCommand = null;
        public RelayCommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                {
                    _AddCommand = new RelayCommand(OnAdd, CanAdd);
                }
                return _AddCommand;
            }
        }

        private bool CanAdd(object parameter)
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Date);
        }

        private void OnAdd(object parameter)
        {
            var note = new Note
            {
                Name = Name,
                IsMonthDay = IsMonthDay,
                Date = Date
            };
            Notes.Items.Add(note);
            Notes.Save();
            Reset();
        }
        #endregion

        #region 修改
        private RelayCommand _UpdateCommand = null;
        public RelayCommand UpdateCommand
        {
            get
            {
                if (_UpdateCommand == null)
                {
                    _UpdateCommand = new RelayCommand(OnUpdate, CanUpdate);
                }
                return _UpdateCommand;
            }
        }

        private bool CanUpdate(object parameter)
        {
            return SelectedNote != null && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Date);
        }

        private void OnUpdate(object parameter)
        {
            SelectedNote.Name = Name;
            SelectedNote.IsMonthDay = IsMonthDay;
            SelectedNote.Date = Date;
            Notes.Save();
        }
        #endregion

        #region 删除
        private RelayCommand _DeleteCommand = null;
        public RelayCommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                {
                    _DeleteCommand = new RelayCommand(OnDelete, CanDelete);
                }
                return _DeleteCommand;
            }
        }

        private bool CanDelete(object parameter)
        {
            return SelectedNote != null;
        }

        private void OnDelete(object parameter)
        {
            if (MessageWindow.ShowDialog("确定删除吗", "删除", true,true) == true)
            {
                Notes.Items.Remove(SelectedNote);
                Notes.Save();
                Reset();
            }
        }
        #endregion

        #region 重置
        private RelayCommand _ResetCommand = null;
        public RelayCommand ResetCommand
        {
            get
            {
                if (_ResetCommand == null)
                {
                    _ResetCommand = new RelayCommand(OnReset, CanReset);
                }
                return _ResetCommand;
            }
        }

        private bool CanReset(object parameter)
        {
            return true;
        }

        private void OnReset(object parameter)
        {
            Reset();
        }

        private void Reset()
        {
            SelectedNote = null;
            IsSelected = false;
            Name = "";
            Date = "";
            IsMonthDay = false;
        }
        #endregion

    }
}
