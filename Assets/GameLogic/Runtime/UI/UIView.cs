using UnityEngine;

namespace CoinDash.GameLogic.Runtime.UI
{
    public class UIView : MonoBehaviour
    {
        /// <summary>
        /// UI prefab instantiated
        /// </summary>
        // public virtual void OnCreate() {
        //     
        // }

        /// <summary>
        /// UI prefab become active again
        /// </summary>
        public virtual void OnShow()
        {
            
        }

        /// <summary>
        /// UI prefab become inactive
        /// </summary>
        public virtual void OnHide()
        {
            
        }

        /// <summary>
        /// UI prefab will destroy
        /// </summary>
        // public virtual void OnDestroy() {
        //     
        // }

        public void Close()
        {
            GameFacade.UIManager.CloseUIView(this);
        }
    }
}