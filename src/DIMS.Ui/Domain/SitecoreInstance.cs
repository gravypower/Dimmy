using System.ComponentModel;
using System.Runtime.CompilerServices;
using DIMS.Ui.Annotations;

namespace DIMS.Ui.Domain
{
    class SitecoreInstance: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
