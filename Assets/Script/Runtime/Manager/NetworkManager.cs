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
        public int position = -1;
        public int score = -1;
        public string rankTitle = "";
        public int experiencePoints = -1;
        public int pointsToNextRank = -1;
    }

    public class GetUserNFTResponseData : ResponseData
    {
        public NFTItem[] list;
    }

    public class GetLeaderboardResponseData : ResponseData
    {
        public LeaderboardUserData[] list;
        public int nextPage = -1;
        public int prevPage = -1;
    }

    public class GetImageResponseData : ResponseData
    {
        public Texture2D texture2D;
    }

    public class UpdateUserResponseData : ResponseData
    {
        public string userName;
        public Texture2D texture2D;
    }

    public class GetTournamentResponseData : ResponseData
    {
        public DateTime endDate;
        public float prizePool;
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

        static string GetDataMessage(JSONNode json)
        {
            if (json != null && !string.IsNullOrEmpty(json["message"]))
            {
                return json["message"];
            }
            // Response received (json is null or message empty)
            return "";
        }

        static ResponseData ValidateResponse(HTTPResponse response)
        {
            ResponseData data = new ResponseData() { result = false };
            JSONNode json;
            if (response == null || response.IsSuccess == false)
            {
                Debug.Log("respons is fail ");
                data.message = "Server error";
                if (response != null)
                    Debug.Log(response.StatusCode);
                if (response != null && !string.IsNullOrEmpty(response.DataAsText))
                {
                    Debug.Log(response.DataAsText);
                    json = JSON.Parse(response.DataAsText);
                    data.message = GetDataMessage(json);
                }
                return data;
            }

            json = JSON.Parse(response.DataAsText);
            Debug.Log("ValidateResponse " + json);
            if (json == null || json["success"] == null || json["success"] != true)
            {
                data.message = GetDataMessage(json);
                return data;
            }
            data.message = GetDataMessage(json);
            data.result = true;
            return data;
        }

        /// <summary>
        /// request token from server, then use the token to request
        /// game list, user data and season data
        /// </summary>
        static void GetAuthToken(Action callback)
        {
            // TODO
            string playFabId = UserData.playFabId;
            callback();
        }

        static void GetGameList(Action<ResponseData> callback = null)
        {
            // get all th game list from api from Config.domain
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "games"),
                HTTPMethods.Get,
                (req, res) =>
                {
                    Debug.Log("GetGameList response");
                    OnGameListRequestFinished(req, res, callback);
                }
            );
            request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        public static void GetLeaderboard(int gameId, int page, int count, Action<GetLeaderboardResponseData> callback)
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
            json.Add("gameId", gameId);
            json.Add("platformId", UserData.playFabId);
            json.Add("count", count);
            json.Add("page", page);
            request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        public static void GetUserData(Action<ResponseData> callback, Action<ResponseData> avatarRequestCallback = null)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + $"users/{UserData.playFabId}"),
                HTTPMethods.Get,
                (req, res) =>
                {
                    Debug.Log("GetUserData response");
                    OnUserDataRequestFinished(req, res, callback, avatarRequestCallback);
                }
            );
            request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnUserDataRequestFinished(HTTPRequest request, HTTPResponse response, Action<ResponseData> callback, Action<ResponseData> avatarRequestCallback = null)
        {
            ResponseData responseData = ValidateResponse(response);
            if (responseData.result)
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
                    if (data["avatarUrl"].IsString && !string.IsNullOrEmpty(data["avatarUrl"]))
                    {
                        GetAvatar(data["avatarUrl"], avatarRequestCallback);
                    }else
                    {
                        avatarRequestCallback?.Invoke(new ResponseData{result = false, message = "User has no avatar" });
                    }
                }
            }
            else
            {
                avatarRequestCallback?.Invoke(new ResponseData { result = false });
            }

            callback?.Invoke(responseData);
        }

        public static void GetImage(string avatarUrl, Action<GetImageResponseData> callback = null)
        {
            Texture2D texture2D = Texture2D.grayTexture;
            if (string.IsNullOrEmpty(avatarUrl))
            {
                callback?.Invoke(new GetImageResponseData { result = false, texture2D = texture2D });
                return;
            }
            HTTPRequest request = new HTTPRequest(
                new Uri(avatarUrl),
                HTTPMethods.Get,
                (req, res) =>
                {
                    bool result = res != null && res.IsSuccess && res.Data != null;
                    Debug.Log("request ");// + req.Uri + "\nresponse is null" +　res == null);
                    if (result)
                        texture2D = res.DataAsTexture2D;
                    callback?.Invoke(new GetImageResponseData
                    {
                        result = result,
                        texture2D = texture2D
                    });
                }
            );
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void GetAvatar(string avatarUrl, Action<ResponseData> callback = null)
        {
            Debug.Log("GetAvatar " + avatarUrl);
            if (string.IsNullOrEmpty(avatarUrl)
                || avatarUrl.Equals("NA")
            // for extra checking
            // || !Uri.IsWellFormedUriString(avatarUrl, UriKind.RelativeOrAbsolute)
            )
            {
                Debug.Log($"GetAvatar {!Uri.IsWellFormedUriString(avatarUrl, UriKind.RelativeOrAbsolute)}");
                callback?.Invoke(new ResponseData { result = false });
                UserData.profilePic = Resources.Load<Texture2D>("default-avatar");
                return;
            }

            Debug.Log("GetAvatar Sending");
            HTTPRequest request = new HTTPRequest(
                new Uri(avatarUrl),
                HTTPMethods.Get,
                (req, res) =>
                {
                    bool result = res != null && res.IsSuccess && res.Data != null;

                    if (result)
                    {
                        UserData.profilePic = res.DataAsTexture2D;
                    }
                    else
                    {
                        Debug.LogError($"GetAvatar fail");
                    }
                    callback?.Invoke(new ResponseData { result = result });
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

            if (ValidateResponse(response).result)
            {
                responseData.result = true;
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnLeaderboardRequestFinished " + response.DataAsText);
                gameData.tournament.prizePool = json["bonus"];
                if (json["users"] != null && json["users"].IsArray)
                {
                    int count = json["users"].AsArray.Count;
                    LeaderboardUserData[] list = new LeaderboardUserData[count];
                    list = GetLeaderboardUserData(json["users"].AsArray);
                    responseData.list = list;
                }
                if (json["boardInfo"] != null)
                {
                    JSONNode boardInfo = json["boardInfo"];
                    gameData.currentUserPosition = boardInfo["position"];
                    gameData.currentUserReward = boardInfo["reward"];
                    gameData.currentUserScore = boardInfo["score"];
                }

                responseData.nextPage = json["nextPageNumber"] != null ? json["nextPageNumber"] : -1;
                responseData.prevPage = json["prevPageNumber"] != null ? json["prevPageNumber"] : -1;
            }
            callback?.Invoke(responseData);
        }

        static void CompleteRequestList(Action<ResponseData> callback, ResponseData data)
        {
            if (!data.result)
            {
                callback?.Invoke(data);
                return;
            }
            Debug.Log($"{_isUserDataRequested} {_isAvatarImageRequested}");
            if (_isUserDataRequested
                && _isAvatarImageRequested
            )
            {
                callback?.Invoke(new ResponseData
                {
                    result = true
                });
            }
        }

        static void OnGameListRequestFinished(HTTPRequest request, HTTPResponse response, Action<ResponseData> callback)
        {
            ResponseData responseData = ValidateResponse(response);
            if (!responseData.result)
            {
                callback?.Invoke(responseData);
                return;
            }

            string data = response.DataAsText;
            JSONNode json = JSON.Parse(data);
            Debug.Log("data " + data);

            if (json["games"] == null || !json["games"].IsArray)
            {
                responseData.message = "No game list found";
                responseData.result = false;
                callback?.Invoke(responseData);
                return;
            }

            JSONArray arr = json["games"].AsArray;
            foreach (JSONNode item in arr)
            {
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
            responseData.result = true;
            callback?.Invoke(responseData);
        }

        public static void LoginRequest(Action<ResponseData> callback)
        {
            if (!CheckConnection())
            {
                return;
            }

            GetAuthToken(
                () =>
                {
                    GetGameList();
                    GetUserData((response) =>
                    {
                        _isUserDataRequested = true;
                        CompleteRequestList(callback, response);
                    }, (response) =>
                    {
                        _isAvatarImageRequested = true;
                        // player will proceed the menu whether avatar request is success or not
                        // here just make sure we get the response. 
                        response.result = true; 
                        CompleteRequestList(callback, response);
                    });
                }
            );
        }

        static JSONArray GetJSONArray(JSONNode node)
        {
            if (node.IsArray)
                return node.AsArray;
            return new JSONArray();
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
            bool result = ValidateResponse(response).result;
            string message = "";
            if (result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
            }
            callback?.Invoke(new ResponseData() { result = result, message = message });
        }

        public static void GetTournament(int gameId, Action<GetTournamentResponseData> callback)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "systems/tournaments/for"),
                HTTPMethods.Post,
                (req, res) => OnGetTournamentRequestFinished(req, res, callback)
            );
            JSONObject json = new JSONObject();
            json.Add("gameId", gameId);
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnGetTournamentRequestFinished(HTTPRequest request, HTTPResponse response, Action<GetTournamentResponseData> callback)
        {
            ResponseData data = ValidateResponse(response);
            GetTournamentResponseData responseData = new GetTournamentResponseData { result = data.result, message = data.message };
            if (data.result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnGetTournamentRequestFinished " + json.ToString());
                if (json["game"] != null)
                {
                    JSONNode game = json["game"];
                    if (GameData.gameDataList.ContainsKey(game["id"]))
                    {
                        Tournament tournament = GameData.gameDataList[game["id"]].tournament;
                        tournament.endTime = new DateTime().FromTimeStamp(game["endTime"]);
                        tournament.prizePool = game["pricePool"].AsFloat;
                        tournament.status = (TournamentStatus)game["status"].AsInt;
                        Debug.Log(tournament.endTime);
                    }

                }
            }
            callback?.Invoke(responseData);
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
            if (ValidateResponse(response).result)
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
            Debug.Log($"UpdateScore {score}");
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
            json.Add("gameId", gameId);
            json.Add("score", score);
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }


        static LeaderboardUserData[] GetLeaderboardUserData(JSONArray leaderboardBeforeUser)
        {
            if (leaderboardBeforeUser == null)
                return new LeaderboardUserData[] { };
            LeaderboardUserData[] list = new LeaderboardUserData[leaderboardBeforeUser.Count];
            for (int i = 0; i < leaderboardBeforeUser.Count; i++)
            {
                Texture2D avatar = new Texture2D(1, 1);
                string userName = "";
                string avatarUrl = "";
                JSONNode item = leaderboardBeforeUser[i];
                avatar.LoadImage(item["avatarTexture"].AsByteArray);
                if (item["userInfo"] != null)
                {
                    userName = item["userInfo"]["username"];
                    avatarUrl = item["userInfo"]["avatarUrl"];
                }
                list[i] = new LeaderboardUserData()
                {
                    rankPosition = item["position"].AsInt,
                    avatarUrl = avatarUrl,
                    name = userName,
                    score = item["score"],
                    reward = item["bonus"],
                };
            }

            return list;
        }

        static void OnUpdateScoreRequestFinished(HTTPRequest request, HTTPResponse response, Action<UpdateScoreResponseData> callback)
        {
            // TODO : confirm the data design with backend
            SessionData.currnetGameScore = -1;
            int position = 8;
            int score = 2;
            float reward = 0.03f;
            LeaderboardUserData[] list = new LeaderboardUserData[] { };
            ResponseData data = ValidateResponse(response);
            bool result = data.result;
            string message = data.message;
            string rank = UserData.rankTitle;
            int experiencePoints = UserData.pointsInCurrentRank;
            int pointsToNextRank = UserData.pointsToNextRank;

            if (result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnUpdateScoreRequestFinished response" + json.ToString());
                if (json["details"] != null)
                {
                    JSONNode details = json["details"];
                    position = details["position"];
                    reward = details["reward"];
                    score = details["score"].AsInt;
                    rank = details["rank"];
                    experiencePoints = details["experiencePoints"];
                    pointsToNextRank = details["pointsToNextRank"];
                    JSONArray leaderboardBeforeUser = GetJSONArray(details["twoBefore"]);
                    JSONArray leaderboardAfterUser = GetJSONArray(details["twoAfter"]);
                    list = new LeaderboardUserData[leaderboardBeforeUser.Count + leaderboardAfterUser.Count + 1];
                    list[leaderboardBeforeUser.Count] = new LeaderboardUserData()
                    {
                        rankPosition = position,
                        name = UserData.userName,
                        avatarTexture = UserData.profilePic,
                        score = score,
                        reward = reward,
                    };
                    GetLeaderboardUserData(leaderboardBeforeUser).CopyTo(list, 0);
                    GetLeaderboardUserData(leaderboardAfterUser).CopyTo(list, leaderboardBeforeUser.Count + 1);
                }
            }
            else
            {
                Debug.Log(Encoding.ASCII.GetString(request.RawData));
            }

            UpdateScoreResponseData responseData = new UpdateScoreResponseData()
            {
                result = result,
                message = message,
                list = list,
                position = position,
                reward = reward,
                score = score,
                rankTitle = rank,
                experiencePoints = experiencePoints,
                pointsToNextRank = pointsToNextRank
            };
            UserData.UpdateUserData(responseData);
            callback?.Invoke(responseData);
        }

        public static void GetUserNFT(Action<GetUserNFTResponseData> callback)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + $"users/{UserData.playFabId}"),
                HTTPMethods.Get,
                (req, res) =>
                {
                    OnGetUserNFTRequestFinished(req, res, callback);
                }
            );
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnGetUserNFTRequestFinished(HTTPRequest req, HTTPResponse response, Action<GetUserNFTResponseData> callback)
        {
            bool result = ValidateResponse(response).result;
            NFTItem[] list = new NFTItem[] { };

            if (result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                if (json["nftData"].IsArray)
                {
                    JSONArray nftList = json["nftData"].AsArray;
                    list = new NFTItem[nftList.Count];
                    for (int i = 0; i < nftList.Count; i++)
                    {
                        JSONNode nftItem = nftList[i];
                        Texture2D texture = new Texture2D(1, 1);
                        texture.LoadImage(nftItem["avatarTexture"].AsByteArray);
                        list[i] = new NFTItem()
                        {
                            id = nftItem["id"],
                            name = nftItem["name"].ToString(),
                            description = nftItem["description"],
                            texture2DUrl = nftItem["image"],
                            attribute = nftItem["attributes"].ToString(),
                            type = NFTItem.ItemType.Cosmetic,
                            isActive = false,
                        };
                    }
                }
            }

            UserData.nftItemList = list;
            callback?.Invoke(new GetUserNFTResponseData()
            {
                result = result,
                list = list
            });
        }

        #region Update User

        public static void UpdateUserWalletAddress(string playFabId, string walletAddress, Action<ResponseData> callback)
        {
            JSONObject json = new JSONObject();
            json.Add("walletAddress", walletAddress);
            json.Add("platformId", playFabId);
            UpdateUser(json, callback);
        }

        public static void UpdateUserName(string playFabId, string userName, Action<ResponseData> callback)
        {
            JSONObject json = new JSONObject();
            json.Add("username", userName);
            json.Add("platformId", playFabId);
            UpdateUser(json, callback);
        }

        public static void UpdateUserProfile(string playFabId, string userName, Texture2D texture2D, Action<ResponseData> callback)
        {
            JSONObject json = new JSONObject();
            string encodedImage = Convert.ToBase64String(texture2D.EncodeToPNG());
            UserData.pendingProfilePic = texture2D;
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

        static void UpdateUser(JSONObject json, Action<ResponseData> callback = null)
        {
            HTTPRequest request = new HTTPRequest(
                new Uri(Config.Domain + "users"),
                HTTPMethods.Put,
                (req, res) =>
                {
                    OnUpdateUserRequestFinished(req, res, callback, json);
                }
            );
            request.SetHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiR2FtaWZpZWRQbGF0Zm9ybSIsImlhdCI6MTY1OTc3NDMzMywiZXhwIjoxNzQ2MTc0MzMzfQ.BtSPOnqfGKdI3j1g7EMm_vdZFkQwxUNF8uzX_jOqGDE");
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.ASCII.GetBytes(json.ToString());
            request.Timeout = TimeSpan.FromSeconds(_timeOut);
            request.Send();
        }

        static void OnUpdateUserRequestFinished(HTTPRequest request, HTTPResponse response, Action<ResponseData> callback, JSONObject requestData)
        {
            ResponseData data = ValidateResponse(response);
            if (data.result)
            {
                JSONNode json = JSON.Parse(response.DataAsText);
                Debug.Log("OnUpdateUserRequestFinished " + json.ToString());
                UserData.profilePic = UserData.pendingProfilePic;
                UserData.pendingProfilePic = null;
                if (requestData["username"] != null && requestData["username"].IsString)
                    UserData.userName = requestData["username"];
                if (requestData["walletAddress"] != null && requestData["walletAddress"].IsString)
                    UserData.walletAddress = requestData["walletAddress"];
            }
            else
            {
                Debug.Log(Encoding.ASCII.GetString(request.RawData));
            }
            callback?.Invoke(new UpdateUserResponseData()
            {
                message = data.message,
                result = data.result,
                texture2D = UserData.pendingProfilePic
            });
        }

        #endregion

    }

}
