using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BestHTTP;

namespace SuperUltra.Container
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] LeaderboardItemUI _leaderboardUIPrefab;
        [SerializeField] RectTransform _rankingItemContainer;
        [SerializeField] RectTransform _gameBannerPrefab;
        [SerializeField] RectTransform _gameBannerContainer;
        [SerializeField] StickyScrollUI _gameBannerStickyScroll;
        [SerializeField] LeaderboardItemUI _userLeaderboardUI;
        [SerializeField] TMP_Text _gameName;
        [SerializeField] TMP_Text _poolSize;
        [SerializeField] TMP_Text _timeLeft;
        [SerializeField] ScrollRect _leaderboardScroll;
        [SerializeField] LoadingUI _loadingUI;
        int _currentGameId = -1;
        Dictionary<int, int> _pageToGameIdMap = new Dictionary<int, int>();
        int lazyLoadCount = 10;
        bool _isRequested = false;

        // Start is called before the first frame update
        void Start()
        {
            CreateGameList();
            SetDefaultLeaderboard();
        }

        public void Initialize()
        {
            SetDefaultLeaderboard();
        }

        void SetDefaultLeaderboard()
        {
            if (_currentGameId <= 0 && GameData.gameDataList.Count > 0)
            {
                var enumerator = GameData.gameDataList.GetEnumerator();
                enumerator.MoveNext();
                KeyValuePair<int, GameData> element = enumerator.Current;
                _currentGameId = element.Value.id;
                Debug.Log("Set Default");
                RefreshLeaderboard(_currentGameId);
            }
        }

        void Update()
        {
            UpdateTimeLeft(_currentGameId);
            DetectScrollLazyLoad();
        }

        void CreateUserTournamentData(int gameId)
        {
            if (_userLeaderboardUI == null)
            {
                return;
            }
            if (!GameData.gameDataList.TryGetValue(gameId, out GameData gameData))
            {
                return;
            }


            LeaderboardUserData userRank = new LeaderboardUserData()
            {
                rankPosition = gameData.currentUserPosition,
                avatarTexture = UserData.profilePic,
                name = UserData.userName,
                score = gameData.currentUserScore
            };
            Debug.Log("CreateUserTournamentData " + userRank.rankPosition + " " + userRank.name);
            _userLeaderboardUI.SetData(userRank);
        }

        void CreateGameList()
        {
            int pageCount = 0;
            foreach (var game in GameData.gameDataList)
            {
                RectTransform gameBanner = Instantiate(_gameBannerPrefab, _gameBannerContainer);
                SetBannerImage(gameBanner.GetComponent<Image>(), game.Key);
                _pageToGameIdMap.Add(pageCount, game.Key);
                pageCount++;
            }
            _gameBannerStickyScroll.Initialize();
            _gameBannerStickyScroll.OnItemChange.AddListener(OnGameChange);
        }


        void SetBannerImage(Image image, int gameId)
        {
            GameInfo info = AddressableManager.GetGameInfo(gameId);
            if (image == null || info == null)
            {
                if (image != null)
                {
                    SetDefaultImage(image);
                }
                return;
            }
            image.sprite = info.bannerImage;
        }

        void SetDefaultImage(Image image)
        {
            if (image == null)
            {
                return;
            }
            image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            float value = UnityEngine.Random.Range(0.1f, 0.9f);
            float value1 = UnityEngine.Random.Range(0.1f, 0.9f);
            float value2 = UnityEngine.Random.Range(0.1f, 0.9f);

            image.color = new Color(value, value1, value2);
        }

        void OnGameChange(float page)
        {
            if (_pageToGameIdMap.TryGetValue(Mathf.FloorToInt(page), out int id))
            {
                Debug.Log("OnGameChange " + page);
                _currentGameId = id;
                RefreshLeaderboard(_currentGameId);
            }
        }

        void ClearLeaderBoard()
        {
            foreach (Transform child in _rankingItemContainer)
            {
                Destroy(child.gameObject);
            }
            _rankingItemContainer.sizeDelta = Vector2.zero;
        }

        public void RefreshLeaderboard(int gameID = 0)
        {
            ClearLeaderBoard();
            _loadingUI.Show();
            NetworkManager.GetLeaderboard(gameID, 0, (GetLeaderboardResponseData data) =>
            {
                _loadingUI.Hide();
                bool isEmpty = data.list.Length <= 0;
                _leaderboardScroll.enabled = !isEmpty && data.result;
                if (data.result && !isEmpty)
                {
                    LazyLoadLeaderBoard(gameID);
                }
                CreateUserTournamentData(gameID);
            });
            UpdateTournamentInfo(gameID);
        }

        void DetectScrollLazyLoad()
        {
            if (!_leaderboardScroll || !_leaderboardScroll.enabled || _isRequested)
            {
                return;
            }

            // only detect when there are items in board 
            // and scrolled to bottom
            if (_leaderboardScroll.verticalNormalizedPosition <= 0)
            {
                if (!GameData.gameDataList.TryGetValue(_currentGameId, out GameData gameData))
                {
                    return;
                }
                if (gameData.leaderboard == null)
                {
                    return;
                }
                Debug.Log($"{gameData.leaderboard.Count} {_rankingItemContainer.childCount + lazyLoadCount}");
                if (gameData.leaderboard.Count < _rankingItemContainer.childCount + lazyLoadCount)
                {
                    Debug.Log("Requesting");
                    _isRequested = true;
                    _loadingUI.Show();
                    NetworkManager.GetLeaderboard(
                        _currentGameId, 
                        _rankingItemContainer.childCount, 
                        OnGetLeaderboardRequestFinish
                    );
                    return;
                }
                LazyLoadLeaderBoard(_currentGameId);
            }
        }

        void OnGetLeaderboardRequestFinish(GetLeaderboardResponseData data)
        {
            _loadingUI.Hide();
            if (!data.result)
            {
                return;
            }
            LazyLoadLeaderBoard(_currentGameId);
            _isRequested = false;
        }

        void LazyLoadLeaderBoard(int gameID)
        {
            if (!GameData.gameDataList.TryGetValue(gameID, out GameData gameData))
            {
                return;
            }
            if (gameData.leaderboard.Count < _rankingItemContainer.childCount + lazyLoadCount)
            {
                return;
            }

            int index = Mathf.Max(0, _rankingItemContainer.childCount - 1);
            List<LeaderboardUserData> list = gameData.leaderboard.GetRange(index, lazyLoadCount);
            foreach (var data in list)
            {
                LeaderboardItemUI item = Instantiate(_leaderboardUIPrefab, _rankingItemContainer);
                Debug.Log("data " + data.rankPosition + " " + data.score + " " + data.reward.ToString());
                item.SetData(data);
            }
            // a hack to prevent _rankingItemContainer.childCount is 0 after LazyLoadLeaderBoard 
            _leaderboardScroll.verticalNormalizedPosition = (1f / (float)_rankingItemContainer.childCount);
        }

        void UpdateTimeLeft(int gameId)
        {
            if (!GameData.gameDataList.TryGetValue(gameId, out GameData data))
            {
                return;
            }

            if (_timeLeft != null)
            {
                // calculate time left using tounament.endTime and Date.now()
                TimeSpan timeLeft = data.tournament.endTime - DateTime.Now;
                string timeText = timeLeft.ToString(@"dd\dhh\hmm\mss\s");
                _timeLeft.text = timeText;
            }
        }

        public void UpdateTournamentInfo(int gameId)
        {
            if (!GameData.gameDataList.TryGetValue(gameId, out GameData data))
            {
                return;
            }

            if (_gameName != null)
            {
                _gameName.text = data.name;
            }

            if (_poolSize != null)
            {
                _poolSize.text = data.tournament.prizePool.ToString();
            }
        }

    }

}
