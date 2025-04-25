using CoinDash.GameLogic.Runtime.PlayerData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class Wall : LevelObject
    {
        public SpriteRenderer spriteRenderer;
        public BoxCollider2D boxCollider2D;
        
        public Vector2 Size
        {
            get => spriteRenderer.size;
            private set
            {
                spriteRenderer.size = value;
                boxCollider2D.size = value;
            }
        }

        private void Start()
        {
            // boxCollider2D.size = Size;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player coin:
                        // var velocity = other.relativeVelocity;
                        // Physics2D.IgnoreCollision(boxCollider2D, coin.GetComponent<Collider2D>(), true);
                        // coin.rigidbody2D.linearVelocity = velocity;
                        break;
                    default:
                        break;
                }
            }
        }

        // public override LevelObjectData Serialize()
        // {
        //     var wallData = new WallData();
        //     SerializeBaseData(wallData);
        //     wallData.size = Size;
        //     
        //     return wallData;
        // }
        //
        // public override void Deserialize(string data)
        // {
        //     var wallData = JsonUtility.FromJson<WallData>(data);
        //     DeserializeBaseData(wallData);
        //     Size = wallData.size;
        // }

#if UNITY_EDITOR
        
        [Button]
        private void UpdateColliderSize()
        {
            boxCollider2D.size = Size;
        }
        
#endif
    }
}