﻿
namespace ChatClient
{
    partial class ChatRoom
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatRoom));
            this.txt_Message = new System.Windows.Forms.TextBox();
            this.lbl_AddToVIP = new System.Windows.Forms.Label();
            this.cb_AddToVIP = new System.Windows.Forms.CheckBox();
            this.btn_SendMessage = new System.Windows.Forms.Button();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.btn_Disconnect = new System.Windows.Forms.Button();
            this.lbl_Events = new System.Windows.Forms.Label();
            this.lbl_Messages = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_Message
            // 
            resources.ApplyResources(this.txt_Message, "txt_Message");
            this.txt_Message.Name = "txt_Message";
            // 
            // lbl_AddToVIP
            // 
            resources.ApplyResources(this.lbl_AddToVIP, "lbl_AddToVIP");
            this.lbl_AddToVIP.Name = "lbl_AddToVIP";
            // 
            // cb_AddToVIP
            // 
            resources.ApplyResources(this.cb_AddToVIP, "cb_AddToVIP");
            this.cb_AddToVIP.Name = "cb_AddToVIP";
            this.cb_AddToVIP.UseVisualStyleBackColor = true;
            // 
            // btn_SendMessage
            // 
            resources.ApplyResources(this.btn_SendMessage, "btn_SendMessage");
            this.btn_SendMessage.Name = "btn_SendMessage";
            this.btn_SendMessage.UseVisualStyleBackColor = true;
            // 
            // btn_Connect
            // 
            resources.ApplyResources(this.btn_Connect, "btn_Connect");
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            // 
            // btn_Disconnect
            // 
            resources.ApplyResources(this.btn_Disconnect, "btn_Disconnect");
            this.btn_Disconnect.Name = "btn_Disconnect";
            this.btn_Disconnect.UseVisualStyleBackColor = true;
            // 
            // lbl_Events
            // 
            resources.ApplyResources(this.lbl_Events, "lbl_Events");
            this.lbl_Events.Name = "lbl_Events";
            // 
            // lbl_Messages
            // 
            resources.ApplyResources(this.lbl_Messages, "lbl_Messages");
            this.lbl_Messages.Name = "lbl_Messages";
            // 
            // ChatRoom
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_Messages);
            this.Controls.Add(this.lbl_Events);
            this.Controls.Add(this.btn_Disconnect);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.btn_SendMessage);
            this.Controls.Add(this.cb_AddToVIP);
            this.Controls.Add(this.lbl_AddToVIP);
            this.Controls.Add(this.txt_Message);
            this.Name = "ChatRoom";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_Message;
        private System.Windows.Forms.Label lbl_AddToVIP;
        private System.Windows.Forms.CheckBox cb_AddToVIP;
        private System.Windows.Forms.Button btn_SendMessage;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Button btn_Disconnect;
        private System.Windows.Forms.Label lbl_Events;
        private System.Windows.Forms.Label lbl_Messages;
    }
}
