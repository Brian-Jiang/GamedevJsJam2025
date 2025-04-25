using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoinDash.GameLogic.Runtime.Input
{
    public class SwipeBehavior : InputBehavior
    {
        public float thresholdTime = 0.5f;
        public float thresholdDistance = 0.08f;
        public float thresholdAngle = 30f;
        
        private bool touching;
        private Vector2 startPosition;
        private float startTime;
        private bool canceled;
        
        public event Action OnSwipeLeft;
        public event Action OnSwipeRight;
        
        /// <inheritdoc />
        public SwipeBehavior(InputManager inputManager) : base(inputManager)
        {
        }
        
        /// <inheritdoc />
        public override void Bind()
        {
            InputActions.Player.PointerDown.performed += OnTouchDown;
            InputActions.Player.PointerDown.canceled += OnTouchLift;
            InputActions.Player.PointerMove.performed += OnTouchMove;
            
            InputActions.Player.SwitchLeft.performed += ctx => OnSwipeLeft?.Invoke();
            InputActions.Player.SwitchRight.performed += ctx => OnSwipeRight?.Invoke();
        }

        /// <inheritdoc />
        public override void UnBind()
        {
            InputActions.Player.PointerDown.performed -= OnTouchDown;
            InputActions.Player.PointerDown.canceled -= OnTouchLift;
            InputActions.Player.PointerMove.performed -= OnTouchMove;
        }
        
        private void OnTouchDown(InputAction.CallbackContext context)
        {
            if (InputUtils.PointerOverUI()) return;

            touching = true;
            canceled = false;
            startPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            startTime = Time.time;
        }

        private void OnTouchMove(InputAction.CallbackContext context)
        {
            if (!touching) return;
            if (canceled) return;
            
            float deltaTime = Time.time - startTime;
            if (deltaTime > thresholdTime)
            {
                canceled = true;
                return;
            }
            
            Vector2 currentPosition = context.ReadValue<Vector2>();
            Vector2 deltaPosition = currentPosition - startPosition;
            float distance = deltaPosition.magnitude / Screen.width;
            if (distance < thresholdDistance)
            {
                return;
            }
            
            float leftDot = Vector2.Dot(deltaPosition.normalized, Vector2.left);
            float leftAngle = Mathf.Acos(leftDot) * Mathf.Rad2Deg;
            if (leftAngle < thresholdAngle)
            {
                OnSwipeLeft?.Invoke();
                canceled = true;
            }
            
            float rightDot = Vector2.Dot(deltaPosition.normalized, Vector2.right);
            float rightAngle = Mathf.Acos(rightDot) * Mathf.Rad2Deg;
            if (rightAngle < thresholdAngle)
            {
                OnSwipeRight?.Invoke();
                canceled = true;
            }
        }

        private void OnTouchLift(InputAction.CallbackContext context)
        {
            if (!touching) return;
            touching = false;
        }
    }
}