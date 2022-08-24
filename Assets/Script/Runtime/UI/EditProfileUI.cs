using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuperUltra.Container
{

    public class EditProfileUI : MonoBehaviour
    {
        [SerializeField] Image _avatarPreview;
        [SerializeField] TMP_InputField _userName;
        [SerializeField] MenuManager _menuManager;

        // Start is called before the first frame update
        void Start()
        {
            SetAvatar(UserData.profilePic);
            SetUserName(UserData.userName);
        }

        void SetUserName(string name)
        {
            if(_userName != null)
            {
                _userName.text = name;
            }
        }

        public void SetAvatar(Texture2D texture)
        {
            if(_avatarPreview)
            {
                _avatarPreview.sprite = Sprite.Create(
                    texture, 
                    new Rect(0, 0, texture.width, texture.height), 
                    new Vector2(0.5f, 0.5f)
                );
            }
        }

        public void SaveProfile()
        {
            if(_avatarPreview && _avatarPreview.sprite && _userName)
            {
                _menuManager.UpdateUserProfileRequest(_userName.text, _avatarPreview.sprite.texture);
            }
        }

    }

}