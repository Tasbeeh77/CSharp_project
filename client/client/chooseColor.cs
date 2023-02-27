using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace client
{
    public partial class chooseColor : Form
    {
        public static string Player1color { get; set; }

        public chooseColor()
        {
            InitializeComponent();
        }
        void connectToServer()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 5500);
            NetworkStream nStream = client.GetStream();
            BinaryWriter binaryWriter = new BinaryWriter(nStream);
            binaryWriter.Write($"{start.UserName}|{CreateRoom.Row}|{CreateRoom.Col}|{Player1color}|createRoom");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread thr = new Thread(() => Application.Run(new gameBoard()));
            thr.Start();
            connectToServer();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Player1color = "red";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Player1color = "yellow";
        }
    }
}
