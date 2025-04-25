using CoinDash.GameLogic.Runtime.PlayerData;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public abstract class LevelObject : MonoBehaviour
    {
        // protected string PrefabPath { get; private set; }
        
        // public abstract LevelObjectData Serialize();
        // public abstract void Deserialize(string data);
        //
        // protected void SerializeBaseData(LevelObjectData data)
        // {
        //     data.position = transform.position;
        //     data.rotation = transform.rotation;
        //     data.scale = transform.localScale;
        // }
        //
        // protected void DeserializeBaseData(LevelObjectData data)
        // {
        //     transform.position = data.position;
        //     transform.rotation = data.rotation;
        //     transform.localScale = data.scale;
        // }
    }
}