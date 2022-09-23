using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using SimpleJSON;

namespace SuperUltra.Container
{

    public class ResponseData
    {
        public bool result;
        public string message;
    }

    public class UpdateScoreResponseData : ResponseData
    {
        public LeaderboardUserData[] list;
        public float reward;
        public int position;
        public int score;
    }

    public class GetUserNFTResponseData : ResponseData
    {
        public NFTItem[] list;
    }

    public class GetLeaderboardResponseData : ResponseData
    {
        public LeaderboardUserData[] list;
    }

    public static class NetworkManager
    {

        static bool _isUserDataRequested = false;
        static bool _isAvatarImageRequested = false;
        static Action _onCompleteLoginRequest;
        const float _timeOut = 6f;

        public static bool CheckConnection()
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
                if (response != null)
                    Debug.Log(response.StatusCode);
                if (response != null && string.IsNullOrEmpty(response.DataAsText))
                    Debug.Log(response.DataAsText);

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

        static void GetGameList(Action callback = null)
        {
            // get all th game list from api from Config.domain
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "games"),
                HTTPMethods.Get,
                (req, res) =>
                {
                    Debug.Log("GetGameList response");
                    OnGameListRequestFinished(req, res);
                    callback?.Invoke();
                }
            );
            request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        public static void GetLeaderboard(int gameId, int position, Action<GetLeaderboardResponseData> callback)
        {
            // get all th game list from api from Config.domain
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "systems/leaderboardandbonus"),
                HTTPMethods.Post,
                (req, res) =>
                {
                    OnLeaderboardRequestFinished(req, res, gameId, callback);
                }
            );
            JSONObject json = new JSONObject();
            json.Add("gameIdentifier", gameId);
            json.Add("position", position);
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

        static void OnLeaderboardRequestFinished(HTTPRequest req, HTTPResponse response, int gameID, Action<GetLeaderboardResponseData> callback = null)
        {
            GetLeaderboardResponseData responseData = new GetLeaderboardResponseData() { result = false };
            if (!GameData.gameDataList.TryGetValue(gameID, out GameData gameData))
            {
                callback?.Invoke(responseData);
                return;
            }

            if (ValidateResponse(response))
            {
                responseData.result = true;
                JSONNode json = JSON.Parse(response.DataAsText);
                if (json["users"] != null && json["users"].IsArray)
                {
                    int count = json["users"].AsArray.Count;
                    LeaderboardUserData[] list = new LeaderboardUserData[count];
                    for (int i = 0; i < count; i++)
                    {
                        JSONNode item = json["users"].AsArray[i];
                        Texture2D avatar = new Texture2D(1, 1);
                        // TODO : reteive in image
                        avatar.LoadImage(item["avatarTexture"].AsByteArray);
                        LeaderboardUserData data = new LeaderboardUserData()
                        {
                            rankPosition = item["position"].AsInt,
                            avatarTexture = avatar,
                            name = item["name"].ToString(),
                            score = item["score"],
                            reward = item["reward"],
                        };
                        gameData.leaderboard.Add(data);
                        list[i] = data;
                    }
                    responseData.list = list;

                    // Debug 
                    // foreach (var item in DebugLeaderboardData())
                    // {
                    //     gameData.leaderboard.Add(item);
                    // }

                }
                gameData.tournament.prizePool = json["bonus"];
                // TODO : confirm the structure
                gameData.currentUserPosition = json["currentUserPosition"];
                gameData.currentUserReward = json["currentUserReward"];
                gameData.currentUserScore = json["currentUserScore"];
            }
            callback?.Invoke(responseData);
        }

        static void CompleteRequestList()
        {
            Debug.Log($"{_isUserDataRequested} {_isAvatarImageRequested} {GameData.gameDataList.Count != 0}");
            if (_isUserDataRequested
                && _isAvatarImageRequested
                && GameData.gameDataList.Count != 0
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
            // request token from server, then use the token to request
            // game list, user data and season data
            GetAuthToken(
                () =>
                {
                    // get game list then get leader board data
                    GetGameList(() =>
                    {
                        CompleteRequestList();
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
                }
            );
        }

        public static void ForgetPasswordRequest(string playFabId, Action<ResponseData> callback)
        {
            // TODO
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users"),
                HTTPMethods.Post,
                (req, res) => OnForgetPasswordRequestFinished(req, res, callback)
            );
            JSONObject json = new JSONObject();
            json.Add("platformId", playFabId);
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Send();
        }

        static void OnForgetPasswordRequestFinished(HTTPRequest request, HTTPResponse response, Action<ResponseData> callback)
        {
            bool result = ValidateResponse(response);
            string message = "";
            if(result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
            }
            callback?.Invoke(new ResponseData() { result = result, message = message });
        }

        public static void ResetPassword(string playFabId, Action<ResponseData> callback)
        { 
            // TODO
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users"),
                HTTPMethods.Post,
                (req, res) => OnResetPasswordRequestFinished(req, res, callback)
            );
            JSONObject json = new JSONObject();
            json.Add("platformId", playFabId);
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Send();
        }

        static void OnResetPasswordRequestFinished(HTTPRequest request, HTTPResponse response, Action<ResponseData> callback)
        {
            bool result = ValidateResponse(response);
            string message = "";
            if (result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
            }

            callback?.Invoke(new ResponseData(){ result = result, message = message });
        }
        

        public static void VerifyResetCode(string playFabId, Action<ResponseData> callback)
        { 
            // TODO
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users"),
                HTTPMethods.Post,
                (req, res) => OnVerifyResetCodeRequestFinished(req, res, callback)
            );
            JSONObject json = new JSONObject();
            json.Add("platformId", playFabId);
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Send();
        }

        static void OnVerifyResetCodeRequestFinished(HTTPRequest request, HTTPResponse response, Action<ResponseData> callback)
        {
            bool result = ValidateResponse(response);
            string message = "";
            if (result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
            }

            callback?.Invoke(new ResponseData(){ result = result, message = message });
        }


        public static void CreateUser(string playFabId, Action success, Action failure = null)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users"),
                HTTPMethods.Post,
                (req, res) => { OnCreateUserRequestFinished(req, res, success, failure); }
            );
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

        public static void UpdateScore(float score, string playFabId, int gameId, Action<UpdateScoreResponseData> callback)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users/submitscore"),
                HTTPMethods.Post,
                (req, res) =>
                {
                    OnUpdateScoreRequestFinished(req, res, callback);
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

        static LeaderboardUserData[] DebugLeaderboardData()
        {
            return new LeaderboardUserData[]{
                new LeaderboardUserData(){
                    rankPosition = 1,
                    name = "User 1",
                    avatarTexture = Texture2D.blackTexture,
                    score = 100,
                    reward = 1
                },
                new LeaderboardUserData(){
                    rankPosition = 2,
                    name = "User 2",
                    avatarTexture = Texture2D.linearGrayTexture,
                    score = 7,
                    reward = 0.5f
                },
                new LeaderboardUserData(){
                    rankPosition = 3,
                    name = "User 4",
                    avatarTexture = Texture2D.redTexture,
                    score = 5,
                    reward = 0.3f
                },
                new LeaderboardUserData(){
                    rankPosition = 4,
                    name = "User 3",
                    avatarTexture = Texture2D.blackTexture,
                    score = 4,
                    reward = 0.2f
                },
                new LeaderboardUserData(){
                    rankPosition = 5,
                    name = "User 5",
                    avatarTexture = Texture2D.linearGrayTexture,
                    score = 7,
                    reward = 0.05f
                },
                new LeaderboardUserData(){
                    rankPosition = 6,
                    name = "User 3",
                    avatarTexture = Texture2D.blackTexture,
                    score = 4,
                    reward = 0.2f
                },
                new LeaderboardUserData(){
                    rankPosition =7,
                    name = "User 5",
                    avatarTexture = Texture2D.linearGrayTexture,
                    score = 7,
                    reward = 0.05f
                },
                new LeaderboardUserData(){
                    rankPosition = 8,
                    name = "User 3",
                    avatarTexture = Texture2D.blackTexture,
                    score = 4,
                    reward = 0.2f
                },
                new LeaderboardUserData(){
                    rankPosition = 9,
                    name = "User 5",
                    avatarTexture = Texture2D.linearGrayTexture,
                    score = 7,
                    reward = 0.05f
                },
                new LeaderboardUserData(){
                    rankPosition = 10,
                    name = "User 3",
                    avatarTexture = Texture2D.blackTexture,
                    score = 4,
                    reward = 0.2f
                }
            };
        }

        static void OnUpdateScoreRequestFinished(HTTPRequest request, HTTPResponse response, Action<UpdateScoreResponseData> callback)
        {
            // TODO : confirm the data design with backend
            SessionData.currnetGameScore = -1;
            int position = 8;
            float reward = 0.03f;
            int score = 2;
            LeaderboardUserData[] list = DebugLeaderboardData();
            bool result = ValidateResponse(response);

            if (result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnUpdateScoreRequestFinished " + json.ToString());
                position = json["position"];
                reward = json["reward"];
                score = json["score"].AsInt;
                if (json["users"].IsArray)
                {
                    JSONArray leaderboardUser = json["users"].AsArray;
                    list = new LeaderboardUserData[leaderboardUser.Count];
                    for (int i = 0; i < leaderboardUser.Count; i++)
                    {
                        list[i] = new LeaderboardUserData()
                        {
                            rankPosition = 1
                        };
                    }
                }
            }

            callback?.Invoke(new UpdateScoreResponseData()
            {
                result = result,
                list = list,
                position = position,
                reward = reward,
                score = score
            });
        }

        public static void GetUserNFT(string playFabId, Action<GetUserNFTResponseData> callback)
        {
            // TODO confirm structure
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users/submitscore"),
                HTTPMethods.Get,
                (req, res) =>
                {
                    OnGetUserNFTRequestFinished(req, res, callback);
                }
            );
            JSONObject json = new JSONObject();
            json.Add("fabId", playFabId);
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnGetUserNFTRequestFinished(HTTPRequest req, HTTPResponse response, Action<GetUserNFTResponseData> callback)
        {
            // TODO confirm structure
            bool result = ValidateResponse(response);
            NFTItem[] list = new NFTItem[] { };

            if (result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                if (json["nftList"].IsArray)
                {
                    JSONArray nftList = json["nftList"].AsArray;
                    list = new NFTItem[nftList.Count];
                    for (int i = 0; i < nftList.Count; i++)
                    {
                        Texture2D texture = new Texture2D(1, 1);
                        texture.LoadImage(nftList[i]["avatarTexture"].AsByteArray);
                        list[i] = new NFTItem()
                        {
                            name = nftList[i]["name"].ToString(),
                            texture2D = texture
                        };
                    }
                }
            }

            callback?.Invoke(new GetUserNFTResponseData()
            {
                result = result,
                list = list
            });
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

        public static void SignOut(Action callback)
        {
            // TODO : May need to call for endpoint to notify the event
            callback?.Invoke();
            UserData.ClearData();
            GameData.ClearData();
            SessionData.ClearData();
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
