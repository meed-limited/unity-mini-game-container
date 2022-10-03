using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.MOP2;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class LevelPooker : MonoBehaviour
    {
        [SerializeField] ObjectPool[] _levelPool;

        public Vector3 spawnOrigin;

        private Vector3 spawnPosition;
        public int chunksToSpawn = 10;
        List<GameObject> _spawnedObject = new List<GameObject>();


        void OnEnable()
        {
            TriggerExit.OnChunkExited += PickAndSpawnChunk;
        }

        private void OnDisable()
        {
            TriggerExit.OnChunkExited -= PickAndSpawnChunk;
        }




        void Start()
        {
            for (int i = 0; i < _levelPool.Length; i++)
            {
                _levelPool[i].Initialize();
                _levelPool[i].ObjectParent.parent = transform;
            }
            for (int i = 0; i < chunksToSpawn; i++)
            {
                PickAndSpawnChunk();
            }
        }



        void PickAndSpawnChunk()
        {
            int x = Random.Range(0, _levelPool.Length);
            spawnPosition += new Vector3(0f, 0, 50);
            GameObject _level = _levelPool[x].GetObject(spawnPosition + spawnOrigin, Quaternion.identity);
            //_spawnedObject.Add(_level);
            //StartCoroutine(Recycle(_level));
            //Debug.Log("Spawn");
        }
        private void Update()
        {


            /**
                if (_spawnedObject.Count >= 10)
                {
                    Recycle(_spawnedObject[0]);
                    _spawnedObject.Remove(_spawnedObject[0]);
                }
            **/
        }
        public void Recycle(GameObject gameObject)
        {
            if (gameObject.layer == 10)
            {
                _levelPool[0].Release(gameObject);
            }
            else if (gameObject.layer == 9)
            {
                _levelPool[1].Release(gameObject);
            }
            else if (gameObject.layer == 11)
            {
                _levelPool[2].Release(gameObject);
            }
            else if (gameObject.layer == 12)
            {
                _levelPool[3].Release(gameObject);
            }

        }

        public void UpdateSpawnOrigin(Vector3 originDelta)
        {
            spawnOrigin = spawnOrigin + originDelta;
        }
    }
}