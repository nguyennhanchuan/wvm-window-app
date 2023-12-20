using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Services
{

    public interface IUartService : IService
    {
        public delegate void Notify(List<FeedResult> feedResults);  // delegate

        public event Notify OnFeedDone;
        public void Feed(string productId, string positionid, int number, int row, int col);

        public void Reset();

        public void StartService();
        public void StopService();

        public void LightOn();
        public void LightOff();

        public bool IsConnected();

    }

    public class FeedCommand
    {
        public string productId { get; set; }
        public string positionId { get; set; }
        public int number { get; set; }
        public int row { get; set; }
        public int col { get; set; }

        public byte[] command { get; set; }

        public static FeedCommand Clone(FeedCommand feedCommand)
        {
            return new FeedCommand()
            {
                productId = feedCommand.productId,
                positionId = feedCommand.positionId,
                number = feedCommand.number,
                row = feedCommand.row,
                col = feedCommand.col,
                command = feedCommand.command
            };
        }
    }

    public class FeedResult
    {
        public string status { get; set; }
        public FeedCommand command { get; set; }
    }

    public class FeedResultStatus
    {
        public static string SUCCESS = "SUCCESS";
        public static string FAILED = "FAILED";

    }
}
