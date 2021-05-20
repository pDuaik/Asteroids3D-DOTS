using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSManager : MonoBehaviour
{
    // Prefabs to convert into Entities.
    public GameObject playerPrefab;
    public GameObject missilePrefab;
    public GameObject asteroidPrefab;

    // Scenario variables.
    public int numberOfAsteroids = 1000;
    public int areaSize = 512;

    private void Start()
    {
        // Initialize manager using world default.
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        // Convert prefabs into entity.
        var asteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);

        // Populate the world with Asteroids.
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            Entity asteroidInstance = manager.Instantiate(asteroidEntity);
            float3 randomPosition = new float3(UnityEngine.Random.Range(-areaSize, areaSize),
                                               UnityEngine.Random.Range(-areaSize, areaSize),
                                               UnityEngine.Random.Range(-areaSize, areaSize)
                                               );
            manager.SetComponentData(asteroidInstance, new Translation { Value = randomPosition });
            manager.SetComponentData(asteroidInstance, new Rotation { Value = quaternion.identity });
        }
    }
}
