using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.GameData
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "GameData/PlayerConfig", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [AssetsOnly]
        public GameObject playerPrefab;
    }
}