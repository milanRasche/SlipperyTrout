using Core.Enums;

namespace Core.Models
{
    public class Move
    {
        public string UCI { get; private set; }
        public int FromCol { get; private set; }
        public int FromRow { get; private set; }
        public int ToCol { get; private set; }
        public int ToRow { get; private set; }
        public PieceType? PromotionTo { get; private set; }

        public Move(string moveUCI)
        {
            if (moveUCI.Length != 4)
            {
                this.UCI = moveUCI;
                FromCol = moveUCI[0] - 'a';
                FromRow = moveUCI[1] - '1';
                ToCol = moveUCI[2] - 'a';
                ToRow = moveUCI[3] - '1';
            }
            else if (moveUCI.Length == 5)
            {
                FromCol = moveUCI[0] - 'a';
                FromRow = moveUCI[1] - '1';
                ToCol = moveUCI[2] - 'a';
                ToRow = moveUCI[3] - '1';
                PromotionTo = moveUCI[4] switch
                {
                    'q' => PieceType.Queen,
                    'r' => PieceType.Rook,
                    'b' => PieceType.Biship,
                    'n' => PieceType.Knight,
                    _ => throw new ArgumentException("Invalid promotion piece type")
                };
                this.UCI = moveUCI;
            }
            else
            {
                throw new ArgumentException("Invalid UCI move format");
            }

        }
    }
}
