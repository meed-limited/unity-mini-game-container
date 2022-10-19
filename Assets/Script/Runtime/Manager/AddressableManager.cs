using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SuperUltra.Container
{

    public class AddressableManager : MonoBehaviour
    {
        [SerializeField] bool _shouldDownload;
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

        void DownloadScene(string gameName, string landingSceneName, int gameId, IResourceLocator locator)
        {
            if (!_shouldDownload)
            {
                return;
            }

            bool isLocationLoaded = locator.Locate("Scene", null, out IList<IResourceLocation> locations);
            List<string> keyList = new List<string>();
            if (isLocationLoaded)
            {
                foreach (var item in locations)
                {
                    if (!keyList.Contains(item.PrimaryKey))
                    {
                        keyList.Add(item.PrimaryKey);
                    }
                }
            }

            if (isLocationLoaded)
            {
                CreateButtons(gameName, locations, landingSceneName, gameId);
            }

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
            // Addressables.ClearDependencyCacheAsync(locations);
            Addressables.GetDownloadSizeAsync(locations).Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    float downloadSize = obj.Result; 
                    Debug.Log($"{downloadSize}, {gameName}");
                    _gameListUI.CreateButtons(
                        gameName,
                        gameId,
                        downloadSize,
                        GetPosterImage(gameId),
                        () =>
                        {
                            if(downloadSize > 1)
                            {
                                Debug.Log(gameName + " download game");
                                DownloadDependeny(locations, landingSceneName, gameId, downloadSize);
                            }else
                            {
                                Debug.Log(gameName + " enter game");
                                LoadGameScene(landingSceneName, gameId);
                            }
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

        void DownloadDependeny(IList<IResourceLocation> item, string landingSceneName, int gameId, float downloadSize)
        {
            AsyncOperationHandle operationHandle = Addressables.DownloadDependenciesAsync(item);
            if (downloadSize > 1)
            {
                ShowDownloadDisplay(gameId);
                StartCoroutine(UpdateProgress(operationHandle, gameId));
            }
            operationHandle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    // "Downloading dependencies...",
                    Debug.Log("download complete");
                    _gameListUI.SetDownloadIconVisible(gameId, true);
                    _gameListUI.SetButtonCallback(
                        gameId,
                        () => LoadGameScene(landingSceneName, gameId)
                    );
                }
            };
        }

        void LoadGameScene(string landingSceneName, int gameId)
        {
            AsyncOperationHandle operationHandle = Addressables.LoadSceneAsync(landingSceneName);
            LoadingUI.ShowInstance();
            operationHandle.Completed += (AsyncOperationHandle obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    LoadingUI.HideInstance();
                    SessionData.currentGameId = gameId;
                    _currentSceneHandle = obj;
                    Debug.Log("LoadGameScene Success");
                }
                else
                {
                    Debug.Log("LoadGameScene Failed");
                }
            };
        }

        void DownloadRemoteCatalog(string gameName, string catalogName, string landingSceneName, int gameId)
        {
            AsyncOperationHandle<IResourceLocator> operationHandle = Addressables.LoadContentCatalogAsync(
                $"{Config.RemoteStagingCatalogUrl}/{gameName}/{Config.BuildTarget}/{catalogName}", true
            );

            // Debug.Log($"{Config.RemoteStagingCatalogUrl}/{gameName}/{Config.BuildTarget}/{catalogName}");

            operationHandle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    DownloadScene(gameName, landingSceneName, gameId, obj.Result);
                    // "Retrive {gameName} {catalogName} data from aws true"
                }
                else
                {
                    // "Retrive {gameName} {catalogName} data from aws false"
                }
            };
        }

        IEnumerator UpdateProgress(AsyncOperationHandle op, int gameId)
        {
            while (op.IsValid() && op.PercentComplete < 1)
            {
                _gameListUI.UpdateProgress(op.PercentComplete, gameId);
                yield return null;
            }
        }

        void ShowDownloadDisplay(int gameId)
        {
            _gameListUI.ShowDownloadDisplay(gameId);
        }

        void UnloadScene()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync("MenuScene");
            operation.completed += (obj) =>
            {
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
