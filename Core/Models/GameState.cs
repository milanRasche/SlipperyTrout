namespace Core.Models
{
    public class GameState
    {
        public string GameId { get; set; }= Guid.NewGuid().ToString();
        public string Status { get; set; } = "Active";
        public string Fen { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public List<string> Moves { get; set; } = [];
        public string SideToMove { get; set; } = "White";
        public int WhiteTimeMs { get; set; }
        public int BlackTimeMs { get; set; }
        public string? Result { get; set; }
    }
}
