using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoinDash.GameLogic.Runtime.UI.Views
{
    public class LevelEntry : MonoBehaviour
    {
        public TMP_Text levelNameText;
        public Button levelButton;
        
        public Action onLevelSelected;
        
        public void Initialize(string levelName)
        {
            levelNameText.text = levelName;
            levelButton.onClick.AddListener(() =>
            {
                onLevelSelected?.Invoke();
            });
        }
    }
}