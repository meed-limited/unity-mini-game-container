using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace SuperUltra.Container
{

    public class NFTItemUI : MonoBehaviour
    {
        [SerializeField] TMP_Text _itemName;
        [SerializeField] TMP_Text _itemDescription;
        [SerializeField] Image _image;
        [SerializeField] Button _button;
        [SerializeField] public NFTItem _nftItem;

        public void Initialize(NFTItem item)
        {
            NetworkManager.GetImage(item.texture2DUrl, (GetImageResponseData data) =>
            {
                Texture2D texture = Texture2D.grayTexture;
                if(data.result)
                {
                    texture = data.texture2D;
                }
                _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            });
            _itemName.text = item.name;
            _itemDescription.text = item.description;
            _nftItem = item;
        }

        public void SetOnClickAction(Action<NFTItem, Sprite> onClickAction)
        {
            Debug.Log("NFTItemUI setClick");
            _button.onClick.AddListener(() =>
            {
                Debug.Log("NFTItemUI Click");
                onClickAction(_nftItem, _image.sprite);
            });
        }

    }

}