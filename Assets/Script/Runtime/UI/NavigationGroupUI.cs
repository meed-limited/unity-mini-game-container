using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperUltra.Container
{
    public class NavigationGroupUI : MonoBehaviour
    {
        [SerializeField] NavigationButtonUI _gameNavButton;
        [SerializeField] NavigationButtonUI _leaderBoardButton;
        [SerializeField] NavigationButtonUI _walletNavButton;
        [SerializeField] NavigationButtonUI _seasonPassNavButton;
        NavigationButtonUI _prevActiveButton;
        Dictionary<Page, NavigationButtonUI> pageMap = new Dictionary<Page, NavigationButtonUI>();

        void Start()
        {
            pageMap.Add(Page.GameList, _gameNavButton);
            pageMap.Add(Page.Leaderboard, _leaderBoardButton);
            pageMap.Add(Page.Wallet, _walletNavButton);
            pageMap.Add(Page.SeasonPass, _seasonPassNavButton);
        }

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

        public void Enable(Page page)
        {
            if (pageMap.TryGetValue(page, out NavigationButtonUI buttonUI))
            {
                buttonUI.Enable();
                Enable(buttonUI);
            }
        }

    }

}
