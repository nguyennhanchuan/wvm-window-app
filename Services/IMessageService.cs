using System;
using VendingMachine.Models;
using VendingMachine.ViewModels;

namespace VendingMachine.Services
{
    public interface IMessageService : IService
    {
        IObservable<Message> Show { get; }

        void Post(string header, ICloseableViewModel viewModel);
    }
}