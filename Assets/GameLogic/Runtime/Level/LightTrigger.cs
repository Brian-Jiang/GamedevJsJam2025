using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoinDash.GameLogic.Runtime.Level
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LightTrigger : MonoBehaviour
    {
        public Light2D globalLight2D;
        public float fadeOutTime = 5f;
        public float targetIntensity = 0f;
        public float targetSpeed = 7.5f;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var coin = other.gameObject.GetComponent<Player>();
            if (coin && globalLight2D)
            {
                DOTween.To(() => globalLight2D.intensity, x => globalLight2D.intensity = x,
                        targetIntensity, fadeOutTime)
                    .SetEase(Ease.Linear);
                
                // var newSpeed = coin.speed * 0.75f;
                DOTween.To(() => coin.speed, x => coin.speed = x,
                        targetSpeed, fadeOutTime)
                    .SetEase(Ease.Linear);
            }
        }
        
        private void OnDrawGizmos()
        {
            var boxCollider2d = GetComponent<BoxCollider2D>();
            Gizmos.color = Color.grey;
            Gizmos.DrawWireCube(transform.position, boxCollider2d.size);
            
#if UNITY_EDITOR
            // You can pass a GUIStyle if you want to customize font size, color, etc.
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = Color.white;

            // Draw the label in world space
            Handles.Label(transform.position + Vector3.right * 5.5f, "Light Trigger", style);
#endif
        }
    }
}