using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProBoost.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isRuning = false;
        public bool IsRunning
        {
            get => _isRuning;
            set
            {
                _isRuning = value;
                OnPropertyChanged("IsRunning");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
