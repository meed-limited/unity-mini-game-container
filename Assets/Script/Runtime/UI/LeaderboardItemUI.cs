using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using BestHTTP;

namespace SuperUltra.Container
{

    public class LeaderboardItemUI : MonoBehaviour
    {
        public TMP_Text _rank;
        public Image _image;
        public TMP_Text _userName;
        public TMP_Text _score;
        public TMP_Text _reward;
        readonly Vector2 _avatarResulotion = new Vector2(512f, 512f);

        public void SetData(LeaderboardUserData data)
        {
            if (_rank)
                _rank.text = data.rankPosition.ToString();
            if (_userName)
                _userName.text = data.name;
            if (_score)
                _score.text = data.score.ToString();
            if (_reward)
                _reward.text = data.reward.ToString();

            GetImage(data);
        }

        void GetImage(LeaderboardUserData data)
        {
            if (_image == null)
                return;
            if (data.avatarTexture != null)
            {
                SetImage(data.avatarTexture);
                return;
            }
            if (!string.IsNullOrEmpty(data.avatarUrl))
            {
                SetImage(data.avatarUrl);
                return;
            }
            GetDefaultAvatar();
        }

        void SetImage(Texture2D texture)
        {
            if (texture == null)
            {
                GetDefaultAvatar();
                return;
            }

            _image.sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
        }

        void SetImage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                GetDefaultAvatar();
            }

            NetworkManager.GetImage(url, (GetImageResponseData data) =>
            {
                if (this == null)
                {
                    return;
                }
                if (!data.result)
                {
                    GetDefaultAvatar();
                    return;
                }
                _image.sprite = Sprite.Create(
                    data.texture2D,
                    new Rect(0, 0, data.texture2D.width, data.texture2D.height),
                    new Vector2(0.5f, 0.5f)
                );
            });
        }

        void GetDefaultAvatar()
        {
            if (_image == null)
            {
                return;
            }
            _image.sprite = Sprite.Create(Texture2D.grayTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }
    }

}