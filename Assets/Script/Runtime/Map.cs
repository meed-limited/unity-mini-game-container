using System;
using System.Collections.Generic;
using UnityEngine;

public class Map<T1 ,T2> : ScriptableObject
{
    [Serializable]
    public class KeyPair
    {
        public T1 key;
        public T2 value;
    }
    
    [SerializeField]
    public List<KeyPair> list;

}
