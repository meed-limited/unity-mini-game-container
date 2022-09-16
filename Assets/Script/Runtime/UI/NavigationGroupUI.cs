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
            if (_prevActiveButton == button)
            {
                return;
            }

            if (_prevActiveButton)
            {
                _prevActiveButton.Disable();
            }

            _prevActiveButton = button;
        }

    }

}
