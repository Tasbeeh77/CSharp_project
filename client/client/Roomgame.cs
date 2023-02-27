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

namespace client
{
    public partial class Roomgame : Form
    {
        List<TextBox> textBoxes= new List<TextBox>();
        List<Button> JoinButtons = new List<Button>();
        List<Button> watchButtons = new List<Button>();
        public static List<TextBox> TextBoxes
        {
            get{return TextBoxes;}
        }
        public Roomgame()
        {
            InitializeComponent();   
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread thr = new Thread(() => Application.Run(new CreateRoom()));
            thr.Start();
            TextBox text1 = new TextBox();
            text1.Location = new Point(100, 84);
            text1.Size = new Size(300, 42);
            textBoxes.Add(text1);
            this.Controls.Add(text1);

            Button join = new Button();
            join.Location = new Point(430, 84);
            join.Size = new Size(100, 55);
            join.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            join.Font = new Font("Microsoft Sans Serif", 16.2F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            join.ForeColor = SystemColors.ButtonFace;
            join.Text = "Join";
            join.Click += Join_Click;
            JoinButtons.Add(join);
            this.Controls.Add(join);

            Button watch= new Button();
            watch.Location = new Point(550, 84);
            watch.Size = new Size(119, 55);
            watch.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            watch.Font = new Font("Microsoft Sans Serif", 16.2F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            watch.ForeColor = SystemColors.ButtonFace;
            watch.Text = "Watch";
            watch.Click += Watch_Click;
            watchButtons.Add(watch);
            this.Controls.Add(watch);
        }

        private void Watch_Click(object sender, EventArgs e)
        {
        }

        private void Join_Click(object sender, EventArgs e)
        {
        }

        private void Roomgame_Load(object sender, EventArgs e)
        {
            this.Text = $"Welcome, {start.UserName}. we wish you Enjoy the Game!";
        }
    }
}
