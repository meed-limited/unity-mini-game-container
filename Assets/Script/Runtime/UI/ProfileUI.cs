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
        [SerializeField] int _score;

        public void Start()
        {
            SetUserName(UserData.userName);
            SetUserRankInfo(UserData.rankLevel, UserData.rankTitle, UserData.pointsToNextRank, UserData.pointsInCurrentRank);
            // GetUserProfilePic(UserData.profilePic);
            SetNumberOfToken(UserData.totalTokenNumber);
        }

        void GetUserProfilePic(string url)
        {
            NetworkManager.GetImage(url, (Texture2D texture) =>
            {
                _profilePic.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            });
        }

        void SetUserRankInfo(int level, string name, int pointsToNextRank, int pointsInCurrentRank)
        {
            _rankLevel.text = level.ToString();
            _rankName.text = name;
            _pointsToNextRank.text = $"{pointsInCurrentRank.ToString()}/{pointsToNextRank.ToString()}";
        }

        void SetNumberOfToken(int number)
        {
            _totalTokenNumber.text = number.ToString();
        }

        void SetUserName(string name = "")
        {
            if (_displayName != null)
            {
                _displayName.text = name;
            }
        }

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
                    Debug.Log("Update Player Statistics Success");
                },
                (error) =>
                {
                    Debug.LogError("Update Player Statistics Error\n" + error.GenerateErrorReport());
                }
            );
        }

    }
}

