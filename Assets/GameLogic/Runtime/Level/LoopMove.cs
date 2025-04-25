using DG.Tweening;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class LoopMove: MonoBehaviour
    {
        public float moveTime = 2f;
        public Vector3 moveDelta = new Vector3(2, 0, 0);

        private void Start()
        {
            Vector3 target = transform.localPosition + moveDelta;
            Vector3 originalPosition = transform.localPosition;
            DOTween.Sequence()
                .Append(transform.DOLocalMove(target, moveTime))
                .Append(transform.DOLocalMove(originalPosition, moveTime))
                .SetLoops(-1);
        }
    }
}