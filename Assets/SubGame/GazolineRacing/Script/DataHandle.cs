using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class DataHandle : MonoBehaviour
    {
        public static DataHandle Instance;
        public int _mapCount = 1;
        private void OnEnable()
        {
            TriggerExit.OnChunkExited += AddMap;
        }
        private void Awake()
        {
            Instance = this;
        }

        public void AddMap()
        {
            _mapCount += 1;
        }


    }
}