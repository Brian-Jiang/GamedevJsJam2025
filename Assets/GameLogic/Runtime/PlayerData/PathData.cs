using System;

namespace CoinDash.GameLogic.Runtime.PlayerData
{
    [Serializable]
    public class PathData : LevelObjectData
    {
        public int pathId;
        public float length;
    }
}