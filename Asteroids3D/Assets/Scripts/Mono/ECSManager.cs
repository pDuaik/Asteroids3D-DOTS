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
        var missileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(GameDataManager.singleton.missilePrefab, settings);

        // Transform prefabs into entities.
        GameDataManager.singleton.asteroids = new Entity[GameDataManager.singleton.numberOfAsteroids];
        GameDataManager.singleton.missiles = new Entity[GameDataManager.singleton.numberOfMissiles];

        PopulateAsteroids(manager, asteroidEntity);

        PopulateMissiles(manager, missileEntity);
    }

    /// <summary>
    /// Populate the world with asteroids.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="asteroidEntity"></param>
    private static void PopulateAsteroids(EntityManager manager, Entity asteroidEntity)
    {
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

            // Asteroid Data
            manager.SetComponentData(asteroidInstance, new AsteroidData
            {
                asteroidAxisRotation = UnityEngine.Random.onUnitSphere,
                asteroidSpeedRotation = UnityEngine.Random.Range(GameDataManager.singleton.asteroidRandomRotationSpeedMinMax.x,
                                                                 GameDataManager.singleton.asteroidRandomRotationSpeedMinMax.y),
                asteroidDirection = UnityEngine.Random.onUnitSphere,
                asteroidSpeed = UnityEngine.Random.Range(GameDataManager.singleton.asteroidRandomSpeedMinMax.x,
                                                         GameDataManager.singleton.asteroidRandomSpeedMinMax.y),
            });

            // Scale
            manager.AddComponentData(asteroidInstance, new NonUniformScale { Value = new float3(1, 1, 1) * GameDataManager.singleton.asteroidSize });

            // Hyperspace Jump
            manager.AddComponentData(asteroidInstance, new HyperspaceJumpData { isPlayer = false });

            // Populate entity array;
            GameDataManager.singleton.asteroids[i] = asteroidInstance;
        }
    }

    /// <summary>
    /// Populate the world with missiles and put them far away from action.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="missileEntity"></param>
    private static void PopulateMissiles(EntityManager manager, Entity missileEntity)
    {
        for (int i = 0; i < GameDataManager.singleton.numberOfMissiles; i++)
        {
            // Instantiate Entity.
            Entity missileInstance = manager.Instantiate(missileEntity);

            // Position
            manager.SetComponentData(missileInstance, new Translation { Value = new float3(0, GameDataManager.singleton.canvasSize * 100, 0) });

            // Rotation
            manager.SetComponentData(missileInstance, new Rotation { Value = quaternion.identity });

            // Scale
            manager.AddComponentData(missileInstance, new NonUniformScale { Value = new float3(1, 1, 1) * GameDataManager.singleton.missileSize });

            // Missile Data
            manager.SetComponentData(missileInstance, new MissileData { lifeSpan = GameDataManager.singleton.missileLifeSpan });

            // Populate entity array;
            GameDataManager.singleton.missiles[i] = missileInstance;
        }
    }
}