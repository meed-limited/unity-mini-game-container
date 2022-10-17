using System.Collections;
using System.Collections.Generic;
using SuperUltra.Container;
using UnityEngine;

namespace SuperUltra.GazolineRacing
{
    public class GRNFTDetector : MonoBehaviour
    {

        NFTItem[] nftList;
        public GameObject[] _player;
        int _skinId;


        private void Awake()
        {
            ApplySkinChange(5);
            //GetActiveNFT();
        }

        public void GetActiveNFT()
        {
            nftList = ContainerInterface.GetNFTItemList();
            for (int i = 0; i < nftList.Length; i++)
            {
                if (nftList[i].type == 0 && nftList[i].isActive)
                {

                    switch (nftList[i].id)
                    {
                        case 21:
                            _skinId = 0; break;
                        case 22:
                            _skinId = 1; break;
                        case 23:
                            _skinId = 2; break;
                        case 24:
                            _skinId = 3; break;
                        case 25:
                            _skinId = 4; break;
                        case 26:
                            _skinId = 5; break;
                        case 27:
                            _skinId = 6; break;
                        default:
                            Debug.Log("Wrong NTF id"); break;
                    }
                    ApplySkinChange(_skinId);
                }
            }
        }

        private void ApplySkinChange(int i)
        {

            _player[i].SetActive(true);
        }
    }
}