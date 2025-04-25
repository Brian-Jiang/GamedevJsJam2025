using System.Collections.Generic;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.UI
{
    public class UIManager : GameFacadeComponent
    {
        public GameObject UIRoot { get; private set; }
        
        private HashSet<UIView> openedViews = new HashSet<UIView>();
        
        public UIManager(GameObject gameObject) : base(gameObject)
        {
        }

        public void Initialize()
        {
            var uiRootPrefab = GameFacade.GameDataManager.UIConfig.uiRoot;
            UIRoot = Object.Instantiate<GameObject>(uiRootPrefab);
            Object.DontDestroyOnLoad(UIRoot);
            UIRoot.name = "UIRoot";
            
            var eventSystemPrefab = GameFacade.GameDataManager.UIConfig.eventSystemPrefab;
            var eventSystem = Object.Instantiate<GameObject>(eventSystemPrefab);
            Object.DontDestroyOnLoad(eventSystem);
            eventSystem.name = "EventSystem";
        }
        
        public void OpenUIView(GameObject prefab)
        {
            var go = Object.Instantiate(prefab, UIRoot.transform);
            go.SetActive(true);
            var uiView = go.GetComponent<UIView>();
            uiView.OnShow();
            
            if (openedViews.Contains(uiView))
            {
                Debug.LogWarning($"UIView {uiView.name} is already opened.");
                return;
            }
            
            openedViews.Add(uiView);
        }

        public void CloseUIView(UIView uiView)
        {
            uiView.OnHide();
            Object.Destroy(uiView.gameObject);
            
            openedViews.Remove(uiView);
        }
        
        public void CloseAllViews()
        {
            foreach (var uiView in openedViews)
            {
                uiView.OnHide();
                Object.Destroy(uiView.gameObject);
            }
            
            openedViews.Clear();
        }
        
        public void CloseUIView<T>() where T : UIView
        {
            foreach (var uiView in openedViews)
            {
                if (uiView is T)
                {
                    CloseUIView(uiView);
                    break;
                }
            }
        }
        
        public T GetUIView<T>() where T : UIView
        {
            foreach (var uiView in openedViews)
            {
                if (uiView is T)
                {
                    return (T) uiView;
                }
            }

            return null;
        }
        
        // public T OpenView<T>() where T: UIView {
        //     // var uiView = new T();  // TODO pool
        //
        //     var path = uiView.PrefabPath;
        //     var viewID = ++nextViewID;
        //
        //     loader.Invoke(path, prefab => {
        //         var go = Instantiate(prefab, layerRoot[uiView.Layer]);
        //         uiView.OnCreate(viewID, go);
        //         uiView.OnCreate();
        //         onViewCreate?.Invoke(uiView);
        //         
        //         uiView.OnShow();
        //     });
        //     
        //     if (!runtimeUI.TryGetValue(uiView.Layer, out var layerStack)) {
        //         layerStack = new Stack<RuntimeView>();
        //         runtimeUI.Add(uiView.Layer, layerStack);
        //     }
        //     
        //     RuntimeView currentView;
        //     switch (openMethod) {
        //         case UIOpenMethod.Stack:
        //             if (layerStack.TryPeek(out currentView)) {
        //                 currentView.view.Hide();
        //             }
        //             break;
        //         case UIOpenMethod.Substitute:
        //             if (layerStack.TryPop(out currentView)) {
        //                 CloseView(currentView);
        //             }
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(openMethod), openMethod, null);
        //     }
        //     
        //     OpenView(uiView, viewID);
        //     return uiView;
        // }
    }
}