using CoinDash.GameLogic.Runtime.Level;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoinDash.GameLogic.Runtime.UI.Views
{
    public class Control : UIView
    {
        // public EventTrigger leftControl;
        // public EventTrigger rightControl;
        private Player controlledPlayer;
        
        /// <inheritdoc />
        public override void OnShow()
        {
            base.OnShow();

            // EventTrigger.Entry pointerDownEvent = new EventTrigger.Entry();
            // pointerDownEvent.eventID = EventTriggerType.PointerDown;
            // pointerDownEvent.callback.AddListener((data) => { OnLeftControlPressed(); });
            // leftControl.triggers.Add(pointerDownEvent);
            //
            // EventTrigger.Entry pointerUpEvent;
            // leftControl.OnPointerDown(); += OnLeftControlPressed;
            // leftControl.OnPointerUpEvent += OnLeftControlReleased;
            // rightControl.OnPointerDownEvent += OnRightControlPressed;
            // rightControl.OnPointerUpEvent += OnRightControlReleased;
        }
        
        public void OnLeftControlPointerDown()
        {
            // GameFacade.GameLevelManager.ActiveCoin.SwitchLeftTrack();
            controlledPlayer.SwitchLeftTrack();
        }
        
        public void OnRightControlPointerDown()
        {
            // GameFacade.GameLevelManager.ActiveCoin.SwitchRightTrack();
            controlledPlayer.SwitchRightTrack();
        }
        
        public void SetCoin(Player player)
        {
            controlledPlayer = player;
            // leftControl.gameObject.SetActive(true);
            // rightControl.gameObject.SetActive(true);
        }
        
        // public void OnLeftControlPressed()
        // {
        //     GameFacade.GameLevelManager.ActiveCoin.touchHForce += -1;
        // }
        //
        // public void OnLeftControlReleased()
        // {
        //     GameFacade.GameLevelManager.ActiveCoin.touchHForce -= -1;
        // }
        //
        // public void OnRightControlPressed()
        // {
        //     GameFacade.GameLevelManager.ActiveCoin.touchHForce += 1;
        // }
        //
        // public void OnRightControlReleased()
        // {
        //     GameFacade.GameLevelManager.ActiveCoin.touchHForce -= 1;
        // }
    }
}