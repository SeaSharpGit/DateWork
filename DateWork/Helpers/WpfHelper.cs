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
            GetWindowThreadProcessId(activatedHandle, out int activeProcId);

            return activeProcId == procId;
        }

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
}
