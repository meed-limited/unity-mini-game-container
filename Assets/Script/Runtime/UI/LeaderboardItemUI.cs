using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using BestHTTP;

namespace SuperUltra.Container
{

    public class LeaderboardItemUI : MonoBehaviour
    {
        public TMP_Text rank;
        public Image image;
        public TMP_Text userName;
        public TMP_Text score;
        public TMP_Text reward;
        readonly Vector2 _avatarResulotion = new Vector2(512f, 512f);

        public void SetData(LeaderboardUserData data)
        {
            rank.text = data.rankPosition.ToString();
            userName.text = data.name;
            score.text = data.score.ToString();
            GetImage(data.avatarTexture);
        }

        void GetImage(Texture2D texture)
        {
            if (texture == null)
            {
                GetDefaultAvatar();
                return;
            }

            image.sprite = Sprite.Create(
                texture, 
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f)
            );
        }

        void GetDefaultAvatar()
        {
            image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }
    }

}