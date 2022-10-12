using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace SuperUltra.Container
{

    public class WalletUI : MonoBehaviour
    {
        [SerializeField] Image _withdrawal;
        [SerializeField] TMP_Text _balanceText;
        [SerializeField] RectTransform _notEnoughFundContent;
        [SerializeField] NFTItemUI _nFTItemUIPrefab;
        [SerializeField] NFTItemDetailUI _nFTItemDetailUI;
        [SerializeField] RectTransform _nftItemContainer;
        [SerializeField] RectTransform _startUpTab;
        [SerializeField] MenuManager _menuManager;
        [SerializeField] TMP_Text _walletAddress;
        [SerializeField] TMP_InputField _walletAddressInput;
        [SerializeField] RectTransform _emptyNFTMessage;

        [Header("Header")]
        [SerializeField] Image _levelBar;
        [SerializeField] Image _avatar;
        [SerializeField] TMP_Text _rankText;
        [SerializeField] TMP_Text _headerBalanceText;
        [SerializeField] TMP_Text _rankTitle;
        RectTransform _previousTab;
        Dictionary<NFTItem, NFTItemUI> _nftIdToItemUIMap = new Dictionary<NFTItem, NFTItemUI>();

        public void Initialize()
        {
            SetWithdrawal(UserData.totalTokenNumber);
            SetLevel(UserData.rankLevel);
            SetLevelBar(UserData.pointsInCurrentRank, UserData.pointsToNextRank);
            SetRankTitle(UserData.rankTitle);
            SetAvatar(UserData.profilePic);
            SetWalletAddress(UserData.walletAddress);
            RequestNFTItem();
            if (_startUpTab)
            {
                _previousTab = _startUpTab;
            }
        }

        void SetWalletAddress(string walletAddress)
        {
            if (_walletAddress)
            {
                bool isEmpty = string.IsNullOrEmpty(walletAddress);
                _walletAddress.text = isEmpty ? "No wallet address saved" : walletAddress;
            }
        }

        void SetAvatar(Texture2D texture)
        {
            if (_avatar && texture)
            {
                _avatar.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
        }

        void RequestNFTItem()
        {
            LoadingUI.ShowInstance();
            NetworkManager.GetUserNFT(
                OnUserNFTDataResponse
            );
        }

        void OnUserNFTDataResponse(GetUserNFTResponseData data)
        {
            LoadingUI.HideInstance();
            if (data.result == false) return;
            SetNFTItemList(data.list);
        }

        void SetNFTItemList(NFTItem[] itemList)
        {
            _nftIdToItemUIMap.Clear();
            _emptyNFTMessage.gameObject.SetActive(false);
            foreach (Transform item in _nftItemContainer.transform)
            {
                Destroy(item.gameObject);
            }
            if(_emptyNFTMessage && itemList.Length <= 0)
            {
                _emptyNFTMessage.gameObject.SetActive(true);
                return;
            }
            foreach (NFTItem item in itemList)
            {
                NFTItemUI prefab = Instantiate(_nFTItemUIPrefab, _nftItemContainer);
                prefab.Initialize(item);
                prefab.SetOnClickAction(OnClickNFTItem);
                _nftIdToItemUIMap.Add(item, prefab);
            }
        }

        public void UpdateNFTItemUIIsActive(NFTItem item)
        {
            foreach (var kvp in _nftIdToItemUIMap)
            {
                kvp.Value.UpdateIsActive();
            }
        }

        void OnClickNFTItem(NFTItem item, Sprite sprite)
        {
            if (item.isActive)
            {
                UserData.DeactivateNFTItem(item);
            }
            else
            {
                UserData.ActivateNFTItem(item);
            }

            UpdateNFTItemUIIsActive(item);
            _nFTItemDetailUI.Initialize(item, sprite);
            _nFTItemDetailUI.Show();
        }

        void SetLevel(int level)
        {
            _rankText.text = level.ToString();
        }

        void SetLevelBar(float experiencePoints, float pointToNextRank)
        {
            _levelBar.DOFillAmount(experiencePoints / pointToNextRank, 1f);
        }

        void SetRankTitle(string title)
        {
            _rankTitle.text = title;
        }

        void SetWithdrawal(int totalTokenNumber)
        {
            if (_withdrawal)
            {
                float amount = Mathf.Min(1f, (float)totalTokenNumber / Config.WithDrawLimit);
                _withdrawal.DOFillAmount(amount, 1f);
            }
            if (_balanceText)
            {
                _balanceText.text = totalTokenNumber.ToString();
            }
            if (_headerBalanceText)
            {
                _headerBalanceText.text = totalTokenNumber.ToString();
            }
        }

        public void OnClickWithdrawal()
        {
            if (UserData.totalTokenNumber < Config.WithDrawLimit)
            {
                _menuManager.ShowPopUP(
                    _notEnoughFundContent,
                    "Back"
                );
                return;
            }

            Application.OpenURL(Config.WithDrawUrl);
        }

        public void OnClickHowToWithDrawal()
        {
            Application.OpenURL(Config.HowToWithDrawalUrl);
        }

        public void SwitchTab(RectTransform transform)
        {
            if (transform.Equals(_previousTab))
            {
                return;
            }
            if (_previousTab != null)
            {
                _previousTab.gameObject.SetActive(false);
            }
            transform.gameObject.SetActive(true);
            _previousTab = transform;
        }

        public void UpdateWalletAddress()
        {
            if (_walletAddressInput == null)
            {
                return;
            }
            string address = _walletAddressInput.text;
            LoadingUI.ShowInstance();
            NetworkManager.UpdateUserWalletAddress(
                UserData.playFabId,
                _walletAddressInput.text,
                (ResponseData data) =>
                {
                    LoadingUI.HideInstance();
                    if (!data.result)
                    {
                        MessagePopUpUI.Show(data.message);
                        return;
                    }
                    // 0x074277bc682e180ead52e48d52ea4633f487370c
                    MessagePopUpUI.Show("Update Wallet success!", "Reload Page", () =>
                    {
                        Initialize();
                    });
                }
            );
        }

    }

}

