using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace SuperUltra.Container
{

    public class RankingItemUI : MonoBehaviour
    {
        public TMP_Text rank;
        public Image image;
        public TMP_Text userName;
        public TMP_Text score;

        public void SetData(RankingInfo data)
        {
            rank.text = data.rank.ToString();
            image.sprite = data.image;
            userName.text = data.name;
            score.text = data.score.ToString();
        }
    }

}