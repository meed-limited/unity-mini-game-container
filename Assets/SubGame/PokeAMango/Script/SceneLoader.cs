using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace SuperUltra.PokeAMango
{
    public class SceneLoader : MonoBehaviour
    {
        static AsyncOperationHandle _currentSceneHandle;
        
        public void ToMainScene()
        {
            AsyncOperationHandle<SceneInstance> operationHandle = Addressables.LoadSceneAsync("MainScene");
            operationHandle.Completed += OnSceneLoaded;
        }
        
        public void ToGameScene()
        {
            AsyncOperationHandle<SceneInstance> operationHandle = Addressables.LoadSceneAsync("SuddenDeath");
            Debug.Log("Poke");
            operationHandle.Completed += OnSceneLoaded;
        }

        public void ToTourScene()
        {
            AsyncOperationHandle<SceneInstance> operationHandle = Addressables.LoadSceneAsync("Tourment");
            operationHandle.Completed += OnSceneLoaded;
        }
        
        void OnSceneLoaded(AsyncOperationHandle<SceneInstance> operationHandle)
        {
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("debug" + operationHandle.Result.Scene.name);
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