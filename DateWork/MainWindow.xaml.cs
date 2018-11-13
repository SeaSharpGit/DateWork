using DateWork.Models;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            AppModel.Current.Refresh();
        }

        private void YearDown_Click(object sender, RoutedEventArgs e)
        {
            AppModel.Current.Year++;
            AppModel.Current.Refresh();
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
            AppModel.Current.Refresh();
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
            AppModel.Current.Refresh();
        }
        #endregion
    }
}
