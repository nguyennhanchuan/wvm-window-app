using System;
using VendingMachine.Models;

namespace VendingMachine.Services
{
    public interface IDiagnosticsService : IService
    {
        IObservable<string> Log { get; }

        IObservable<Memory> Memory { get; }

        IObservable<int> Cpu { get; }
    }
}