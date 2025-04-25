using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class PlayerController
    {
        public float Power { get; set; }
        public int Score { get; set; }
        
        public GameLevelManager GameLevelManager { get; }
        
        public PlayerController(GameLevelManager gameLevelManager)
        {
            GameLevelManager = gameLevelManager;
        }
        
        public void AddScore(int score)
        {
            Score += score;
        }
    }
}