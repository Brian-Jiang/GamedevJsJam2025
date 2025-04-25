using UnityEngine;
using UnityEngine.InputSystem;

namespace CoinDash.GameLogic.Runtime.Input
{
    public class InputManager : GameFacadeComponent
    {
        public GameInputActions InputActions { get; private set; }
        
        public DragBehavior DragBehavior { get; private set; }
        public SwipeBehavior SwipeBehavior { get; private set; }
        
        public InputManager(GameObject gameObject) : base(gameObject)
        {
            InputActions = new GameInputActions();
        }
        
        public void BindInputBehaviors()
        {
            InputActions.Enable();
            DragBehavior = new DragBehavior(this);
            DragBehavior.Bind();
            
            SwipeBehavior = new SwipeBehavior(this);
            SwipeBehavior.Bind();
        }
        
        public void UnBindInputBehaviors()
        {
            InputActions.Disable();
            DragBehavior.UnBind();
            SwipeBehavior.UnBind();
        }
    }
}