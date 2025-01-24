using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JY 
{
    public class SceneLoaderEx : MonoBehaviour
    {
        public static SceneLoaderEx Instance => _instance;
        private static SceneLoaderEx _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadSceneAsync(string sceneName, Action callback = null)
            => StartCoroutine(Co_LoadSceneAsync(sceneName, callback));

        private IEnumerator Co_LoadSceneAsync(string sceneName, Action callback = null)
        {
            var op = SceneManager.LoadSceneAsync(sceneName);
            op.completed += (op) =>
            {
                op.allowSceneActivation = true;
                callback?.Invoke();
            };

            while (!op.isDone)
            {
                yield return null;
            }
            Debug.Log($"{sceneName} Loaded.");
        }
    }
}

