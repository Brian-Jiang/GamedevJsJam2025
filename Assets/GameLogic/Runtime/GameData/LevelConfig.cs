using Sirenix.OdinInspector;
using SmartReference.Runtime;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.GameData
{
    [System.Serializable]
    public struct LevelData
    {
        public string levelName;
        public SceneReference scene;
    }
    
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "GameData/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        public float defaultMaxForce;
        public float defaultMaxPower;
        // public float pathTriggerThresholdDistance;
        [MaxValue(0.0)]
        public float levelObjectDestroyThresholdDistance;
        
        [MinValue(0.0)]
        public float controlForceMultiplier;
        
        [AssetsOnly]
        public SmartReference<GameObject> virtualCameraPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> pathTriggerPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> wallPrefab;

        [AssetsOnly]
        public SmartReference<GameObject> pointPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> boosterPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> spotlightPrefab;
        
        public LevelData[] levels;
    }
}