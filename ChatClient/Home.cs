using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            btn_NewClient.Enabled = false;

            txt_Name.TextChanged += (sender, args) => 
            {
                btn_NewClient.Enabled = txt_Name.Text != null && txt_Name.Text.Length > 0 ;
            };
            txt_Name.KeyPress += (sender, args) =>
            {
                if (args.KeyChar == 13) 
                    CreateNewClient();
            };
            btn_NewClient.Click += (sender, args) => 
            {
                CreateNewClient();
            };

            string[] defaultClients = new string[] { "Ioannis", "Dennis", "Theop" };
            for(int i = 0; i <= 2; i++)
            {
                CreateNewClient(defaultClients[i]);
            }
        }

        #region Helpers

        private void CreateNewClient(string name = null)
        {
            string clientName = name;

            if (clientName == null)
                clientName = txt_Name.Text;

            var newForm = new Form
            {
                Size = new Size(770, 470),
                FormBorderStyle = FormBorderStyle.FixedSingle,
                Text = clientName,
                StartPosition = FormStartPosition.CenterParent
            };
            newForm.Controls.Add(new ChatRoom(clientName) { Dock = DockStyle.Fill });
            newForm.Show();
            txt_Name.Text = string.Empty;
        }

        #endregion
    }
}
