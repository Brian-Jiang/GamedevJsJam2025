using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class FlashSpotLight : MonoBehaviour
    {
        public AnimationCurve intensityCurve;
        public AnimationCurve radiusCurve;

        private Light2D light2D;
        private float timer;
        
        private void Start()
        {
            light2D = GetComponent<Light2D>();
            timer = 0f;
            
            Destroy(gameObject, 5f);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            light2D.intensity = intensityCurve.Evaluate(timer);
            light2D.pointLightOuterRadius = radiusCurve.Evaluate(timer);
        }

        // [Button]
        // private void Play()
        // {
        //     timer = 0f;
        // }
    }
}