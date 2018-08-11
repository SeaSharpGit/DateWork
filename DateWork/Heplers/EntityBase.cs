/* 
 * author      : singba singba@163.com 
 * version     : 20161221
 * source      : AF.Core
 * license     : free use or modify
 * description : 通用类型基类
 */
using System;
using System.Linq.Expressions;

namespace DateWork.Helpers
{
    /// <summary>
    /// singba:20120807
    /// 这个基类只作为INotifyPropertyChanged的实现，不再是Entity的基类
    /// </summary>
    public class EntityBase : System.ComponentModel.INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }

        protected virtual void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression ue)
            {
                memberExpression = (MemberExpression)ue.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            RaisePropertyChanged(memberExpression.Member.Name);
        }
        #endregion
    }
}
