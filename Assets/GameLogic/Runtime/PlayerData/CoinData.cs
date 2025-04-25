using System;

namespace CoinDash.GameLogic.Runtime.PlayerData
{
    [Serializable]
    public class CoinData : LevelObjectData
    {
        public int coinId;
        public bool isActiveCoin;
    }
}