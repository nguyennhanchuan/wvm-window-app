using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using VendingMachine.Models;

namespace VendingMachine.Services
{
    internal class SignalrService
    {
        private HubConnection hubConnection;

        private Dictionary<string, SignalrRequest> requests;
        private static SignalrService instance = null;
        private string deviceId;
        private OnMessageReceive onMessageReceive;
        private bool isStopped = false;

        public static SignalrService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SignalrService();
                }
                return instance;
            }
        }


        SignalrService()
        {
            this.requests = new Dictionary<string, SignalrRequest>();
        }

        public void Initialize(string url, string deviceId, OnMessageReceive onMessageReceive)
        {
            this.deviceId = deviceId;
            this.onMessageReceive = onMessageReceive;
            hubConnection = new HubConnectionBuilder().Build();
            // hubConnection.
            hubConnection.Closed += async (error) =>
            {
                if (!isStopped)
                {
                    if (error != null)
                    {
                        Console.WriteLine(error.StackTrace);
                    }
                    while (!await StartSignalrService())
                    {
                        Console.WriteLine("SIGNALR: Reconnect Fail");
                    }
                    Console.WriteLine("SIGNALR: Reconnected");
                }
                else
                {
                    Console.WriteLine("SIGNALR: Stop service");
                }
            };


            hubConnection.On<string>("CheckLostClientReceive", (message) =>
            {
                if (requests.ContainsKey(message))
                {
                    requests.Remove(message);
                    Console.WriteLine("Remove" + message);

                }
            });

            hubConnection.On<string, string>("ClientReceiveFromBe", (from, message) =>
            {
                try
                {
                    Console.WriteLine(from + " " + message);
                    JObject json = JObject.Parse(message);
                    string id = json.GetValue("ID").ToString();
                    hubConnection.InvokeAsync("MessageRecive", from, id);
                    onMessageReceive.OnReceive(JsonConvert.DeserializeObject<Data>(json.GetValue("Data").ToString()), id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);

                }
            });
        }

        public async Task<bool> StartSignalrService()
        {
            try
            {
                await hubConnection.StartAsync();
                // String _conId = hubConnection.getConnectionId();
                Console.WriteLine("Connected: " + hubConnection.ConnectionId);
                await hubConnection.InvokeAsync("InitDevice", hubConnection.ConnectionId, deviceId);
                isStopped = false;
                return true;
            }

            catch (Exception error)
            {
                Console.WriteLine("Connect fail" + error.StackTrace);
                return false;
            }
        }

        public async void StopSignalrService()
        {
            await hubConnection.StopAsync();
            isStopped = true;

        }

        public async void SendMessage(SignalrRequest request)
        {
            void onTimerElapsed(Object source, ElapsedEventArgs e)
            {

                if (requests.ContainsKey(request.ID.ToString()))
                {
                    var retryRequest = requests[request.ID.ToString()];
                    if (retryRequest.SentTime < 3)
                    {
                        SendMessage(retryRequest);
                    }
                }
            }

            if (request != null)
            {
                await hubConnection.InvokeAsync("PushDataBe", this.deviceId, request.Receiver, JsonConvert.SerializeObject(request));
                AddRequest(request);
                Console.WriteLine("MsgSent");
                System.Timers.Timer retryTimer = new System.Timers.Timer()
                {
                    Interval = 1000
                };
                retryTimer.Enabled = true;
                retryTimer.Elapsed += onTimerElapsed;
                retryTimer.Start();
            }

        }

        private void AddRequest(SignalrRequest request)
        {
            if (requests != null)
            {
                if (!requests.ContainsKey(request.ID.ToString()))
                {
                    requests.Add(request.ID.ToString(), request);
                }
                requests[request.ID.ToString()].SentTime++;
            }
        }

        public interface OnMessageReceive
        {
            void OnReceive(Data data, string id);
        }
    }
}
