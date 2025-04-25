using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.GameData
{
    [Serializable]
    public struct CoinInfo
    {
        [AssetsOnly]
        public GameObject prefab;
    }
    
    [CreateAssetMenu(fileName = "CoinConfig", menuName = "GameData/CoinConfig", order = 0)]
    public class CoinConfig : ScriptableObject
    {
        public List<CoinInfo> coinInfos;
    }
}