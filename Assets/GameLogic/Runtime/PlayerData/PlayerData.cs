using System;
using System.Collections.Generic;

namespace CoinDash.GameLogic.Runtime.PlayerData
{
    [Serializable]
    public class PolymorphicWrapper
    {
        public string type;
        public string json;
    }
    
    [Serializable]
    public class PlayerData
    {
        public float power;
        public int score;
        public float maxForce;
        public float topOffset;
        public List<PolymorphicWrapper> levelObjects = new();
    }
}