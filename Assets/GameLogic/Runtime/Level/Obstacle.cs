using CoinDash.GameLogic.Runtime.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class Obstacle : LevelObject
    {
        [AssetsOnly] public GameObject pointPrefab;
        public SpriteRenderer spriteRenderer;
        public BoxCollider2D boxCollider;
        public float pointDensity = 1f;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var coin = other.gameObject.GetComponent<Player>();
            if (coin && !coin.IsDebugInvincible)
            {
                if (coin.HasShield || coin.IsOnInvincibleTrack)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Time.timeScale = 0f;
                    var endGameUIPrefab = GameFacade.GameDataManager.UIConfig.endGameUIPrefab;
                    GameFacade.UIManager.OpenUIView(endGameUIPrefab);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var coin = collision2D.gameObject.GetComponent<Player>();
            if (coin && !coin.IsDebugInvincible)
            {
                if (coin.HasShield || coin.IsOnInvincibleTrack)
                {
                    SpawnRandomPoints();
                    Destroy(gameObject);
                }
                else
                {
                    if (Mathf.Abs(collision2D.relativeVelocity.x) < 0.5f)
                    {
                        Debug.Log(collision2D.relativeVelocity, gameObject);
                        Time.timeScale = 0f;
                        var endGameUIPrefab = GameFacade.GameDataManager.UIConfig.endGameUIPrefab;
                        GameFacade.UIManager.OpenUIView(endGameUIPrefab);
                    }
                    else
                    {
                        SpawnRandomPoints();
                        Destroy(gameObject);
                    }
                    
                    // Debug.Log(collision2D.relativeVelocity);
                }
            }
            
        }

        private void SpawnRandomPoints()
        {
            var sizeX = spriteRenderer.size.x;
            var sizeY = spriteRenderer.size.y;
            var area = sizeX * sizeY;
            var pointCount = Mathf.FloorToInt(area * pointDensity);
            for (int i = 0; i < pointCount; i++)
            {
                var randomX = Random.Range(-sizeX / 2f, sizeX / 2f);
                var randomY = Random.Range(-sizeY / 2f, sizeY / 2f);
                var pointPosition = new Vector3(randomX, randomY, 0f) + transform.position;
                
                var point = Instantiate(pointPrefab, pointPosition, Quaternion.identity);
                // point.transform.SetParent(transform);
                var playerCollectible = point.GetComponent<PlayerCollectible>();
                if (playerCollectible)
                {
                    playerCollectible.SetCollectTarget(GameFacade.GameLevelManager.ActivePlayer.transform);
                }
            }
        }
    }
}