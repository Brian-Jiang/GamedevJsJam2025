using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CoinDash.GameLogic.Runtime.Input
{
    public static class InputUtils
    {
        public static Vector2 GetPointerPosition() {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            return Mouse.current.position.ReadValue();
#elif UNITY_IOS || UNITY_ANDROID
            return Touchscreen.current.position.ReadValue();
#endif
        }

        public static bool PointerOverUI() {
            var pointer = new PointerEventData(EventSystem.current) {
                position = GetPointerPosition()
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);
            return results.Count > 0;
        }
    }
}