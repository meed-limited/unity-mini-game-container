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
        [SerializeField] GameListUI _gameListUI;
        [SerializeField] GameInfoList _gameInfoListAndroid;
        [SerializeField] GameInfoList _gameInfoListIOS;
        static bool _intialized = false;
        static bool _hasSubscribed = false;
        static AsyncOperationHandle _currentSceneHandle;
        static Dictionary<int, GameInfo> _gameInfoMap = new Dictionary<int, GameInfo>();

        void OnEnable()
        {
            if (!_hasSubscribed)
            {
                ContainerInterface.OnReturnMenu += UnloadScene;
                _hasSubscribed = true;
            }
        }

        void OnDestory()
        {
            ContainerInterface.OnReturnMenu -= UnloadScene;
        }

        void Start()
        {
            Debug.Log($"Caching.cacheCount {Caching.cacheCount}");

            if (Caching.cacheCount > 0 && _deleteCache)
            {
                Debug.Log($"deleteing cache");
                Caching.ClearCache();
            }

            List<GameInfo> gameInfoList = GetGameList();
            SetGameInfoMap(gameInfoList);

            if (!_intialized)
            {
                _intialized = true;
                Addressables.InitializeAsync().Completed += (obj) =>
                {
                    foreach (GameInfo item in gameInfoList)
                    {
                        DownloadRemoteCatalog(item.remoteFolderName, item.catalogName, item.mainSceneKey, item.gameId);
                    }
                };
            }
            else
            {
                foreach (GameInfo item in gameInfoList)
                {
                    DownloadRemoteCatalog(item.remoteFolderName, item.catalogName, item.mainSceneKey, item.gameId);
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

        void SetGameInfoMap(List<GameInfo> gameInfoList)
        {
            if (_gameInfoMap == null)
            {
                _gameInfoMap = new Dictionary<int, GameInfo>();
            }

            foreach (GameInfo item in gameInfoList)
            {
                if (!_gameInfoMap.ContainsKey(item.gameId))
                {
                    _gameInfoMap.Add(item.gameId, item);
                }
            }
        }

        void CreateButtons(string gameName, IList<IResourceLocation> locations, string landingSceneName, int gameId)
        {
            foreach (IResourceLocation item in locations)
            {
                Debug.Log($"{gameName} {item.PrimaryKey}");
            }
            Addressables.GetDownloadSizeAsync(locations).Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    _gameListUI.CreateButtons(
                        gameName,
                        gameId,
                        obj.Result,
                        GetPosterImage(gameId),
                        () =>
                        {
                            DownloadDependeny(locations, landingSceneName, gameId);
                        }
                    );
                }
            };
        }

        Sprite GetPosterImage(int gameId)
        {
            if (_gameInfoMap.TryGetValue(gameId, out GameInfo gameInfo))
            {
                return gameInfo.posterImage;
            }
            return null;
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
                    _gameListUI.UpdateResult("Downloading dependencies...", true);
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
                    SessionData.currentGameId = gameId;
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
                    _gameListUI.UpdateResult($"Retrive {gameName} {catalogName} data from aws", true);
                }
                else
                {
                    _gameListUI.UpdateResult($"Retrive {gameName} {catalogName} data from aws", false);
                }
            };
        }

        IEnumerator UpdateProgress(AsyncOperationHandle op, string taskName)
        {
            while (op.IsValid() && op.PercentComplete < 1)
            {
                _gameListUI.UpdateProgress(op.PercentComplete, taskName);
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

        public static GameInfo GetGameInfo(int gameId)
        {
            if (_gameInfoMap == null || !_gameInfoMap.ContainsKey(gameId))
            {
                return null;
            }
            return _gameInfoMap[gameId];
        }

    }

}
