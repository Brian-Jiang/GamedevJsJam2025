using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoinDash.GameLogic.Runtime.Level
{
    public class MoveTrigger : MonoBehaviour
    {
        public SimpleMove simpleMove;

        private bool triggered;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var coin = other.gameObject.GetComponent<Player>();
            if (coin && simpleMove && !triggered)
            {
                simpleMove.TriggerMove();
                triggered = true;
            }
        }

        private void OnDrawGizmos()
        {
            var boxCollider2d = GetComponent<BoxCollider2D>();
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, boxCollider2d.size);
            
#if UNITY_EDITOR
            // You can pass a GUIStyle if you want to customize font size, color, etc.
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = Color.white;

            // Draw the label in world space
            Handles.Label(transform.position + Vector3.right * 5.5f, "Move Trigger", style);
#endif
        }
    }
}