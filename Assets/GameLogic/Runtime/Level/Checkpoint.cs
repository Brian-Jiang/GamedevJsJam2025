using CoinDash.GameLogic.Runtime.Level.Tracks;
using UnityEngine;
using UnityEngine.U2D;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoinDash.GameLogic.Runtime.Level
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Checkpoint : LevelObject
    {
        public Color inactiveColor;
        public Color activeColor;
        
        private TrackBase savedTrack;
        // private int savedTrackIndex;
        private Vector2 savedPosition;
        // private Vector2 savedVelocity;
        private bool isActive;
        // private bool isSwitchingTrack;
        private SpriteRenderer[] spriteRenderers;
        private BoxCollider2D collider2D;

        private void Start()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            collider2D = GetComponent<BoxCollider2D>();
            
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = inactiveColor;
            }
            
            var colliders = Physics2D.OverlapBoxAll(transform.position, collider2D.size, 0f);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<TrackBase>(out var track))
                {
                    savedTrack = track;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if (player)
            {
                // Check if the player is already at this checkpoint
                if (player.CurrentCheckpoint == this)
                {
                    return;
                }

                if (player.CurrentCheckpoint)
                {
                    player.CurrentCheckpoint.ResetCheckpoint();
                }

                // Set the player's current checkpoint to this one
                player.CurrentCheckpoint = this;
                isActive = true;
                // isSwitchingTrack = player.IsSwitchingTrack;
                if (player.CurrentTrack != null)
                {
                    savedTrack = player.CurrentTrack;
                    // savedTrackIndex = player.CurrentTrackIndex;
                }
                else
                {
                    // savedVelocity = player.rigidbody2D.linearVelocity;
                }
                
                savedPosition = player.transform.position;

                // Change the color of the checkpoint to indicate it's active
                foreach (var spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.color = activeColor;
                }
            }
        }

        private void ResetCheckpoint()
        {
            savedTrack = null;
            // savedTrackIndex = -1;
            isActive = false;
            
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = inactiveColor;
            }
        }
        
        public void RestorePlayerState(Player player)
        {
            if (savedTrack != null)
            {
                player.CurrentTrack = savedTrack;
                // player.CurrentTrackIndex = savedTrackIndex;
            }
            else
            {
                // var hitArray = Physics2D.BoxCastAll(transform.position, collider2D.size, 0f, Vector2.zero);
                // foreach (var hit2D in hitArray)
                // {
                //     hit2D.
                // }
                
                // player.rigidbody2D.linearVelocity = savedVelocity;
            }
            
            // player.IsSwitchingTrack = isSwitchingTrack;
            
            var splinePosition = player.CurrentTrack.GetSplinePosition(savedPosition.y);
            var targetPosition = new Vector2(splinePosition.x, savedPosition.y);
            player.transform.position = targetPosition;

            // Change the color of the checkpoint back to inactive
            // foreach (var spriteRenderer in spriteRenderers)
            // {
            //     spriteRenderer.color = inactiveColor;
            // }
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
            Handles.Label(transform.position + Vector3.right * 5.5f, "Checkpoint", style);
#endif
        }
    }
}