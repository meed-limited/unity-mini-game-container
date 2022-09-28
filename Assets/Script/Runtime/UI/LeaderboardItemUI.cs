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
            if(_rank)
                _rank.text = data.rankPosition.ToString();
            if(_userName)
                _userName.text = data.name;
            if(_score)
                _score.text = data.score.ToString();
            if(_reward)
                _reward.text = data.reward.ToString();
            GetImage(data.avatarTexture);
        }

        void GetImage(Texture2D texture)
        {
            if(_image == null)
                return;
            
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

        void GetDefaultAvatar()
        {
            _image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }
    }

}