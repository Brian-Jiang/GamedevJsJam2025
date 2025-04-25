using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class FlashBrick : LevelObject
    {
        // [AssetsOnly]
        public GameObject flashBrickLight;

        private void Start()
        {
            flashBrickLight.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player player:
                        flashBrickLight.SetActive(true);
                        // var go = Instantiate(flashBrickLightPrefab, transform);
                        // Destroy(go, 5f);
                        // Destroy(gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}