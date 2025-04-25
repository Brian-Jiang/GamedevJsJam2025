using System.Collections;
using System.Collections.Generic;
using CoinDash.GameLogic.Runtime.AssetManagement;
using CoinDash.GameLogic.Runtime.GameData;
using CoinDash.GameLogic.Runtime.Input;
using CoinDash.GameLogic.Runtime.Level;
using CoinDash.GameLogic.Runtime.PlayerData;
using CoinDash.GameLogic.Runtime.UI;
using DG.Tweening;
using SmartReference.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace CoinDash.GameLogic.Runtime
{
    public class GameFacade
    {
        public static Scheduler Scheduler { get; private set; }
        public static AssetManager AssetManager { get; private set; }
        public static GameDataManager GameDataManager { get; private set; }
        public static InputManager InputManager { get; private set; }
        public static PlayerDataManager PlayerDataManager { get; private set; }
        public static UIManager UIManager { get; private set; }
        public static GameLevelManager GameLevelManager { get; private set; }
        
        private static GameObject persistentGameObject;

        public static void Init(GameObject gameObject)
        {
            persistentGameObject = gameObject;
            Scheduler = persistentGameObject.AddComponent<Scheduler>();
            
            AssetManager = new AssetManager(persistentGameObject);
            GameDataManager = new GameDataManager(persistentGameObject);
            InputManager = new InputManager(persistentGameObject);
            PlayerDataManager = new PlayerDataManager(persistentGameObject);
            UIManager = new UIManager(persistentGameObject);
            // TODO Audio
            
            GameLevelManager = new GameLevelManager(persistentGameObject);

            var customLoader = new CustomLoader();
            customLoader.loader = (path, type) =>
            {
                Object asset = null;
                AssetManager.LoadAsset(path, type, false, element =>
                {
                    asset = element.Result as Object;
                });

                return asset;
            };

            customLoader.loaderAsync = (path, type, callback) =>
            {
                AssetManager.LoadAsset(path, type, false, element =>
                {
                    callback(element.Result as Object);
                });
            };
            
            SmartReference.Runtime.SmartReference.InitWithCustomLoader(customLoader);
            
            // SmartReference.Runtime.SmartReference.InitWithAddressablesLoader();
            
            // gameObject.AddComponent<SceneController>();
            // gameObject.AddComponent<CameraManager>();
            // gameObject.AddComponent<EffectManager>();
            // gameObject.AddComponent<PlayerNetDataManager>();
        }
        
        public static void GameStartUp() {
            Application.targetFrameRate = 60;
            
            var routine = GameStartUpWithAssets();
            var startupCoroutine = Scheduler.StartCoroutine(routine);
            
            // DOTween.Init();
            //
            // PlayerDataManager.Initialize();
            // UIManager.Initialize();
            //
            // var chooseLevelUIPrefab = GameDataManager.UIConfig.chooseLevelUIPrefab;
            // UIManager.OpenUIView(chooseLevelUIPrefab);
        }
        
        private static IEnumerator GameStartUpWithAssets()
        {
            yield return LoadGroupByLabel("Preload");
            
            DOTween.Init();
            
            GameDataManager.Initialize();
            PlayerDataManager.Initialize();
            UIManager.Initialize();
            
            var op = SceneManager.LoadSceneAsync("MainMenu");
            op!.allowSceneActivation = true;
            op.completed += OnSceneLoaded;
        }
        
        private static IEnumerator LoadGroupByLabel(string label)
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            yield return locationsHandle;

            if (!locationsHandle.IsValid() || locationsHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to get resource locations.");
                yield break;
            }

            var locations = locationsHandle.Result;

            var handles = new List<AsyncOperationHandle>();
            foreach (var loc in locations)
            {
                string address = loc.PrimaryKey;
                var assetElement = AssetManager.LoadAsset(address, typeof(UnityEngine.Object), true, asset =>
                {
                    
                });
                
                handles.Add(assetElement.handle);
            }
            
            // TODO use unitask
            while (!AllDone(handles))
            {
                yield return null;
            }

            Addressables.Release(locationsHandle); // Clean up
        }
        
        private static bool AllDone(List<AsyncOperationHandle> handles)
        {
            foreach (var h in handles)
            {
                if (!h.IsDone)
                    return false;
            }
            return true;
        }
        
        private static void OnSceneLoaded(AsyncOperation op)
        {
            var chooseLevelUIPrefab = GameDataManager.UIConfig.chooseLevelUIPrefab;
            UIManager.OpenUIView(chooseLevelUIPrefab);
        }
    }
}