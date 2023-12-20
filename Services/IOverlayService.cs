using System;
using VendingMachine.ViewModels;

namespace VendingMachine.Services
{
    public interface IOverlayService : IService
    {
        IObservable<OverlayViewModel> Show { get; }

        void Post(string header, BaseViewModel viewModel, IDisposable lifetime);
    }
}