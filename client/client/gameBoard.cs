using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class gameBoard : Form
    {
        private Rectangle[] boardColumns;
        /************ Board Measurements *************/
        int boardWidth;
        int boardHeight;
        int XStart;
        int YStart;
        /************ Cells Measurments  ****************/
        int XSpace;
        int YSpace;
        public gameBoard()
        {
            InitializeComponent();
            this.boardColumns = new Rectangle[7];
            label1.Text = $"Player: {start.UserName}";
        }
        public gameBoard(string username)
        {
            InitializeComponent();
            this.boardColumns = new Rectangle[7];
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            DisplayBoard();
        }
        public void DisplayBoard()
        {
            Graphics g = this.CreateGraphics();
            boardHeight = 300;
            boardWidth = 350;
            XStart = 24;
            YStart = 70;
            XSpace = 50;
            YSpace = 50;

            g.FillRectangle(Brushes.Blue, XStart, YStart, boardWidth, boardHeight);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0)
                    {
                        this.boardColumns[j] = new Rectangle(32 + XSpace * j, YStart, 32, boardHeight);
                    }
                    g.FillEllipse(Brushes.White, 32 + XSpace * j, 80 + (YSpace * i), 32, 32);
                }
            }
        }
        private void button_WOC1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
