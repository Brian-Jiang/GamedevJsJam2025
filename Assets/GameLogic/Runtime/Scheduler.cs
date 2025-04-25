using System;
using System.Collections;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime
{
    public class Scheduler : MonoBehaviour
    {
        public event Action OnStart;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action onDestroy;

        private void Start() {
            OnStart?.Invoke();
        }

        private void Update() {
            OnUpdate?.Invoke();
        }

        private void FixedUpdate() {
            OnFixedUpdate?.Invoke();
        }
        
        private void OnDestroy() {
            onDestroy?.Invoke();
        }
    }
}