using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class GameDataManager : MonoBehaviour
{
    // Allow DOTS to access the Data Manager.
    public static GameDataManager singleton;

    // Asteroids Array
    public Entity[] asteroids;

    //MainCamera
    public Transform mainCamera;

    // Player Positon
    // Main Camera is feeding this information.
    public float3 playerPosition { get; set; }

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
