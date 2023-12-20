using System;
using System.Reactive;

namespace VendingMachine.ViewModels
{
    public interface IAddResourceViewModel : ICloseableViewModel
    {
        string Path { get; set; }

        string Json { get; set; }

        IObservable<Unit> Added { get; }
    }
}