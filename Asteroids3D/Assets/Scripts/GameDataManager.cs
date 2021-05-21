using UnityEngine;
using Unity.Mathematics;

public class GameDataManager : MonoBehaviour
{
    // Allow DOTS to access the Data Manager.
    public static GameDataManager singleton;

    //MainCamera
    public Transform mainCamera;

    // Prefabs to convert into Entities.
    public GameObject missilePrefab;
    public GameObject asteroidPrefab;

    // Canvas variables
    public float canvasSize = 512;
    public int numberOfAsteroids = 1000;
    public float2 asteroidRotationSpeedRange = float2.zero;
    public float2 asteroidSpeed = float2.zero;
    public float3 asteroidSize = float3.zero;

    private void Awake()
    {
        // Make sure there is only one instance of the class.
        if (singleton != null && singleton != this)
            Destroy(gameObject);
        else
            singleton = this;
    }
}
