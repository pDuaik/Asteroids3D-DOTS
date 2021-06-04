using Asteroids3D;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    // GameObjects to initialize the game
    public GameObject characterTracker;
    public GameObject playerPrefab;
    public GameObject missilePrefab;
    public GameObject shieldPrefab;

    // Player
    public float rotationSpeed;
    public float acceleration;
    public float shootingCooldownTime;

    // Missile
    public float missileLifeSpan;
    public float missileSpeed;

    private void Start()
    {
        // Initialize manager using world default.
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        // Convert prefabs into entity.
        var playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
        var missileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(missilePrefab, settings);
        var shieldEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(shieldPrefab, settings);

        // Instantiate
        InstantiatePlayer(manager, playerEntity, missileEntity);
        InstantiateShield(manager, shieldEntity);

        Destroy(gameObject);
    }

    private void InstantiatePlayer(EntityManager manager, Entity playerEntity, Entity missileEntity)
    {
        // Instantiate Player.
        Entity playerInstance = manager.Instantiate(playerEntity);
        // Set Camera tracker
        characterTracker.GetComponent<CameraMovement>().SetReceivedEntity(playerInstance);
        // Set Hyperspace Jump
        manager.SetComponentData(playerInstance, new WarpingData
        {
            isPlayer = true,
            canvasHalfSize = CanvasSpace.CanvasHalfSize()
        });
        // Set missile
        manager.SetComponentData(playerInstance, new EntityData { entity = missileEntity });
        // Set Player Data
        manager.SetComponentData(playerInstance, new PlayerData
        {
            rotationSpeed = rotationSpeed,
            acceleration = acceleration,
            shootingCooldownTime = shootingCooldownTime,
            missileLifeSpan = missileLifeSpan,
            missileSpeed = missileSpeed
        });

        // Give a name for debugging
#if UNITY_EDITOR
        manager.SetName(playerInstance, "Player");
#endif
    }

    private void InstantiateShield(EntityManager manager, Entity shieldEntity)
    {
        // Instantiate Shield.
        Entity shieldInstance = manager.Instantiate(shieldEntity);

        // Very far awy position to keep the shield while off
        var pos = 100000;

        // Position
        manager.SetComponentData(shieldInstance, new ShieldData { distance = pos });

        // Position
        manager.SetComponentData(shieldInstance, new Translation { Value = new float3(0, pos, 0) });
    }
}