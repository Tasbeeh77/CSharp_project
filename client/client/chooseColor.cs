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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace client
{
    public partial class chooseColor : Form
    {
        public static string Player1color { set; get; }
        string sendAs;
        public chooseColor(string type)
        {
            InitializeComponent();
            sendAs= type;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread thr = new Thread(() => Application.Run(new gameBoard("player")));
            thr.Start();
            if (sendAs== "createRoom")
               Connection.sendRoomData(Player1color);
            else
                Connection.join(Roomgame.RoomNo);
            this.Close();
            Connection.getWriter().WriteLine($"PlayersData|{start.UserName}");
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
