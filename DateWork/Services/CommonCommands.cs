using DateWork.Heplers;
using DateWork.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DateWork.Services
{
    public static class CommonCommands
    {
        #region 打开笔记窗口
        private static RelayCommand _OpenNoteWindowCommand = null;
        public static RelayCommand OpenNoteWindowCommand
        {
            get
            {
                if (_OpenNoteWindowCommand == null)
                {
                    _OpenNoteWindowCommand = new RelayCommand(OnOpenNoteWindow, CanOpenNoteWindow);
                }
                return _OpenNoteWindowCommand;
            }
        }

        private static bool CanOpenNoteWindow(object parameter)
        {
            return true;
        }

        private static void OnOpenNoteWindow(object parameter)
        {
            var win = new NoteWindow
            {
                Owner = MainWindow.Current,
                ShowInTaskbar = false
            };
            var vm = new NoteViewModel
            {
                CloseWindow = () =>
                {
                    if (win.IsActive)
                    {
                        win.Close();
                    }
                }
            };
            win.DataContext = vm;
            win.ShowDialog();
        }
        #endregion

    }
}
