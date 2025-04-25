using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoinDash.GameLogic.Runtime.UI.Views
{
    public class InGame : UIView
    {
        public TextMeshProUGUI powerText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI timeNotOnTrackText;
        public TextMeshProUGUI consecutiveHitsText;
        public TextMeshProUGUI instructionText;
        public GameObject instructionFrame;
        public Button pauseButton;
        
        private float instructionTimer;

        public override void OnShow()
        {
            base.OnShow();
            
            // forceBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
            // forceBarImage.fillAmount = 0f;
            // forceBar.SetActive(false);
            
            UpdatePowerText();
            UpdateScoreText();
            // UpdateTimeNotOnTrackText();
            // UpdateConsecutiveHitsText();
            consecutiveHitsText.gameObject.SetActive(false);
            timeNotOnTrackText.gameObject.SetActive(false);
            
            instructionFrame.SetActive(false);

            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnPauseButtonClicked()
        {
            Time.timeScale = 0f;
            var pauseUIPrefab = GameFacade.GameDataManager.UIConfig.pauseUIPrefab;
            GameFacade.UIManager.OpenUIView(pauseUIPrefab);
        }

        private void Update()
        {
            UpdatePowerText();
            UpdateScoreText();
            // UpdateTimeNotOnTrackText();
            // UpdateConsecutiveHitsText();
            
            if (instructionTimer > 0f)
            {
                instructionTimer -= Time.deltaTime;
                if (instructionTimer <= 0f)
                {
                    instructionFrame.SetActive(false);
                }
            }
        }
        
        private void UpdatePowerText() {
            // var power = GameFacade.GameLevelManager.PlayerController.Power;
            var power = GameFacade.GameLevelManager.ActivePlayer.Power;
            powerText.text = $"Power: {power:F0}";
        }
        
        private void UpdateScoreText() {
            var score = GameFacade.GameLevelManager.PlayerController.Score;
            scoreText.text = $"Score: {score}";
        }

        private void UpdateTimeNotOnTrackText()
        {
            if (GameFacade.GameLevelManager.ActivePlayer.TimeNotOnTrack > 0f)
            {
                timeNotOnTrackText.text = $"Time Not On Track: {GameFacade.GameLevelManager.ActivePlayer.TimeNotOnTrack:F2}";
            }
            else
            {
                timeNotOnTrackText.text = "";
            }
        }
        
        private void UpdateConsecutiveHitsText()
        {
            var consecutiveHits = GameFacade.GameLevelManager.ActivePlayer.ConsecutiveHits;
            if (consecutiveHits > 0)
            {
                consecutiveHitsText.text = $"Consecutive Hits: {consecutiveHits}";
            }
            else
            {
                consecutiveHitsText.text = "";
            }
        }
        
        // private void OnClearData() {
        //     var savingPath = Path.Combine(Application.persistentDataPath, PlayerDataManager.SavingSubFolder);
        //     if (Directory.Exists(savingPath))
        //     {
        //         Directory.Delete(savingPath, true);
        //     }
        // }

        public void ShowText(string text, float time)
        {
            instructionText.text = text;
            instructionFrame.SetActive(true);
            instructionTimer = time;
        }
    }
}