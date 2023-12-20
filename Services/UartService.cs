using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO.Ports;
using VendingMachine.Models;
using System.Net;
using System.Collections.Concurrent;
using VendingMachine.Helpers;
using System.Threading;
using System.ComponentModel;
using System.Management;
using System.Diagnostics;
using System.Timers;

namespace VendingMachine.Services
{
    public class UartService : DisposableObject, IUartService
    {
        //private readonly Queue<ModbusCommand> _waitingMessages = new Queue<ModbusCommand>();

        private const string VID = "1A86";
        private const string PID = "7523";
        private const int BAUD = 115200;
        private const int DATA_BIT = 8;
        private const Parity PARITY = Parity.None;
        private const StopBits STOP_BIT = StopBits.One;

        private SerialPort _uart;
        private FeedCommand _lastCommand = null;

        private Queue<FeedCommand> _commands = new Queue<FeedCommand>();
        private List<FeedResult> _results = new List<FeedResult>();


        private bool _isStopped = false;

        private Thread _excuteThread = null;
        private BackgroundWorker bgwDriveDetector = null;

        private System.Timers.Timer _mockTimer = null;

        public UartService()
        {

        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(500);
                string data = _uart.ReadExisting();
                if (data.Length > 2)
                {
                    data = data.Substring(data.Length - 2);
                }
                Debug.WriteLine("RECEIVE FEED" + data);
                if (data.Equals(RollerCommandHelper.SUCCESS))
                {
                    Debug.WriteLine("SUCCESS FEED");
                    _results.Add(new FeedResult()
                    {
                        status = FeedResultStatus.SUCCESS,
                        command = FeedCommand.Clone(_lastCommand)
                    });
                }

                else
                {
                    Debug.WriteLine("SUCCESS FALSE");
                    _results.Add(new FeedResult()
                    {
                        status = FeedResultStatus.FAILED,
                        command = FeedCommand.Clone(_lastCommand)
                    });

                }
                _lastCommand = null;
                if (_commands.Count == 0)
                {
                    OnFeedDone(_results);
                    _results = new List<FeedResult>();
                }

            }
            catch (Exception ex)
            {
                int a = 0;
            }

        }


        private void StartMockTimer()
        {
            Debug.WriteLine("MOCK FEED");
            if (_mockTimer != null)
            {
                _mockTimer.Enabled = false;
                _mockTimer.Stop();
                _mockTimer.Dispose();
            }

            _mockTimer = new System.Timers.Timer();
            _mockTimer.Elapsed += new ElapsedEventHandler(OnMockTimerEllapsed);
            _mockTimer.Interval = 5000;
            _mockTimer.Enabled = true;
            _mockTimer.AutoReset = false;
            _mockTimer.Start();
        }


        private void StopMockTimer()
        {
            if (_mockTimer != null)
            {
                _mockTimer.Enabled = false;
                _mockTimer.Stop();
                _mockTimer.Dispose();
            }
        }

