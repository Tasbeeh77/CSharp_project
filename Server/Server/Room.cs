using System.Collections.Generic;

namespace Server
{
    public class Room
    {
        public string player1Name { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        string player1Color;
        public int roomIndex { get; set; }
        public List <User> players { get; set; }
        public Room()
        {
            roomIndex = 0;
        }
        public Room(string player1Name,int row, int col, string player1Color,int roomIndex, List<User> users)
        {
            this.player1Name = player1Name;
            this.row = row; 
            this.col = col;
            this.player1Color = player1Color;
            this.roomIndex = roomIndex;
            this.players = users;
        }
    }
}
