using UnityEngine;
using Unity.Mathematics;

public class GameDataManager : MonoBehaviour
{
    // Allow DOTS to access the Data Manager.
    public static GameDataManager singleton;

    // Player prefab.
    public GameObject playerPrefab;
    public GameObject missilePrefab;

    // Player output
    public quaternion playerRotation { get; set; }

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

        playerRotation = quaternion.identity;
    }

    private void Update()
    {
        print(playerRotation);
    }
}
