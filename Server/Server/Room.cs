namespace Server
{
    public class Room
    {
        public string id { get; set; }
        string player1Name;
        int row;
        int col;
        string player1Color;
        int roomIndex;
        public Room()
        {

        }
        public Room(string player1Name,int row, int col, string player1Color,int roomIndex)
        {
            this.player1Name = player1Name;
            this.row = row; 
            this.col = col;
            this.player1Color = player1Color;
            this.roomIndex = roomIndex;
        }
    }
}
