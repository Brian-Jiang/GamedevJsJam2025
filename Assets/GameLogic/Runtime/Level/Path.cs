using CoinDash.GameLogic.Runtime.PlayerData;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class Path : LevelObject
    {
        // public int Id { get; private set; }
        //
        // public SpriteRenderer spriteRenderer;

        // public override LevelObjectData Serialize()
        // {
        //     var pathData = new PathData();
        //     SerializeBaseData(pathData);
        //     pathData.pathId = Id;
        //     pathData.length = spriteRenderer.size.y;
        //     
        //     return pathData;
        // }
        //
        // public override void Deserialize(string data)
        // {
        //     var pathData = JsonUtility.FromJson<PathData>(data);
        //     DeserializeBaseData(pathData);
        //     Id = pathData.pathId;
        //     spriteRenderer.size = new Vector2(spriteRenderer.size.x, pathData.length);
        // }
    }
}