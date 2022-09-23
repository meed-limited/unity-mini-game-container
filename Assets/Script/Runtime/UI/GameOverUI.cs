using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;
        [SerializeField] PopUpUI _popUp;
        [SerializeField] CompletionLeaderboardUI _completionLeaderboard;
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
            _popUp.Hide().OnComplete(callback);
        }

        void Show()
        {
            _canvas.gameObject.SetActive(true);
            _popUp.Show();
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
            // TODO
            // if (!data.result)
            // {
            //     MessagePopUpUI.Show(
            //         "Post Score failed",
            //         "Confirm",
            //         SceneLoader.ToMenu
            //     );
            //     return;
            // }
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

