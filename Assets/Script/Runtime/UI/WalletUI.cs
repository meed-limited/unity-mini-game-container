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
        [SerializeField] RectTransform _nftItemContainer;
        [SerializeField] Image _levelBar;
        [SerializeField] Image _avatar;
        [SerializeField] TMP_Text _rankText;
        [SerializeField] TMP_Text _headerBalanceText;
        [SerializeField] TMP_Text _rankTitle;
        [SerializeField] RectTransform _startUpTab;
        [SerializeField] MenuManager _menuManager;
        RectTransform _previousTab;


        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            // SetWithdrawal(UserData.totalTokenNumber);
            SetWithdrawal(1000);
            SetLevel(UserData.rankLevel);
            SetRankTitle(UserData.rankTitle);
            SetAvatar(UserData.profilePic);
            SetNFTItem(new NFTItem[]{});
            if (_startUpTab)
            {
                _previousTab = _startUpTab;
            }
        }

        public void SetAvatar(Texture2D texture)
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

        void SetNFTItem(NFTItem[] itemList)
        {
            foreach (NFTItem item in itemList)
            {
                NFTItemUI prefab = Instantiate(_nFTItemUIPrefab, _nftItemContainer);
                prefab.Initialize(item);
            }
        }

        void SetLevel(int level)
        {
            _rankText.text = level.ToString();
            _levelBar.DOFillAmount(level / 100f, 1f);
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
                    "Back",
                    null,
                    true
                );
                return;
            }

            Application.OpenURL("https://www.youtube.com/watch?v=FOfV8TvwaZ8");
        }

        public void OnClickHowToWithDrawal()
        { }

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

    }


}

