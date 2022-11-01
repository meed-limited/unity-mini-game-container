using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SuperUltra.Container
{

    public class NFTItemDetailUI : MonoBehaviour
    {
        [SerializeField] TMP_Text _itemName;
        [SerializeField] TMP_Text _itemDescription;
        [SerializeField] Image _image;
        [SerializeField] TMP_Text _attribute;
        [SerializeField] PopUpUI _popUpUI;
        [SerializeField] FadeUI _fadeBackground;

        public void Initialize(NFTItem item, Sprite sprite)
        {
            _image.sprite = sprite;
            _itemName.text = item.name;
            _itemDescription.text = item.description;
            _attribute.text = GetAttributeText(item.attribute);
        }

        string GetAttributeText(Dictionary<string, string> map)
        {
            string text = "";
            if (map == null)
            {
                return text;
            }

            foreach (var item in map)
            {
                text += $"{item.Key}: {item.Value}\n";
            }

            return text;
        }

        public void Show()
        {
            _fadeBackground.FadeIn();
            _popUpUI.Show();
        }

        public void Hide()
        {
            _fadeBackground.FadeOut();
            _popUpUI.Hide();
        }

    }

}