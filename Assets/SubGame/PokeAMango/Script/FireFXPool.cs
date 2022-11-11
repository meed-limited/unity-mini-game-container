using UnityEngine;
using QFSW.MOP2;

namespace SuperUltra.JungleDrum
{
    public class FireFXPool : MonoBehaviour
    {
        [SerializeField] ObjectPool _triggerPool = null;

        private void Start()
        {
            _triggerPool.Initialize();
            _triggerPool.ObjectParent.parent = transform;
        }

        public void Spawn()
        {
            _triggerPool.GetObject(transform.position);
        }
    }
}