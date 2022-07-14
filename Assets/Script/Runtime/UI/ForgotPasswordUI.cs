using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace SuperUltra.Container
{

    public class ForgotPasswordUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField _email;
        [SerializeField] LoginManager _loginManager;

        public void UpdateUsername(string name)
        {
            _loginManager.UpdateUsername(name);
        }

        public void Back()
        {
            _loginManager.ToLoginSelection();
        }

        public void OnSubmit()
        {
            Debug.Log("Reset password");
        }
    }
    
}