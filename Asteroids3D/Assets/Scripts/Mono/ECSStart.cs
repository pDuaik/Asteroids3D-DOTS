using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSStart : MonoBehaviour
{
    // GameObjects to initialize the game
    public GameObject characterTracker;
    public GameObject asteroidPrefab;
    public GameObject playerPrefab;

    // Canvas variables
    public int canvasHalfSize = 8192;
    public int numberOfAsteroids = 1600;
    public int asteroidRadius = 256;
    public float asteroidRotationSpeedMin = 0.1f;
    public float asteroidRotationSpeedMax = 1;
    public float asteroidSpeedMin = 5;
    public float asteroidSpeedMax = 10;

    private void Start()
    {
        // Initialize manager using world default.
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        // Convert prefabs into entity.
        var asteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);
        var playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);

        // Instantiate
        InstantiatePlayer(manager, playerEntity);
        PopulateAsteroids(manager,
                          asteroidEntity,
                          canvasHalfSize,
                          numberOfAsteroids, 
                          asteroidRadius,
                          asteroidRotationSpeedMin,
                          asteroidRotationSpeedMax,
                          asteroidSpeedMin,
                          asteroidSpeedMax);
    }

    private void InstantiatePlayer(EntityManager manager, Entity playerEntity)
    {
        // Instantiate Player.
        Entity playerInstance = manager.Instantiate(playerEntity);
        // Set Camera tracker
        characterTracker.GetComponent<CameraMovement>().SetReceivedEntity(playerInstance);

        // Give a name for debugging
#if UNITY_EDITOR
        manager.SetName(playerInstance, "Player");
#endif
    }

    private static void PopulateAsteroids(EntityManager manager,
                                          Entity asteroidEntity,
                                          int canvasHalfSize, 
                                          int numberOfAsteroids,
                                          int asteroidRadius,
                                          float asteroidRotationSpeedMin,
                                          float asteroidRotationSpeedMax,
                                          float asteroidSpeedMin,
                                          float asteroidSpeedMax)
    {
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            // Instantiate Entity.
            Entity asteroidInstance = manager.Instantiate(asteroidEntity);

            // Position
            float3 randomPosition = float3.zero;
            // Prevent asteroid to awake overlaping player.
            while (math.distancesq(randomPosition, float3.zero) < math.pow(60, 2))
            {
                randomPosition = new float3(UnityEngine.Random.Range(-canvasHalfSize, canvasHalfSize),
                                               UnityEngine.Random.Range(-canvasHalfSize, canvasHalfSize),
                                               UnityEngine.Random.Range(-canvasHalfSize, canvasHalfSize)
                                               );
            }
            manager.SetComponentData(asteroidInstance, new Translation { Value = randomPosition });

            // Rotation
            quaternion randomRotation = quaternion.Euler(UnityEngine.Random.Range(0.0f, 360.0f),
                                                         UnityEngine.Random.Range(0.0f, 360.0f),
                                                         UnityEngine.Random.Range(0.0f, 360.0f)
                                                         );
            manager.SetComponentData(asteroidInstance, new Rotation { Value = randomRotation });

            // Asteroid Data
            manager.SetComponentData(asteroidInstance, new AsteroidData
            {
                asteroidAxisRotation = UnityEngine.Random.onUnitSphere,
                asteroidSpeedRotation = UnityEngine.Random.Range(asteroidRotationSpeedMin,
                                                                 asteroidRotationSpeedMax),
                asteroidDirection = UnityEngine.Random.onUnitSphere,
                asteroidSpeed = UnityEngine.Random.Range(asteroidSpeedMin,
                                                         asteroidSpeedMax),
                isActive = i < numberOfAsteroids ? true : false
            });

            // Scale
            manager.AddComponentData(asteroidInstance, new NonUniformScale { Value = new float3(1, 1, 1) * asteroidRadius });
        }
    }
}
