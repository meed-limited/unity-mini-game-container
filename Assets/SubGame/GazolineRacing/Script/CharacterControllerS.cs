using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using QFSW.MOP2;
using SuperUltra.GazolineRacing;
using SuperUltra.Container;
using System;

namespace SuperUltra.GazolineRacing
{

    public class CharacterControllerS : MonoBehaviour
    {
        [SerializeField]
        GameObject _levelGenerator;
        [SerializeField]
        Timer _timer;
        [SerializeField]
        private float _forwardSpeed;
        private Rigidbody _rb;

        private int _desiredLane = 2; //0= left, 1 , 2, 3=right
        [SerializeField]
        private float _landeDistance = 5;
        [SerializeField]
        private ParticleSystem _bananaFx;
        [SerializeField]
        private Transform _bananaPos;
        [SerializeField]
        private GameObject _exploFx;
        bool _blocked;
        private ItemGenerator _itemGen;
        private GameManager _gm;
        private CarGenarator _cg;
        [SerializeField]
        AudioSource _coinSFX, _expolSFX;
        Animator _ani;
        private bool _moveBlocked;
        [SerializeField]
        GameObject _coins, _coinsPos;
        [SerializeField]
        GameObject _Firework;

        public delegate void CoinAction();
        public static event CoinAction OnCoinHit;

        private void OnEnable()
        {
            GetComponent<AudioSource>().enabled = false;
            _ani = GetComponent<Animator>();
            ContainerInterface.OnEffectVolumeChange += SFXToggle;
        }

        private void OnDisable()
        {
            ContainerInterface.OnEffectVolumeChange -= SFXToggle;
        }
        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _itemGen = _levelGenerator.GetComponent<ItemGenerator>();
            _gm = _levelGenerator.GetComponent<GameManager>();
            _cg = _levelGenerator.GetComponent<CarGenarator>();
            //_rb.velocity = transform.forward * Time.deltaTime*_forwardSpeed;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _blocked = false;
                }
            }
            //_rb.AddForce(transform.forward * Time.deltaTime * 0.1f);
            if (!_blocked)
                LaneChange();

            if(_gm.isEnd == false)
                CarMovement();


        }

        private void SFXToggle(bool isOn)
        {
            if (isOn)
            {
                _coinSFX.enabled = true;
                _expolSFX.enabled = true;
            }
            else
            {
                _coinSFX.enabled = false;
                _expolSFX.enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (_gm.isEnd == false )
            {
                if (_timer._timeRemaining > 45)
                {
                    _forwardSpeed = 30;
                }
                else if (_timer._timeRemaining > 30 && _timer._timeRemaining <= 45)
                {
                    _forwardSpeed = 45;
                }
                else if (_timer._timeRemaining > 15 && _timer._timeRemaining <= 30)
                {
                    _forwardSpeed = 60;
                }
                else if (_timer._timeRemaining <= 15)
                {
                    _forwardSpeed = 75;
                }
                _rb.velocity = new Vector3(0, 0, _forwardSpeed);
            }
            else
            {
                if (_forwardSpeed>0)
                    _forwardSpeed -= 0.4f;
                _rb.velocity = new Vector3(0, 0, _forwardSpeed);
            }
        }
        private void LaneChange()
        {

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {

                    Vector2 touchDeltaPosition = touch.deltaPosition;
                    if (touchDeltaPosition.x > 0)
                    {
                        // swipe right
                        Debug.Log("Swipe Right");
                        if (_desiredLane < 3)
                        {
                            _ani.SetTrigger("TurnRight");
                            _desiredLane++;
                            Debug.Log(_desiredLane);
                            _blocked = true;
                        }
                    }
                    else if (touchDeltaPosition.x < 0)
                    {
                        // swipe left
                        Debug.Log("Swipe Left");
                        if (_desiredLane > 0)
                        {
                            _ani.SetTrigger("TurnLeft");
                            _desiredLane--;
                            Debug.Log(_desiredLane);
                            _blocked = true;
                        }
                    }
                }
            }

        }


        private void CarMovement()
        {
            //Debug.Log(_desiredLane);
            //Vector3 targetPosition = transform.position.z * transform.forward;

            if (_desiredLane == 0)
            {
                //gameObject.transform.position = new Vector3(-10,1,transform.position.z);
                gameObject.transform.DOMoveX(-10, 0.5f);
            }
            else if (_desiredLane == 1)
            {
                //gameObject.transform.position += new Vector3(-5, 1, transform.position.z);
                gameObject.transform.DOMoveX(-5, 0.5f);
            }
            else if (_desiredLane == 2)
            {
                //gameObject.transform.position += new Vector3(0, 1, transform.position.z);
                gameObject.transform.DOMoveX(0, 0.5f);
            }
            else if (_desiredLane == 3)
            {
                //gameObject.transform.position += new Vector3(5, 1, transform.position.z);
                gameObject.transform.DOMoveX(5, 0.5f);
            }
            //Debug.Log(_desiredLane);
            //transform.position = targetPosition;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log("Enemy!");
                StartCoroutine(_gm.Noise(2.5f, 2.5f));
                StartCoroutine(PlayExplo());
                Instantiate(_Firework, _bananaPos);
                collision.gameObject.GetComponent<AudioSource>().enabled = true;
                Rigidbody _eRb = collision.gameObject.GetComponent<Rigidbody>();
                _eRb.constraints = RigidbodyConstraints.None;
                _eRb.AddForce(transform.up * 400);
                _gm.ReduceLife();
                collision.gameObject.tag = "No";

            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Banana"))
            {
                _coinSFX.PlayOneShot(_coinSFX.clip);
                
                var myNewSmoke = Instantiate(_coins, _coinsPos.transform);
                myNewSmoke.transform.parent = _coinsPos.transform;
                //ParticleSystem BananaFx = Instantiate(_bananaFx, transform.position, Quaternion.Euler(-70f, 0f, 0f));
                //BananaFx.transform.position = new Vector3(_bananaPos.position.x, _bananaPos.position.y, _bananaPos.position.z + 2f);
                _gm.GetScore(5);
                _itemGen.Recycle(other.gameObject);
            }

        }
        IEnumerator PlayExplo()
        {
            yield return new WaitForSeconds(1.5f);
            _expolSFX.PlayOneShot(_expolSFX.clip);
        }
    }


}