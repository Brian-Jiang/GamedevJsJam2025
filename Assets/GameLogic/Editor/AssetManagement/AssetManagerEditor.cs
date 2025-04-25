using CoinDash.GameLogic.Runtime.AssetManagement;
using UnityEditor;

namespace CoinDash.GameLogic.Editor.AssetManagement {
    public static class AssetManagerEditor {
        private const string BundleModeMenuName = "Settings/Use Bundle Mode";
        private const string SimulateAsyncLoadMenuName = "Settings/Simulate Async Load";

        // [MenuItem(BundleModeMenuName, priority = 1000)]
        // private static void SwitchBundleMode() {
        //     AssetManager.UseBundleMode = !AssetManager.UseBundleMode;
        // }
        //
        // [MenuItem(BundleModeMenuName, true, priority = 1000)]
        // public static bool SwitchBundleModeValidate() {
        //     Menu.SetChecked(BundleModeMenuName, AssetManager.UseBundleMode);
        //     return true;
        // }
        
        [MenuItem(SimulateAsyncLoadMenuName, priority = 1000)]
        private static void SwitchSimulateAsyncLoad() {
            AssetManager.SimulateAsyncLoad = !AssetManager.SimulateAsyncLoad;
        }
        
        [MenuItem(SimulateAsyncLoadMenuName, true, priority = 1000)]
        public static bool SwitchSimulateAsyncLoadValidate() {
            Menu.SetChecked(SimulateAsyncLoadMenuName, AssetManager.SimulateAsyncLoad);
            return true;
        }
    }
}