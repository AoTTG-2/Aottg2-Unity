using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;

    public bool DontDestroyToggle = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (DontDestroyToggle)
            DontDestroyOnLoad(gameObject);
    }
}