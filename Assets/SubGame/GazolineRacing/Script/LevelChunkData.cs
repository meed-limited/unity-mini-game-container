using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    [CreateAssetMenu(menuName = "LevelChunkData")]
    public class LevelChunkData : ScriptableObject
    {
        public enum Direction
        {
            North, East, South, West
        }

        public Vector2 chunkSize = new Vector2(10f, 10f);

        public GameObject[] levelChunks;
        public Direction entryDirection;
        public Direction exitDirection;

    }
}