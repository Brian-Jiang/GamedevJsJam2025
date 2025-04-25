using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class PlayerCollectible : MonoBehaviour
    {
        private float speed;
        public float acceleration = 10f;
        
        private Transform target;
        private bool isMoving;
        
        public void SetCollectTarget(Transform newTarget)
        {
            target = newTarget;
            isMoving = true;
        }
        
        private void Update()
        {
            if (isMoving)
            {
                speed += acceleration * Time.deltaTime;
                transform.position = Vector3.MoveTowards(
                    transform.position, target.position, speed * Time.deltaTime);
            }
        }
    }
}