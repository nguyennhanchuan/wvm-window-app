using System;
using System.ComponentModel;

namespace VendingMachine.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {
        IDisposable SuspendNotifications();
    }
}