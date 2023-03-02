using Newtonsoft.Json;
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
using System.Security.Principal;
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
        List<User> users = new List<User>();
        List<Room> availableRooms = new List<Room>();
        bool flag = true;
        int playerId;
        public serverBoard()
        {
            InitializeComponent();
        }
        void  StartConnection()
        {
            playerId= 0;
            IPAddress localaddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
            listener = new TcpListener(localaddr, 1025);
            listener.Start();
            listView1.Items.Add(new ListViewItem("The server started..."));
            Thread myThread = new Thread(()=>{
            while (flag)
            {
                Socket Connection = listener.AcceptSocket();
                User user = new User(Connection);
                if (Connection != null) 
                {
                    users.Add(user);
                }               
                listView1.Items.Add(new ListViewItem("new client entered game..."));
                user.newClientMessage += this.Item_newClientMessage;
                user.publishEvent();
            }
            });
            myThread.Start();
        }
        private void Item_newClientMessage(object sender, StreamWriter Writer, StreamReader Reader, string[] streamData, Socket Connection)
        {
            if (streamData[1] == "signIn")
            {
                signIn(streamData, Writer);
            }
            else if (streamData[4] == "createRoom")
            {
                createRoomRequest(streamData);
            }
            else if (streamData[0] == "Close")
            {
                Writer.Close();
                Reader.Close();
                Connection.Close();
            }
        }

        public void signIn(string[] streamData,StreamWriter Writer)
        {
            MessageBox.Show(users.Count.ToString());
            users[playerId].Id = playerId;
            users[playerId++].UserName = streamData[0];
            listView1.Items.Add(new ListViewItem($"CLIENT Name = {streamData[0]} & Id = {playerId}"));
            label2.Text = $"Number of players: {users.Count}";
            //sending room data to stream 
            //displayAvailableRooms(Writer);
        }
        public void createRoomRequest(string[] streamData)
        {
            Room room= new Room(streamData[0], int.Parse(streamData[1]), int.Parse(streamData[2]), streamData[3],1,users);
            availableRooms.Add(room);
            foreach (var item in streamData)
            { 
              listView1.Items.Add(new ListViewItem($"Room Data = {item}"));
            }
        }
        void displayAvailableRooms(StreamWriter Writer)
        {
            if (availableRooms.Count > 0)
            {
                MessageBox.Show($"roomData from server: {availableRooms[0].roomIndex} {availableRooms[0].row} {availableRooms[0].col} {availableRooms[0].players.Count}");
                Writer.WriteLine($"roomData,{availableRooms[0].roomIndex}|{availableRooms[0].row}|{availableRooms[0].col}|{availableRooms[0].players.Count}");
                listView1.Items.Add(new ListViewItem("room data sent done"));
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
            this.Close();
            flag = false;
        }
        private void serverBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
            flag = false;
        }
    }
}
