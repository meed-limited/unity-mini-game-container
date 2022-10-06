using System;
using System.Collections.Generic;

namespace SuperUltra.Container
{
    public class VolumeSetting
    {
        public bool isMusicOn;
        public bool isEffectOn;   
    }

    public class NFTItem
    {
        public enum ItemType { 
            Cosmetic, 
            Utilities 
        }
        public ItemType type;
        public int id;
        public bool isActive;
        public string name;
        public string description;
        public string texture2DUrl;
        public string attribute;
    }

    public static class ContainerInterface
    {

        public static event Action OnReturnMenu;
        public static event Action OnPauseMenuShow;
        public static event Action OnPauseMenuHide;
        public static event Action OnPlayAgain;
        public static event Action OnClickHowToPlay;
        public static event Action OnGameOver;
        public static event Action<float> OnSetScore;
        public static event Func<VolumeSetting> OnGetVolumeSetting;
        public static event Action<Action<bool>> OnRequestRewardedAds;
        /// <summary> When user finish watching a rewarded ads </summary>
        public static event Action<bool> OnMusicVolumeChange;
        public static event Action<bool> OnEffectVolumeChange;
        public static event Func<NFTItem[]> OnGetNFTItemList;

        public static void EffectVolumeChange(bool isOn) => OnEffectVolumeChange?.Invoke(isOn);
        public static void MusicVolumeChange(bool isOn) => OnMusicVolumeChange?.Invoke(isOn);
        /// <summary> Return to Main menu from the game, unloading current game scene </summary>
        public static void ReturnToMenu() => OnReturnMenu?.Invoke();
        /// <summary> Pause the game and show pause menu during game play </summary>
        public static void Pause() => OnPauseMenuShow?.Invoke();
        /// <summary> Resume game play from the Pause menu </summary>
        public static void Resume() => OnPauseMenuHide?.Invoke();
        /// <summary> Play the game again after game over </summary>
        public static void PlayAgain() => OnPlayAgain?.Invoke();
        /// <summary> Show How to play panel </summary>
        public static void HowToPlay() => OnClickHowToPlay?.Invoke();
        /// <summary> Player fail to complete game </summary>
        public static void GameOver() => OnGameOver?.Invoke();
        public static void SetScore(float score) => OnSetScore?.Invoke(score);
        public static void RequestRewardedAds(Action<bool> callback) => OnRequestRewardedAds?.Invoke(callback);
        public static VolumeSetting GetVolumeSetting() => OnGetVolumeSetting?.Invoke();
        public static NFTItem[] GetNFTItemList() => OnGetNFTItemList?.Invoke();

    }

}