using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace SuperUltra.Container
{

    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;
        [SerializeField] PopUpUI _popUp;
        [SerializeField] CompletionLeaderboardUI _completionLeaderboard;
        [SerializeField] TMP_Text _score;
        static GameOverUI _instance;

        void Awake()
        {
            if (_instance)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            ContainerInterface.OnGameOver += Show;
            Hide();
        }

        void OnDestory()
        {
            ContainerInterface.OnGameOver -= Show;
        }

        void Hide()
        {
            _popUp.Hide().OnComplete(
                () =>
                {
                    _canvas.gameObject.SetActive(false);
                }
            );
        }

        void Hide(TweenCallback callback)
        {
            _popUp.Hide().SetUpdate(true).OnComplete(callback);
        }

        void Show()
        {
            _canvas.gameObject.SetActive(true);
            _score.text = SessionData.currnetGameScore.ToString();
            _popUp.Show();
        }

        public void OnClickDoubleScoreAd()
        {
            ContainerInterface.RequestRewardedAds(
                (bool isRewarded) =>
                {
                    float score = isRewarded ? SessionData.currnetGameScore * 2 : SessionData.currnetGameScore;
                    Hide(() =>
                    {
                        LoadingUI.ShowInstance();
                        NetworkManager.UpdateScore(
                            score,
                            UserData.playFabId,
                            SessionData.currentGameId,
                            OnUpdateScore
                        );
                    });
                }
            );
        }

        public void OnClickContinue()
        {
            Hide(() =>
            {
                LoadingUI.ShowInstance();
                NetworkManager.UpdateScore(
                    SessionData.currnetGameScore,
                    UserData.playFabId,
                    SessionData.currentGameId,
                    OnUpdateScore
                );
            });
        }

        void OnUpdateScore(UpdateScoreResponseData data)
        {
            LoadingUI.HideInstance();
            _canvas.gameObject.SetActive(false);
            if (!data.result)
            {
                MessagePopUpUI.Show(
                    data.message,
                    "Back to Menu",
                    ContainerInterface.ReturnToMenu
                );
                return;
            }
            _completionLeaderboard.Show();
            _completionLeaderboard.GenerateRanking(data.list);
            _completionLeaderboard.SetUserData(new LeaderboardUserData(){
                rankPosition = data.position,
                reward = data.reward,
                name = UserData.userName,
                avatarTexture = UserData.profilePic,
                score = data.score
            });
        }

    }

}

