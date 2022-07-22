using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;

namespace SuperUltra.Container
{
    public class ProfileUI : MonoBehaviour
    {

        [SerializeField] TMP_Text _displayName;
        [SerializeField] TMP_Text _email;
        
        public void Start()
        {
            PlayfabLogin.GetPlayerInfo(
                (GetPlayerCombinedInfoResult result) => {
                    _displayName.text = result.InfoResultPayload.AccountInfo.TitleInfo.DisplayName;
                }
            );
        }

        public void Back()
        {
            SceneLoader.ToMenu();
        }
    }
}

