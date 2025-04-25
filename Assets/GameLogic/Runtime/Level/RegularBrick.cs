using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class RegularBrick : LevelObject
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player player:
                        // var go = Instantiate(lightBallLightPrefab, player.transform);
                        // Destroy(go, 5f);
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}