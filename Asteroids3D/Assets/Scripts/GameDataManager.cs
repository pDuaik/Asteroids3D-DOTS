using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // Allow DOTS to access the Data Manager.
    public static GameDataManager singleton;

    // Player prefab.
    public GameObject playerPrefab;

    private void Awake()
    {
        // Make sure there is only one instance of the class.
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
        }
    }
}
