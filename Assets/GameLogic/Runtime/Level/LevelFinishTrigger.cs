using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoinDash.GameLogic.Runtime.Level
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelFinishTrigger : LevelObject
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if (player)
            {
                Time.timeScale = 0f;
                var levelFinishUIPrefab = GameFacade.GameDataManager.UIConfig.levelFinishUIPrefab;
                GameFacade.UIManager.OpenUIView(levelFinishUIPrefab);
            }
        }

        private void OnDrawGizmos()
        {
            var boxCollider2d = GetComponent<BoxCollider2D>();
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, boxCollider2d.size);
            
#if UNITY_EDITOR
            // You can pass a GUIStyle if you want to customize font size, color, etc.
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = Color.white;

            // Draw the label in world space
            Handles.Label(transform.position + Vector3.right * 5.5f, "Level Finish Trigger", style);
#endif
        }
    }
}