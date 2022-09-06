using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace SuperUltra.Container
{

    public class AddressableManager : MonoBehaviour
    {
        [SerializeField] bool _shouldDownload;
        [SerializeField] bool _deleteCache;
        [SerializeField] GameListUI _menuUIManager;
        [SerializeField] GameInfoList _gameInfoListAndroid;
        [SerializeField] GameInfoList _gameInfoListIOS;
        static bool _intialized = false;
        static bool _hasSubscribed = false;
        static AsyncOperationHandle _currentSceneHandle;

        void OnEnable()
        {
            if (!_hasSubscribed)
            {
                ContainerInterface.OnReturnMenu += UnloadScene;
                _hasSubscribed = true;
            }
        }

        void Start()
        {
            Debug.Log($"Caching.cacheCount {Caching.cacheCount}");

            if (Caching.cacheCount > 0 && _deleteCache)
            {
                Debug.Log($"deleteing cache");
                Caching.ClearCache();
            }

            if (!_intialized)
            {
                _intialized = true;
                Addressables.InitializeAsync().Completed += (obj) =>
                {
                    foreach (GameInfo item in GetGameList())
                    {
                        DownloadRemoteCatalog(item.gameName, item.catalogName, item.mainSceneName, item.gameId);
                    }
                };
            }
            else
            {
                foreach (GameInfo item in GetGameList())
                {
                    DownloadRemoteCatalog(item.gameName, item.catalogName, item.mainSceneName, item.gameId);
                }
            }
        }

        void PrintProfile()
        {
            Addressables.InternalIdTransformFunc += location =>
            {
                if (location.InternalId.StartsWith("http://") && location.InternalId.EndsWith(".json"))
                {
                    //Do something with remote catalog location.
                }

                return location.InternalId;
            };
        }

        // Update is called once per frame
        void Update()
        {

        }

        List<GameInfo> GetGameList()
        {
            List<GameInfo> gameInfoList;
#if UNITY_ANDROID
            gameInfoList = _gameInfoListAndroid.list;
#elif UNITY_IOS
            gameInfoList = _gameInfoListIOS.list;
#else
            gameInfoList = _gameInfoListAndroid.list;
#endif      
            return gameInfoList;
        }

        void DownloadScene(string gameName, string landingSceneName, int gameId)
        {
            if (!_shouldDownload)
            {
                return;
            }

            AsyncOperationHandle<IList<IResourceLocation>> operationHandle = Addressables.LoadResourceLocationsAsync("Scene");
            operationHandle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    CreateButtons(gameName, obj.Result, landingSceneName, gameId);
                }
            };
        }

        void CreateButtons(string gameName, IList<IResourceLocation> locations, string landingSceneName, int gameId)
        {
            foreach (IResourceLocation item in locations)
            {
                Debug.Log($"{gameName} {item.PrimaryKey}");
            }
            Addressables.GetDownloadSizeAsync(locations).Completed += (obj) =>
            {
                if(obj.Status == AsyncOperationStatus.Succeeded)
                {
                    _menuUIManager.CreateButtons(
                        gameName,
                        obj.Result,
                        () =>
                        {
                            DownloadDependeny(locations, landingSceneName, gameId);
                        }
                    );
                }
            };
        }

        void DownloadDependeny(IList<IResourceLocation> item, string landingSceneName, int gameId)
        {
            AsyncOperationHandle operationHandle = Addressables.DownloadDependenciesAsync(item);
            StartCoroutine(UpdateProgress(operationHandle, "Downloading dependencies..."));
            operationHandle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    LoadGameScene(landingSceneName, gameId);
                    _menuUIManager.UpdateResult("Downloading dependencies...", true);
                }
            };
        }

        void LoadGameScene(string landingSceneName, int gameId)
        {
            AsyncOperationHandle operationHandle = Addressables.LoadSceneAsync(landingSceneName);
            operationHandle.Completed += (AsyncOperationHandle obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    GameData.currentGameId = gameId;
                    _currentSceneHandle = obj;
                    Debug.Log("Load Success");
                }
                else
                {
                    Debug.Log("Load Failed");
                }
            };
        }

        void DownloadRemoteCatalog(string gameName, string catalogName, string landingSceneName, int gameId)
        {
            AsyncOperationHandle operationHandle = Addressables.LoadContentCatalogAsync(
                $"{Config.RemoteStagingCatalogUrl}/{gameName}/{Config.BuildTarget}/{catalogName}", true
            );
            Debug.Log($"{Config.RemoteStagingCatalogUrl}/{gameName}/{Config.BuildTarget}/{catalogName}");
            StartCoroutine(UpdateProgress(operationHandle, $"Retrive {gameName} {catalogName} data from aws"));
            operationHandle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    DownloadScene(gameName, landingSceneName, gameId);
                    _menuUIManager.UpdateResult($"Retrive {gameName} {catalogName} data from aws", true);
                }
                else
                {
                    _menuUIManager.UpdateResult($"Retrive {gameName} {catalogName} data from aws", false);
                }
            };
        }

        IEnumerator UpdateProgress(AsyncOperationHandle op, string taskName)
        {
            while (op.IsValid() && op.PercentComplete < 1)
            {
                _menuUIManager.UpdateProgress(op.PercentComplete, taskName);
                yield return null;
            }
        }

        void UnloadScene()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync("MenuScene");
            operation.completed += (obj) =>
            {
                Debug.Log("loaded");
                if (_currentSceneHandle.IsValid())
                {
                    Debug.Log("unloaded");
                    Addressables.UnloadSceneAsync(_currentSceneHandle);
                }
            };
        }

    }

}
