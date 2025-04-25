using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class SecretPickup : LevelObject
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player player:
                        // player.AddShieldEffect(shieldTime);
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}