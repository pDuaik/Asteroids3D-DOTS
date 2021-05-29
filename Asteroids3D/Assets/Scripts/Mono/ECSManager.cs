using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSManager : MonoBehaviour
{
    // GameObjects to initialize the game
    public GameObject characterTracker;
    public GameObject asteroidPrefab;
    public GameObject missilePrefab;
    public GameObject playerPrefab;


    private void Start()
    {
        // Initialize manager using world default.
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        // Convert prefabs into entity.
        var asteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);
        var missileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(missilePrefab, settings);
        var playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);

        // Calculate quadrant space size
        int currentQuadrantSpaceSize = 1;
        do
        {
            currentQuadrantSpaceSize *= 2;
        } while (currentQuadrantSpaceSize < GameDataManager.singleton.asteroidSize * 2);
        

        // Instantiate
        InstantiatePlayer(manager, missileEntity, playerEntity);
        PopulateAsteroids(manager, asteroidEntity);
    }

    private void InstantiatePlayer(EntityManager manager, Entity missileEntity, Entity playerEntity)
    {
        // Instantiate Player.
        Entity playerInstance = manager.Instantiate(playerEntity);
        manager.SetComponentData(playerInstance, new PlayerData
        {
            missile = missileEntity,
            acceleration = manager.GetComponentData<PlayerData>(playerInstance).acceleration,
            shootingCooldownTime = manager.GetComponentData<PlayerData>(playerInstance).shootingCooldownTime,
            rotationSpeed = manager.GetComponentData<PlayerData>(playerInstance).rotationSpeed,
            powerUpPosition = manager.GetComponentData<PlayerData>(playerInstance).powerUpPosition,
            powerUpRadius = manager.GetComponentData<PlayerData>(playerInstance).powerUpRadius
        });
        characterTracker.GetComponent<CameraMovement>().SetReceivedEntity(playerInstance);
#if UNITY_EDITOR
        manager.SetName(playerInstance, "Player");
#endif
    }

    private static void PopulateAsteroids(EntityManager manager, Entity asteroidEntity)
    {
        for (int i = 0; i < GameDataManager.singleton.numberOfAsteroids; i++)
        {
            // Instantiate Entity.
            Entity asteroidInstance = manager.Instantiate(asteroidEntity);

            // Position
            float canvasSize = GameDataManager.singleton.canvasSize;
            float3 randomPosition = float3.zero;
            // Prevent asteroid to awake overlaping player.
            while (math.distancesq(randomPosition, float3.zero) < math.pow(60, 2))
            {
                randomPosition = new float3(UnityEngine.Random.Range(-canvasSize, canvasSize),
                                               UnityEngine.Random.Range(-canvasSize, canvasSize),
                                               UnityEngine.Random.Range(-canvasSize, canvasSize)
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
                asteroidSpeedRotation = UnityEngine.Random.Range(GameDataManager.singleton.asteroidRandomRotationSpeedMinMax.x,
                                                                 GameDataManager.singleton.asteroidRandomRotationSpeedMinMax.y),
                asteroidDirection = UnityEngine.Random.onUnitSphere,
                asteroidSpeed = UnityEngine.Random.Range(GameDataManager.singleton.asteroidRandomSpeedMinMax.x,
                                                         GameDataManager.singleton.asteroidRandomSpeedMinMax.y),
                isActive = i < GameDataManager.singleton.numberOfAsteroids ? true : false
            });

            // Scale
            manager.AddComponentData(asteroidInstance, new NonUniformScale { Value = new float3(1, 1, 1) * GameDataManager.singleton.asteroidSize });
        }
    }
}
