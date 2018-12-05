using DateWork.Models;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Windows;

namespace DateWork
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public static MainWindow Current { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
            Current = this;
            DataContext = AppModel.Current;
            Closed += MainWindow_Closed;
        }

        #region 事件
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            AppModel.Clear();
        }

        private void YearUp_Click(object sender, RoutedEventArgs e)
        {
            AppModel.Current.Year--;
            AppModel.Current.RefreshDays();
        }

        private void YearDown_Click(object sender, RoutedEventArgs e)
        {
            AppModel.Current.Year++;
            AppModel.Current.RefreshDays();
        }

        private void MonthUp_Click(object sender, RoutedEventArgs e)
        {
            if (AppModel.Current.Month == 1)
            {
                AppModel.Current.Year--;
                AppModel.Current.Month = 12;
            }
            else
            {
                AppModel.Current.Month--;
            }
            AppModel.Current.RefreshDays();
        }

        private void MonthDown_Click(object sender, RoutedEventArgs e)
        {
            if (AppModel.Current.Month == 12)
            {
                AppModel.Current.Year++;
                AppModel.Current.Month = 1;
            }
            else
            {
                AppModel.Current.Month++;
            }
            AppModel.Current.RefreshDays();
        }
        #endregion
    }
}
