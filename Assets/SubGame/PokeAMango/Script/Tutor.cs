using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SuperUltra.JungleDrum;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using SuperUltra.Container;

namespace SuperUltra.JungleDrum
{
    public class Tutor : MonoBehaviour
    {
        [SerializeField]
        private GameObject _phase1;
        [SerializeField]
        private GameObject _phase2;
        [SerializeField]
        private GameObject _start;
        [SerializeField]
        private RectTransform _clicker;
        [SerializeField]
        private EffectControl _ev;
        [SerializeField]
        private Timer _timer;
        [SerializeField]
        private Vector3 _object;

        private void Start()
        {
            _object = gameObject.transform.position;
        }
        public void P1OnClick()
        {
            _phase1.SetActive(false);
            _phase2.SetActive(true);
            _clicker.position = new Vector3(11f, -1000f, 0f);
            Time.timeScale = 0;
        }

        public void P2OnClick()
        {
            //AsyncOperationHandle<SceneInstance> operationHandle = Addressables.LoadSceneAsync("Assets/Scenes/Tourment.unity");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            ContainerInterface.PlayAgain();
        }
    }
}