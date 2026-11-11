namespace Core.Models
{
    public class GameState
    {
        public string GameId { get; set; }= Guid.NewGuid().ToString();
        public string Status { get; set; } = "Active";
        public Piece?[,] Board { get; set; } = new Piece?[8, 8];
        public List<Move> Moves { get; set; } = [];
        public string SideToMove { get; set; } = "White";
        public int WhiteTimeMs { get; set; }
        public int BlackTimeMs { get; set; }
        public string? Result { get; set; }
    }
}
