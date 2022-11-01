using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SuperUltra.Container
{
    public class HeaderUI : MonoBehaviour
    {
        [SerializeField] Image _levelBar;
        [SerializeField] TMP_Text _rankText;
        [SerializeField] Image _avatar;
        [SerializeField] TMP_Text _balanceText;
        [SerializeField] TMP_Text _rankTitle;

        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            SetLevelBar(UserData.pointsInCurrentRank, UserData.pointsToNextRank);
            SetLevel(UserData.rankLevel);
            SetBalance(UserData.totalTokenNumber);
            SetRankTitle(UserData.rankTitle);
            SetAvatar(UserData.profilePic);
        }

        void SetLevel(int level)
        {
            _rankText.text = level.ToString();
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

        void SetLevelBar(float experiencePoints, float pointToNextRank)
        {
            _levelBar.DOFillAmount(experiencePoints / pointToNextRank, 1f);
        }

        void SetBalance(int balance)
        {
            _balanceText.text = balance.ToString();
        }

        void SetRankTitle(string title)
        {
            _rankTitle.text = title;
        }

    }

}
