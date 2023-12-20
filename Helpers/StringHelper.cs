using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Helpers
{
    public static class RollerCommandHelper
    {

        private static string RESET = "FB";
        private static string LIGHT_ON = "010101FE";
        private static string LIGHT_OFF = "000000FE";

        private static string TEST = "FC";
        private static string FEED = "{0}{1}FF";

        public static string SUCCESS = "OK";
        public static string FAIL = "NO";

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static byte[] ResetCommand()
        {
            return StringToByteArray(RESET);
        }

        public static byte[] TestCommand()
        {
            return StringToByteArray(TEST);
        }

        public static byte[] LightOn()
        {
            return StringToByteArray(LIGHT_ON);
        }

        public static byte[] LightOff()
        {
            return StringToByteArray(LIGHT_OFF);
        }

        public static byte[] FeedCommand(int amount, int row, int col)
        {
            var number = ((row - 1) * 10) + col;
            var pos = number.ToString("x2");
            var num = amount.ToString("x2");
            return StringToByteArray(String.Format(FEED, pos, num));
        }
    }
}
