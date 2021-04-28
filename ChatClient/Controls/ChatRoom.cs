using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClient
{
    public partial class ChatRoom : UserControl
    {
        public ChatRoom(string name)
        {
            InitializeComponent();
            InitializeControl(name);
            SubscribeToEvents();
        }

        #region Locals

        private HubConnection _connection;
        private string _name;
        private Timer _timer;
        private bool _isVIP;

        #endregion

        #region Handlers

        private async void OnConnectClick(object sender, EventArgs e)
        {
            UpdateState(false);

            _connection = new HubConnectionBuilder().WithUrl("https://localhost:44348/chatHub").Build();
            ListenToServer();

            Log(Color.Gray, "Staring connection...");

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.Message);
            }

            Log(Color.Gray, "Connection established.");
            UpdateState(connected: true);
            txt_Message.Focus();
        }
        private async void OnDisconnectClick(object sender, EventArgs e)
        {
            Log(Color.Gray, "Stopping connection...");

            try
            {
                await _connection.StopAsync();
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.Message);
            }

            lb_Messages.Items.Clear();
            Log(Color.Gray, "Connection terminated.");
            UpdateActiveClients(0);
            UpdateState(connected: false);
        }
        private async void OnSendMessageClick(object sender, EventArgs e)
        {
            try
            {
                await _connection.InvokeAsync("SendMessage", txt_Message.Text, _name, _isVIP);
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.Message);
            }

            var txt = _isVIP ? "VIP" : "All";
            Log(Color.CornflowerBlue, $"Me ({txt}): {txt_Message.Text}");
            txt_Message.Text = string.Empty;
        }
        private async void OnCheckedChanged(object sender, EventArgs e)
        {
            _isVIP = cb_AddToVIP.Checked;
            ParentForm.Text = _isVIP ? $"{_name} (VIP)" : _name;

            if (_isVIP)
            {
                await _connection.InvokeAsync("AddToVIPGroup", _name);
            }
            else
            {
                await _connection.InvokeAsync("RemoveFromVIPGroup", _name);
            }

            string value = _isVIP ? "enter" : "left";
            Log(Color.Green, $"I {value} the VIP room!");
        }
        private async void OnMessageTextChanged(object sender, EventArgs e)
        {
            _timer.Start();
            await _connection.InvokeAsync("ClientIsTyping", _name, true, _isVIP);
        }
        private async void OnIntervalTimeElapsed(object sender, EventArgs e)
        {
            _timer.Stop();
            await _connection.InvokeAsync("ClientIsTyping", _name, false, _isVIP);
        }
        private void OnNewMessageLog(object sender, DrawItemEventArgs e)
        {
            var message = (LogMessage)lb_Messages.Items[e.Index];
            e.Graphics.DrawString(message.Content, lb_Messages.Font, new SolidBrush(message.MessageColor), e.Bounds);
        }

        #endregion

        #region Helpers

        private void InitializeControl(string name)
        {
            UpdateState(false);
            _timer = new Timer() { Interval = 1500 };
            _isVIP = false;
            _name = name;
        }
        private void SubscribeToEvents()
        {
            lb_Messages.DrawItem += OnNewMessageLog;
            cb_AddToVIP.CheckedChanged += OnCheckedChanged;
            btn_Connect.Click += OnConnectClick;
            btn_Disconnect.Click += OnDisconnectClick;
            btn_SendMessage.Click += OnSendMessageClick;
            txt_Message.TextChanged += OnMessageTextChanged;
            _timer.Tick += OnIntervalTimeElapsed;
        }

        private void UpdateState(bool connected)
        {
            btn_Disconnect.Enabled = connected;
            btn_Connect.Enabled = !connected;
            txt_Message.Enabled = connected;
            btn_SendMessage.Enabled = connected;
            cb_AddToVIP.Enabled = connected;
            lb_Messages.Enabled = connected;
        }
        private void Log(Color color, string message)
        {
            lb_Messages.Items.Add(new LogMessage(color, message));
        }

        private void ListenToServer()
        {
            _connection.On<int>("UpdateActiveClients", (c) => UpdateActiveClients(c));
            _connection.On<string, string, bool>("BroadcastMessage", (c, m, v) => MessageReceived(m, c, v));
            _connection.On<string>("AddedToVIPGroup", (c) => AddedToVIPGroup(c));
            _connection.On<string>("RemovedFromVIPGroup", (c) => RemovedFromVIPGroup(c));
            _connection.On<string, bool>("IsTyping", (c, v) => ClientIsTyping(c, v));
            _connection.On("ClearMessages", () => ClearMessages());
            _connection.On("HelloFromServer", () => HelloFromServer());
        }

        private void UpdateActiveClients(int numberOfClients)
        {
            lbl_ActiveClient.Text = $"Active clients: {numberOfClients}";
        }

        private void MessageReceived(string message, string client, bool isVIPMessage)
        {
            var txt = isVIPMessage ? "VIP" : "All";
            Log(Color.Black, client + $" ({txt}): " + message);
        }

        private void AddedToVIPGroup(string client)
        {
            Log(Color.DarkGray, $"{client} joins the VIP room!");
        }
        private void RemovedFromVIPGroup(string client)
        {
            Log(Color.DarkGray, $"{client} left the VIP room!");
        }

        private void ClientIsTyping(string client, bool isTyping)
        {
            lbl_Events.Text = isTyping ? $"{client} is typing..." : string.Empty;
        }

        private void ClearMessages()
        {
            Log(Color.DarkGray, "This is a message from the server!");
        }
        private void HelloFromServer()
        {
            Log(Color.DarkGray, "Server message for VIPs only!");
        }

        #endregion
    }
}
