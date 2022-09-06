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
            Debug.Log("GameOver Show");
            _canvas.gameObject.SetActive(true);
            _popUp.Show();
        }

        public void OnClickContinue()
        {
            Hide(() =>
            {
                LoadingUI.Show();
                NetworkManager.UpdateScore(
                    100,
                    UserData.playFabId,
                    GameData.currentGameId,
                    () =>
                    {
                        LoadingUI.Show();
                        _completionLeaderboard.Show();
                        _canvas.gameObject.SetActive(false);
                    }
                );
            });
        }

    }

}

