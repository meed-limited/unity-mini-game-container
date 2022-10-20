using System;
using System.Collections;
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
        [SerializeField] RectTransform _emptyLeaderboardMessage;
        [SerializeField] StickyScrollUI _gameBannerStickyScroll;
        [SerializeField] LeaderboardItemUI _userLeaderboardUI;
        [SerializeField] TMP_Text _gameName;
        [SerializeField] TMP_Text _poolSize;
        [SerializeField] TMP_Text _day;
        [SerializeField] TMP_Text _hour;
        [SerializeField] TMP_Text _minute;
        [SerializeField] ScrollRect _leaderboardScroll;
        [Tooltip("Loading icon in leaderboard list")]
        [SerializeField] LoadingUI _loadingUI;
        [Tooltip("Shows when there are no game data receive from server")]
        [SerializeField] RectTransform _emptyGameMessage;
        int _currentGameId = -1;
        int _nextPage = 0;
        bool _isLastPage = false;
        Dictionary<int, int> _pageToGameIdMap = new Dictionary<int, int>();
        int _lazyLoadCount = 10;
        bool _isRequested = false;

        public void Initialize()
        {
            if (GameData.gameDataList.Count <= 0)
            {
                SetEmptyLeaderboard(true);
                return;
            }

            SetEmptyLeaderboard(false);
            CreateGameList();
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
                score = gameData.currentUserScore,
                reward = gameData.currentUserReward
            };
            _userLeaderboardUI.SetData(userRank);
        }

        void CreateGameList()
        {
            int pageCount = 0;
            ClearGameBanner();
            _pageToGameIdMap.Clear();
            _gameBannerStickyScroll.OnItemChange.RemoveAllListeners();

            foreach (var game in GameData.gameDataList)
            {
                GameData gameData = game.Value;
                RectTransform gameBanner = Instantiate(_gameBannerPrefab, _gameBannerContainer);
                SetBannerImage(gameBanner.GetComponent<Image>(), game.Key);
                _pageToGameIdMap.Add(pageCount, game.Key);
                pageCount++;
            }
            _gameBannerStickyScroll.Initialize();
            _gameBannerStickyScroll.OnItemChange.AddListener(OnGameChange);
        }

        void SetEmptyLeaderboard(bool shouldShow)
        {
            _emptyGameMessage.gameObject.SetActive(shouldShow);
        }

        void ClearGameBanner()
        {
            foreach (Transform item in _gameBannerContainer.transform)
            {
                Destroy(item.gameObject);
            }
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

        void RefreshLeaderboard(int gameId = 0)
        {
            LoadingUI.ShowInstance();
            ClearLeaderBoard();
            _nextPage = 0;
            _isLastPage = false;
            RequestLeaderboardList();
            NetworkManager.GetTournament(
                gameId,
                (GetTournamentResponseData data) =>
                {
                    LoadingUI.HideInstance();
                    if (!data.result)
                    {
                        MessagePopUpUI.Show(data.message);
                    }
                    UpdateTournamentInfo(gameId, data);
                }
            );
        }

        void DetectScrollLazyLoad()
        {
            if (!_leaderboardScroll || _isRequested || _isLastPage)
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
                RequestLeaderboardList();
            }
        }

        void RequestLeaderboardList()
        {
            _isRequested = true;
            _loadingUI.Show();
            NetworkManager.GetLeaderboard(
                _currentGameId,
                _nextPage,
                _lazyLoadCount,
                OnGetLeaderboardRequestFinish
            );
        }

        void OnGetLeaderboardRequestFinish(GetLeaderboardResponseData data)
        {
            _loadingUI.Hide();
            _nextPage = data.nextPage;
            _isLastPage = _nextPage == -1;
            CreateUserTournamentData(_currentGameId);
            LazyLoadLeaderBoard(data);
            _isRequested = false;
        }

        void LazyLoadLeaderBoard(GetLeaderboardResponseData responseData)
        {

            _emptyLeaderboardMessage.gameObject.SetActive(false);
            if (!responseData.result || responseData.list == null)
            {
                string message = string.IsNullOrEmpty(responseData.message) ? "ServerError" : responseData.message;
                MessagePopUpUI.Show(message);
                return;
            }

            if (_emptyLeaderboardMessage && responseData.list.Length <= 0)
            {
                _emptyLeaderboardMessage.gameObject.SetActive(true);
                return;
            }

            int index = Mathf.Max(0, _rankingItemContainer.childCount - 1);
            foreach (var data in responseData.list)
            {
                LeaderboardItemUI item = Instantiate(_leaderboardUIPrefab, _rankingItemContainer);
                item.SetData(data);
            }
        }

        void UpdateTimeLeft(int gameId)
        {
            if (!GameData.gameDataList.TryGetValue(gameId, out GameData data))
            {
                return;
            }
            TimeSpan timeLeft = data.tournament.endTime - DateTime.Now;
            if (_day != null)
                _day.text = timeLeft.Days.ToString();
            if (_hour != null)
                _hour.text = timeLeft.Hours.ToString();
            if (_minute != null)
                _minute.text = timeLeft.Minutes.ToString();
        }

        public void UpdateTournamentInfo(int gameId, GetTournamentResponseData responseData)
        {

            if (!responseData.result)
            {
                if (_gameName != null)
                {
                    _gameName.text = "----";
                }
                if (_poolSize != null)
                {
                    _poolSize.text = "----";
                }
                return;
            }

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
