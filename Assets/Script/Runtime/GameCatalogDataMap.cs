using System;
using UnityEngine;

[Serializable]
public class GameCatalogData
{
    public string landingSceneName;
    public string catalogURL;
}

[CreateAssetMenu(fileName = "GameCatalogDataMap", menuName = "SuperUltra/GameCatalogDataMap")]
public class GameCatalogDataMap : Map<string, GameCatalogData>
{

}
