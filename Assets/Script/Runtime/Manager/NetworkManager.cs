using System;
using System.Text;
using UnityEngine;
using BestHTTP;
using SimpleJSON;

namespace SuperUltra.Container
{

    public static class NetworkManager
    {
        static bool _isUserDataRequested = false;
        static bool _isUserSeasonDataRequested = false;
        static Action _onCompleteLoginRequest;
        static int _requestedLeaderboardCount;
        const float _timeOut = 6f;

        static bool CheckConnection()
        {
            if (
                !Application.internetReachability.Equals(NetworkReachability.ReachableViaLocalAreaNetwork)
                && !Application.internetReachability.Equals(NetworkReachability.ReachableViaCarrierDataNetwork)
            )
            {
                return false;
            }
            return true;
        }

        static bool ValidateResponse(HTTPResponse response)
        {
            if (response == null || response.IsSuccess == false)
            {
                return false;
            }

            JSONNode json = JSON.Parse(response.DataAsText);
            Debug.Log("ValidateResponse " + json);

            if (json == null || json["success"] == null || json["success"] != true)
            {
                return false;
            }
            return true;
        }

        static void GetAuthToken(Action callback)
        {
            string playFabId = UserData.playFabId;
            callback();
        }

        static void GetGameList(Action callback)
        {
            // get all th game list from api from Config.domain
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "games"),
                HTTPMethods.Get,
                (req, res) =>
                {
                    Debug.Log("GetGameList response");
                    OnGameListRequestFinished(req, res);
                    callback();
                }
            );
            request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void GetLeaderboardList(Action callback)
        {
            foreach (var item in GameData.gameDataList)
            {
                int id = item.Value.id;
                GetLeaderboard(id, callback);
            }
        }

