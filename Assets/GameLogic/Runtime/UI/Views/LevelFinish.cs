using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CoinDash.GameLogic.Runtime.UI.Views
{
    public class LevelFinish : UIView
    {
        public TMP_Text scoreText;
        public Button restartFromBeginningButton;
        public Button quitLevelButton;

        /// <inheritdoc />
        public override void OnShow()
        {
            base.OnShow();
            
            var score = GameFacade.GameLevelManager.PlayerController.Score.ToString();
            scoreText.text = $"Score: {score}";
            restartFromBeginningButton.onClick.AddListener(OnRestartFromBeginningButtonClicked);
            quitLevelButton.onClick.AddListener(OnQuitLevelButtonClicked);
        }
        
        private void OnQuitLevelButtonClicked()
        {
            var op = SceneManager.LoadSceneAsync("MainMenu");
            op!.allowSceneActivation = true;
            op.completed += OnMainMenuSceneLoaded;
        }
        
        private void OnRestartFromBeginningButtonClicked()
        {
            var op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            op!.allowSceneActivation = true;
            op.completed += OnSceneLoaded;
        }
        
        private void OnSceneLoaded(AsyncOperation op)
        {
            Time.timeScale = 1f;
            GameFacade.GameLevelManager.Restart();
            
            Close();
        }
        
        private void OnMainMenuSceneLoaded(AsyncOperation obj)
        {
            GameFacade.UIManager.CloseUIView<InGame>();
            
            Time.timeScale = 1f;
            var chooseLevelUIPrefab = GameFacade.GameDataManager.UIConfig.chooseLevelUIPrefab;
            GameFacade.UIManager.OpenUIView(chooseLevelUIPrefab);
            Close();
        }
    }
}