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
            ListenToServer();
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
            try
            {
                await _connection.StartAsync();
                lbl_Events.Text = $"Connected!";

                btn_SendMessage.Enabled = true;
                btn_Disconnect.Enabled = true;
                btn_Connect.Enabled = false;

                txt_Message.TextChanged += OnMessageTextChanged;
                cb_AddToVIP.CheckedChanged += OnCheckedChanged;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                lbl_Events.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lbl_Events.Text = $"Connection failed. Exception: {ex.Message}";
            }
        }
        private async void OnDisconnectClick(object sender, EventArgs e)
        {
            await _connection.StopAsync();
            lbl_Events.Text = "Disconnected!";
            btn_SendMessage.Enabled = false;
            btn_Disconnect.Enabled = false;
            btn_Connect.Enabled = true;

            txt_Message.TextChanged -= OnMessageTextChanged;
            cb_AddToVIP.CheckedChanged -= OnCheckedChanged;
        }
        private async void OnSendMessageClick(object sender, EventArgs e)
        {
            await _connection.InvokeAsync("SendMessage", txt_Message.Text, _name, _isVIP);

            if (lbl_Messages.Text == "No messages yet!")
                lbl_Messages.Text = string.Empty;

            if (_isVIP)
            {
                lbl_Messages.Text += $"I said to VIPs: {txt_Message.Text}{Environment.NewLine}";
            }
            else
            {
                lbl_Messages.Text += $"I said: {txt_Message.Text}{Environment.NewLine}";
            }

            txt_Message.Text = string.Empty;
        }
        private async void OnCheckedChanged(object sender, EventArgs e)
        {
            _isVIP = cb_AddToVIP.Checked;
            ParentForm.Text = _isVIP ? $"{_name} (VIP)" : _name;

            if (_isVIP)
            {
                await _connection.InvokeAsync("AddToVIPGroup", _name);
                lbl_Events.Text = "You entered the VIP group!";
            }
            else
            {
                await _connection.InvokeAsync("RemoveFromVIPGroup", _name);
                lbl_Events.Text = "You left the VIP group!";
            }
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

        #endregion

        #region Helpers

        private void InitializeControl(string name)
        {
            btn_Disconnect.Enabled = false;
            btn_SendMessage.Enabled = false;

            _connection = new HubConnectionBuilder().WithUrl("https://localhost:44348/chatHub").Build();
            _timer = new Timer() { Interval = 1500 };
            _isVIP = false;
            _name = name;
        }
        private void SubscribeToEvents()
        {
            btn_SendMessage.Click += OnSendMessageClick;
            btn_Disconnect.Click += OnDisconnectClick;
            _timer.Tick += OnIntervalTimeElapsed;
            btn_Connect.Click += OnConnectClick;
        }
        private void ListenToServer()
        {
            _connection.On<int>("ClientJoined", (c) => ClientJoined(c));
            _connection.On<int>("ClientLeft", (c) => ClientLeft(c));
            _connection.On<string, string, bool>("BroadcastMessage", (c, m, v) => MessageReceived(m, c, v));
            _connection.On<string>("AddedToVIPGroup", (c) => AddedToVIPGroup(c));
            _connection.On<string>("RemovedFromVIPGroup", (c) => RemovedFromVIPGroup(c));
            _connection.On<string, bool>("IsTyping", (c, v) => ClientIsTyping(c, v));
            _connection.On("ClearMessages", () => ClearMessages());
            _connection.On("HelloFromServer", () => HelloFromServer());
        }
        private void ClientJoined(int numberOfClients)
        {
            lbl_Events.Text = $"A new client entered the chat. Total clients: {numberOfClients}";
        }
        private void ClientLeft(int numberOfClients)
        {
            lbl_Events.Text = $"A client left the chat. Total clients: {numberOfClients}";
        }
        private void MessageReceived(string message, string client, bool isVIPMessage)
        {
            if (lbl_Messages.Text == "No messages yet!")
                lbl_Messages.Text = string.Empty;

            if (isVIPMessage)
            {
                lbl_Messages.Text += client + " says to VIPs: " + message + Environment.NewLine;
            }
            else
            {
                lbl_Messages.Text += client + " says: " + message + Environment.NewLine;
            }
        }
        private void AddedToVIPGroup(string client)
        {
            lbl_Events.Text = $"{client} joins the VIP group!";
        }
        private void RemovedFromVIPGroup(string client)
        {
            lbl_Events.Text = $"{client} left the VIP group!";
        }
        private void ClientIsTyping(string client, bool isTyping)
        {
            if (isTyping)
                lbl_Events.Text = $"{client} is typing...";
            else
                lbl_Events.Text = string.Empty;
        }
        private void ClearMessages()
        {
            lbl_Messages.Text = "Server commands to delete all the messages!";
        }
        private void HelloFromServer()
        {
            if (lbl_Messages.Text == "No messages yet!")
                lbl_Messages.Text = string.Empty;

            lbl_Messages.Text += "Server greets you!" + Environment.NewLine;
        }

        #endregion    
    }
}
