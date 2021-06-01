using Asteroids3D;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class AsteroidsStart : MonoBehaviour
{
    // Asteroid
    public GameObject asteroidPrefab;

    // Asteroids
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

        // Instantiate
        PopulateAsteroids(manager, asteroidEntity);

        Destroy(gameObject);
    }

    private void PopulateAsteroids(EntityManager manager, Entity asteroidEntity)
    {
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            // Instantiate Entity.
            Entity asteroidInstance = manager.Instantiate(asteroidEntity);

            // Position
            float3 randomPosition = float3.zero;
            int canvasHalfSize = CanvasSpace.CanvasHalfSize();

            do // Prevent asteroid to awake overlaping player.
            {
                randomPosition = new float3(UnityEngine.Random.Range(-canvasHalfSize, canvasHalfSize),
                                            UnityEngine.Random.Range(-canvasHalfSize, canvasHalfSize),
                                            UnityEngine.Random.Range(-canvasHalfSize, canvasHalfSize)
                                            );
            } while (math.distancesq(randomPosition, float3.zero) < 10000);
            manager.SetComponentData(asteroidInstance, new Translation { Value = randomPosition });

            // Rotation
            quaternion randomRotation = quaternion.Euler(UnityEngine.Random.onUnitSphere * 360);
            manager.SetComponentData(asteroidInstance, new Rotation { Value = randomRotation });

            // Scale
            manager.AddComponentData(asteroidInstance, new NonUniformScale { Value = new float3(1, 1, 1) * asteroidRadius });

            // Make sure min e max values won't break the code.
            float rotationSpeed = 0;
            try
            {
                rotationSpeed = UnityEngine.Random.Range(asteroidRotationSpeedMin, asteroidRotationSpeedMax);
            }
            catch
            {
                rotationSpeed = UnityEngine.Random.Range(asteroidRotationSpeedMax, asteroidRotationSpeedMin);
            }
            float asteroidSpeed = 0;
            try
            {
                asteroidSpeed = UnityEngine.Random.Range(asteroidSpeedMin, asteroidSpeedMax);
            }
            catch
            {
                asteroidSpeed = UnityEngine.Random.Range(asteroidSpeedMax, asteroidSpeedMin);
            }

            // Asteroid Data
            manager.SetComponentData(asteroidInstance, new AsteroidData
            {
                asteroidAxisRotation = UnityEngine.Random.onUnitSphere,
                asteroidSpeedRotation = rotationSpeed,
                asteroidDirection = UnityEngine.Random.onUnitSphere,
                asteroidSpeed = asteroidSpeed
            });

            // Hyperspace Jump Data
            manager.SetComponentData(asteroidInstance, new HyperspaceJumpData
            {
                isPlayer = false,
                canvasHalfSize = canvasHalfSize
            });
        }
    }
}
