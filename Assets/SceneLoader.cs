using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SuperUltra
{

    public class SceneLoader : MonoBehaviour
    {
        public AssetReference scene;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log(obj.Result.Scene.name + " Load Success");
            }
        }

        public void ToGameScene()
        {
            AsyncOperationHandle<SceneInstance> operation = scene.LoadSceneAsync();
            operation.Completed += OnSceneLoaded;
        }
    }

}