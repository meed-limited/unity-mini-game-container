using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameInfo
{
    public string gameName;
    public string catalogName;
    public string mainSceneName;
    public Sprite coverImage;
}

[CreateAssetMenu(fileName = "GameInfoList", menuName = "SuperUltra/GameInfoList")]
public class GameInfoList : ScriptableObject
{
    [SerializeField]
    public List<GameInfo> list;
}
