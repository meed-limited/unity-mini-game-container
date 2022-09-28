using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SuperUltra.Container
{
    [RequireComponent(typeof(RectTransform))]
    public class LoadingUI : MonoBehaviour
    {
        public static LoadingUI instance
        {
            get; private set;
        }
        [SerializeField] RectTransform _container;
        [SerializeField] Image _loadingIcon;
        [SerializeField] bool _isPersistance;

        void Awake()
        {
            if(_isPersistance)
            {
                CreateSingleton();
            }
            ToggleLoadingCanvas(false);
        }

        void CreateSingleton()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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
            _container = GetComponent<RectTransform>();
        }

        public void Show()
        {
            ToggleLoadingCanvas(true);
        }

        public void Hide()
        {
            ToggleLoadingCanvas(false);
        }

        public static void ShowInstance()
        {
            if (instance == null) return;
            instance.ToggleLoadingCanvas(true);
        }

        public static void HideInstance()
        {
            if (instance == null) return;
            instance.ToggleLoadingCanvas(false);
        }

        void ToggleLoadingCanvas(bool isOn)
        { 
            if(_container == null) return;
            _container.gameObject.SetActive(isOn);
        }
        
    }

}
