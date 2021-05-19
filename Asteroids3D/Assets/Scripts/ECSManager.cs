using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSManager : MonoBehaviour
{
    private void Start()
    {
        // Initialize manage using world default.
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        // Convert prefab into entity.
        var playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(GameDataManager.singleton.playerPrefab, settings);

        // Instantiate player.
        Entity instance = manager.Instantiate(playerEntity);
        manager.SetComponentData(instance, new Translation { Value = float3.zero });
        manager.SetComponentData(instance, new Rotation { Value = quaternion.identity });
    }
}
