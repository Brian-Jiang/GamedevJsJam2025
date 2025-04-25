using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class LightBall : LevelObject
    {
        [AssetsOnly]
        public GameObject lightBallLightPrefab;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player player:
                        var go = Instantiate(lightBallLightPrefab, player.transform);
                        Destroy(go, 5f);
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}