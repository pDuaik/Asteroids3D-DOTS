using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class MissileMoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // Retrieve from Engine DeltaTime and Inputs.
        float deltaTime = UnityEngine.Time.deltaTime;
        bool shoot = UnityEngine.Input.GetKeyDown("space");
        float3 playerVelocity = GameDataManager.singleton.playerVelocity;
        float3 playerPosition = GameDataManager.singleton.playerPosition;

        if (shoot)
        {
            foreach (var item in GameDataManager.singleton.missiles)
            {
                if (!EntityManager.GetComponentData<MissileData>(item).isActive)
                {
                    EntityManager.SetComponentData(item, new MissileData { awake = true });
                    break;
                }
            }
        }

        JobHandle jobHandle = Entities
            .WithName("MissileMoveSystem")
            .ForEach((ref Translation position, ref Rotation rotation, ref MissileData missileData) =>
            {
                // Move all missiles
                if (missileData.isActive)
                {
                    position.Value += missileData.initialVector * 2 * deltaTime;
                }

                // Awake missile if player is shooting
                if (missileData.awake)
                {
                    missileData.awake = false;
                    missileData.isActive = true;
                    missileData.initialVector = playerVelocity;
                    position.Value = playerPosition;
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