        static void GetLeaderboard(int gameId, Action callback)
        {
            // get all th game list from api from Config.domain
            Debug.Log("GetLeaderboard");
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "systems/leaderboard"),
                HTTPMethods.Post,
                (req, res) =>
                {
                    Debug.Log("GetLeaderboard response");
                    OnLeaderBoardRequestFinished(req, res, gameId);
                    callback();
                }
            );
            JSONObject json = new JSONObject();
            json.Add("gameIdentifier", gameId);
            request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void GetUserData(Action callback)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + $"users/{UserData.playFabId}"),
                HTTPMethods.Get,
                (req, res) =>
                {
                    Debug.Log("GetUserData response");
                    OnUserDataRequestFinished(req, res);
                    callback();
                }
            );
            request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnUserDataRequestFinished(HTTPRequest request, HTTPResponse response)
        {
            if (ValidateResponse(response))
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                UserData.email = json["emailAddress"];
                UserData.totalTokenNumber = json["totalTokenNumber"];
                UserData.pointsInCurrentRank = json["pointsInCurrentRank"];
                UserData.pointsToNextRank = json["pointsToNextRank"];
                UserData.rankLevel = json["rankLevel"];
                UserData.rankTitle = json["rankTitle"];
            }
        }

        static void GetSeasonPass(Action callback)
        {
            callback();
        }

        static void OnLeaderBoardRequestFinished(HTTPRequest req, HTTPResponse response, int gameID)
        {
            if (!GameData.gameDataList.TryGetValue(gameID, out GameData gameData))
            {
                return;
            }

            if (ValidateResponse(response))
            {
                Debug.Log("OnLeaderBoardRequestFinished");
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnLeaderBoardRequestFinished " + json.ToString());
                if (json["users"] == null || !json["users"].IsArray)
                {
                    return;
                }

                foreach (JSONNode item in json["users"].AsArray)
                {
                    LeaderboardUserData data = new LeaderboardUserData()
                    {
                        rankPosition = item["position"].AsInt,
                        avatarUrl = item["avatarUrl"],
                        name = item["name"].ToString(),
                        score = item["score"],
                        reward = item["reward"],
                    };
                    gameData.leaderboard.Add(data);
                }
            }
        }

        static void CompleteRequestList()
        {
            Debug.Log($"CompleteRequestList {_isUserDataRequested} {_isUserSeasonDataRequested} {_requestedLeaderboardCount} {GameData.gameDataList.Count}");
            if (_isUserDataRequested
                && _isUserSeasonDataRequested
                && GameData.gameDataList.Count != 0
                && _requestedLeaderboardCount == GameData.gameDataList.Count
            )
            {
                _onCompleteLoginRequest?.Invoke();
            }
        }

        static void OnGameListRequestFinished(HTTPRequest request, HTTPResponse response)
        {
            if (!ValidateResponse(response))
            {
                Debug.Log("Game list request failed");
                return;
            }

            string data = response.DataAsText;
            JSONNode json = JSON.Parse(data);

            if (json["games"] == null || !json["games"].IsArray)
            {
                Debug.Log("No game list found");
                return;
            }

            Debug.Log("Game list found");
            JSONArray arr = json["games"].AsArray;
            foreach (JSONNode item in arr)
            {
                Debug.Log($"item {item}");
                if (item["identifier"] == null || item["title"] == null)
                {
                    continue;
                }
                int gameid = item["identifier"].AsInt;
                GameData.gameDataList.Add(
                    gameid,
                    new GameData()
                    {
                        id = gameid,
                        name = item["title"].Value
                    }
                );
            }

        }

        public static void LoginRequest(Action success, Action fail)
        {
            if (!CheckConnection())
            {
                return;
            }

            _onCompleteLoginRequest = success;
            _requestedLeaderboardCount = 0;
            // request token from server
            GetAuthToken(
                () =>
                {
                    // get game list then get leader board data
                    GetGameList(() =>
                    {
                        GetLeaderboardList(() =>
                        {
                            _requestedLeaderboardCount++;
                            CompleteRequestList();
                        });
                    });
                    // get user data
                    GetUserData(() =>
                    {
                        _isUserDataRequested = true;
                        CompleteRequestList();
                    });
                    // get season pass data
                    GetSeasonPass(() =>
                    {
                        _isUserSeasonDataRequested = true;
                        CompleteRequestList();
                    });
                }
            );
        }

        public static void GetImage(string url, Action<Texture2D> callback)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(url),
                HTTPMethods.Get,
                (req, res) =>
                {
                    if (ValidateResponse(res))
                    {
                        Texture2D texture = new Texture2D(1, 1);
                        texture.LoadImage(res.Data);
                        callback(texture);
                    }
                }
            );
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        public static void CreateUser(string playFabId, Action success, Action failure = null)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users"),
                HTTPMethods.Post,
                (req, res) => { OnCreateUserRequestFinished(req, res, success, failure); }
            );
            Debug.Log("CreateUser url " + request.Uri.ToString());
            JSONObject json = new JSONObject();
            json.Add("platformId", playFabId);
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Send();
        }

        static void OnCreateUserRequestFinished(HTTPRequest request, HTTPResponse response, Action success = null, Action failure = null)
        {
            if (ValidateResponse(response))
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnCreateUserRequestFinished " + json.ToString());
                UserData.playFabId = json["playFabId"];
                UserData.email = json["emailAddress"];
                UserData.totalTokenNumber = json["totalTokenNumber"];
                UserData.pointsInCurrentRank = json["pointsInCurrentRank"];
                UserData.pointsToNextRank = json["pointsToNextRank"];
                UserData.rankLevel = json["rankLevel"];
                UserData.rankTitle = json["rankTitle"];
                success?.Invoke();
            }
            else
            {
                failure?.Invoke();
            }
        }

        public static void UpdateUser(string playFabId, string userName = "", Texture2D avatarImage = null, Action success = null)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users"),
                HTTPMethods.Post,
                (req, res) => { OnUpdateUserRequestFinished(req, res); }
            );
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            JSONObject json = new JSONObject();
            json.Add("playFabId", playFabId);
            json.Add("userName", userName);
            if (avatarImage != null)
            {
                json.Add("avatarImage", Convert.ToBase64String(avatarImage.EncodeToPNG()));
            }
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Send();
        }

        static void OnUpdateUserRequestFinished(HTTPRequest request, HTTPResponse response)
        {
            if (ValidateResponse(response))
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnUpdateUserRequestFinished " + json.ToString());
            }
        }

    }

}
