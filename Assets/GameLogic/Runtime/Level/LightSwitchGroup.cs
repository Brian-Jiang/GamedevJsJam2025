using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class LightSwitchGroup : LevelObject
    {
        public LightSwitch[] lightSwitches;
        public GameObject[] connectionLights;
        
        private int nextLightSwitchIndex;
        private bool allLightsOn;

        private void Start()
        {
            for (var i = 0; i < lightSwitches.Length; i++)
            {
                lightSwitches[i].Init(this, i);
            }
            
            nextLightSwitchIndex = 0;
            allLightsOn = true;
        }

        public void ActivateLightSwitch(int lightSwitchIndex)
        {
            lightSwitches[lightSwitchIndex].Activate();
            if (lightSwitchIndex < connectionLights.Length && lightSwitchIndex >= 0)
            {
                connectionLights[lightSwitchIndex].SetActive(true);
            }
            
            if (lightSwitchIndex != nextLightSwitchIndex)
            {
                allLightsOn = false;
            }

            if (lightSwitchIndex == lightSwitches.Length - 1)
            {
                if (allLightsOn)
                {
                    // All lights are on in a row
                }
            }
            else
            {
                nextLightSwitchIndex = lightSwitchIndex + 1;
                lightSwitches[nextLightSwitchIndex].PrepareLightSwitch();
            }
        }
    }
}