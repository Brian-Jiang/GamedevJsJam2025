using UnityEngine;

namespace CoinDash.GameLogic.Runtime.PlayerData
{
    [System.Serializable]
    public class SpotLightData : LevelObjectData
    {
        public Color color;
        public float intensity;
        public float innerAngle;
        public float outerAngle;
        public float innerRadius;
        public float outerRadius;
    }
}