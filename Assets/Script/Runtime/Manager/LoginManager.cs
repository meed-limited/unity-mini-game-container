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
        [SerializeField] AvatarSelectionUI _avatarSelectionUI;
        RectTransform _currentUI;
        bool _isSelectedAvatar = false;
        /// <summary> 
        /// a flag to indicate the landing of user open the app.
        /// It tells whether the user is opening the app, back from the menu or logout
        /// </summary>
        static bool _isLanding = true;

        void Start()
        {
            FacebookAuthen.Initialize();
#if UNITY_ANDROID
            GoogleAuthen.Initialize();
#endif
            Application.targetFrameRate = 60;
            // HidePanel(_loginUI.transform);
            HidePanel(_registerUI.transform);
            HidePanel(_enterNameUI.transform);
            HidePanel(_forgotPasswordUI.transform);
            HidePanel(MessagePopUpUI.instance?.transform);
            HidePanel(_avatarSelectionUI.transform);
            if (!CheckInternetConnection())
            {
                return;
            }
            ToLoginSelection();
            AutoLoginWithToken();
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
            if (!NetworkManager.CheckConnection())
            {
                MessagePopUpUI.Show("No Connection", "Retry", () => { CheckInternetConnection(); }, false);
                return false;
            }
            return true;
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

        public void AutoLoginWithToken()
        {
            if (_isLanding
                && PlayerPrefs.HasKey(Config.KEY_AUTH_TOKEN)
                && PlayerPrefs.HasKey(Config.KEY_PLAYFAB_ID))
            {
                UserData.playFabId = PlayerPrefs.GetString(Config.KEY_PLAYFAB_ID); 
                string token = PlayerPrefs.GetString(Config.KEY_AUTH_TOKEN);
                LoadingUI.ShowInstance();
                _isLanding = false;
                NetworkManager.AutoLogin(
                    token,
                    (data) =>
                    {
                        if (data.result)
                        {
                            SceneLoader.ToMenu();
                            LoadingUI.HideInstance();
                        }
                        else
                        {
                            MessagePopUpUI.Show(MessageConst.AutoSignInFail);
                            LoadingUI.HideInstance();
                        }
                    }
                );
            }
        }

        public void OnClickFacebookLogin()
        {
            LoadingUI.ShowInstance();
            FacebookAuthen.Login(
                (bool isCreatingAccount) =>
                {
                    Debug.Log(isCreatingAccount ? "creating user" : "login to user");
                    if (isCreatingAccount)
                    {
                        OnFacebookCreatePlayfabUser();
                    }
                    else
                    {
                        ToMenu();
                    }
                },
                (string errorMessage) =>
                {
                    LoadingUI.HideInstance();
                    MessagePopUpUI.Show($"Facebook Login Failed. {errorMessage}");
                }
            );
        }

        void OnFacebookCreatePlayfabUser()
        {
            NetworkManager.GetAuthToken(
                (getAuthResponse) =>
                {
                    if (!getAuthResponse.result)
                    {
                        LoadingUI.HideInstance();
                        MessagePopUpUI.Show($"Register fail\n{getAuthResponse.message}");
                        return;
                    }
                    NetworkManager.CreateUser(
                        UserData.playFabId,
                        () =>
                        {
                            LoadingUI.HideInstance();
                            ToEnterUserName();
                        },
                        (data) =>
                        {
                            LoadingUI.HideInstance();
                            MessagePopUpUI.Show($"Register fail\n{data.message}");
                        }
                    );
                }
            );
        }

        public void OnClickGoogleLogin()
        {
#if UNITY_ANDROID
            LoadingUI.ShowInstance();
            GoogleAuthen.Login(
                (ResponseData response) =>
                {
                    LoadingUI.HideInstance();
                    if (response.result)
                    {
                        Debug.Log("asdfasdf");
                        // ToMenu();
                    }
                    else
                    {
                        MessagePopUpUI.Show(response.message);
                    }
                }
            );
#endif
        }

        public void OnClickEmailLogin(string email, string password)
        {
            LoadingUI.ShowInstance();
            EmailAuthen.Login(
                email,
                password,
                (result) =>
                {
                    ToMenu();
                },
                (string errorMessage) =>
                {
                    LoadingUI.HideInstance();
                    MessagePopUpUI.Show("Login fail\n" + errorMessage);
                }
            );
        }

        public void OnClickEmailRegister(string email, string password)
        {
            LoadingUI.ShowInstance();
            EmailAuthen.Register(
                email,
                password,
                OnEmailRegisterRequestFinished,
                (string errorMessage) =>
                {
                    LoadingUI.HideInstance();
                    MessagePopUpUI.Show(errorMessage);
                }
            );
        }

        void OnEmailRegisterRequestFinished(string playFabId)
        {
            NetworkManager.GetAuthToken(
                (getAuthResponse) =>
                {
                    if (!getAuthResponse.result)
                    {
                        LoadingUI.HideInstance();
                        MessagePopUpUI.Show($"Register fail\n{getAuthResponse.message}");
                        return;
                    }
                    NetworkManager.CreateUser(
                        playFabId,
                        () =>
                        {
                            LoadingUI.HideInstance();
                            ToEnterUserName();
                        },
                        (ResponseData data) =>
                        {
                            LoadingUI.HideInstance();
                            MessagePopUpUI.Show($"Register fail\n{data.message}");
                        }
                    );
                }
            );
        }

        public void OnClickForgotPassword(string email)
        {
            EmailAuthen.ForgotPassword(
                email,
                (result) =>
                {
                    MessagePopUpUI.Show("Reset password email is sent to your mailbox");
                    ToPage(_loginUI);
                }
            );
        }

        public void OnSelectAvatar(Sprite avatar)
        {
            _enterNameUI.SetAvatar(avatar);
        }

        public void ToMenu()
        {
            NetworkManager.LoginRequest(
                (ResponseData data) =>
                {
                    if (data.result)
                    {
                        SceneLoader.ToMenu();
                        LoadingUI.HideInstance();
                    }
                    else
                    {
                        MessagePopUpUI.Show($"Login fail\n {data.message}");
                        LoadingUI.HideInstance();
                    }
                }
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

            /* select defaultAvatar on the first time user proceed to enterNameUI */
            if (!_isSelectedAvatar && _avatarSelectionUI)
            {
                _isSelectedAvatar = true;
                Sprite avatar = _avatarSelectionUI.GetDefaultAvatar();
                OnSelectAvatar(avatar);
            }
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