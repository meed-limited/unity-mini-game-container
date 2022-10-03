using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class EnemyMovement : MonoBehaviour
    {

        private Rigidbody _rb;
        private CarGenarator _lv;
        [SerializeField]
        private GameObject _exploFX;
        private GameObject _explo;

        private void OnEnable()
        {
            gameObject.tag = "Enemy";
        }

        private void Start()
        {
            _lv = GameObject.FindGameObjectWithTag("GameController").GetComponent<CarGenarator>();
            _rb = gameObject.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (gameObject.transform.position.x == -10 || gameObject.transform.position.x == -5)
                _rb.velocity = new Vector3(0, 0, 20);
            else if (gameObject.transform.position.x == 0 || gameObject.transform.position.x == 5)
                _rb.velocity = new Vector3(0, 0, -20);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(Explosion());
                Destroy(gameObject, 3f);
                _lv._spawnedObject.Remove(gameObject);

            }
        }
        IEnumerator Explosion()
        {
            yield return new WaitForSeconds(1.5f);
            _explo = Instantiate(_exploFX, gameObject.transform) as GameObject;
            _explo.transform.position = transform.position;

        }

    }


}