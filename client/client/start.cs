using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class start : Form
    {
        public static string UserName { set; get;}
        public start()
        {
            InitializeComponent();
        }
        void connectToServer()
        {
            TcpClient client= new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 5500);
            NetworkStream nStream= client.GetStream();
            BinaryWriter binaryWriter= new BinaryWriter(nStream);
            binaryWriter.Write(UserName);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                UserName = textBox1.Text;
                Thread thr = new Thread(() => Application.Run(new Roomgame()));
                thr.Start();
                this.Close();
                connectToServer();
            }
            else
            {
                MessageBox.Show("UserName is Required!");
            }   
        } 
    }
}
