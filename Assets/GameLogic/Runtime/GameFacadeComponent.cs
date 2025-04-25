using System.Collections;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime
{
    public abstract class GameFacadeComponent
    {
        public GameObject GameObject { get; private set; }

        protected GameFacadeComponent(GameObject gameObject)
        {
            GameObject = gameObject;
        }
        
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return GameFacade.Scheduler.StartCoroutine(routine);
        }
    }
}