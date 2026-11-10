
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
                        try
                        {
                            game = ChessGame.FromFEN(fen);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Invalid FEN: {ex.Message}");
                        }
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
                PlayGame(game);
            }
        }
    }

    static void PlayGame(GameState game)
    {
        ChessGame chessGame = new ChessGame();

        bool playing = true;

        while (playing)
        {
            Console.WriteLine("\n" + GameFormatter.Format(game));

            Console.WriteLine(
                game.WhiteToMove ? "\nWhite to move." : "\nBlack to move.");

            Console.Write("Enter move (e.g. e2e4), or 'exit': ");

            string? move = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(move))
                continue;

            if (move.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                playing = false;
                continue;
            }

            try
            {
                game = chessGame.MakeMove(game, move);
                Console.WriteLine("Move successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Move rejected: {ex.Message}");
            }
        }
    }
}
