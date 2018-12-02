using DateWork.Heplers;
using DateWork.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DateWork.Controls
{
    public class DayControl : Control
    {
        static DayControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayControl), new FrameworkPropertyMetadata(typeof(DayControl)));
        }

        #region Day DependencyProperty
        public DateTime Day
        {
            get { return (DateTime)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }
        public static readonly DependencyProperty DayProperty =
                DependencyProperty.Register("Day", typeof(DateTime), typeof(DayControl),
                new PropertyMetadata(DateTime.Now, new PropertyChangedCallback(DayControl.OnDayPropertyChanged)));

        private static void OnDayPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is DayControl)
            {
                (obj as DayControl).OnDayValueChanged();
            }
        }

        protected void OnDayValueChanged()
        {
            RefreshDayName();
            RefreshMonthDayName();
            RefreshBackground();
        }
        #endregion

        #region DayName DependencyProperty
        public string DayName
        {
            get { return (string)GetValue(DayNameProperty); }
            set { SetValue(DayNameProperty, value); }
        }
        public static readonly DependencyProperty DayNameProperty =
                DependencyProperty.Register("DayName", typeof(string), typeof(DayControl),
                new PropertyMetadata(null, new PropertyChangedCallback(DayControl.OnDayNamePropertyChanged)));

        private static void OnDayNamePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is DayControl)
            {
                (obj as DayControl).OnDayNameValueChanged();
            }
        }

        protected void OnDayNameValueChanged()
        {

        }

        private void RefreshDayName()
        {
            DayName = Day.Day.ToString();
        }
        #endregion

        #region MonthDayName DependencyProperty
        public string MonthDayName
        {
            get { return (string)GetValue(MonthDayNameProperty); }
            set { SetValue(MonthDayNameProperty, value); }
        }
        public static readonly DependencyProperty MonthDayNameProperty =
                DependencyProperty.Register("MonthDayName", typeof(string), typeof(DayControl),
                new PropertyMetadata(null, new PropertyChangedCallback(DayControl.OnMonthDayNamePropertyChanged)));

        private static void OnMonthDayNamePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is DayControl)
            {
                (obj as DayControl).OnMonthDayNameValueChanged();
            }
        }

        protected void OnMonthDayNameValueChanged()
        {

        }

        private void RefreshMonthDayName()
        {
            MonthDayName = MonthDayHelper.GetMonthDateTimeWithoutYearR(Day);
        }
        #endregion

        #region NoteText DependencyProperty
        public string NoteText
        {
            get { return (string)GetValue(NoteTextProperty); }
            set { SetValue(NoteTextProperty, value); }
        }
        public static readonly DependencyProperty NoteTextProperty =
                DependencyProperty.Register("NoteText", typeof(string), typeof(DayControl),
                new PropertyMetadata(null, new PropertyChangedCallback(DayControl.OnNoteTextPropertyChanged)));

        private static void OnNoteTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is DayControl)
            {
                (obj as DayControl).OnNoteTextValueChanged();
            }
        }

        protected void OnNoteTextValueChanged()
        {

        }
        #endregion

        #region DayType DependencyProperty
        public DayType DayType
        {
            get { return (DayType)GetValue(DayTypeProperty); }
            set { SetValue(DayTypeProperty, value); }
        }
        public static readonly DependencyProperty DayTypeProperty =
                DependencyProperty.Register("DayType", typeof(DayType), typeof(DayControl),
                new PropertyMetadata(DayType.Normal, new PropertyChangedCallback(DayControl.OnDayTypePropertyChanged)));

        private static void OnDayTypePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is DayControl)
            {
                (obj as DayControl).OnDayTypeValueChanged();
            }
        }

        protected void OnDayTypeValueChanged()
        {

        }
        #endregion


        private void RefreshBackground()
        {
            var now = DateTime.Now;
            var notes = Notes.Current.Items;

            var dayNotes = notes.Where(a => !a.IsMonthDay
                && MonthDayHelper.IsSameMonthDay(Convert.ToDateTime(a.Date), Day))
                .Select(a => a.Name).ToList();
            var monthDayNotes = notes.Where(a => a.IsMonthDay
                && MonthDayHelper.IsSameMonthMonthMonthDay(Convert.ToDateTime(a.Date), Day))
                .Select(a => a.Name); ;
            dayNotes.AddRange(monthDayNotes);
            if (dayNotes.Count > 0)
            {
                DayType = DayType.Note;
                NoteText = string.Join("\r\n", dayNotes);
            }
            else if (Day.Year == now.Year && Day.Month == now.Month && Day.Day == now.Day)
            {
                DayType = DayType.Today;
                NoteText = "今天";
            }
            else if (Day.Year == AppModel.Current.Year && Day.Month == AppModel.Current.Month)
            {
                DayType = DayType.Normal;
            }
            else
            {
                DayType = DayType.OtherMonth;
            }
        }


    }
}
