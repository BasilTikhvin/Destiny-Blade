using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T: MonoBehaviour
{
    [Header("Singleton")]
    [SerializeField] private bool dontDestroyOnLoad;

    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this as T;

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
