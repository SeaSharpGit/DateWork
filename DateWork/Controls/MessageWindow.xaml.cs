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
        private bool _IsOK = false;
        private bool _InActiveFlash = false;
        //private FlashWindowHelper _FlashWindow = null;

        private MessageWindow()
        {
            InitializeComponent();
            //this.Closed += MessageWindow_Closed;
            //this.Activated += MessageWindow_Activated;
            //this.Deactivated += MessageWindow_Deactivated;
        }

        //private void MessageWindow_Activated(object sender, EventArgs e)
        //{
        //    if (_FlashWindow != null)
        //    {
        //        _FlashWindow.StopFlashing();
        //        _FlashWindow = null;
        //    }
        //    if (this.Topmost)
        //    {
        //        this.Topmost = false;
        //    }
        //}

        //private void MessageWindow_Deactivated(object sender, EventArgs e)
        //{
        //    if (_InActiveFlash)
        //    {
        //        if (_FlashWindow == null)
        //        {
        //            _FlashWindow = new FlashWindowHelper(this);
        //            _FlashWindow.FlashApplicationWindow();
        //        }
        //    }
        //}

        //private void MessageWindow_Closed(object sender, EventArgs e)
        //{
        //    _Callback?.Invoke(_IsOK);
        //}

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
                window._InActiveFlash = showInTaskBar;
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
                    this.Close();
                }
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (_AutoCloseTimer != null)
            {
                _AutoCloseTimer.Stop();
            }
            if (IsShowDialog)
            {
                this.DialogResult = false;
            }
            else
            {
                this.Close();
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
                this.DialogResult = true;
            }
            else
            {
                _IsOK = true;
                this.Close();
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
                this.DialogResult = false;
            }
            else
            {
                this.Close();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}
