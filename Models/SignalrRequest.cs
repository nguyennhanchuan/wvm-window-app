using System;

namespace VendingMachine.Models
{
    public class SignalrRequest
    {
        public Guid ID {get; set;}
        public Data Data {get; set;}
        public int SentTime {get; set;}
        public string Receiver {get; set;}
        public SignalrRequest(Data data, string receiver) {
            this.ID = Guid.NewGuid();
            this.Data = data;
            this.SentTime = 0;
            this.Receiver = receiver;
        }
    }

    public class Data
    {
        public object body { get; set; }
        public string title { get; set; }

    }
}