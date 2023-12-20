using System;
using System.Reactive;

namespace VendingMachine.Services
{
    public interface IHeartbeatService : IService
    {
        IObservable<Unit> Listen { get; }
    }
}