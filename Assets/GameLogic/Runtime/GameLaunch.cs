using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime
{
    public class GameLaunch : MonoBehaviour
    {
        [AssetsOnly]
        public GameObject debugConsolePrefab;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            InitDebugConsole();
            GameFacade.Init(gameObject);
            GameFacade.GameStartUp();
        }
        
        private void InitDebugConsole() {
            if (Application.isEditor || !Debug.isDebugBuild) return;
            
            var go = Instantiate(debugConsolePrefab);
            go.name = "InGameDebugConsole";
        }
    }
}
