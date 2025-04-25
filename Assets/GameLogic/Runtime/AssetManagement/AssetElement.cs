using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CoinDash.GameLogic.Runtime.AssetManagement
{
    public class AssetElement
    {
        public int assetId;
        public int refCount;
        public string assetPath;
        public System.Type type;
        public AsyncOperationHandle<Object> handle;

        public bool IsLoading => handle.IsValid() && !handle.IsDone;
        
        public object Result => handle.Result;
        
        private event System.Action<AssetElement> onLoadComplete;
        public event System.Action<AssetElement> OnLoadComplete
        {
            add
            {
                if (handle.IsValid())
                {
                    onLoadComplete += value;
                }
                else
                {
                    value.Invoke(this);
                }
            }
            remove
            {
                if (handle.IsValid())
                {
                    onLoadComplete -= value;
                }
            }
        }

        public AssetElement(int id, string path) {
            assetId = id;
            assetPath = path;
            refCount = 0;
        }
        
        public void RegisterLoadCompleteCallback()
        {
            handle.Completed += InvokeLoadComplete;
        }
        
        private void InvokeLoadComplete(AsyncOperationHandle<Object> operationHandle)
        {
            onLoadComplete?.Invoke(this);
        }
    }
}