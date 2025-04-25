using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class Magnet : MonoBehaviour
    {
        public CircleCollider2D magnetCollider;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<PlayerCollectible>(out var playerCollectible))
            {
                playerCollectible.SetCollectTarget(transform);
            }
            
            // if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            // {
            //     switch (levelObject)
            //     {
            //         case Point point:
            //             point.target = transform;
            //             break;
            //         default:
            //             break;
            //     }
            // }
        }
        
        public void SetMagnetRadius(float radius)
        {
            magnetCollider.radius = radius;
        }
    }
}