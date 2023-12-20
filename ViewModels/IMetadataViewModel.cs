using System;
using System.Reactive;
using VendingMachine.Commands;
using VendingMachine.Models;

namespace VendingMachine.ViewModels
{
    public interface IMetadataViewModel : ITransientViewModel
    {
        Uri Url { get; }
        bool Editable { get; }
        Metadata Metadata { get; }
        ReactiveCommand<object> ModifyCommand { get; }
        ReactiveCommand<object> DeleteCommand { get; }
        IObservable<Unit> Deleted { get; }
    }
}