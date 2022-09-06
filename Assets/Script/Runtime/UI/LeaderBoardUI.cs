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
        int _currentGameId = -1;
        float _listSpacing = 0;
        float _itemHeight = 0;
        Dictionary<int, int> _pageToGameMap = new Dictionary<int, int>();

        // Start is called before the first frame update
        void Start()
        {
            CacheSpacingAndheight();
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
                RefreshLeaderboard(_currentGameId);
            }
        }

        void Update()
        {
            UpdateTimeLeft(_currentGameId);
        }

        void CreateUserRank()
        {
            if (_userLeaderboardUI == null)
            {
                return;
            }
            LeaderboardUserData userRank = new LeaderboardUserData()
            {
                rankPosition = 45,
                avatarUrl = "",
                name = "LiftTastic",
                score = 608
            };
            _userLeaderboardUI.SetData(userRank);
        }

        void CreateGameList()
        {
            int pageCount = 0;
            foreach (var game in GameData.gameDataList)
            {
                RectTransform gameBanner = Instantiate(_gameBannerPrefab, _gameBannerContainer);
                gameBanner.GetComponent<Image>().sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
                float value = UnityEngine.Random.Range(0.1f, 0.9f);
                float value1 = UnityEngine.Random.Range(0.1f, 0.9f);
                float value2 = UnityEngine.Random.Range(0.1f, 0.9f);

                gameBanner.GetComponent<Image>().color = new Color(value, value1, value2);
                _pageToGameMap.Add(pageCount, game.Key);
                CreateRankingList(game.Key);
                pageCount++;
            }
            _gameBannerStickyScroll.Initialize();
            _gameBannerStickyScroll.OnItemChange.AddListener(OnGameChange);
        }

        void OnGameChange(float page)
        {
            Debug.Log("OnGameChange " + page);
            if(_pageToGameMap.TryGetValue(Mathf.FloorToInt(page), out int id))
            {
                _currentGameId = id;
                RefreshLeaderboard(_currentGameId);
            }
        }

        void CacheSpacingAndheight()
        {
            VerticalLayoutGroup verticalLayoutGroup = _rankingItemContainer.GetComponent<VerticalLayoutGroup>();
            if (verticalLayoutGroup)
            {
                _listSpacing = verticalLayoutGroup.spacing;
            }

            RectTransform itemRect = _leaderboardUIPrefab.GetComponent<RectTransform>();
            if (itemRect)
            {
                _itemHeight = itemRect.sizeDelta.y;
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

        void CreateRankingList(int gameID = 0)
        {
            if (!GameData.gameDataList.TryGetValue(gameID, out GameData gameData))
            {
                return;
            }

            foreach (var item in gameData.leaderboard)
            {
                LeaderboardItemUI rankingItemUI = Instantiate(_leaderboardUIPrefab, _rankingItemContainer);
                rankingItemUI.SetData(item);
                _rankingItemContainer.sizeDelta += new Vector2(
                    0,
                    _itemHeight + _listSpacing
                );
            }
        }

        public void RefreshLeaderboard(int gameID = 0)
        {
            ClearLeaderBoard();
            CreateRankingList(gameID);
            UpdateTournamentInfo(gameID);
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
