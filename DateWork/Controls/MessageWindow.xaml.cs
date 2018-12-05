using DateWork.Helpers;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Threading;

namespace DateWork.Controls
{
    public partial class MessageWindow : ModernWindow
    {
        private const int CloseSeconds = 5;
        private int _ShowSeconds = 0;
        private DispatcherTimer _AutoCloseTimer = null;
        private object _MessageLock = new object();

        private Action<bool> _Callback = null;

        private MessageWindow()
        {
            InitializeComponent();
        }

        #region 属性 IsShowDialog
        private bool _IsShowDialog = false;
        public bool IsShowDialog
        {
            get
            {
                return _IsShowDialog;
            }
            private set
            {
                _IsShowDialog = value;
            }
        }
        #endregion

        #region 属性 IsCancelButtonVisible
        private bool _IsCancelButtonVisible = false;
        public bool IsCancelButtonVisible
        {
            get
            {
                return _IsCancelButtonVisible;
            }
            set
            {
                _IsCancelButtonVisible = value;
                this.ButtonCancel.Visibility =
                    _IsCancelButtonVisible ? Visibility.Visible :
                    Visibility.Collapsed;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 显示消息对话框
        /// </summary>
        /// <param name="text">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="autoClose">是否自动关闭</param>
        public static void Show(string text, string title = "", bool autoClose = true)
        {
            CreateWindow(text, title, false, autoClose).Show();
        }

        public static void SafeShow(string text, string title = "", bool autoClose = true)
        {
            WpfApplication.SafeRun(() =>
            {
                CreateWindow(text, title, false, autoClose).Show();
            });
        }

        public static void ShowAsync(string text, Action<bool> onOK,
            string title = "", bool showCancel = false, bool autoClose = false, bool showInTaskBar = true)
        {
            var window = CreateWindow(text, title, showCancel, autoClose);
            window._Callback = onOK;
            window.ShowInTaskbar = showInTaskBar;
            if (showInTaskBar)
            {
                window.Topmost = true;
            }
            window.Show();
        }

        /// <summary>
        /// 显示消息对话框
        /// </summary>
        /// <param name="text">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="showCancel">是否显示取消按钮</param>
        /// <param name="autoClose">是否自动关闭</param>
        /// <returns></returns>
        public static bool? ShowDialog(string text, string title = "", bool showCancel = false, bool autoClose = false)
        {
            var window = CreateWindow(text, title, showCancel, autoClose);
            window.IsShowDialog = true;
            window.Topmost = true;
            return window.ShowDialog();
        } 
        #endregion

        #region Private Methods
        private static MessageWindow CreateWindow(string text, string title = "", bool showCancel = false, bool autoClose = false)
        {
            var messageWindow = new MessageWindow();
            messageWindow.Init(text, title, showCancel, autoClose);
            return messageWindow;
        }

        private void Init(string text, string title = "", bool showCancel = false, bool autoClose = false)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Title = title;
            }
            MessageText.Text = text;
            IsCancelButtonVisible = showCancel;
            if (autoClose)
            {
                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                _AutoCloseTimer = timer;
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _ShowSeconds++;
            TextSeconds.Text = string.Format("{0}秒", CloseSeconds - _ShowSeconds + 1);
            if (_ShowSeconds > CloseSeconds)
            {
                lock (_MessageLock)
                {
                    (sender as DispatcherTimer).Stop();
                    Close();
                }
            }
        } 
        #endregion

        #region Events
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (_AutoCloseTimer != null)
            {
                _AutoCloseTimer.Stop();
            }
            if (IsShowDialog)
            {
                DialogResult = false;
            }
            else
            {
                Close();
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (_AutoCloseTimer != null)
            {
                _AutoCloseTimer.Stop();
            }
            if (IsShowDialog)
            {
                DialogResult = true;
            }
            else
            {
                Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_AutoCloseTimer != null)
            {
                _AutoCloseTimer.Stop();
            }
            if (IsShowDialog)
            {
                DialogResult = false;
            }
            else
            {
                Close();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        } 
        #endregion
    }
}
