using UnityEngine;
using UnityEngine.UI;

namespace CoinDash.GameLogic.Runtime.UI.Views
{
    public class Credit : UIView
    {
        public Button backButton;
        public Button addDiscordButton;
        
        /// <inheritdoc />
        public override void OnShow()
        {
            base.OnShow();
            
            backButton.onClick.AddListener(OnBackButtonClicked);
            addDiscordButton.onClick.AddListener(OnAddDiscordButtonClicked);
        }

        private void OnAddDiscordButtonClicked()
        {
            Application.OpenURL("https://discord.gg/AGpJuZ85");
        }

        private void OnBackButtonClicked()
        {
            Close();
        }
    }
}