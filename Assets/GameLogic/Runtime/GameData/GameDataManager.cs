using UnityEngine;

namespace CoinDash.GameLogic.Runtime.GameData
{
    public class GameDataManager: GameFacadeComponent
    {
        public PlayerConfig PlayerConfig { get; private set; }
        public CoinConfig CoinConfig { get; private set; }
        public PathConfig PathConfig { get; private set; }
        public SegmentConfig SegmentConfig { get; private set; }
        public LevelConfig LevelConfig { get; private set; }
        public UIConfig UIConfig { get; private set; }
        
        private const string GameDataPath = "Assets/GameData/";
        
        public GameDataManager(GameObject gameObject) : base(gameObject)
        {
        }

        public void Initialize()
        {
            LoadAllGameData();
        }

        private void LoadAllGameData()
        {
            PlayerConfig = GameFacade.AssetManager.LoadAsset<PlayerConfig>(GameDataPath + "PlayerConfig.asset");
            CoinConfig = GameFacade.AssetManager.LoadAsset<CoinConfig>(GameDataPath + "CoinConfig.asset");
            PathConfig = GameFacade.AssetManager.LoadAsset<PathConfig>(GameDataPath + "PathConfig.asset");
            SegmentConfig = GameFacade.AssetManager.LoadAsset<SegmentConfig>(GameDataPath + "SegmentConfig.asset");
            LevelConfig = GameFacade.AssetManager.LoadAsset<LevelConfig>(GameDataPath + "LevelConfig.asset");
            UIConfig = GameFacade.AssetManager.LoadAsset<UIConfig>(GameDataPath + "UIConfig.asset");
        }
    }
}