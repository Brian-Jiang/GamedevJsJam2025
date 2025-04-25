using CoinDash.GameLogic.Runtime.PlayerData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class Point : LevelObject
    {
        public SpriteRenderer spriteRenderer;
        public int score = 10;
        public float power = 0.1f;
        
        public GameObject sfxPrefab;
        
        [AssetsOnly]
        public AudioClip collectSound;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player player:
                        // if (coin.IsActiveCoin)
                        // {
                        //     AudioSource.PlayClipAtPoint(collectSound, transform.position);
                        //     GameFacade.GameLevelManager.PlayerController.AddScore(100);
                        //     coin.power += power;
                        //     GameFacade.GameLevelManager.RemoveLevelObject(this);
                        // }
                        // PlaySFX(collectSound);
                        GameFacade.GameLevelManager.PlayerController.AddScore(score);
                        player.Power += power;
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
        
        // private void PlaySFX(AudioClip clip)
        // {
        //     var go = Instantiate(sfxPrefab);
        //     var audioSource = go.GetComponent<AudioSource>();
        //     audioSource.clip = clip;
        //     audioSource.Play();
        //     
        //     Destroy(go, clip.length + 0.1f);
        // }

        // public override LevelObjectData Serialize()
        // {
        //     var pointData = new PointData();
        //     SerializeBaseData(pointData);
        //     pointData.color = spriteRenderer.color;
        //     return pointData;
        // }
        //
        // public override void Deserialize(string data)
        // {
        //     var pointData = JsonUtility.FromJson<PointData>(data);
        //     DeserializeBaseData(pointData);
        //     spriteRenderer.color = pointData.color;
        // }
    }
}