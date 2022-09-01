using UnityEngine;
using System;
using TMPro;

namespace SuperUltra.Container
{

    public class NFTItem
    {
        public string name; 
        public Texture2D texture2D;
    }

    public class NFTItemUI : MonoBehaviour
    {
        [SerializeField] TMP_Text _itemName;

        public void Initialize(NFTItem item)
        {
            _itemName.text = item.name;
        }

    }

}