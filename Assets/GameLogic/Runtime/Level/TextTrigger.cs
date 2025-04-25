using CoinDash.GameLogic.Runtime.UI.Views;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoinDash.GameLogic.Runtime.Level
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TextTrigger : LevelObject
    {
        public string text;
        public float time;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if (player)
            {
                var inGameView = GameFacade.UIManager.GetUIView<InGame>();
                if (inGameView != null)
                {
                    inGameView.ShowText(text, time);
                }
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
            Handles.Label(transform.position + Vector3.right * 5.5f, "Text Trigger", style);
#endif
        }
    }
}