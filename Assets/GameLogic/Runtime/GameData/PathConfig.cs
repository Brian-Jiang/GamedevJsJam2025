using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.GameData
{
    [Serializable]
    public class PathInfo
    {
        [AssetsOnly]
        public GameObject prefab;
    }
    
    [CreateAssetMenu(fileName = "PathConfig", menuName = "GameData/PathConfig", order = 0)]
    public class PathConfig : ScriptableObject
    {
        public List<PathInfo> pathInfos;
    }
}