using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Server
{
    public partial class serverBoard : Form
    {
        TcpListener listener;
        TcpClient client;
        BinaryReader binReader;
        BinaryWriter binWriter;
        NetworkStream nstream;
        Socket Connection;
        List<User> users = new List<User>();
        bool flag = true;
        public static int playerId { get; set; }
        public serverBoard()
        {
            InitializeComponent();
        }
        void StartConnection()
        {
            IPAddress localaddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
            listener = new TcpListener(localaddr, 5500);
            listener.Start();
            MessageBox.Show("started...");
            playerId= 0;

            Thread myThread = new Thread(() => {
                while (flag)
                {
                Connection = listener.AcceptSocket();
                nstream = new NetworkStream(Connection);
                binReader = new BinaryReader(nstream);
                string name= binReader.ReadString();
                users.Add(new User(playerId++, name));
                listView1.Items.Add(new ListViewItem($"CLIENT Name = {name} & Id = {playerId}"));
                label2.Text = $"Number of players: {users.Count}";
                binWriter = new BinaryWriter(nstream);
                binWriter.Write("Connected");
                } 
            });
            myThread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Details form = new Details();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            binReader.Close();
            binWriter.Close();
            nstream.Close();
            Connection.Shutdown(SocketShutdown.Both);
            Connection.Close();
            flag= false;
        }
    }
}
