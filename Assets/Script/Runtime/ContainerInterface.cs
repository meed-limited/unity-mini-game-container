using System;

namespace SuperUltra.Container
{

    public static class ContainerInterface
    {
        public static bool isEffectOn { get; private set; } = false;
        public static bool isMusicOn { get; private set; } = false;
        public static event Action OnReturnMenu;
        public static event Action OnShowMenu;
        public static event Action OnHideMenu;

        public static void ReturnToMenu()
        {
            OnReturnMenu?.Invoke();
        }

        public static void ShowPauseMenu()
        {
            OnShowMenu?.Invoke();
        }

        public static void HidePauseMenu()
        {
            OnHideMenu?.Invoke();
        }

    }

}