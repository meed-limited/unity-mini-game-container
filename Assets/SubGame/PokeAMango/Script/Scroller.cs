using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SuperUltra.JungleDrum;
namespace SuperUltra.JungleDrum
{
    public class Scroller : MonoBehaviour
    {
        [SerializeField]
        GameObject _mountin, _cloud, _farBack, _midBack, _closeBack, _road, _frontBack, _superBack, _ultraBack, _dirt, _wood;
        [SerializeField]
        GameObject _mountin1, _mountin2, _cloud1, _cloud2, _far1, _far2, _mid1, _mid2, _close1, _close2, _road1, _road2, _front1, _front2, _super1, _super2, _ultra1, _ultra2, _dirt1, _dirt2, _wood1, _wood2;
        [SerializeField]
        private float _mountinSpeed = 0.6f;
        [SerializeField]
        private float _cloudSpeed = 1.1f;
        [SerializeField]
        private float _farSpeed = 0.8f;
        [SerializeField]
        private float _midSpeed = 1f;
        [SerializeField]
        private float _closeSpeed = 1.2f;
        [SerializeField]
        private float _roadSpeed = 1.4f;
        [SerializeField]
        private float _frontSpeed = 1.6f;
        [SerializeField]
        private float _superSpeed = 1.8f;
        [SerializeField]
        private float _ultraSpeed = 2f;
        [SerializeField]
        private float _dirtSpeed = 1.4f;
        [SerializeField]
        private float _woodSpeed = 1.8f;


        [SerializeField]
        Transform _endPt, _mountSpawn, _farSpawn, _closeSpawn, _superSpawn, _dirtEnd, _dirtSpawn;

        private void Update()
        {
            MountSpawn();
            FarSpawn();
            CloudSpawn();
            CloseSpawn();
            MidSpawn();
            RoadSpawn();
            FrontSpawn();
            SuperSpawn();
            UltraSpawn();
            DirtSpawn();
            WoodSpawn();

        }

        private void MountSpawn()
        {
            if (_mountin1.transform.position.x <= _endPt.transform.position.x)
            {
                _mountin1.transform.position = new Vector3(_mountSpawn.transform.position.x, _mountin1.transform.position.y, _mountin1.transform.position.z);
            }
            else if (_mountin2.transform.position.x <= _endPt.transform.position.x)
            {
                _mountin2.transform.position = new Vector3(_mountSpawn.transform.position.x, _mountin1.transform.position.y, _mountin1.transform.position.z);
            }


        }

