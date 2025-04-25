using Sirenix.OdinInspector;
using SmartReference.Runtime;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.GameData
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "GameData/UIConfig", order = 0)]
    public class UIConfig : ScriptableObject
    {
        [AssetsOnly]
        public SmartReference<GameObject> uiRoot;
        
        [AssetsOnly]
        public SmartReference<GameObject> eventSystemPrefab;
            
        [AssetsOnly]
        public SmartReference<GameObject> inGameUIPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> controlUIPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> endGameUIPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> levelFinishUIPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> pauseUIPrefab;
            
        [AssetsOnly]
        public SmartReference<GameObject> chooseLevelUIPrefab;
        
        [AssetsOnly]
        public SmartReference<GameObject> creditUIPrefab;
    }
}