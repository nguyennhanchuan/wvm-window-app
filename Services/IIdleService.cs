using System;
using System.Reactive;

namespace VendingMachine.Services
{
    public interface IIdleService : IService
    {
        IObservable<Unit> Idling { get; }
    }
}