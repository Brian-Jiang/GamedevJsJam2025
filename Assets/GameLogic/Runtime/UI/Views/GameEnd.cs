using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CoinDash.GameLogic.Runtime.UI.Views
{
    public class GameEnd : UIView
    {
        public TextMeshProUGUI scoreText;
        // public TextMeshProUGUI distanceText;
        public Button restartButton;

        /// <inheritdoc />
        public override void OnShow()
        {
            base.OnShow();

            // var Score = GameFacade.GameLevelManager.PlayerController.Score.ToString();
            // scoreText.text = $"Score: {Score}";
            
            // var Distance = GameFacade.GameLevelManager.PlayerController.Distance.ToString();
            // distanceText.text = $"Distance: {Distance}";
            
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }
        
        private void OnRestartButtonClicked()
        {
            var player = GameFacade.GameLevelManager.ActivePlayer;
            var checkpoint = player.CurrentCheckpoint;
            if (checkpoint != null)
            {
                checkpoint.RestorePlayerState(player);
                player.trailRenderer.Clear();
                GameFacade.GameLevelManager.VirtualCamera.OnTargetObjectWarped(player.transform, Vector3.zero);
                Time.timeScale = 1f;
                GameFacade.UIManager.CloseUIView(this);
            } 
            else
            {
                var op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
                op!.allowSceneActivation = true;
                op.completed += OnSceneLoaded;
            }
        }
        
        private void OnSceneLoaded(AsyncOperation op)
        {
            Time.timeScale = 1f;
            GameFacade.GameLevelManager.Restart();
            
            GameFacade.UIManager.CloseUIView(this);
        }
    }
}