using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.MOP2;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class CarGenarator : MonoBehaviour
    {
        [SerializeField]
        private ObjectPool[] _pools;
        private int _carNum, _carSelect;
        private List<int> _spawnZPos = new List<int>();
        private int[] _spawnXPos = { -10, -5, 0, 5 };
        private bool _isFull = false;
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        public List<GameObject> _spawnedObject = new List<GameObject>();
        [SerializeField] GameObject _car;
        void OnEnable()
        {
            TriggerExit.OnChunkExited += SpwanCar;
        }

        private void OnDisable()
        {
            TriggerExit.OnChunkExited -= SpwanCar;
        }
        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                _pools[i].Initialize();
                _pools[i].ObjectParent.parent = transform;
            }

            //calculate item number


        }

        public void SpwanCar()
        {
            _carNum = Random.Range(0, 10);
            _carSelect = Random.Range(0, _pools.Length);

            //for loop to spawn item in SpawnPos
            for (int i = 0; i < _carNum; i++)
            {
                //caculate Xpos
                int _xpos = _spawnXPos[Random.Range(0, _spawnXPos.Length)];
                //caculate Zpos between -5 ~ 45
                while (!_isFull)
                {
                    int z = Random.Range(-5, 45);
                    if (z == -5 || z == 5 || z == 15 || z == 25 || z == 35 || z == 45)
                    {
                        if (_spawnZPos.Count < _carNum && !_spawnZPos.Contains(z))
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
                    if (_xpos == -10 || _xpos == -5)
                    {
                        //Debug.Log(_xpos);
                        GameObject _car = _pools[_carSelect].GetObject(new Vector3(_xpos, 0, DataHandle.Instance._mapCount * 50 + _spawnZPos[i]), Quaternion.Euler(0, 0, 0));
                        _spawnedObject.Add(_car);
                    }
                    else if (_xpos == 0 || _xpos == 5)
                    {
                        GameObject _car2 = _pools[_carSelect].GetObject(new Vector3(_xpos, 0, DataHandle.Instance._mapCount * 50 + _spawnZPos[i]), Quaternion.Euler(0, 180, 0));
                        _spawnedObject.Add(_car2);
                    }
                }
            }


        }
        void Update()
        {
            if (_spawnedObject.Count > 1)
            {

                if (_spawnedObject[0].transform.position.z < _car.transform.position.z - 20f)
                {
                    Recycle(_spawnedObject[0]);
                    Debug.Log(0);
                    _spawnedObject.Remove(_spawnedObject[0]);
                }
                else if (_spawnedObject[1].transform.position.z < _car.transform.position.z - 20f)
                {
                    Recycle(_spawnedObject[1]);
                    Debug.Log(1);
                    _spawnedObject.Remove(_spawnedObject[1]);
                }

            }
        }

        public void Recycle(GameObject gameObject)
        {
            if (gameObject.layer == 8)
            {
                _pools[1].Release(gameObject);
            }
            else if (gameObject.layer == 6)
            {
                _pools[0].Release(gameObject);
            }
            else if (gameObject.layer == 7)
            {
                _pools[2].Release(gameObject);
            }
        }
    }
}