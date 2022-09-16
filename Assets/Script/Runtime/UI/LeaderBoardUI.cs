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
        int _currentGameId = -1;
        Dictionary<int, int> _pageToGameIdMap = new Dictionary<int, int>();
        int lazyLoadCount = 10;
        bool _isRequested = false;

        // Start is called before the first frame update
        void Start()
        {
            CreateGameList();
            CreateUserRank();
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

        void CreateUserRank()
        {
            if (_userLeaderboardUI == null)
            {
                return;
            }
            LeaderboardUserData userRank = new LeaderboardUserData()
            {
                rankPosition = 45, // TODO
                avatarTexture = UserData.profilePic,
                name = UserData.userName,
                score = 608 // TODO 
            };
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
                Debug.Log("OnGameChange");
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
            LazyLoadLeaderBoard(gameID);
            UpdateTournamentInfo(gameID);
        }

        void DetectScrollLazyLoad()
        {
            if (!_leaderboardScroll || _isRequested)
            {
                return;
            }

            Debug.Log(_leaderboardScroll.normalizedPosition.y);
            // scroll to bottom
            if (_leaderboardScroll.normalizedPosition.y < 0)
            {
                if (!GameData.gameDataList.TryGetValue(_currentGameId, out GameData gameData))
                {
                    return;
                }
                if (gameData.leaderboard == null)
                {
                    return;
                }
                if (gameData.leaderboard.Count < _rankingItemContainer.childCount + lazyLoadCount)
                {
                    _isRequested = true;
                    NetworkManager.GetLeaderboard(_currentGameId, _rankingItemContainer.childCount, () =>
                    {
                        LazyLoadLeaderBoard(_currentGameId);
                        // a hack to prevent _rankingItemContainer.childCount is 0 after LazyLoadLeaderBoard 
                        _leaderboardScroll.normalizedPosition = new Vector2(0, (1f / (float)_rankingItemContainer.childCount));
                        _isRequested = false;
                    });
                    return;
                }
                Debug.Log("DetectScrollLazyLoad");
                LazyLoadLeaderBoard(_currentGameId);
            }
        }

        void LazyLoadLeaderBoard(int gameID)
        {
            Debug.Log("LazyLoadLeaderBoard");
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
                item.SetData(data);
            }
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
