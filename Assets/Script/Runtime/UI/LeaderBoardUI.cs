using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;

namespace SuperUltra.Container
{
    public class RankingInfo
    {
        public int rank;
        public Sprite image;
        public string name;
        public int score;
    }

    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] RankingItemUI _rankingItemUI;
        [SerializeField] RectTransform _rankingItemContainer;
        [SerializeField] RectTransform _gameBannerPrefab;
        [SerializeField] RectTransform _gameBannerContainer;
        [SerializeField] StickyScrollUI _gameBannerStickyScroll;
        [SerializeField] RankingItemUI _userRankingUI;
        Dictionary<int, List<RankingInfo>> _gameToRankingInfoMap = new Dictionary<int, List<RankingInfo>>();
        float _listSpacing = 0;
        float _itemHeight = 0;
        Dictionary<int, int> _pageToGameMap = new Dictionary<int, int>();

        // Start is called before the first frame update
        void Start()
        {
            CreateGameList();
            CreateRankingList();
            CacheSpacingAndheight();
            CreateUserRank();
        }

        void CreateUserRank()
        {
            if (_userRankingUI == null)
            {
                return;
            }
            RankingInfo userRank = new RankingInfo(){ 
                rank = 45, 
                image = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f)),
                name = "LiftTastic", 
                score = 608 
            };
            _userRankingUI.SetData(userRank);
        }

        void CreateGameList()
        {
            // TODO : create game list from API
            var gameList = new List<int>() { 1024, 162, 783, 8 };
            int pageCount = 0;
            foreach (var game in gameList)
            {
                RectTransform gameBanner = Instantiate(_gameBannerPrefab, _gameBannerContainer);
                gameBanner.GetComponent<Image>().sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
                float value = UnityEngine.Random.Range(0.1f, 0.9f);
                float value1 = UnityEngine.Random.Range(0.1f, 0.9f);
                float value2 = UnityEngine.Random.Range(0.1f, 0.9f);

                gameBanner.GetComponent<Image>().color = new Color(value, value1, value2);
                _pageToGameMap.Add(pageCount, game);
                pageCount++;
            }
            _gameBannerStickyScroll.Initialize();
            _gameBannerStickyScroll.OnItemChange.AddListener(OnGameChange);
        }

        void OnGameChange(float a)
        {
            Debug.Log("OnGameChange : " + a + " / " + _pageToGameMap[(int)a]);
            RefreshLeaderboard(_pageToGameMap[(int)a]);
        }

        void CacheSpacingAndheight()
        {
            VerticalLayoutGroup verticalLayoutGroup = _rankingItemContainer.GetComponent<VerticalLayoutGroup>();
            if (verticalLayoutGroup)
            {
                _listSpacing = verticalLayoutGroup.spacing;
            }

            RectTransform itemRect = _rankingItemUI.GetComponent<RectTransform>();
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
            GetRankingList(gameID, (List<RankingInfo> list) =>
            {
                foreach (var item in list)
                {
                    RankingItemUI rankingItemUI = Instantiate(_rankingItemUI, _rankingItemContainer);
                    rankingItemUI.SetData(item);
                    _rankingItemContainer.sizeDelta += new Vector2(
                        0,
                        _itemHeight + _listSpacing
                    );
                }
            });
        }

        void GetRankingList(int gameID, Action<List<RankingInfo>> callback)
        {
            // download texture with BestHTTP
            HTTPRequest request = new HTTPRequest(
                new Uri("https://img.favpng.com/24/13/11/spider-man-2099-miles-morales-spider-verse-ultimate-marvel-png-favpng-4fdqqJRkK38KvXgNLMA5rhshE.jpg"),
                HTTPMethods.Get,
                (request, response) => OnRequestFinished(request, response, callback, gameID)
            );
            request.Send();
        }

        List<RankingInfo> GetPlayerList(Sprite sprite, int gameID = 0)
        {
            List<RankingInfo> rankingList = new List<RankingInfo>();
            switch (gameID)
            {
                case 162:
                    rankingList.Add(new RankingInfo { rank = 1, image = sprite, name = "John", score = 100 });
                    rankingList.Add(new RankingInfo { rank = 2, image = sprite, name = "Jane", score = 90 });
                    rankingList.Add(new RankingInfo { rank = 3, image = sprite, name = "Jack", score = 80 });
                    rankingList.Add(new RankingInfo { rank = 4, image = sprite, name = "Jill", score = 70 });
                    rankingList.Add(new RankingInfo { rank = 5, image = sprite, name = "Joe", score = 60 });
                    rankingList.Add(new RankingInfo { rank = 1, image = sprite, name = "John", score = 100 });
                    rankingList.Add(new RankingInfo { rank = 2, image = sprite, name = "Jane", score = 90 });
                    break;
                case 783:
                    rankingList.Add(new RankingInfo { rank = 1, image = sprite, name = "John", score = 100 });
                    rankingList.Add(new RankingInfo { rank = 2, image = sprite, name = "Jane", score = 90 });
                    rankingList.Add(new RankingInfo { rank = 3, image = sprite, name = "Jack", score = 80 });
                    rankingList.Add(new RankingInfo { rank = 4, image = sprite, name = "Jill", score = 70 });
                    break;
                default:
                case 1024:
                    rankingList.Add(new RankingInfo { rank = 1, image = sprite, name = "John", score = 100 });
                    rankingList.Add(new RankingInfo { rank = 2, image = sprite, name = "Jane", score = 90 });
                    rankingList.Add(new RankingInfo { rank = 3, image = sprite, name = "Jack", score = 80 });
                    rankingList.Add(new RankingInfo { rank = 4, image = sprite, name = "Jill", score = 70 });
                    rankingList.Add(new RankingInfo { rank = 5, image = sprite, name = "Joe", score = 60 });
                    rankingList.Add(new RankingInfo { rank = 1, image = sprite, name = "John", score = 100 });
                    rankingList.Add(new RankingInfo { rank = 2, image = sprite, name = "Jane", score = 90 });
                    rankingList.Add(new RankingInfo { rank = 1, image = sprite, name = "John", score = 100 });
                    rankingList.Add(new RankingInfo { rank = 2, image = sprite, name = "Jane", score = 90 });
                    rankingList.Add(new RankingInfo { rank = 3, image = sprite, name = "Jack", score = 80 });
                    rankingList.Add(new RankingInfo { rank = 4, image = sprite, name = "Jill", score = 70 });
                    rankingList.Add(new RankingInfo { rank = 5, image = sprite, name = "Joe", score = 60 });
                    rankingList.Add(new RankingInfo { rank = 1, image = sprite, name = "John", score = 100 });
                    rankingList.Add(new RankingInfo { rank = 2, image = sprite, name = "Jane", score = 90 });
                    break;
            }
            return rankingList;
        }

        void OnRequestFinished(HTTPRequest request, HTTPResponse response, Action<List<RankingInfo>> callback, int gameID)
        {
            if (response == null)
            {
                Debug.Log("Request Finished, but received no response.");
                return;
            }

            if (response.IsSuccess)
            {
                Debug.Log("Request Finished Successfully! " + response.DataAsTexture2D);
                Texture2D texture = response.DataAsTexture2D;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback(GetPlayerList(sprite, gameID));
            }
            else
            {
                Debug.Log("Request Finished Successfully, but with Error! " + response.StatusCode + " " + response.Message);
            }

        }

        public void RefreshLeaderboard(int gameID = 0)
        {
            ClearLeaderBoard();
            CreateRankingList(gameID);
        }

    }

}
