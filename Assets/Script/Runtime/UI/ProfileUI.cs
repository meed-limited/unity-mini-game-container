using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using BestHTTP;

namespace SuperUltra.Container
{
    public class ProfileUI : MonoBehaviour
    {

        [SerializeField] TMP_Text _displayName;
        [SerializeField] TMP_Text _rankLevel;
        [SerializeField] TMP_Text _rankName;
        [SerializeField] TMP_Text _pointsToNextRank;
        [SerializeField] TMP_Text _totalTokenNumber;
        [SerializeField] Image _profilePic;
        [SerializeField] Image _rankLevelLevelBar;
        int _score;

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            SetUserName(UserData.userName);
            SetUserRankInfo(UserData.rankLevel, UserData.rankTitle, UserData.pointsToNextRank, UserData.pointsInCurrentRank);
            SetAvatar(UserData.profilePic);
            SetNumberOfToken(UserData.totalTokenNumber);
        }

        public void SetAvatar(Texture2D texture)
        {
            if (_profilePic && texture)
            {
                _profilePic.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }

        void SetUserRankInfo(int level, string name, int pointsToNextRank, int pointsInCurrentRank)
        {
            _rankLevel.text = level.ToString();
            _rankName.text = name;
            _pointsToNextRank.text = $"{pointsInCurrentRank.ToString()}/{pointsToNextRank.ToString()}";
            _rankLevelLevelBar.fillAmount = ((float)pointsInCurrentRank / (float)pointsToNextRank);
        }

        void SetNumberOfToken(int number)
        {
            _totalTokenNumber.text = number.ToString();
        }

        public void SetUserName(string name = "")
        {
            if (_displayName != null)
            {
                _displayName.text = name;
            }
        }

        public void ToYoutube() => Application.OpenURL(Config.YotubeUrl);
        public void ToTwitter() => Application.OpenURL(Config.TwitterUrl);
        public void ToDiscord() => Application.OpenURL(Config.DiscordUrl);

        /// <summary>
        ///     to update Playfab Statistic directly. Testing purpose only
        /// </summary>
        public void PostScore()
        {
            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest()
                {
                    Statistics = new List<StatisticUpdate>(){
                        new StatisticUpdate(){
                            StatisticName = "Score",
                            Value = _score
                        }
                    }
                },
                (result) =>
                {
                    // Update Player Statistics Success
                },
                (error) =>
                {
                    // Update Player Statistics Error
                }
            );
        }

    }
}

