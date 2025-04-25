using System.Collections.Generic;
using CoinDash.GameLogic.Runtime.PlayerData;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class PathTrigger : LevelObject
    {
        
        private readonly HashSet<Player> overlappedCoins = new();

        private void Start()
        {
            // GameFacade.GameLevelManager.OnActiveCoinChanged += OnActiveCoinChanged;
        }

        private void OnActiveCoinChanged(Player newPlayer)
        {
            if (overlappedCoins.Contains(newPlayer)) {
                // var length = GameFacade.GameLevelManager.GenerateRandomSegment();
                // transform.Translate(Vector3.up * length);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<LevelObject>(out var levelObject)) {
                if (levelObject is Player coin) {
                    // var length = GameFacade.GameLevelManager.GenerateRandomSegment();
                    // transform.Translate(Vector3.up * length);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<LevelObject>(out var levelObject)) {
                if (levelObject is Player coin) {
                    overlappedCoins.Remove(coin);
                }
            }
        }

        // public override LevelObjectData Serialize()
        // {
        //     var pathTriggerData = new PathTriggerData();
        //     SerializeBaseData(pathTriggerData);
        //     
        //     return pathTriggerData;
        // }
        //
        // public override void Deserialize(string data)
        // {
        //     var pathTriggerData = JsonUtility.FromJson<PathTriggerData>(data);
        //     DeserializeBaseData(pathTriggerData);
        // }
    }
}