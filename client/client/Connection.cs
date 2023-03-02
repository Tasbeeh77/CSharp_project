using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace client
{
    public class Connection
    {
        TcpClient client;
        NetworkStream nStream;
        static StreamReader Reader;
        static StreamWriter Writer;
        public Connection() 
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 1025);
            nStream = client.GetStream();
            Writer = new StreamWriter(nStream);
            Writer.AutoFlush= true;
            Reader = new StreamReader(nStream);
        }
        public void sendUserName(string UserName)
        {
            Writer.WriteLine($"{UserName}|signIn");
        }
        public static void sendRoomData(string Player1color)
        {
           Writer.WriteLine($"{start.UserName}|{CreateRoom.Row}|{CreateRoom.Col}|{Player1color}|createRoom");
        }
        public static StreamReader displayRooms()
        {
            return Reader;
        } 
        public static void ClosingForm()
        {
            Writer.WriteLine($"Close|send");
        }
    }
}
