using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.MOP2;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class ItemGenerator : MonoBehaviour
    {

        void OnEnable()
        {
            TriggerExit.OnChunkExited += SpwanObject;
        }

        private void OnDisable()
        {
            TriggerExit.OnChunkExited -= SpwanObject;
        }

        private int _itemNum;
        private List<int> _spawnZPos = new List<int>();
        private int[] _spawnXPos = { -10, -5, 0, 5 };
        private bool _isFull = false;
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        [SerializeField]
        ObjectPool pool;
        List<GameObject> _spawnedObject = new List<GameObject>();
        [SerializeField] GameObject _car;

        private void Start()
        {
            pool.Initialize();
            pool.ObjectParent.parent = transform;
            //calculate item number
            SpwanObject();



        }

        public void SpwanObject()
        {

            _itemNum = Random.Range(0, 10);

            //for loop to spawn item in SpawnPos
            for (int i = 0; i < _itemNum; i++)
            {
                //caculate Xpos
                int _xpos = _spawnXPos[Random.Range(0, _spawnXPos.Length)];
                //caculate Zpos between -5 ~ 45
                while (!_isFull)
                {
                    int z = Random.Range(-5, 45);
                    if (z % 5 == 0)
                    {
                        if (_spawnZPos.Count < _itemNum && !_spawnZPos.Contains(z))
                        {
                            _spawnZPos.Add(z);
                        }
                        else
                        {
                            _isFull = true;
                        }
                    }

                }
                //spawn item every 5 on z

                //Debug.Log($"{i} {_spawnZPos.Count}");
                if (i < _spawnZPos.Count)
                {
                    GameObject _bana = pool.GetObject(new Vector3(_xpos, 1, DataHandle.Instance._mapCount * 50 + _spawnZPos[i]), rotation);
                    _spawnedObject.Add(_bana);

                }
            }


        }
        void Update()
        {
            for (int i = 0; i < _spawnedObject.Count; i++)
            {
                if (_spawnedObject[i].transform.position.z < _car.transform.position.z - 20f)
                {
                    Recycle(_spawnedObject[i]);
                    _spawnedObject.Remove(_spawnedObject[i]);
                }

            }
        }

        public void Recycle(GameObject gameObject)
        {
            pool.Release(gameObject);
        }


    }
}