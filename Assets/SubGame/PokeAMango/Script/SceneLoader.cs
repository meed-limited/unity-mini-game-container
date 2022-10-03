using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{
    public class SceneLoader : MonoBehaviour
    {
        static AsyncOperationHandle _currentSceneHandle;
        
        public void ToMainScene()
        {
            AsyncOperationHandle operationHandle = Addressables.LoadSceneAsync("MainScene");
            operationHandle.Completed += OnSceneLoaded;
        }
        
        public void ToGameScene()
        {
            AsyncOperationHandle operationHandle = Addressables.LoadSceneAsync("SuddenDeath");
            operationHandle.Completed += OnSceneLoaded;
        }

        public void ToTourScene()
        {
            AsyncOperationHandle operationHandle = Addressables.LoadSceneAsync("Tourment");
            operationHandle.Completed += OnSceneLoaded;
        }
        
        void OnSceneLoaded(AsyncOperationHandle operationHandle)
        {
            //Debug.Log("        void OnSceneLoaded(AsyncOperationHandle operationHandle)");
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