using DG.Tweening;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class SimpleMove : MonoBehaviour
    {
        public float moveTime = 2f;
        public Vector3 moveDelta = new Vector3(2, 0, 0);
        
        public void TriggerMove()
        {
            var target = transform.localPosition + moveDelta;
            DOTween.To(() => transform.localPosition, x => transform.localPosition = x, target, moveTime)
                .SetEase(Ease.Linear);
        }
    }
}