        void OnMockTimerEllapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                Thread.Sleep(500);
                Random random = new Random();
                if (random.Next(3) == 0)
                {
                    Debug.WriteLine("SUCCESS FEED");
                    _results.Add(new FeedResult()
                    {
                        status = FeedResultStatus.SUCCESS,
                        command = FeedCommand.Clone(_lastCommand)
                    });
                }
                else
                {
                    Debug.WriteLine("SUCCESS FALSE");
                _results.Add(new FeedResult()
                {
                    status = FeedResultStatus.FAILED,
                    command = _lastCommand
                });

                }
                _lastCommand = null;
                if (_commands.Count == 0)
                {
                    OnFeedDone(_results);
                    _results = new List<FeedResult>();
                }

            }
            catch (Exception ex)
            {
                int a = 0;
            }

        }

        public void Excute(byte[] command)
        {
            _uart.Write(command, 0, command.Length);
        }

        public bool Connect(string portName, int baud, Parity parity, int dataBits, StopBits stopBits)
        {
            try
            {
                _uart = new SerialPort(portName, baud, parity, dataBits, stopBits);
                _uart.Handshake = Handshake.None;
                _uart.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                _uart.WriteTimeout = 100000;
                _uart.Open();
                //Reset();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Start()
        {
            try
            {
                _isStopped = false;
                _excuteThread = new Thread(() =>
                {
                    while (!_isStopped)
                    {
                        if (_commands.Count > 0 && _lastCommand == null)
                        {
                            _lastCommand = _commands.Dequeue();
                            //Excute(_lastCommand.command);
                            StartMockTimer();
                        }
                    }
                });
                _excuteThread.Start();
            }
            catch (Exception ex)
            {

            }
        }


        public void Stop()
        {
            _isStopped = true;
        }


        public bool Disconnect()
        {
            try
            {
                if (_uart != null)
                {

                    _uart.Close();
                    _uart.Dispose();
                    _uart = null;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void Feed(string productId, string positionId, int number, int row, int col)
        {

            try
            {
                //while(number > 0)
                // {
                _commands.Enqueue(new FeedCommand()
                {
                    productId = productId,
                    positionId = positionId,
                    number = number,
                    row = row,
                    col = col,
                    command = RollerCommandHelper.FeedCommand(number, row, col)
                }); ;
                //number--;
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void Reset()
        {
            try
            {
                var resetCommand = RollerCommandHelper.ResetCommand();
                _uart.Write(resetCommand, 0, resetCommand.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static ComDevice attachedRoller = null;
        public static List<string> allComPorts = new List<string>();

        public event IUartService.Notify OnFeedDone;

        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            string[] updatedAddedComPorts = SerialPort.GetPortNames();
            string[] result = updatedAddedComPorts.Except(allComPorts).ToArray();
            allComPorts.Add(result[0]);

            ManagementObjectCollection ManObjReturn;
            ManagementObjectSearcher ManObjSearch;
            ManObjSearch = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\"");
            ManObjReturn = ManObjSearch.Get();
            foreach (ManagementObject ManObj in ManObjReturn)
            {
                int s = ManObj.Properties.Count;

                string name = ManObj["Name"].ToString();
                string device = ManObj["DeviceID"].ToString();
                string vendorId = device.Substring(device.IndexOf("VID") + 4, 4);
                string producId = device.Substring(device.IndexOf("PID") + 4, 4);


                if (string.Equals(vendorId, VID) && string.Equals(producId, PID) && name.Contains(result[0]))
                {
                    if (attachedRoller != null)
                    {
                        Stop();
                        Disconnect();
                    }

                    attachedRoller = new ComDevice() { ComPortName = result[0], ProductId = producId, VendorId = vendorId };
                    Connect(attachedRoller.ComPortName, BAUD, PARITY, DATA_BIT, STOP_BIT);
                    Start();
                    break;
                }
            }

        }

        public void StartService()
        {
            Start();

            //allComPorts = SerialPort.GetPortNames().ToList();
            //ManagementObjectCollection ManObjReturn;
            //ManagementObjectSearcher ManObjSearch;
            //ManObjSearch = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\"");
            //ManObjReturn = ManObjSearch.Get();
            //foreach (ManagementObject ManObj in ManObjReturn)
            //{
            //    foreach (var portName in allComPorts)
            //    {

            //        string name = ManObj["Name"].ToString();
            //        string device = ManObj["DeviceID"].ToString();
            //        string vendorId = device.Substring(device.IndexOf("VID") + 4, 4);
            //        string producId = device.Substring(device.IndexOf("PID") + 4, 4);

            //        if (string.Equals(vendorId, VID) && string.Equals(producId, PID) && name.Contains(portName))
            //        {
            //            if (attachedRoller != null)
            //            {
            //                Stop();
            //                Disconnect();
            //            }

            //            attachedRoller = new ComDevice() { ComPortName = portName, ProductId = producId, VendorId = vendorId };
            //            Connect(attachedRoller.ComPortName, BAUD, PARITY, DATA_BIT, STOP_BIT);
            //            Start();
            //            break;
            //        }
            //    }
            //}

            //bgwDriveDetector = new BackgroundWorker();
            //bgwDriveDetector.DoWork += backgroundWorker1_DoWork;
            //bgwDriveDetector.RunWorkerAsync();
            //bgwDriveDetector.WorkerReportsProgress = true;
            //bgwDriveDetector.WorkerSupportsCancellation = true;
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            string[] updatedRemovedComPorts = SerialPort.GetPortNames();
            string[] result = allComPorts.Except(updatedRemovedComPorts).ToArray();
            if (result.Length > 0)
            {
                allComPorts.Remove(result[0]);
                if (result[0] != null && result[0] == attachedRoller.ComPortName)
                {
                    attachedRoller = null;
                    Stop();
                    Disconnect();
                }
            }
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");

            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
            removeWatcher.Start();

            // Do something while waiting for events
            System.Threading.Thread.Sleep(20000000);
        }

        public void StopService()
        {
            if (bgwDriveDetector != null)
            {
                bgwDriveDetector.CancelAsync();
            }

            Stop();
            Disconnect();
        }

        public bool IsConnected()
        {
            return attachedRoller != null && _uart.IsOpen;
        }

        public void LightOn()
        {
            try
            {
                var lightOn = RollerCommandHelper.LightOn();
                _uart.Write(lightOn, 0, lightOn.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void LightOff()
        {
            try
            {
                var lightOff = RollerCommandHelper.LightOff();
                _uart.Write(lightOff, 0, lightOff.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public class ComDevice
        {
            public string ComPortName { get; set; }
            public string VendorId { get; set; }
            public string ProductId { get; set; }

        }
    }
}
