using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scroller : MonoBehaviour
{
    [SerializeField]
    GameObject[] _tracks;
    [SerializeField]
    Transform _player;

    [SerializeField]
    Transform _endPt, _roadSpawn;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            Debug.Log("Check Point");
            RandomTrack();

        }
    }


    public void RandomTrack()
    {
        var i = Random.Range(0, _tracks.Length);
        Debug.Log("Random Index: " + i);
        RoadSpawn(i);
    }
    private void RoadSpawn(int index)
    {
        Debug.Log(_roadSpawn.position);
        GameObject road = Instantiate(_tracks[index], _roadSpawn);
        Debug.Log("Spawned");
        _roadSpawn = road.transform.GetChild(1).gameObject.transform;


        

    }



}
