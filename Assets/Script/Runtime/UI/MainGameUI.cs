using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class MainGameUI : MonoBehaviour
    {
        [Header("Header")]
        [SerializeField] Image _levelBar;
        [SerializeField] TMP_Text _levelText;
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
            SetBalance(UserData.totalTokenNumber);
            SetRankTitle(UserData.rankTitle);
            SetAvatar(UserData.profilePic);
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
