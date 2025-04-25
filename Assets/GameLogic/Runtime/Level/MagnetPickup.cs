using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class MagnetPickup : LevelObject
    {
        public float magnetTime = 5f;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player player:
                        // var go = Instantiate(lightBallLightPrefab, player.transform);
                        // Destroy(go, 5f);
                        player.AddMagnetEffect(magnetTime);
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}