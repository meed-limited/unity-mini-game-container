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

    public T2 this[T1 key]
    {
        get
        {
            Map<string, string> map = new Map<string, string>();
            foreach (KeyPair item in list)
            {
                if (item.key.Equals(key))
                {
                    return item.value;
                }
            }
            return default(T2);
        }
    }

}
