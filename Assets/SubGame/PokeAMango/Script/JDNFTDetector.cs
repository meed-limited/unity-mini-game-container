using System.Collections;
using System.Collections.Generic;
using SuperUltra.Container;
using UnityEngine;

public class JDNFTDetector : MonoBehaviour
{

    NFTItem[] nftList;
    public GameObject[] _player;
    int _skinId;
    public Material outline;

    private void Awake()
    {
        ApplySkinChange(0);
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
                    case 6:
                        _skinId = 6; break;
                    case 5:
                        _skinId = 4; break;
                    case 4:
                        _skinId = 3; break;
                    case 3:
                        _skinId = 2; break;
                    case 2:
                        _skinId = 5; break;
                    case 1:
                        _skinId = 1; break;
                    default:
                        _skinId = 0; break;
                }
                ApplySkinChange(_skinId);
            }
        }
    }

    private void ApplySkinChange(int i)
    {
        if(i == 0 || i ==2)
        {
            outline.SetFloat("_Thickness", 0.02f);
        }
        else
        {
            outline.SetFloat("_Thickness", 1.3f);
        }
        _player[i].SetActive(true);
    }
}
