using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class MenuManager : MonoBehaviour
    {
        [SerializeField] Canvas _gameListPage;
        [SerializeField] Canvas _leaderboardUI;
        [SerializeField] Canvas _seasonPassPage;
        [SerializeField] Canvas _walletPage;
        [SerializeField] Canvas _newsPage;
        [SerializeField] Canvas _settingPage;
        [SerializeField] Canvas _profilePage;
        [SerializeField] Canvas _profileEditPage;
        [SerializeField] Canvas _avatarSelectPage;
        [SerializeField] Canvas _navigationPage;
        [SerializeField] MessagePopUpUI _messagePopUpUI;
        [SerializeField] GameListUI _gameListUI;
        RectTransform _prevUI;
        int _prevPagenumber;
        Dictionary<Canvas, int> _pageToCanvas = new Dictionary<Canvas, int>();

        void Start()
        {
            _pageToCanvas.Add(_gameListPage, 0);
            _pageToCanvas.Add(_leaderboardUI, 1);
            _pageToCanvas.Add(_seasonPassPage, 2);
            _pageToCanvas.Add(_walletPage, 3);

            _pageToCanvas.Add(_profilePage, 21);
            _pageToCanvas.Add(_newsPage, 22);
            _pageToCanvas.Add(_settingPage, 31);
            _pageToCanvas.Add(_profileEditPage, 32);
            _pageToCanvas.Add(_avatarSelectPage, 41);
            _prevUI = _gameListPage.GetComponent<RectTransform>();
            _prevPagenumber = 0;

            _leaderboardUI.gameObject.SetActive(false);
            _seasonPassPage.gameObject.SetActive(false);
            _walletPage.gameObject.SetActive(false);
            _newsPage.gameObject.SetActive(false);
            _settingPage.gameObject.SetActive(false);
            _profilePage.gameObject.SetActive(false);
            _profileEditPage.gameObject.SetActive(false);
            _avatarSelectPage.gameObject.SetActive(false);
            _messagePopUpUI.gameObject.SetActive(false);

            _navigationPage.gameObject.SetActive(true);
            _gameListPage.gameObject.SetActive(true);
        }

        void SwitchRayCastOnOff(Transform transform, bool isOn = true)
        {
            GraphicRaycaster graphicRaycaster = transform.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null)
            {
                return;
            }
            graphicRaycaster.enabled = isOn;
        }

        void SlideOutCurrentUI()
        {
            RectTransform prev = _prevUI; // cache current UI, because it will be modified to target beofre animation ends
            ISlidable prevSlidable = prev.GetComponent<ISlidable>();
            SwitchRayCastOnOff(prev, false);
            if (prevSlidable != null)
            {
                prevSlidable.SlideOut().OnComplete(() =>
                {
                    prev.gameObject.SetActive(false);
                });
            }
        }

        void FadeOutCurrentUI()
        {
            RectTransform prev = _prevUI; // cache current UI, because it will be modified to target beofre animation ends
            FadeUI prevFadeUI = prev.GetComponent<FadeUI>();
            if (prevFadeUI != null)
            {
                prevFadeUI.FadeOut().OnComplete(() =>
                {
                    prev.gameObject.SetActive(false);
                });
            }
        }

        void SlideInCurrentUI(Canvas target)
        {
            ISlidable targetSlidable = target.GetComponent<ISlidable>();
            if (targetSlidable != null)
            {
                targetSlidable.SlideIn().OnComplete(
                    () =>
                    {
                        SwitchRayCastOnOff(target.transform, true);
                    }
                );
            }
        }

        void FadeInCurrentUI(Canvas target)
        {
            FadeUI targetFade = target.GetComponent<FadeUI>();
            if (targetFade != null)
            {
                targetFade.FadeIn().OnComplete(
                    () => SwitchRayCastOnOff(target.transform, true)
                );
            }
        }

        void SetPrevPageDirection(Canvas target)
        {
            if (!_pageToCanvas.TryGetValue(target, out int targetPageNumber))
            {
                return;
            }
            ISlidable prevSlideUI = _prevUI.GetComponent<ISlidable>();
            if (prevSlideUI == null) return;
            // for prev page, set slide direction to opposite of previous page
            prevSlideUI.ChangeSlideDirection(
                _prevPagenumber < targetPageNumber ? SlideDirection.Left : SlideDirection.Right
            );
        }

        void SetTargetPageDirection(Canvas target)
        {
            if (!_pageToCanvas.TryGetValue(target, out int targetPageNumber))
            {
                return;
            }
            ISlidable targetSlideUI = target.GetComponent<ISlidable>();
            if (targetSlideUI == null) return;
            // for current page, set slide direction to opposite of previous page
            targetSlideUI.ChangeSlideDirection(
                _prevPagenumber < targetPageNumber ? SlideDirection.Right : SlideDirection.Left
            );
        }

        void ToggelNavigation(Canvas target)
        {
            if (!_pageToCanvas.TryGetValue(target, out int targetPageNumber))
            {
                return;
            }
            ISlidable baseSlideUI = _navigationPage.GetComponent<ISlidable>();
            if (baseSlideUI == null) return;
            // if page number is greater smaller than 10, show the navigation
            if (targetPageNumber > 10 && _prevPagenumber < 10)
            {
                baseSlideUI.SlideOut();
            }
            if (targetPageNumber < 10 && _prevPagenumber > 10)
            {
                baseSlideUI.SlideIn();
            }
        }

        void ToPage(Canvas target)
        {
            if(NetworkManager.CheckConnection())
            {
                _messagePopUpUI.Show("No Connection", "Retry", () => { ToPage(target); }, false);
                return;
            }

            if (target == null)
            {
                return;
            }

            if (!_pageToCanvas.TryGetValue(target, out int targetPageNumber))
            {
                return;
            }

            if (targetPageNumber == _prevPagenumber)
            {
                return;
            }

            if (_prevUI)
            {
                SetPrevPageDirection(target);
                SlideOutCurrentUI();
                FadeOutCurrentUI();
            }
            ToggelNavigation(target);
            target.gameObject.SetActive(true);
            SetTargetPageDirection(target);
            FadeInCurrentUI(target);
            SlideInCurrentUI(target);

            _prevUI = target.GetComponent<RectTransform>();
            _prevPagenumber = targetPageNumber;
        }

        public void UpdateUserProfileRequest(string userName, Texture2D texture2D)
        {
            LoadingUI.ShowInstance();
            NetworkManager.UpdateUserProfile(
                UserData.playFabId,
                userName,
                texture2D,
                () => NetworkManager.GetUserData(null, () =>
                {
                    ToProfilePage();
                    LoadingUI.HideInstance();
                })
            );
        }

        public void UpdateUserNameRequest(string userName)
        {
            LoadingUI.ShowInstance();
            NetworkManager.UpdateUserName(
                UserData.playFabId,
                userName,
                () => NetworkManager.GetUserData(() =>
                {
                    ToProfilePage();
                    LoadingUI.HideInstance();
                })
            );
        }

        public void ShowPopUP(RectTransform content, string actionButtonMessage = "", Action actionButtonCallback = null, bool shouldHideAfterAction = true)
        {
            _messagePopUpUI.gameObject.SetActive(true);
            _messagePopUpUI.Show(content, actionButtonMessage, actionButtonCallback, shouldHideAfterAction);
        }

        public void ToNewsPage() => ToPage(_newsPage);
        public void ToSettingPage() => ToPage(_settingPage);
        public void ToProfilePage()
        {
            ProfileUI profileUI = _profilePage.GetComponent<ProfileUI>();
            if (profileUI)
            {
                profileUI.Initialize();
            }
            ToPage(_profilePage);
        }
        public void ToProfileEditPage()
        {
            EditProfileUI editProfileUI = _profileEditPage.GetComponent<EditProfileUI>();
            if (editProfileUI)
            {
                editProfileUI.Initialize();
            }
            ToPage(_profileEditPage);
        }
        public void ToSeasonPage() => ToPage(_seasonPassPage);
        public void ToGamePage() => ToPage(_gameListPage);
        public void ToAvatarSelectPage() => ToPage(_avatarSelectPage);
        public void ToLeaderboardPage()
        {
            LeaderboardUI leaderboardUI = _leaderboardUI.GetComponent<LeaderboardUI>();
            if (leaderboardUI)
            {
                leaderboardUI.Initialize();
            }
            ToPage(_leaderboardUI);
        }
        public void ToWalletPage()
        {
            WalletUI walletUI = _walletPage.GetComponent<WalletUI>();
            if (walletUI)
            {
                walletUI.Initialize();
            }
            ToPage(_walletPage);
        }

    }

}

