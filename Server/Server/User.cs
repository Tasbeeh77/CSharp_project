
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
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Server
{
    public class User
    {
        TcpClient tcpClient;
        public static int Id { get; set; }
        public static string UserName { get; set; }
        public static string Color { get; set; }
        public static string CurrentRoom { get; set; }
        public User(int id, string name)
        {
            Id = id;
            UserName = name;
        }
        public User()
        {

        }
        void sum() { }
    }
}
