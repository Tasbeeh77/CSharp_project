using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class gameBoard : Form
    {
        Brush brush;
        Color color;
        int roomNo;
        private Rectangle[] boardColumns;
        /************ Board Measurements *************/
        int boardWidth;
        int boardHeight;
        int XStart;
        int YStart;
        /************ Cells Measurments  ****************/
        int XSpace;
        int YSpace;
        //
        int[,] board;
        int rowN;
        int colN;
        //******************
        int MyNumber;
        int otherPlayerNo;
        int turn;
        Thread thread;
        int columnIndex;
        int rowIndex;
        Color myColor;
        bool flag = true;
        public gameBoard(string playerType)
        {
            InitializeComponent();
            this.boardColumns = new Rectangle[7];
            label1.Text = $"Player: {start.UserName}";
            rowN = 6;
            colN = 7;
            board = new int[6, 7];
            this.turn = 1;
            thread = new Thread(new ThreadStart(PositionChanged));
            thread.Start();
            roomNo = int.Parse(Roomgame.RoomNo);
            if(playerType=="watcher")
            {
               // this.Enabled= false;
                button_WOC1.Enabled = true;
                button_WOC1.Visible = true;
                label1.Text = $"Watcher: {start.UserName}";
            }
        }
        private void PositionChanged()
        {
            while (true)
            {
                if (Connection.getStream() != null)
                {
                    string message = Connection.getReader().ReadLine();
                    string[] x = message.Split('|');
                    if (x[0] == "roomNumber")
                    {
                        roomNo = int.Parse(x[1]);
                    }
                    if (x[0] == "ChangePoint") //"pointChanged|row|col|color"
                    {
                        if (x[3] == "red")
                        {
                            color = Color.Red;
                        }
                        else
                        {
                            color = Color.Yellow;
                        }
                        drawEllipse(int.Parse(x[1]), int.Parse(x[2]), color);
                    }
                }
            }
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
            for (int i = 0; i < rowN; i++)
            {
                for (int j = 0; j < colN; j++)
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
            Connection.stopWatch(roomNo.ToString());
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        void drawEllipse(int row, int col, Color playercolor)
        {
            Graphics g = this.CreateGraphics();
            brush = new SolidBrush(playercolor);
            g.FillEllipse(brush, 32 + 50 * col, 80 + (50 * row), 32, 32);
        }
        private void gameBoard_MouseClick(object sender, MouseEventArgs e)
        {
            columnIndex = this.columnNumber(e.Location);
            if (columnIndex != -1)
            {
                rowIndex = this.emptyRow(columnIndex);
                if (rowIndex != -1)
                {
                    this.board[rowIndex, columnIndex] = this.turn;
                    if (turn == 1)
                    {
                        if (chooseColor.Player1color == "red")
                        {
                            color = Color.Red;
                        }
                        else
                        {
                            color = Color.Yellow;
                        }
                        drawEllipse(rowIndex, columnIndex, color);
                        Connection.getWriter().WriteLine($"pointChanged|{Roomgame.RoomNo}|{1}|{rowIndex}|{columnIndex}");
                    }
                    else if (turn == 2)
                    {
                        if (chooseColor.Player1color == "red")
                        {
                            color = Color.Yellow;
                        }
                        else
                        {
                            color = Color.Red;
                        }
                        drawEllipse(rowIndex, columnIndex, color);
                        Connection.getWriter().WriteLine($"pointChanged|{Roomgame.RoomNo}|{2}|{rowIndex}|{columnIndex}");
                    }
                    int winner = this.Winner(this.turn);
                    if (winner != -1)
                    {
                        string player = (winner == 1) ? "Red" : "Yellow";
                        MessageBox.Show("Congratulations!" + player + " Player has won");
                        // Application.Restart();
                    }
                    if (this.turn == 1)
                    {
                        this.turn = 2;
                    }
                    else
                    {
                        this.turn = 1;
                    }

                }
            }
        }
        private int columnNumber(Point mouse)
        {
            int spaceX = 9;
            int spaceY = 3;
            for (int i = 0; i < boardColumns.Length; i++)
            {
                if ((mouse.X >= boardColumns[i].X) && (mouse.Y >= boardColumns[i].Y))
                {
                    if ((mouse.X <= this.boardColumns[i].X + spaceX * (i + 1)) &&
                     (mouse.Y <= this.boardColumns[i].Y + spaceY * (i + 1)))
                    {
                        if (i > 3)
                        {
                            spaceX = 2;
                        }
                        return i;
                    }
                }
            }
            return -1;
        }
        private int emptyRow(int col)
        {
            for (int i = 5; i >= 0; i--)
            {
                if (this.board[i, col] == 0)
                { return i; }
            }
            return -1;
        }
        /******************************************************************************/

        private bool EqualNums(int toCheck, params int[] numbers)
        {
            foreach (int num in numbers)
            {
                if (toCheck != num)
                    return false;
            }
            return true;
        }

        private int Winner(int playerToCheck)
        {
            //vertical win check
            for (int row = 0; row < this.board.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < this.board.GetLength(1); col++)
                {
                    if (this.EqualNums(playerToCheck, this.board[row, col], this.board[row + 1, col],
                        this.board[row + 2, col], this.board[row + 3, col]))
                        return playerToCheck;
                }
            }
            //horizontal win check
            for (int row = 0; row < this.board.GetLength(0); row++)
            {
                for (int col = 0; col < this.board.GetLength(1) - 3; col++)
                {
                    if (this.EqualNums(playerToCheck, this.board[row, col], this.board[row, col + 1],
                        this.board[row, col + 2], this.board[row, col + 3]))
                        return playerToCheck;
                }
            }
            //top-left diagonal check
            for (int row = 0; row < this.board.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < this.board.GetLength(1) - 3; col++)
                {
                    if (this.EqualNums(playerToCheck, this.board[row, col], this.board[row + 1, col + 1],
                        this.board[row + 2, col + 2], this.board[row + 3, col + 3]))
                        return playerToCheck;
                }
            }
            //Back-right diagonal check
            for (int row = 0; row < this.board.GetLength(0) - 3; row++)
            {
                for (int col = 3; col < this.board.GetLength(1); col++)
                {
                    if (this.EqualNums(playerToCheck, this.board[row, col], this.board[row + 1, col - 1],
                        this.board[row + 2, col - 2], this.board[row + 3, col - 3]))
                    {

                        return playerToCheck;
                    }
                }
            }

            return -1;
        }
    }
}
