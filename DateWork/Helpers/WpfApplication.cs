using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DateWork.Helpers
{
    public static class WpfApplication
    {
        public static Dispatcher MainDispatcher { get; set; }

        private static bool _IsClosed = false;

        private static readonly List<Action> _WaitingActions = new List<Action>();

        private static readonly List<Func<object>> _WaitingFunctions = new List<Func<object>>();

        private static readonly object _WaitingLock = new object();


        #region 属性 IsClosing
        private static bool _IsClosing = false;
        public static bool IsClosing
        {
            get
            {
                return _IsClosing;
            }
            set
            {
                _IsClosing = value;
            }
        }
        #endregion

        public static bool IsStart()
        {
            return MainDispatcher != null && _IsClosed == false;
        }

        public static void Close(int exitCode = 0)
        {
            if (_IsClosed)
            {
                return;
            }
            _IsClosed = true;
            try
            {
                Process.GetCurrentProcess().Close();
                System.Windows.Application.Current.Shutdown(exitCode);
            }
            catch
            {
            }
        }

        public static void SafeRun(this Action action)
        {
            if (MainDispatcher == null)
            {
                action();
                return;
            }

            if (!MainDispatcher.CheckAccess())
            {
                MainDispatcher.BeginInvoke(action);
                return;
            }
            action();
        }

        public static async Task RunAsync(this Action action)
        {
            if (MainDispatcher == null ||
                MainDispatcher.CheckAccess())
            {
                action();
                return;
            }

            await MainDispatcher.InvokeAsync(action);
        }

        public static async Task<T> RunAsync<T>(this Func<T> func)
        {
            if (MainDispatcher == null ||
                MainDispatcher.CheckAccess())
            {
                return func();
            }

            return await MainDispatcher.InvokeAsync(func);
        }


        #region CreateMenu
        /// <summary>
        /// 新增Menu到弹出菜单的最上面
        /// </summary>
        /// <param name="notifyIcon"></param>
        /// <param name="text"></param>
        /// <param name="onClick"></param>
        /// <returns></returns>
        public static System.Windows.Forms.MenuItem AddMenu(this NotifyIcon notifyIcon, string text, Action onClick)
        {
            System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem(text);
            if (onClick != null)
            {
                menu.Click += (sender, e) =>
                {
                    onClick();
                };
            }
            notifyIcon.ContextMenu.MenuItems.Add(0, menu);
            return menu;
        }
        #endregion

        private static void ShowHide(Window window)
        {
            if (window.Visibility == Visibility.Visible)
            {
                window.ShowInTaskbar = false;
                window.Visibility = Visibility.Hidden;
            }
            else
            {
                window.Visibility = System.Windows.Visibility.Visible;
                window.ShowInTaskbar = true;
                window.Activate();
            }
        }

        public static void AutoRun(string appName, bool set = true)
        {
            var path = System.Windows.Forms.Application.ExecutablePath;
            var rk = Registry.CurrentUser;
            var rgkRun = rk.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rgkRun == null)
            {
                rgkRun = rk.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }
            if (set) //设置开机自启动  
            {
                rgkRun.SetValue(appName, path);
            }
            else //取消开机自启动  
            {
                rgkRun.DeleteValue(appName, false);
            }
            rgkRun.Close();
            rk.Close();
        }

        public static bool IsAutoRun(string appName)
        {
            bool value = false;
            string path = System.Windows.Forms.Application.ExecutablePath;
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            value = rk2.GetValue(appName, "").Equals(path);
            rk2.Close();
            rk.Close();
            return value;
        }

    }
}
