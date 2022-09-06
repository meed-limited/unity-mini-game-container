using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using BestHTTP;
using SimpleJSON;

namespace SuperUltra.Container
{

    public static class NetworkManager
    {
        static bool _isUserDataRequested = false;
        static bool _isAvatarImageRequested = false;
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
                Debug.Log("respons is fail ");
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
            if (GameData.gameDataList.Count <= 0)
            {
                callback?.Invoke();
            }

            foreach (var item in GameData.gameDataList)
            {
                int id = item.Value.id;
                GetLeaderboard(id, callback);
            }
        }

        static void GetLeaderboard(int gameId, Action callback)
        {
            // get all th game list from api from Config.domain
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "systems/leaderboardandbonus"),
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

        public static void GetUserData(Action callback, Action avatarRequestCallback = null)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + $"users/{UserData.playFabId}"),
                HTTPMethods.Get,
                (req, res) =>
                {
                    Debug.Log("GetUserData response");
                    OnUserDataRequestFinished(req, res, avatarRequestCallback);
                    callback?.Invoke();
                }
            );
            request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnUserDataRequestFinished(HTTPRequest request, HTTPResponse response, Action avatarRequestCallback = null)
        {
            if (ValidateResponse(response))
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                JSONNode data = json["data"];
                if (data != null)
                {
                    Debug.Log("OnUserDataRequestFinished " + response.DataAsText);
                    UserData.email = data["emailAddress"];
                    UserData.userName = data["username"];
                    UserData.totalTokenNumber = data["totalTokenNumber"];
                    UserData.walletAddress = data["walletAddress"];
                    UserData.pointsInCurrentRank = data["experiencePoints"];
                    UserData.pointsToNextRank = data["pointsToNextRank"];
                    UserData.rankLevel = data["rankLevel"];
                    UserData.rankTitle = data["rank"];
                    GetAvatar(data["avatarUrl"], avatarRequestCallback);
                }
            }
            else
            {
                avatarRequestCallback?.Invoke();
            }
        }

        static void GetAvatar(string avatarUrl, Action callback = null)
        {
            Debug.Log("GetAvatar " + avatarUrl);
            if (string.IsNullOrEmpty(avatarUrl)
                || avatarUrl.Equals("NA")
            // for extra checking
            // || !Uri.IsWellFormedUriString(avatarUrl, UriKind.RelativeOrAbsolute)
            )
            {
                Debug.Log($"GetAvatar {!Uri.IsWellFormedUriString(avatarUrl, UriKind.RelativeOrAbsolute)}");
                callback?.Invoke();
                UserData.profilePic = Resources.Load<Texture2D>("default-avatar");
                return;
            }

            Debug.Log("GetAvatar Sending");
            HTTPRequest request = new HTTPRequest(
                new Uri(avatarUrl),
                HTTPMethods.Get,
                (req, res) =>
                {
                    if (res.IsSuccess && res.Data != null)
                    {
                        UserData.profilePic = res.DataAsTexture2D;
                    }
                    else
                    {
                        Debug.LogError($"GetAvatar fail");
                    }
                    callback?.Invoke();
                }
            );
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
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
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnLeaderBoardRequestFinished " + json.ToString());
                if (json["users"] != null && json["users"].IsArray)
                {
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
                gameData.tournament.prizePool = json["bonus"];
            }
        }

        static void CompleteRequestList()
        {
            Debug.Log($"{_isUserDataRequested} {_isAvatarImageRequested} {_isUserSeasonDataRequested} {GameData.gameDataList.Count != 0} {_requestedLeaderboardCount == GameData.gameDataList.Count}");
            if (_isUserDataRequested
                && _isAvatarImageRequested
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
            Debug.Log("data " + data);

            if (json["games"] == null || !json["games"].IsArray)
            {
                Debug.Log("No game list found");
                return;
            }

            JSONArray arr = json["games"].AsArray;
            foreach (JSONNode item in arr)
            {
                Debug.Log($"item {item}");
                if (item["id"] == null || item["title"] == null)
                {
                    continue;
                }
                int gameid = item["id"].AsInt;
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
            // request token from server, then use the token to request
            // game list, user data and season data
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
                    }, () =>
                    {
                        _isAvatarImageRequested = true;
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
                success?.Invoke();
            }
            else
            {
                failure?.Invoke();
            }
        }

        public static void UpdateScore(float score, string playFabId, int gameId, Action callback)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users/submitscore"), 
                HTTPMethods.Post, 
                (req, res) => {
                    OnUpdateScoreRequestFinished();
                    callback?.Invoke();
                }
            );
            JSONObject json = new JSONObject();
            json.Add("fabId", playFabId);
            json.Add("gameIdentifier", gameId);
            json.Add("score", score);
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnUpdateScoreRequestFinished()
        { 
            // TODO
            Debug.Log("OnUpdateScoreRequestFinished");
        }

        #region Update User

        public static void UpdateUserName(string playFabId, string userName, Action callback)
        {
            JSONObject json = new JSONObject();
            json.Add("username", userName);
            json.Add("platformId", playFabId);
            UpdateUser(json, callback);
        }

        public static void UpdateUserProfile(string playFabId, string userName, Texture2D texture2D, Action callback)
        {
            JSONObject json = new JSONObject();
            string encodedImage = Convert.ToBase64String(texture2D.EncodeToPNG());
            json.Add("avatarUrl", encodedImage);
            json.Add("username", userName);
            json.Add("platformId", playFabId);
            UpdateUser(json, callback);
        }

        static void UpdateUser(JSONObject json, Action callback = null)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users"),
                HTTPMethods.Put,
                (req, res) =>
                {
                    OnUpdateUserRequestFinished(req, res);
                    callback?.Invoke();
                }
            );
            Debug.Log("json " + json.ToString());
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnUpdateUserRequestFinished(HTTPRequest request, HTTPResponse response)
        {
            Debug.Log("OnUpdateUserRequestFinished ");
            if (ValidateResponse(response))
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnUpdateUserRequestFinished " + json.ToString());
            }
        }

        #endregion

    }

}
