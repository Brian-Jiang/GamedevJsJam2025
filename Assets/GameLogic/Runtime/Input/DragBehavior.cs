using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoinDash.GameLogic.Runtime.Input
{
    public class DragBehavior : InputBehavior
    {
        public event Action<Vector2> OnDragStart;
        public event Action<Vector2> OnDrag;
        public event Action<Vector2> OnDragEnd;

        private Vector2 lastPosition;
        private bool touching;
        private bool moving;
        
        /// <inheritdoc />
        public DragBehavior(InputManager inputManager) : base(inputManager)
        {
        }
        
        public override void Bind()
        {
            InputActions.Player.PointerDown.performed += OnTouchDown;
            InputActions.Player.PointerDown.canceled += OnTouchLift;
            InputActions.Player.PointerMove.performed += OnTouchMove;
        }

        public override void UnBind()
        {
            InputActions.Player.PointerDown.performed -= OnTouchDown;
            InputActions.Player.PointerDown.canceled -= OnTouchLift;
            InputActions.Player.PointerMove.performed -= OnTouchMove;
        }
        
        private void OnTouchDown(InputAction.CallbackContext context) {
            if (InputUtils.PointerOverUI()) return;

            touching = true;
        }

        private void OnTouchMove(InputAction.CallbackContext context) {
            if (!touching) return;

            var value = context.ReadValue<Vector2>();
            if (!moving) {
                moving = true;
                OnDragStart?.Invoke(value);
            } else {
                OnDrag?.Invoke(value);
                lastPosition = value;
            }
        }

        private void OnTouchLift(InputAction.CallbackContext context) {
            if (!touching) return;
            touching = false;
            
            if (!moving) return;
            moving = false;
            
            OnDragEnd?.Invoke(lastPosition);
        }
    }
}