        private void FarSpawn()
        {
            if (_far1.transform.position.x <= _endPt.transform.position.x)
            {
                _far1.transform.position = new Vector3(_farSpawn.transform.position.x, _far1.transform.position.y, _far1.transform.position.z);
            }
            else if (_far2.transform.position.x <= _endPt.transform.position.x)
            {
                _far2.transform.position = new Vector3(_farSpawn.transform.position.x, _far1.transform.position.y, _far1.transform.position.z);
            }
        }
        private void CloudSpawn()
        {
            if (_cloud1.transform.position.x <= _endPt.transform.position.x)
            {
                _cloud1.transform.position = new Vector3(_closeSpawn.transform.position.x, _cloud1.transform.position.y, _cloud1.transform.position.z);
            }
            else if (_cloud2.transform.position.x <= _endPt.transform.position.x)
            {
                _cloud2.transform.position = new Vector3(_closeSpawn.transform.position.x, _cloud1.transform.position.y, _cloud1.transform.position.z);
            }
        }
        private void CloseSpawn()
        {
            if (_close1.transform.position.x <= _endPt.transform.position.x)
            {
                _close1.transform.position = new Vector3(_closeSpawn.transform.position.x, _close1.transform.position.y, _close1.transform.position.z);
            }
            else if (_close2.transform.position.x <= _endPt.transform.position.x)
            {
                _close2.transform.position = new Vector3(_closeSpawn.transform.position.x, _close1.transform.position.y, _close1.transform.position.z);
            }
        }
        private void MidSpawn()
        {
            if (_mid1.transform.position.x <= _endPt.transform.position.x)
            {
                _mid1.transform.position = new Vector3(_farSpawn.transform.position.x, _mid1.transform.position.y, _mid1.transform.position.z);
            }
            else if (_mid2.transform.position.x <= _endPt.transform.position.x)
            {
                _mid2.transform.position = new Vector3(_farSpawn.transform.position.x, _mid1.transform.position.y, _mid1.transform.position.z);
            }
        }
        private void RoadSpawn()
        {
            if (_road1.transform.position.x <= _endPt.transform.position.x)
            {
                _road1.transform.position = new Vector3(_closeSpawn.transform.position.x, _road1.transform.position.y, _road1.transform.position.z);
            }
            else if (_road2.transform.position.x <= _endPt.transform.position.x)
            {
                _road2.transform.position = new Vector3(_closeSpawn.transform.position.x, _road1.transform.position.y, _road1.transform.position.z);
            }
        }
        private void FrontSpawn()
        {
            if (_front1.transform.position.x <= _endPt.transform.position.x)
            {
                _front1.transform.position = new Vector3(_closeSpawn.transform.position.x, _front1.transform.position.y, _front1.transform.position.z);
            }
            else if (_front2.transform.position.x <= _endPt.transform.position.x)
            {
                _front2.transform.position = new Vector3(_closeSpawn.transform.position.x, _front1.transform.position.y, _front1.transform.position.z);
            }
        }
        private void SuperSpawn()
        {
            if (_super1.transform.position.x <= _endPt.transform.position.x)
            {
                _super1.transform.position = new Vector3(_superSpawn.transform.position.x, _super1.transform.position.y, _super1.transform.position.z);
            }
            else if (_super2.transform.position.x <= _endPt.transform.position.x)
            {
                _super2.transform.position = new Vector3(_superSpawn.transform.position.x, _super1.transform.position.y, _super1.transform.position.z);
            }
        }
        private void UltraSpawn()
        {
            if (_ultra1.transform.position.x <= _endPt.transform.position.x)
            {
                _ultra1.transform.position = new Vector3(_closeSpawn.transform.position.x, _ultra1.transform.position.y, _ultra1.transform.position.z);
            }
            else if (_ultra2.transform.position.x <= _endPt.transform.position.x)
            {
                _ultra2.transform.position = new Vector3(_closeSpawn.transform.position.x, _ultra1.transform.position.y, _ultra1.transform.position.z);
            }
        }

        private void DirtSpawn()
        {
            if (_dirt1.transform.position.x <= _dirtEnd.transform.position.x)
            {
                _dirt1.transform.position = new Vector3(_dirtSpawn.transform.position.x, _dirt1.transform.position.y, _dirt1.transform.position.z);
            }
            else if (_dirt2.transform.position.x <= _dirtEnd.transform.position.x)
            {
                _dirt2.transform.position = new Vector3(_dirtSpawn.transform.position.x, _dirt1.transform.position.y, _dirt1.transform.position.z);
            }
        }
        private void WoodSpawn()
        {
            if (_wood1.transform.position.x <= _endPt.transform.position.x)
            {
                _wood1.transform.position = new Vector3(_mountSpawn.transform.position.x, _wood1.transform.position.y, _wood1.transform.position.z);
            }
            else if (_wood2.transform.position.x <= _endPt.transform.position.x)
            {
                _wood2.transform.position = new Vector3(_mountSpawn.transform.position.x, _wood1.transform.position.y, _wood1.transform.position.z);
            }
        }

