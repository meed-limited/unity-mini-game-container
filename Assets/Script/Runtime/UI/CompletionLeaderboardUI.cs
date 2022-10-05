using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SuperUltra.Container
{
    public class CompletionLeaderboardUI : MonoBehaviour
    {
        [SerializeField] RectTransform _leaderboardItemContainer;
        [SerializeField] LeaderboardItemUI _leaderboardItemUIPrefab;
        [SerializeField] LeaderboardItemUI _userLeaderboardUI;
        [SerializeField] Canvas _canvas;
        [SerializeField] PopUpUI _popUp;
        static CompletionLeaderboardUI _instance;

        void Awake()
        {
            if (_instance)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            ContainerInterface.OnPlayAgain += Hide;
            ContainerInterface.OnReturnMenu += Hide;
            Hide();
        }

        void OnDestory()
        {
            ContainerInterface.OnPlayAgain -= Hide;
            ContainerInterface.OnReturnMenu -= Hide;
        }

        public void Show()
        {
            _canvas.gameObject.SetActive(true);
            _popUp.Show();
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

        public void GenerateRanking(LeaderboardUserData[] list)
        {
            foreach (Transform item in _leaderboardItemContainer.transform)
            {
                Destroy(item.gameObject);
            }
            foreach (LeaderboardUserData data in list)
            {
                LeaderboardItemUI rankingItemUI = Instantiate(_leaderboardItemUIPrefab, _leaderboardItemContainer);
                rankingItemUI.SetData(data);
            }
        }

        public void PlayAgain()
        {
            ContainerInterface.PlayAgain();
        }

        public void SetUserData(LeaderboardUserData data)
        {
            _userLeaderboardUI.SetData(data);
        }

        public void Back()
        {
            ContainerInterface.ReturnToMenu();
        }

    }
}