using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

namespace SuperUltra
{

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        bool shouldDownload;
        [SerializeField]
        bool _deleteCache;
        [SerializeField]
        RectTransform m_loadButton;
        [SerializeField]
        TMP_Text _progressText;
        [SerializeField]
        RectTransform m_buttonContainer;
        AsyncOperationHandle _op;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log($"Caching.cacheCount {Caching.cacheCount}");
            // if(Caching.cacheCount > 0 && _deleteCache)
            // {
            //     Caching.ClearCache();
            // }
            
            Addressables.InitializeAsync().Completed += (obj) =>
            {
                DownloadScene();
            };
        }

        // Update is called once per frame
        void Update()
        {
            // show _op progress
            if (_op.IsValid())
            {
                _progressText.text = $"Loading... {_op.PercentComplete:P0}";
            }
        }

        void DownloadScene()
        {
            if (!shouldDownload)
            {
                return;
            }

            AsyncOperationHandle<IList<IResourceLocation>> op = Addressables.LoadResourceLocationsAsync("GameScene");
            Addressables.GetDownloadSizeAsync("GameScene").Completed += (obj) =>
            {
                Debug.Log($"GetDownloadSizeAsync status: {obj.Status}");
                Debug.Log($"Download size: " + obj.Result + " bytes");
            };
            _op = op;
            op.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    CreateButtons(obj.Result);
                }
            };
        }

        void CreateButtons(IList<IResourceLocation> locations)
        {
            foreach (IResourceLocation item in locations)
            {
                RectTransform loadButton = Instantiate(m_loadButton, m_buttonContainer);
                Button button = loadButton.GetComponentInChildren<Button>();
                TMP_Text text = loadButton.GetComponentsInChildren<TMP_Text>()[1];
                button.onClick.AddListener(() =>
                {
                    DownloadDependeny(item);
                });
                button.GetComponentInChildren<TMP_Text>().text = $"{item.PrimaryKey}";
                Addressables.GetDownloadSizeAsync(item.PrimaryKey).Completed += (obj) =>
                {
                    text.text = $"Download size: " + obj.Result + " bytes";
                };
            }
        }

        void DownloadDependeny(IResourceLocation item)
        {
            Addressables.DownloadDependenciesAsync(item.PrimaryKey).Completed += (obj2) =>
            {
                if (obj2.Status == AsyncOperationStatus.Succeeded)
                {
                    LoadGameScene(item);
                    Debug.Log("Load Games Success");
                }
            };
        }

        void LoadGameScene(IResourceLocation item)
        {
            Addressables.LoadSceneAsync(item.PrimaryKey).Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("Load Success");
                }
                else
                {
                    Debug.Log("Load Failed");
                }
            };
        }
        
        void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log(obj.Result.Scene.name + " Load Success");
            }
        }

        public void ToMenu()
        {
            SceneManager.LoadScene(0);
        }

    }

}