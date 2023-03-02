using Newtonsoft.Json.Bson;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace client
{
    public partial class start : Form
    {
        public static string UserName { set; get;}
        public start()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                UserName = textBox1.Text;
                Thread thr = new Thread(() => Application.Run(new Roomgame()));
                thr.Start();
                this.Close();
                Connection obj = new Connection();
                obj.sendUserName(UserName);
            }
            else
            {
                MessageBox.Show("UserName is Required!");
            }   
        } 
    }
}
