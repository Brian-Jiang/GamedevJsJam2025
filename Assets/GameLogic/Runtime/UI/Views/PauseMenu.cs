using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CoinDash.GameLogic.Runtime.UI.Views
{
    public class PauseMenu : UIView
    {
        public Button resumeButton;
        public Button restartFromLastCheckpointButton;
        public Button restartFromBeginningButton;
        public Button quitLevelButton;
        
        /// <inheritdoc />
        public override void OnShow()
        {
            base.OnShow();
            
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
            restartFromLastCheckpointButton.onClick.AddListener(OnRestartFromLastCheckpointButtonClicked);
            restartFromBeginningButton.onClick.AddListener(OnRestartFromBeginningButtonClicked);
            quitLevelButton.onClick.AddListener(OnQuitLevelButtonClicked);
        }

        private void OnResumeButtonClicked()
        {
            Time.timeScale = 1f;
            Close();
        }

        private void OnQuitLevelButtonClicked()
        {
            var op = SceneManager.LoadSceneAsync("MainMenu");
            op!.allowSceneActivation = true;
            op.completed += OnMainMenuSceneLoaded;
        }

        private void OnMainMenuSceneLoaded(AsyncOperation obj)
        {
            GameFacade.UIManager.CloseUIView<InGame>();
            
            Time.timeScale = 1f;
            var chooseLevelUIPrefab = GameFacade.GameDataManager.UIConfig.chooseLevelUIPrefab;
            GameFacade.UIManager.OpenUIView(chooseLevelUIPrefab);
            Close();
        }

        private void OnRestartFromBeginningButtonClicked()
        {
            var op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            op!.allowSceneActivation = true;
            op.completed += OnSceneLoaded;
        }

        private void OnRestartFromLastCheckpointButtonClicked()
        {
            var player = GameFacade.GameLevelManager.ActivePlayer;
            var checkpoint = player.CurrentCheckpoint;
            if (checkpoint != null)
            {
                checkpoint.RestorePlayerState(player);
                player.trailRenderer.Clear();
                GameFacade.GameLevelManager.VirtualCamera.OnTargetObjectWarped(player.transform, Vector3.zero);
                Time.timeScale = 1f;
                
                Close();
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
            
            Close();
        }
    }
}