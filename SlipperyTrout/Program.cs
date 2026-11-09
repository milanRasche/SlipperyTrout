using SlipperyTrout.Chess;

namespace Chess.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n=== Chess Engine ===");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load FEN");
            Console.WriteLine("3. Exit");
            Console.Write("Select option: ");

            string? input = Console.ReadLine();

            GameState? game = null;

            switch (input)
            {
                case "1":
                    game = ChessGame.NewGame();
                    break;

                case "2":
                    Console.Write("Enter FEN: ");
                    string? fen = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(fen))
                    {
                        game = ChessGame.FromFEN(fen);
                    }
                    break;

                case "3":
                    running = false;
                    continue;

                default:
                    Console.WriteLine("Invalid option.");
                    continue;
            }

            if (game != null)
            {
                Console.WriteLine("\n" + GameFormatter.Format(game));
            }
        }
    }
}