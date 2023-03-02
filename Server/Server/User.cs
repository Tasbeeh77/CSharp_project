
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
   
    /*public class UserEventArgs : EventArgs
    {
        public StreamReader Reader { get; set; }
        public StreamWriter Writer { get; set; }
        public Socket userConnection { get; set; }
        public string[] streamData { get; set; }
    }*/
    public class User
    {
        //public delegate void NewClientMessageHandeler(object sender, UserEventArgs args);
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
        public User(Socket tcpClient)
        {
            streamData = new string[10];
            userConnection = tcpClient;
            nstream = new NetworkStream(userConnection); ;
            Writer = new StreamWriter(nstream);
            Writer.AutoFlush = true;
            Reader = new StreamReader(nstream);
        }
        protected virtual void ReadMessages()
        {
            while (true)
            {
                if(nstream!=null)
                {
                    string value = Reader.ReadLine();
                    streamData = value.Split('|');
                    newClientMessage(this, Writer, Reader, streamData, userConnection);
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
