using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static event System.Action InstanceChanged;
    public static T Instance
    {
        get;
        private set;
    }
    protected bool willSelfDestruct {
        get;
        private set;
    }

    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            willSelfDestruct = true;
            return;
        }

        Instance = this as T;
        InstanceChanged?.Invoke();
    }

    private void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }
}
