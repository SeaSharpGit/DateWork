/* 
 * author      : singba singba@163.com 
 * version     : 20161221
 * source      : AF.Wpf
 * license     : free use or modify
 * description : WPF应用程序的操作帮助类
 */
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;

namespace DateWork.Helpers
{
    public static class WpfHelper
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        public static bool IsApplicationActive()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }

        //private static bool IsActive(Window wnd)
        //{
        //    // workaround for minimization bug
        //    // Managed .IsActive may return wrong value
        //    if (wnd == null) return false;
        //    return GetForegroundWindow() == new WindowInteropHelper(wnd).Handle;
        //}

        //public static bool IsApplicationActive()
        //{
        //    foreach (var wnd in System.Windows.Application.Current.Windows.OfType<Window>())
        //        if (IsActive(wnd)) return true;
        //    return false;
        //}

        public static void ShowWithThreadSafe(this Window win)
        {
            WpfApplication.SafeRun(win.Show);
        }

        public static void ShowDialogWithThreadSafe(this Window win, Action<bool?> callback = null)
        {
            WpfApplication.SafeRun(() =>
            {
                var result = win.ShowDialog();
                callback?.Invoke(result);
            });
        }

        public static T FindChild<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }
                    T childItem = FindChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        public static IEnumerable<T> FindAllChild<T>(this DependencyObject obj)
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    object child = VisualTreeHelper.GetChild(obj, i);
                    if (child == null)
                    {
                        continue;
                    }
                    if (child is T)
                    {
                        yield return (T)child;
                    }
                    if (child is DependencyObject)
                    {
                        foreach (var item in (child as DependencyObject).FindAllChild<T>())
                        {
                            yield return item;
                        }
                    }
                }
            }
            yield break;
        }

        public static T FindParent<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            var p = VisualTreeHelper.GetParent(obj);
            if (p == null)
            {
                return default(T);
            }
            if (p is T)
            {
                return (T)p;
            }
            return FindParent<T>(p);
        }
        public static bool IsInDesignMode(this DependencyObject control)
        {
            return System.ComponentModel.DesignerProperties.GetIsInDesignMode(control);
        }
        public static bool IsInDesignMode()
        {
            return WpfApplication.MainDispatcher == null;
        }

        public static void SaveDefaultTemplate()
        {
            var control = System.Windows.Application.Current.FindResource(typeof(System.Windows.Controls.TreeView));
            using (XmlTextWriter writer = new XmlTextWriter(@"C:\Users\大海\Desktop\defaultTemplate.xml", System.Text.Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                XamlWriter.Save(control, writer);
            }
        }
    }

    public static class WpfApplication
    {
        public static Dispatcher MainDispatcher { get; set; }

        private static bool _IsClosed = false;

        private static List<Action> _WaitingActions
            = new List<Action>();

        private static List<Func<object>> _WaitingFunctions
            = new List<Func<object>>();

        private static object _WaitingLock =
            new object();


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
            if (set) //设置开机自启动  
            {
                string path = System.Windows.Forms.Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue(appName, path);
                rk2.Close();
                rk.Close();
            }
            else //取消开机自启动  
            {
                string path = System.Windows.Forms.Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue(appName, false);
                rk2.Close();
                rk.Close();
            }
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

        public static Thickness ToThickness(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new Thickness(0);
            }
            else
            {
                var arr = str.Split(',');
                var count = arr.Length;
                switch (count)
                {
                    case 1:
                        return new Thickness(double.Parse(arr[0]), double.Parse(arr[0]), double.Parse(arr[0]), double.Parse(arr[0]));
                    case 2:
                        return new Thickness(double.Parse(arr[0]), double.Parse(arr[1]), double.Parse(arr[0]), double.Parse(arr[1]));
                    case 4:
                        return new Thickness(double.Parse(arr[0]), double.Parse(arr[1]), double.Parse(arr[2]), double.Parse(arr[3]));
                    default:
                        return new Thickness(0, 0, 0, 0);
                }
            }
        }

    }
}
