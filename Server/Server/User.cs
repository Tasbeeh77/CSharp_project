
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Server
{
    public delegate void NewClientMessageHandeler(object sender,StreamWriter Writer,StreamReader Reader,string[] streamData, Socket userConnection);
    public class User
    {
        public event NewClientMessageHandeler newClientMessage;

        public int Id { get; set; }
        public string UserName { get; set; }
        public static string Color { get; set; }
        public static string CurrentRoom { get; set; }
        StreamReader Reader;
        StreamWriter Writer;
        Socket userConnection;
        NetworkStream nstream;
        string[] streamData;
        public User(Socket socket)
        {
            streamData = new string[10];
            userConnection = socket;
            nstream = new NetworkStream(userConnection); ;
            Writer = new StreamWriter(nstream);
            Writer.AutoFlush = true;
            Reader = new StreamReader(nstream);
        }
        async protected virtual void ReadMessages()
        {
            while (true)
            {
                if(nstream!=null)
                {
                    string value =await Reader.ReadLineAsync();
                    streamData = value.Split('|');
                    newClientMessage(this, Writer, Reader, streamData, userConnection); //publish event
                    nstream.Flush();
                }
            }
        }
        public void publishEvent()
        {
             ReadMessages();
        }
    }
}
