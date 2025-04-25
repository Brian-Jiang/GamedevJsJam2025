using CoinDash.GameLogic.Runtime.PlayerData;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class SpotLight : LevelObject
    {
        public Light2D light2D;
        
        // public override LevelObjectData Serialize()
        // {
        //     var lightData = new SpotLightData();
        //     SerializeBaseData(lightData);
        //     lightData.color = light2D.color;
        //     lightData.intensity = light2D.intensity;
        //     lightData.innerAngle = light2D.pointLightInnerAngle;
        //     lightData.outerAngle = light2D.pointLightOuterAngle;
        //     lightData.innerRadius = light2D.pointLightInnerRadius;
        //     lightData.outerRadius = light2D.pointLightOuterRadius;
        //     return lightData;
        // }
        //
        // public override void Deserialize(string data)
        // {
        //     var lightData = JsonUtility.FromJson<SpotLightData>(data);
        //     DeserializeBaseData(lightData);
        //     light2D.color = lightData.color;
        //     light2D.intensity = lightData.intensity;
        //     light2D.pointLightInnerAngle = lightData.innerAngle;
        //     light2D.pointLightOuterAngle = lightData.outerAngle;
        //     light2D.pointLightInnerRadius = lightData.innerRadius;
        //     light2D.pointLightOuterRadius = lightData.outerRadius;
        // }
    }
}