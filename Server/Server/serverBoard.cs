using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        int roomsCount = 1;

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
                HandleExceptionOnControls("new client entered game...","listView1");
                user.newClientMessage += this.Item_newClientMessage; //subscribe to newClientMessage event
                user.publishEvent();
            }
            });
            myThread.Start();
        }
        private void Item_newClientMessage(object sender, StreamWriter Writer, StreamReader Reader, string[] streamData, Socket Connection)
        {
            if (streamData[0] == "Close")
            {
                Writer.Close();
                Reader.Close();
                Connection.Close();
            }
            else if (streamData[0]=="join")
            {
                joinGameRequest(streamData);
            }
            else if (streamData[0] == "watch")
            {
                watchGameRequest(streamData);
            }
            else if (streamData[1] == "signIn")
            {
                signIn(streamData, Writer);
            }
            else if (streamData[4] == "createRoom")
            {
                createRoomRequest(streamData);
            }
        }
        private void watchGameRequest(string[] streamData)
        {
            int roomIndex = int.Parse(streamData[1])-1;
            availableRooms[roomIndex].watchers.Add(users[users.Count - 1]);
            MessageBox.Show("watch data done");
        }
        private void joinGameRequest(string[] streamData)
        {
            int roomIndex = int.Parse(streamData[1])-1;
            availableRooms[roomIndex].player2Name = streamData[2];
            availableRooms[roomIndex].players.Add(users[users.Count - 1]);
            availableRooms[roomIndex].setPlayer2Color();
            MessageBox.Show("join data done");
        }
        public void signIn(string[] streamData,StreamWriter Writer)
        {
            users[playerId].Id = playerId;
            users[playerId++].UserName = streamData[0];
            HandleExceptionOnControls($"CLIENT Name = {streamData[0]} & Id = {playerId}", "listView1");
            HandleExceptionOnControls($"Number of players: {users.Count}", "label2");
            displayAvailableRooms(Writer);
        }
        public void createRoomRequest(string[] streamData)
        {
            Room room= new Room(streamData[0], int.Parse(streamData[1]), int.Parse(streamData[2]), streamData[3],roomsCount++, users[users.Count - 1]);
            availableRooms.Add(room);
            foreach (var item in streamData)
            {
                HandleExceptionOnControls($"Room Data = {item}", "listView1");
            }
        }
        void displayAvailableRooms(StreamWriter Writer)
        {
            string allRooms = "";
            if (availableRooms.Count > 0)
            {
                foreach (var item in availableRooms)
                {
                    allRooms+=$"{item.roomIndex}|{item.row}|{item.col}|{item.players.Count}&";
                }
                MessageBox.Show(allRooms);
                Writer.WriteLine(allRooms);
                HandleExceptionOnControls("room data sent done", "listView1");
            }
        }
        void HandleExceptionOnControls(string message, string control)
        {
            if (control == "listView1")
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke(new MethodInvoker(() =>
                    {
                        listView1.Items.Add(new ListViewItem(message));
                    }));
                }
            }
            else
            {
                if (label2.InvokeRequired)
                {
                    label2.Invoke(new MethodInvoker(() =>
                    {
                        label2.Text = message;
                    }));
                }
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
