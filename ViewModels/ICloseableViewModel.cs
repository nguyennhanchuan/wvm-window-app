using System;
using System.Reactive;

namespace VendingMachine.ViewModels
{
    public interface ICloseableViewModel : ITransientViewModel
    {
        IObservable<Unit> Closed { get; }
        IObservable<Unit> Denied { get; }
        IObservable<Unit> Confirmed { get; }
    }
}