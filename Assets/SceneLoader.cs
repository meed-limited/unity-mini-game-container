using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace SuperUltra.Container
{

    public class SceneLoader : MonoBehaviour
    {

        public void ToMenu()
        {
            SceneManager.LoadScene(0);
        }

    }

}