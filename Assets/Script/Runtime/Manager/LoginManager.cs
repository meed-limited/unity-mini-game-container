using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class LoginManager : MonoBehaviour
    {
        [SerializeField] LoginUI _loginUI;
        [SerializeField] RegisterUI _registerUI;
        [SerializeField] EnterNameUI _enterNameUI;
        [SerializeField] ForgotPasswordUI _forgotPasswordUI;
        [SerializeField] VerifyUI _verifyUI;
        [SerializeField] MessagePopUpUI _messagePopUpUI;
        [SerializeField] ResetPasswordUI _resetPasswordUI;
        [SerializeField] AvatarSelectionUI _avatarSelectionUI;
        RectTransform _currentUI;

        void SwitchRayCastOnOff(Transform transform, bool isOn = true)
        {
            GraphicRaycaster graphicRaycaster = transform.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null)
            {
                return;
            }
            graphicRaycaster.enabled = isOn;
        }

        void ToPage(MonoBehaviour target)
        {
            if (_currentUI)
            {
                RectTransform current = _currentUI; // cache current UI, because it will be modified to target beofre animation ends
                ISlidable currentSlidable = current.GetComponent<ISlidable>();
                SwitchRayCastOnOff(current, false);
                currentSlidable.SlideOut().OnComplete(() =>
                {
                    current.gameObject.SetActive(false);
                });
            }
            target.gameObject.SetActive(true);
            ISlidable targetSlidable = target.GetComponent<ISlidable>();
            targetSlidable.SlideIn().OnComplete(
                () => SwitchRayCastOnOff(target.transform, true)
            );
            _currentUI = target.GetComponent<RectTransform>();
        }

        bool CheckInternetConnection()
        {
            if (
                !Application.internetReachability.Equals(NetworkReachability.ReachableViaLocalAreaNetwork)
                && !Application.internetReachability.Equals(NetworkReachability.ReachableViaCarrierDataNetwork)
            )
            {
                _messagePopUpUI.Show("No Connection", "Retry", () => { CheckInternetConnection(); }, false);
                return false;
            }
            return true;
        }

        void Start()
        {
            FacebookAuthen.Initialize();
            Application.targetFrameRate = 60;
            // HidePanel(_loginUI.transform);
            HidePanel(_registerUI.transform);
            HidePanel(_enterNameUI.transform);
            HidePanel(_forgotPasswordUI.transform);
            HidePanel(_messagePopUpUI.transform);
            HidePanel(_resetPasswordUI.transform);
            HidePanel(_verifyUI.transform);
            HidePanel(_avatarSelectionUI.transform);
            if (!CheckInternetConnection())
            {
                return;
            }
            ToLoginSelection();
            AutoLoginWithToken();
        }


        void HidePanel(Transform transform)
        {
            ISlidable slidable = transform.GetComponent<ISlidable>();
            if (slidable != null)
            {
                slidable.SlideOut(0f).OnComplete(
                    () => transform.gameObject.SetActive(false)
                );
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }

        public void ReTryConnection()
        {
            Start();
        }

        public void AutoLoginWithToken()
        {
            if (PlayerPrefs.HasKey("token"))
            {
                string token = PlayerPrefs.GetString("token");
            }
        }

        public void OnClickFacebookLogin()
        {
            LoadingUI.Show();
            FacebookAuthen.Login(
                () => { ToMenu(); },
                (string errorMessage) =>
                {
                    LoadingUI.Hide();
                    _messagePopUpUI.Show(errorMessage, "", null, true);
                },
                false
            );
        }

        public void OnClickFacebookRegister()
        {
            LoadingUI.Show();
            FacebookAuthen.Login(
                () => {
                    LoadingUI.Hide();
                    ToEnterUserName(); 
                },
                (string errorMessage) =>
                {
                    LoadingUI.Hide();
                    _messagePopUpUI.Show(errorMessage, "", null, true);
                },
                true
            );
        }

        public void OnClickEmailLogin(string email, string password)
        {
            LoadingUI.Show();
            EmailAuthen.Login(
                email,
                password,
                (result) =>
                {
                    ToMenu();
                },
                (string errorMessage) =>
                {
                    LoadingUI.Hide();
                    _messagePopUpUI.Show(errorMessage, "", null, true);
                }
            );
        }

        public void OnClickEmailRegister(string email, string password)
        {
            LoadingUI.Show();
            EmailAuthen.Register(
                email,
                password,
                OnEmailRegisterRequestFinished,
                (string errorMessage) =>
                {
                    LoadingUI.Hide();
                    _messagePopUpUI.Show(errorMessage, "", null, true);
                }
            );
        }

        void OnEmailRegisterRequestFinished(string playFabId)
        {
            NetworkManager.CreateUser(
                playFabId,
                () => {
                    LoadingUI.Hide();
                    ToEnterUserName();
                },
                () =>
                {
                    LoadingUI.Hide();
                    _messagePopUpUI.Show("Register fail", "", null, true);
                }
            );
        }

        public void OnClickForgotPassword(string email)
        {
            EmailAuthen.ForgotPassword(
                email,
                () => ToPage(_verifyUI),
                (string errorMessage) =>
                {
                    _messagePopUpUI.Show(errorMessage, "", null, true);
                }
            );
        }

        public void OnClickVerify(string code)
        {
            EmailAuthen.Verify(
                code,
                () => ToPage(_resetPasswordUI),
                (string errorMessage) =>
                {
                    _messagePopUpUI.Show(errorMessage, "", null, true);
                }
            );
        }

        public void OnSelectAvatar(Sprite avatar)
        {
            _enterNameUI.SetAvatar(avatar);
        }

        public void OnClickResetPassword(string password)
        {
            EmailAuthen.ResetPassword(
                password,
                () =>
                {
                    _messagePopUpUI.Show("Password has been reset", "Sign In", () =>
                    {
                        ToLoginSelection();
                    }, true, false);
                },
                (string errorMessage) =>
                {
                    _messagePopUpUI.Show(errorMessage, "", null, true);
                }
            );
        }
        
        public void ToMenu()
        {
            NetworkManager.LoginRequest(
                () =>
                {
                    SceneLoader.ToMenu();
                    LoadingUI.Hide();
                },
                () => { }
            );
        }

        public void ToRegister()
        {
            ToPage(_registerUI);
        }

        public void ToLoginSelection()
        {
            ToPage(_loginUI);
        }

        public void ToEnterUserName()
        {
            ToPage(_enterNameUI);
        }

        public void ToForgotPasword()
        {
            ToPage(_forgotPasswordUI);
        }

        public void ToAvatarSelection()
        {
            ToPage(_avatarSelectionUI);
        }

    }

}