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
        string[] streamData =new string[10];
        List<User> users = new List<User>();
        List<Room> availableRooms = new List<Room>();
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
            //MessageBox.Show("started...");
            playerId= 0;
            Thread myThread = new Thread(() => {
                while (flag)
                {
                    Connection = listener.AcceptSocket();
                    nstream = new NetworkStream(Connection);
                    binReader = new BinaryReader(nstream);
                    string data = binReader.ReadString();
                    streamData = data.Split('|');
                    //MessageBox.Show(streamData[1]);
                    if (streamData[1] == "signIn") 
                    {
                        signIn();
                    }
                    else if (streamData[4] == "createRoom")
                    {
                        createRoomRequest();
                    }
                    
                } 
            });
            myThread.Start();
        }
        void signIn()
        {
            users.Add(new User(playerId++, streamData[0]));
            listView1.Items.Add(new ListViewItem($"CLIENT Name = {streamData[0]} & Id = {playerId}"));
            label2.Text = $"Number of players: {users.Count}";
            binWriter = new BinaryWriter(nstream);
            binWriter.Write("Connected");
        }
        void createRoomRequest()
        {
            Room room= new Room(streamData[0], int.Parse(streamData[1]), int.Parse(streamData[2]), streamData[3],1);
            availableRooms.Add(room);
            foreach (var item in streamData)
            { 
              listView1.Items.Add(new ListViewItem($"Room Data = {item} "));
            }
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

        private void serverBoard_Leave(object sender, EventArgs e)
        {
            binReader.Close();
            binWriter.Close();
            nstream.Close();
            Connection.Shutdown(SocketShutdown.Both);
            Connection.Close();
            flag = false;
        }
    }
}