        public void MountMove()
        {

            _mountin.transform.DOMoveX(_mountin.transform.position.x - _mountinSpeed, 0.3f, false);

            _farBack.transform.DOMoveX(_farBack.transform.position.x - _farSpeed, 0.3f, false);

            _cloud.transform.DOMoveX(_cloud.transform.position.x - _cloudSpeed, 0.3f, false);

            _midBack.transform.DOMoveX(_midBack.transform.position.x - _midSpeed, 0.3f, false);

            _closeBack.transform.DOMoveX(_closeBack.transform.position.x - _closeSpeed, 0.3f, false);

            _road.transform.DOMoveX(_road.transform.position.x - _roadSpeed, 0.3f, false);

            _frontBack.transform.DOMoveX(_frontBack.transform.position.x - _frontSpeed, 0.3f, false);

            _superBack.transform.DOMoveX(_superBack.transform.position.x - _superSpeed, 0.3f, false);

            _ultraBack.transform.DOMoveX(_ultraBack.transform.position.x - _ultraSpeed, 0.3f, false);

            _dirt.transform.DOMoveX(_dirt.transform.position.x - _dirtSpeed, 0.3f, false);

            _wood.transform.DOMoveX(_wood.transform.position.x - _woodSpeed, 0.3f, false);

        }



        public IEnumerator MoveBack()
        {
            yield return new WaitForSeconds(0.1f);
            _mountin.transform.DOMoveX(_mountin.transform.position.x + _mountinSpeed * 4, 0.3f, false);

            _farBack.transform.DOMoveX(_farBack.transform.position.x + _farSpeed * 4, 0.3f, false);

            _cloud.transform.DOMoveX(_cloud.transform.position.x + _cloudSpeed * 4, 0.3f, false);

            _midBack.transform.DOMoveX(_midBack.transform.position.x + _midSpeed * 4, 0.3f, false);

            _closeBack.transform.DOMoveX(_closeBack.transform.position.x + _closeSpeed * 4, 0.3f, false);

            _road.transform.DOMoveX(_road.transform.position.x + _roadSpeed * 4, 0.3f, false);

            _frontBack.transform.DOMoveX(_frontBack.transform.position.x + _frontSpeed * 4, 0.3f, false);

            _superBack.transform.DOMoveX(_superBack.transform.position.x + _superSpeed * 4, 0.3f, false);

            _ultraBack.transform.DOMoveX(_ultraBack.transform.position.x + _ultraSpeed * 4, 0.3f, false);

            _dirt.transform.DOMoveX(_dirt.transform.position.x + _dirtSpeed, 0.3f, false);

            _wood.transform.DOMoveX(_wood.transform.position.x + _woodSpeed, 0.3f, false);
        }
        public void MoveBackLong()
        {

            _mountin.transform.DOMoveX(_mountin.transform.position.x - _mountinSpeed * 4, 1.5f, false);

            _farBack.transform.DOMoveX(_farBack.transform.position.x - _farSpeed * 4, 1.5f, false);

            _cloud.transform.DOMoveX(_cloud.transform.position.x - _cloudSpeed * 4, 1.5f, false);

            _midBack.transform.DOMoveX(_midBack.transform.position.x - _midSpeed * 4, 1.5f, false);

            _closeBack.transform.DOMoveX(_closeBack.transform.position.x - _closeSpeed * 4, 1.5f, false);

            _road.transform.DOMoveX(_road.transform.position.x - _roadSpeed * 4, 1.5f, false);

            _frontBack.transform.DOMoveX(_frontBack.transform.position.x - _frontSpeed * 4, 1.5f, false);

            _superBack.transform.DOMoveX(_superBack.transform.position.x - _superSpeed * 4, 1.5f, false);

            _ultraBack.transform.DOMoveX(_ultraBack.transform.position.x - _ultraSpeed * 4, 1.5f, false);

            _dirt.transform.DOMoveX(_dirt.transform.position.x - _dirtSpeed, 1.5f, false);

            _wood.transform.DOMoveX(_wood.transform.position.x - _woodSpeed, 1.5f, false);
        }
    }
}