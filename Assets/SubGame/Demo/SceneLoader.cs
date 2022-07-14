using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace SuperUltra.Demo
{
    public class SceneLoader : MonoBehaviour
    {
        static AsyncOperationHandle _currentSceneHandle;

        public void ToMainScene()
        {
            AsyncOperationHandle<SceneInstance> operationHandle = Addressables.LoadSceneAsync("DemoMainScene");
            operationHandle.Completed += OnSceneLoaded;
        }

        public void ToGameScene()
        {
            AsyncOperationHandle<SceneInstance> operationHandle = Addressables.LoadSceneAsync("DemoGameScene");
            Debug.Log("DEMO");
            operationHandle.Completed += OnSceneLoaded;
        }

        void OnSceneLoaded(AsyncOperationHandle<SceneInstance> operationHandle)
        {
            Debug.Log("debug" + operationHandle.Result.Scene.name);
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                // Unload previous scene
                if (_currentSceneHandle.IsValid())
                {
                    Debug.Log("unloaded");
                    Addressables.UnloadSceneAsync(_currentSceneHandle);
                }
                // Save current scene handle
                _currentSceneHandle = operationHandle;
            }
        }
    }
}