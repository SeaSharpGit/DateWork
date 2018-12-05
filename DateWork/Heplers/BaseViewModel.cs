using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DateWork.Heplers
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
