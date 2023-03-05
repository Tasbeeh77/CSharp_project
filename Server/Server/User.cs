
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
    public delegate void NewClientMessageHandeler(User sender,StreamWriter Writer,StreamReader Reader,string[] streamData, Socket userConnection);
    public class User
    {
        public event NewClientMessageHandeler newClientMessage;

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Color { get; set; }
        StreamReader Reader;
        StreamWriter Writer;
        Socket userConnection;
        public NetworkStream nstream;
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
                    //MessageBox.Show(value);
                    streamData = value.Split('|');
                    newClientMessage(this, Writer, Reader, streamData, userConnection); //publish event
                    //MessageBox.Show(value+"after event");
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
