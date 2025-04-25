using System;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class InputController
    {
        public GameLevelManager GameLevelManager { get; }
        
        public event Action OnMoveLeft; 
        public event Action OnMoveRight;
        public event Action<bool> OnSpeedUp;
        public event Action<bool> OnInvincible;

        private bool isInvincible;
        
        public InputController(GameLevelManager gameLevelManager) {
            GameLevelManager = gameLevelManager;
        }
        
        public void BindInput() {
            GameFacade.InputManager.BindInputBehaviors();
            
            GameFacade.InputManager.SwipeBehavior.OnSwipeLeft += OnSwipeLeft;
            GameFacade.InputManager.SwipeBehavior.OnSwipeRight += OnSwipeRight;

            GameFacade.InputManager.InputActions.Player.SpeedUp.performed += ctx => OnSpeedUp?.Invoke(true);
            GameFacade.InputManager.InputActions.Player.SpeedUp.canceled += ctx => OnSpeedUp?.Invoke(false);
            GameFacade.InputManager.InputActions.Player.Invincible.performed += ctx =>
            {
                isInvincible = !isInvincible;
                OnInvincible?.Invoke(isInvincible);
            };
        }

        private void OnSwipeLeft()
        {
            OnMoveLeft?.Invoke();
        }
        
        private void OnSwipeRight()
        {
            OnMoveRight?.Invoke();
        }
    }
}