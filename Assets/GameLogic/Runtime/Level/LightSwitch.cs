using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class LightSwitch : MonoBehaviour
    {
        public Light2D lightDimmed;
        public Light2D lightActivated;
        
        private LightSwitchGroup lightSwitchGroup;
        private int lightSwitchIndex;

        public void Init(LightSwitchGroup switchGroup, int index)
        {
            lightDimmed.gameObject.SetActive(index == 0);
            lightActivated.gameObject.SetActive(false);
            lightSwitchGroup = switchGroup;
            lightSwitchIndex = index;
        }
        
        public void PrepareLightSwitch()
        {
            lightDimmed.gameObject.SetActive(true);
            lightActivated.gameObject.SetActive(false);
        }
        
        public void Activate()
        {
            lightDimmed.gameObject.SetActive(false);
            lightActivated.gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Player player:
                        if (lightSwitchGroup != null)
                        {
                            // lightDimmed.gameObject.SetActive(false);
                            // lightActivated.gameObject.SetActive(true);
                            lightSwitchGroup.ActivateLightSwitch(lightSwitchIndex);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}