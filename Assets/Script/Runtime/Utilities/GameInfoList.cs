using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperUltra.Container
{

    [Serializable]
    /// <summary>
    ///     Addressable infomation of each game
    /// </summary>
    public class GameInfo
    {
        public int gameId;
        /// <summary> the folder name on remote cloud storage that contains the catalog info </summary>
        public string remoteFolderName;
        public string catalogName;
        /// <summary> Addressable Key of the first scene (aka landing scene) when user enter in a game </summary>
        public string mainSceneKey;
        public Sprite posterImage;
        public Sprite bannerImage;
    }

    [CreateAssetMenu(fileName = "GameInfoList", menuName = "SuperUltra/GameInfoList")]
    public class GameInfoList : ScriptableObject
    {
        [SerializeField]
        public List<GameInfo> list;

    }

}

