using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class TriggerExit : MonoBehaviour
    {
        public float delay = 1f;

        public delegate void ExitAction();
        public static event ExitAction OnChunkExited;

        private bool exited = false;
        LevelPooker _lvPooker;
        GameManager _gm;

        private void Awake()
        {
            _lvPooker = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelPooker>();
            _gm = _lvPooker.gameObject.GetComponent<GameManager>();
        }


        private void OnDisable()
        {
            exited = false;
        }
        private void OnTriggerExit(Collider other)
        {
            CarTag carTag = other.GetComponent<CarTag>();
            if (carTag != null)
            {
                if (!exited)
                {
                    //Debug.Log("Exit!");
                    exited = true;
                    OnChunkExited();
                    StartCoroutine(WaitAndDeactivate());
                }


            }
        }



        IEnumerator WaitAndDeactivate()
        {
            yield return new WaitForSeconds(delay);
            if(_gm.isEnd == false)
                _lvPooker.Recycle(transform.parent.gameObject);
            //transform.parent.gameObject.SetActive(false);
            //Debug.Log("InActive");

        }



    }
}