using System;
using System.IO;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.PlayerData
{
    public class PlayerDataManager : GameFacadeComponent
    {
        public PlayerData PlayerData { get; private set; }
        public string SavingPath { get; private set; }
        public bool IsFirstTimePlay { get; private set; }
        
        public const string SavingSubFolder = "Save";
        
        public PlayerDataManager(GameObject gameObject) : base(gameObject)
        {
            SavingPath = Path.Combine(Application.persistentDataPath, SavingSubFolder);
        }
        
        public void Initialize()
        {
            IsFirstTimePlay = !Directory.Exists(SavingPath);
            if (IsFirstTimePlay)
            {
                Directory.CreateDirectory(SavingPath);
                PlayerData = new PlayerData();
                SaveAllData();
            }
            else
            {
                LoadAllData();
            }
        }
        
        public void SaveAllData()
        {
            var json = JsonUtility.ToJson(PlayerData, true);
            File.WriteAllText(Path.Combine(SavingPath, "PlayerData.json"), json);
        }
        
        public void LoadAllData()
        {
            var json = File.ReadAllText(Path.Combine(SavingPath, "PlayerData.json"));
            PlayerData = JsonUtility.FromJson<PlayerData>(json);
        }
    }
}