using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuperUltra.Container
{

    public class SceneLoader : MonoBehaviour
    {

        public static void ToMenu()
        {
            SceneManager.LoadScene(SceneConst.MainMenuSceneIndex);
        }

        public static void ToLogin()
        {
            SceneManager.LoadScene(SceneConst.LoginSceneIndex);
        }

        public static void ToProfile()
        {
            SceneManager.LoadScene(SceneConst.ProfileSceneIndex);
        }
        

    }

}