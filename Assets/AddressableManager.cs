using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace SuperUltra.Container
{

    public class AddressableManager : MonoBehaviour
    {
        [SerializeField]
        bool shouldDownload;
        [SerializeField]
        bool _deleteCache;
        [SerializeField]
        RectTransform _loadButton;
        [SerializeField]
        TMP_Text _progressText;
        [SerializeField]
        Image _progressBar;
        [SerializeField]
        RectTransform _buttonContainer;
        AsyncOperationHandle _op;

        void Start()
        {
            // PrintProfile();
            Debug.Log($"Caching.cacheCount {Caching.cacheCount}");
            if (Caching.cacheCount > 0 && _deleteCache)
            {
                Debug.Log($"deleteing cache");
                Caching.ClearCache();
            }

            Addressables.InitializeAsync().Completed += (obj) =>
            {
                // DownloadScene("GameScene");
                DownloadRemoteCatalog();
            };
        }

        void PrintProfile()
        {
            Addressables.InternalIdTransformFunc += location =>
            {
                if (location.InternalId.StartsWith("http://") && location.InternalId.EndsWith(".json"))
                {
                    //Do something with remote catalog location.
                }
                Debug.Log("location.InternalId  " + location.InternalId);

                return location.InternalId;
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

        void DownloadScene(string key)
        {
            if (!shouldDownload)
            {
                return;
            }

            AsyncOperationHandle<IList<IResourceLocation>> operationHandle = Addressables.LoadResourceLocationsAsync(key);
            Addressables.GetDownloadSizeAsync(key).Completed += (obj) =>
            {
                Debug.Log($"GetDownloadSizeAsync status: {obj.Status}");
                Debug.Log($"Download size: " + obj.Result + " bytes");
            };
            operationHandle.Completed += (obj) =>
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
                RectTransform loadButton = Instantiate(_loadButton, _buttonContainer);
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
            AsyncOperationHandle operationHandle = Addressables.DownloadDependenciesAsync(item.PrimaryKey);
            StartCoroutine(UpdateProgress(operationHandle, "Downloading dependencies..."));
            operationHandle.Completed += (obj2) =>
            {
                if (obj2.Status == AsyncOperationStatus.Succeeded)
                {
                    LoadGameScene(item);
                    UpdateResult("Downloading dependencies...", true);
                }
            };
        }

        void LoadGameScene(IResourceLocation item)
        {
            AsyncOperationHandle operationHandle = Addressables.LoadSceneAsync(item.PrimaryKey);
            operationHandle.Completed += (AsyncOperationHandle<ScneeIns> obj) =>
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

        void DownloadRemoteCatalog()
        {
            AsyncOperationHandle operationHandle = Addressables.LoadContentCatalogAsync(
                $"{Config.RemoteStagingCatalogUrl}/poke-a-mango/{Config.BuildTarget}/{Config.CatalogName}", true
            );
            StartCoroutine(UpdateProgress(operationHandle, "Retrive data from aws"));
            operationHandle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    DownloadScene("MainScene");
                    UpdateResult("Retrive data from aws", true);
                }
                else
                {
                    UpdateResult("Retrive data from aws", false);
                }
            };
        }

        IEnumerator UpdateProgress(AsyncOperationHandle op, string taskName)
        {
            while (op.IsValid() && op.PercentComplete < 1)
            {
                _progressBar.fillAmount = op.PercentComplete;
                _progressText.text = taskName;
                yield return null;
            }
        }

        void UpdateResult(string taskName, bool result)
        {
            _progressBar.fillAmount = 1;
            _progressText.text = $"{taskName} {(result ? "Success" : "Failed")}";
        }

    }

}
