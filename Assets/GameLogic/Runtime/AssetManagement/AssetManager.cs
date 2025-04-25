using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoinDash.GameLogic.Runtime.AssetManagement
{
    public class AssetManager : GameFacadeComponent
    {
        private int currentAssetID;
        private readonly Dictionary<string, AssetElement> assetElementsPathMap = new();
        private readonly Dictionary<int, AssetElement> assetElementsIdMap = new();

#if UNITY_EDITOR
        private const float AssetDatabaseLoadDelay = 0.2f;
        
        private const string BundleModeSettingName = "BundleMode";

        public static bool UseBundleMode {
            get => EditorPrefs.GetBool(BundleModeSettingName, false);
            set => EditorPrefs.SetBool(BundleModeSettingName, value);
        }
        
        private const string SimulateAsyncLoadSettingName = "SimulateAsyncLoad";

        public static bool SimulateAsyncLoad {
            get => EditorPrefs.GetBool(SimulateAsyncLoadSettingName, false);
            set => EditorPrefs.SetBool(SimulateAsyncLoadSettingName, value);
        }
#endif

        public AssetManager(GameObject gameObject) : base(gameObject)
        {
            currentAssetID = 0;
        }

        public AssetElement LoadAsset(string path, Type type, bool async, Action<AssetElement> callback)
        {
            // if (!assetLoadCallbackCache.TryAdd(path, callback)) {
            //     assetLoadCallbackCache[path] += callback;
            // }
            
            LoadAssetImpl(path, type, async, callback);
            
            var assetElement = assetElementsPathMap[path];
            return assetElement;
        }
        
        public void LoadAsset<T>(string path, bool async, Action<AssetElement> callback) where T : Object
        {
            // if (!assetLoadCallbackCache.TryAdd(path, callback)) {
            //     assetLoadCallbackCache[path] += callback;
            // }

            LoadAssetImpl(path, typeof(T), async, callback);
        }
        
        public Object LoadAsset(string path) {
            return LoadAsset<Object>(path);
        }
        
        public T LoadAsset<T>(string path) where T : Object
        {
            T result = null;
            // if (assetElementsPathMap.TryGetValue(path, out AssetElement assetElement))
            // {
            //     
            // }
            
            LoadAssetImpl(path, typeof(T), false, assetElement => {
                var asset = assetElement.Result as T;
                if (asset == null) {
                    Debug.LogError($"Asset loaded is null: {assetElement.assetPath}, type: {typeof(T)}");
                }

                result = asset;
            });

            return result;
        }

        
        
        public void ReleaseAsset(int id) {
// #if UNITY_EDITOR
//             if (!UseBundleMode) {
//                 return;
//             }
// #endif

            if (assetElementsIdMap.TryGetValue(id, out var assetElement)) {
                assetElement.refCount--;
                if (assetElement.refCount <= 0) {
                    assetElement.handle.Release();
                    assetElementsIdMap.Remove(id);
                    assetElementsPathMap.Remove(assetElement.assetPath);
                }
            }
        }

        private void LoadAssetImpl(string path, Type type, bool async, Action<AssetElement> callback) {
#if UNITY_EDITOR
            // if (!UseBundleMode) {
            //     var loadRoutine = LoadAssetDelayed(path, type, async, callback);
            //     StartCoroutine(loadRoutine);
            //     return;
            // }
#endif

            LoadAssetInternal(path, type, async, callback);
        }
        
#if UNITY_EDITOR
        private IEnumerator LoadAssetDelayed(string path, Type type, bool async, Action<AssetElement> callback)
        {
            if (SimulateAsyncLoad && async) {
                yield return new WaitForSeconds(AssetDatabaseLoadDelay);
            }
            
            LoadAssetInternal(path, type, async, callback);
        }
#endif

        private void LoadAssetInternal(string path, Type type, bool async, Action<AssetElement> callback)
        {
            if (assetElementsPathMap.TryGetValue(path, out var assetElement))
            {
                if (assetElement.IsLoading)
                {
                    assetElement.refCount++;
                    assetElement.OnLoadComplete += callback;
#if !UNITY_WEBGL
                    if (!async)
                    {

                        assetElement.handle.WaitForCompletion();

                    }
#endif
                    return;
                }

                callback.Invoke(assetElement);
                return;
            }

            assetElement = new AssetElement(currentAssetID, path);
            assetElement.refCount = 1;
            assetElement.type = type;
            var handle = Addressables.LoadAssetAsync<Object>(path);
            assetElement.handle = handle;
            assetElement.OnLoadComplete += callback;
            assetElement.RegisterLoadCompleteCallback();

            assetElementsIdMap.Add(currentAssetID, assetElement);
            assetElementsPathMap.Add(path, assetElement);
            
            ++currentAssetID;

#if !UNITY_WEBGL
            if (!async)
            {

                handle.WaitForCompletion();

            }
#endif
        }
    }
}
