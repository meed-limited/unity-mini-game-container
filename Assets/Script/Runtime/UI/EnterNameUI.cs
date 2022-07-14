using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace SuperUltra.Container
{

    public class EnterNameUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField _name;
        [SerializeField] Button _submit;
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
            if (_name.text.Length > 0)
            {
                PlayerPrefs.SetString("name", _name.text);
                SceneLoader.ToMenu();
            }
        }
    }
    
}