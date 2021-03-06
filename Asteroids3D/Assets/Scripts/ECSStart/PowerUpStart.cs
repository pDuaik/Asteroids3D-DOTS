using Asteroids3D;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PowerUpStart : MonoBehaviour
{
    // Prefabs
    public GameObject powerPrefab;
    public GameObject shieldPrefab;

    // Asteroids
    public int number = 16;
    public float size = 250;

    private void Start()
    {
        // Initialize manager using world default.
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        // Convert prefabs into entity.
        var powerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(powerPrefab, settings);
        var shieldEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(shieldPrefab, settings);

        // Instantiate
        //PopulatePowerUps(manager, powerEntity, true, false);
        PopulatePowerUps(manager, shieldEntity, true, false);

        Destroy(gameObject);
    }

    private void PopulatePowerUps(EntityManager manager, Entity powerUpEntity, bool power, bool shield)
    {
        for (int i = 0; i < number; i++)
        {
            // Instantiate Entity.
            Entity PowerUpInstance = manager.Instantiate(powerUpEntity);

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
            manager.SetComponentData(PowerUpInstance, new Translation { Value = randomPosition });

            // Rotation
            manager.SetComponentData(PowerUpInstance, new Rotation { Value = quaternion.identity });

            // Scale
            manager.AddComponentData(PowerUpInstance, new NonUniformScale { Value = new float3(1, 1, 1) * size });

            // Hyperspace Jump Data
            manager.SetComponentData(PowerUpInstance, new WarpingData
            {
                isPlayer = false,
                canvasHalfSize = canvasHalfSize
            });

            manager.SetComponentData(PowerUpInstance, new CollectibleData { power = power, shield = shield });
        }
    }
}
