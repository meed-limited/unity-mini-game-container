using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameInfo
{
    public int gameId;
    public string gameName;
    public string catalogName;
    /// <summary> Name of the first scene where user will enter after choosing a game in Main menu </summary>
    public string mainSceneName;
    public Sprite coverImage;
}

[CreateAssetMenu(fileName = "GameInfoList", menuName = "SuperUltra/GameInfoList")]
public class GameInfoList : ScriptableObject
{
    [SerializeField]
    public List<GameInfo> list;
}
