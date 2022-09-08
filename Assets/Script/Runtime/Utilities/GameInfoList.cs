using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameInfo
{
    public int gameId;
    public string gameName;
    public string catalogName;
    /// <summary> Addressable Key of the first scene (aka landing scene) when user enter in a game </summary>
    public string mainSceneName;
    public Sprite coverImage;
}

[CreateAssetMenu(fileName = "GameInfoList", menuName = "SuperUltra/GameInfoList")]
public class GameInfoList : ScriptableObject
{
    [SerializeField]
    public List<GameInfo> list;
}
