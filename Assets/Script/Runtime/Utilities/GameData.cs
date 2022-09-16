using System;
using System.Collections.Generic;

namespace SuperUltra.Container
{
    public class GameData
    {
        static Dictionary<int, GameData> _gameDataList;
        /// <summary>
        /// a Dictionary of gameId to GameData map
        /// </summary>
        public static Dictionary<int, GameData> gameDataList
        {
            get
            {
                if (_gameDataList == null)
                {
                    _gameDataList = new Dictionary<int, GameData>();
                }
                return _gameDataList;
            }
            private set { _gameDataList = value; }
        }

        List<LeaderboardUserData> _leaderboard;
        /// <summary>
        /// a List of LeaderboardUserData
        /// </summary>
        public List<LeaderboardUserData> leaderboard
        {
            get
            {
                if (_leaderboard == null)
                {
                    _leaderboard = new List<LeaderboardUserData>();
                }
                return _leaderboard;
            }
            private set { _leaderboard = value; }
        }

        Tournament _tournament;
        /// <summary>
        /// a Tournament object
        /// </summary>
        public Tournament tournament
        {
            get
            {
                if (_tournament == null)
                {
                    _tournament = new Tournament();
                }
                return _tournament;
            }
            private set { _tournament = value; }
        }

        public static void ClearData()
        {
            _gameDataList.Clear();
        }
        
        public int id;
        public string name;
        public string description;

    }
}