using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SuperUltra.Container
{
    public class LoadingUI : MonoBehaviour
    {
        public static LoadingUI instance
        {
            get; private set;
        }
        [SerializeField] Canvas _loadingCanvas;
        [SerializeField] Image _loadingIcon;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            ToggleLoadingCanvas(false);
        }

        void Start()
        {
            AnimateIcon();
        }

        void AnimateIcon()
        {
            if(_loadingIcon == null) return;
            _loadingIcon.rectTransform.DORotate(
                new Vector3(0f, 180f, 180),
                0.4f
            ).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }

        void Reset()
        {
            _loadingCanvas = GetComponent<Canvas>();
        }

        public static void Show()
        {
            if (instance == null) return;
            instance.ToggleLoadingCanvas(true);
        }

        public static void Hide()
        {
            if (instance == null) return;
            instance.ToggleLoadingCanvas(false);
        }

        void ToggleLoadingCanvas(bool isOn)
        { 
            if(_loadingCanvas == null) return;
            _loadingCanvas.gameObject.SetActive(isOn);
        }
        
    }

}
