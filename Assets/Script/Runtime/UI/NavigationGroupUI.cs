using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperUltra.Container
{
    public class NavigationGroupUI : MonoBehaviour
    {
        NavigationButtonUI _prevActiveButton;

        public void Enable(NavigationButtonUI button)
        {
            Debug.Log(_prevActiveButton == null);
            if (_prevActiveButton == button)
            {
                return;
            }

            if (_prevActiveButton)
            {
                Debug.Log(_prevActiveButton.gameObject.name);
                _prevActiveButton.Disable();
            }

            _prevActiveButton = button;
        }

    }

}
