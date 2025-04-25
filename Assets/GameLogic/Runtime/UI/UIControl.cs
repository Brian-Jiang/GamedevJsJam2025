using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CoinDash.GameLogic.Runtime.UI
{
    public class UIControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnPointerDownEvent;
        public event Action OnPointerUpEvent;
        
        /// <inheritdoc />
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke();
        }

        /// <inheritdoc />
        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke();
        }
    }
}