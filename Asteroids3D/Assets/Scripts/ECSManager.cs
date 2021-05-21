using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSManager : MonoBehaviour
{
    private void Start()
    {
        // Initialize manager using world default.
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        // Convert prefabs into entity.
        var asteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(GameDataManager.singleton.asteroidPrefab, settings);

        // Populate the world with Asteroids.
        for (int i = 0; i < GameDataManager.singleton.numberOfAsteroids; i++)
        {
            // Instantiate Entity.
            Entity asteroidInstance = manager.Instantiate(asteroidEntity);

            // Position
            float canvasSize = GameDataManager.singleton.canvasSize;
            float3 randomPosition = new float3(UnityEngine.Random.Range(-canvasSize, canvasSize),
                                               UnityEngine.Random.Range(-canvasSize, canvasSize),
                                               UnityEngine.Random.Range(-canvasSize, canvasSize)
                                               );
            manager.SetComponentData(asteroidInstance, new Translation { Value = randomPosition });

            // Rotation
            quaternion randomRotation = quaternion.Euler(UnityEngine.Random.Range(0.0f, 360.0f),
                                                         UnityEngine.Random.Range(0.0f, 360.0f),
                                                         UnityEngine.Random.Range(0.0f, 360.0f)
                                                         );
            manager.SetComponentData(asteroidInstance, new Rotation { Value = randomRotation });
            manager.SetComponentData(asteroidInstance, new AsteroidData
            {
                asteroidAxisRotation = UnityEngine.Random.onUnitSphere,
                asteroidSpeedRotation = UnityEngine.Random.Range(GameDataManager.singleton.asteroidRotationSpeedRange.x,
                                                                 GameDataManager.singleton.asteroidRotationSpeedRange.y)
            }
            );
            manager.AddComponentData(asteroidInstance, new NonUniformScale { Value = GameDataManager.singleton.asteroidSize });
            manager.AddComponentData(asteroidInstance, new HyperspaceJumpData { isPlayer = false });
        }
    }
}
