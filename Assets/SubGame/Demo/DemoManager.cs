using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.Container;

namespace SuperUltra.Demo
{

    public class DemoManager : MonoBehaviour
    {

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
            }
        }

    }

}

