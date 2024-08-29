using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    
    private static T _Instance;
    
    // Lock object for thread safety
    private static object lockObject = new object();

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                // This lock ensures that only one thread can enter this section at a time.
                lock (lockObject)
                {
                    _Instance = FindObjectOfType<T>();

                    if (_Instance == null)
                    {
                        GameObject newObject = new GameObject(typeof(T).Name);
                        _Instance = newObject.AddComponent<T>();
                    }
                }
            }

            return _Instance;
        }
    }
    
    protected virtual void Awake()
    {
        if (_Instance == null)
            _Instance = this as T;
        else
            Destroy(gameObject);
    }
    
    void OnDestroy()
    {
        if( _Instance == this )
            _Instance = null;
    }
}