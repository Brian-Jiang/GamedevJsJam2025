using CoinDash.GameLogic.Runtime.PlayerData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class Booster : LevelObject
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
            boxCollider2D.size = Size;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player coin:
                        // if (coin.IsActiveCoin)
                        // {
                        //     // var force = new Vector2(0, 15f);
                        //     // coin.Fire(force);
                        //     // coin.AddVelocity(new Vector2(0f, 3f));
                        // }
                        break;
                    default:
                        break;
                }
            }
        }

        // public override LevelObjectData Serialize()
        // {
        //     var boosterData = new BoosterData();
        //     SerializeBaseData(boosterData);
        //     boosterData.size = Size;
        //     return boosterData;
        // }
        //
        // public override void Deserialize(string data)
        // {
        //     var boosterData = JsonUtility.FromJson<BoosterData>(data);
        //     DeserializeBaseData(boosterData);
        //     Size = boosterData.size;
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