using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

namespace SuperUltra.Container
{
    public class ProfileUI : MonoBehaviour
    {

        [SerializeField] TMP_Text _displayName;
        [SerializeField] TMP_Text _email;
        [SerializeField] TMP_Text _scoreText;
        [SerializeField] TMP_Text _highSCore;
        int _score;

        public void Start()
        {
            _score = 0;
            PlayFabClientAPI.GetPlayerCombinedInfo(
                new GetPlayerCombinedInfoRequest()
                {
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true, GetUserAccountInfo = true },
                    PlayFabId = UserData.playFabId
                },
                (GetPlayerCombinedInfoResult result) =>
                {
                    _displayName.text = result.InfoResultPayload.AccountInfo.TitleInfo.DisplayName;
                },
                (result) => Debug.Log(result.ErrorMessage)
            );
            PlayFabClientAPI.GetPlayerStatistics(
                new GetPlayerStatisticsRequest(),
                OnGetStatistics,
                error => Debug.LogError(error.GenerateErrorReport())
            );

        }

        void OnGetStatistics(GetPlayerStatisticsResult result)
        {
            foreach (var stat in result.Statistics)
            {
                Debug.Log(stat.StatisticName + ": " + stat.Value);
                if (stat.StatisticName == "Score")
                {
                    _highSCore.text = _score.ToString();
                }
            }
        }

        public void Back()
        {
            SceneLoader.ToMenu();
        }

        public void IncreaseScore()
        {
            _score++;
            _scoreText.text = $"{_score}";
        }

        public void PostScore()
        {
            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest()
                {
                    Statistics = new List<StatisticUpdate>(){
                        new StatisticUpdate(){
                            StatisticName = "Score",
                            Value = _score
                        }
                    }
                },
                (result) =>
                {
                    Debug.Log("Update Player Statistics Success");
                },
                (error) =>
                {
                    Debug.LogError("Update Player Statistics Error\n" + error.GenerateErrorReport());
                }
            );
        }

    }
}

