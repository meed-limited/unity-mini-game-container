using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.Container;
using TMPro;

namespace SuperUltra.Demo
{

    public class DemoManager : MonoBehaviour
    {
        [SerializeField] TMP_Text _scoreText;
        float _score = -1;

        void Start()
        {
            _score = 0;
            _scoreText.text = _score.ToString();
        }

        void OnEnable()
        {
            ContainerInterface.OnEffectVolumeChange += OnEffectVolumeChange; 
        }

        void OnDisable()
        {
            ContainerInterface.OnEffectVolumeChange -= OnEffectVolumeChange;
        }

        void OnEffectVolumeChange(bool isOn)
        {
            Debug.Log("isOn " + isOn);
        }

        void OnUserRewarded()
        {
            Debug.Log("Demo Rewarded!");
        }

        public void GameOver()
        {
            ContainerInterface.GameOver();
        }

        public void Pause()
        {
            ContainerInterface.Pause();
        }

        public void RequestLifeAds()
        {
            ContainerInterface.RequestRewardedAds(
                RequestLifeRewardedAdCallback
            );
        }

        void RequestLifeRewardedAdCallback(bool result)
        {
            Debug.Log("RequestLifeRewardedAdCallback " + result);
            if (result)
            {
                // do stuff
            }
        }

        public void RequestTimeAds()
        {
            ContainerInterface.RequestRewardedAds(
                RequestTimeAdsCallback
            );
        }

        void RequestTimeAdsCallback(bool result)
        {
            Debug.Log("RequestTimeAdsCallback " + result);
            if (result)
            {
                // do stuff
                Debug.Log("successfully get reward");
            }else
            {
                Debug.Log("fail to get reward");
            }
        }

        public void SubmitScore()
        {
            _score++;
            _scoreText.text = _score.ToString();
            ContainerInterface.SetScore(_score);
        }

    }

}

