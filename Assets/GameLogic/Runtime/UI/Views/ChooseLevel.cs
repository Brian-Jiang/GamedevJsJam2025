using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CoinDash.GameLogic.Runtime.UI.Views
{
    public class ChooseLevel : UIView
    {
        [AssetsOnly]
        public GameObject levelEntryPrefab;
        
        public RectTransform levelEntryContainer;
        public Button creditButton;

        /// <inheritdoc />
        public override void OnShow()
        {
            base.OnShow();
            
            var levelConfig = GameFacade.GameDataManager.LevelConfig;
            foreach (var levelData in levelConfig.levels)
            {
                var levelEntryGO = Instantiate(levelEntryPrefab, levelEntryContainer);
                var levelEntry = levelEntryGO.GetComponent<LevelEntry>();
                levelEntry.Initialize(levelData.levelName);
                levelEntry.onLevelSelected = () =>
                {
                    var op = SceneManager.LoadSceneAsync(levelData.scene);
                    op!.allowSceneActivation = true;
                    op.completed += OnSceneLoaded;
                };
            }
            
            creditButton.onClick.AddListener(OnCreditButtonClicked);
        }
        
        private void OnSceneLoaded(AsyncOperation op)
        {
            GameFacade.GameLevelManager.Initialize();
            Close();
        }
        
        private void OnCreditButtonClicked()
        {
            var creditUIPrefab = GameFacade.GameDataManager.UIConfig.creditUIPrefab;
            GameFacade.UIManager.OpenUIView(creditUIPrefab);
        }
    }
}