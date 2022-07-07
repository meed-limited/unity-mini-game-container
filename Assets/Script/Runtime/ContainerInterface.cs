using System;

namespace SuperUltra.Container
{

    public static class ContainerInterface
    {

        public static event Action OnReturnMenu;
        public static void ReturnToMenu()
        {
            OnReturnMenu?.Invoke();
        }

    }

}