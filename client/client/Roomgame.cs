using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace client
{
    public partial class Roomgame : Form
    {
        string RoomNo;
        public Roomgame()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread thr = new Thread(() => Application.Run(new CreateRoom()));
            thr.Start();
        }
        private void Roomgame_Load(object sender, EventArgs e)
        {
            this.Text = $"Welcome, {start.UserName}. we wish you Enjoy the Game!";
        }
        async private void button1_Click(object sender, EventArgs e)
        {
            string[] roomsData = new string[10];
            string[] room = new string[5];
            string data = await Connection.displayRooms().ReadLineAsync();
            //MessageBox.Show("client read : "+data);
            roomsData = data.Split('&');
            MessageBox.Show(roomsData[0]);
            foreach (var item in roomsData)
            {
                room = item.Split('|');
                listView1.Items.Add(new ListViewItem($"RoomNo: {room[0]} & numbers of players : {room[3]}"));
            }
            
        }
        private void button4_Click(object sender, EventArgs e)//watch
        {
            RoomNo = textBox1.Text;
            Connection.watch(RoomNo);
        }
        private void button2_Click(object sender, EventArgs e) //join
        {
            RoomNo = textBox1.Text;
            Connection.join(RoomNo);
        }
        private void Roomgame_FormClosing(object sender, FormClosingEventArgs e)
        {
           Connection.ClosingForm();
        }

        private void Roomgame_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
