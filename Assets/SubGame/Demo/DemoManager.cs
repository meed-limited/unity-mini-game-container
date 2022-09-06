using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.Container;

namespace SuperUltra.Demo
{
    
    public class DemoManager : MonoBehaviour
    {
        
        public void GameOver()
        {
            ContainerInterface.GameOver();
        }

        public void Pause()
        {
            ContainerInterface.Pause();
        }

    }

}

