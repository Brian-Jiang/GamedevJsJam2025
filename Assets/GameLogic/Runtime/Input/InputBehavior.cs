namespace CoinDash.GameLogic.Runtime.Input
{
    public abstract class InputBehavior
    {
        public GameInputActions InputActions { get; private set; } // => GameFacade.InputManager.InputActions;

        public InputBehavior(InputManager inputManager)
        {
            InputActions = inputManager.InputActions;
        }
        
        public abstract void Bind();

        public abstract void UnBind();
    }
}