using UnityEngine;
using UnityEngine.SceneManagement;

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