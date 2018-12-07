/* 
 * author      : singba singba@163.com 
 * version     : 20161221
 * source      : AF.Core
 * license     : free use or modify
 * description : WPF应用程序的操作帮助类
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;

namespace DateWork.Helpers
{
    public class CollectionBase<T> :
        System.Collections.ObjectModel.ObservableCollection<T>,
        System.ComponentModel.INotifyPropertyChanged where T : INotifyPropertyChanged, new()
    {
    }

    public class ObservableList<T> : IList<T>, IList, INotifyCollectionChanged where T : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }
        #endregion

        protected List<T> Items = new List<T>();
        private readonly object sync = new object();

        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        delegate void OnCollectionChangedDelegate(NotifyCollectionChangedEventArgs e);
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            lock (sync)
            {
                var eh = CollectionChanged;
                if (eh == null)
                    return;
                foreach (NotifyCollectionChangedEventHandler nh in eh.GetInvocationList())
                {
                    try
                    {
                        if (nh.Target is DispatcherObject dpo)
                        {
                            if (dpo.Dispatcher != null && dpo.Dispatcher.CheckAccess() == false)
                            {
                                dpo.Dispatcher.BeginInvoke(new OnCollectionChangedDelegate(OnCollectionChanged), e);
                            }
                            else
                            {
                                nh.Invoke(this, e);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("OnCollectionChanged{0}", ex);
                        continue;
                    }
                }
            }
        }

        public int IndexOf(T item)
        {
            lock (sync)
            {
                return Items.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (sync)
            {
                Items.Insert(index, item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }
        }

        public void RemoveAt(int index)
        {
            lock (sync)
            {
                var item = Items[index];
                Items.RemoveAt(index);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            }
        }

        public T this[int index]
        {
            get
            {
                lock (sync) { return Items[index]; }
            }
            set
            {
                lock (sync) { Items[index] = value; }
            }
        }

        public void Add(T item)
        {
            lock (sync)
            {
                Items.Add(item);
#if SILVERLIGHT
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1));
#else
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
#endif
            }
        }

        public void Clear()
        {
            lock (sync)
            {
                var list = Items.ToList();
                Items.Clear();
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, list));
            }
        }

        public bool Contains(T item)
        {
            lock (sync) { return Items.Contains(item); }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (sync) { Items.CopyTo(array, arrayIndex); }
        }

        public int Count
        {
            get { lock (sync) { return Items.Count; } }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            lock (sync)
            {
                var index = Items.IndexOf(item);
                var result = Items.Remove(item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                return result;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Add(object value)
        {
            if (value != null && value is T)
            {
                Add((T)value);
                return Items.Count - 1;
            }
            return -1;
        }

        public bool Contains(object value)
        {
            if (value != null && value is T)
            {
                return Contains((T)value);
            }

            return false;
        }

        public int IndexOf(object value)
        {
            if (value != null && value is T)
            {
                return IndexOf((T)value);
            }
            return -1;
        }

        public void Insert(int index, object value)
        {
            if (value != null && value is T)
            {
                Insert(index, (T)value);
            }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            if (value != null && value is T)
            {
                Remove((T)value);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (value != null && value is T)
                {
                    this[index] = (T)value;
                }
            }
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo((T[])array, index);
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public object SyncRoot
        {
            get { return Items; }
        }
    }
}
