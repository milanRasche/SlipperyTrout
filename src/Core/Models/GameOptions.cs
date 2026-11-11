using System;
namespace Core.Models
{
    public class GameOptions
    {
        public string Provider { get; set; } = "Local";
        public string GameMode { get; set; } = "PlayerVsEngine";
        public int InitialTimeSeconds { get; set; } = 300;
        public int IncrementSeconds { get; set; } = 0;
    }
}
