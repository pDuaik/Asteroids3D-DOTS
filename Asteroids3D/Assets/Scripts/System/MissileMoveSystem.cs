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
        quaternion playerRotation = GameDataManager.singleton.playerRotation;
        float missileSpeed = GameDataManager.singleton.missileSpeed;

        // Turn on missiles if player is shooting.
        if (shoot)
        {
            foreach (var item in GameDataManager.singleton.missiles)
            {
                if (!EntityManager.GetComponentData<MissileData>(item).isActive)
                {
                    EntityManager.SetComponentData(item, new MissileData { awake = true, lifeSpan = GameDataManager.singleton.missileLifeSpan, currentLifeSpan = 0 });
                    break;
                }
            }
        }

        // Check for collisions.

        JobHandle jobHandle = Entities
            .WithName("MissileMoveSystem")
            .ForEach((ref Translation position, ref Rotation rotation, ref MissileData missileData) =>
            {
                // Awake missile if player is shooting
                if (missileData.awake)
                {
                    missileData.awake = false;
                    missileData.isActive = true;
                    missileData.initialVector = playerVelocity;
                    position.Value = playerPosition;
                }

                // Move all missiles
                if (missileData.isActive)
                {
                    if (missileData.currentLifeSpan >= missileData.lifeSpan)
                    {
                        position.Value = new float3(0, 800000, 0);
                        rotation.Value = quaternion.identity;
                        missileData.awake = false;
                        missileData.isActive = false;
                        missileData.initialVector = float3.zero;
                        missileData.currentLifeSpan = 0;
                    }
                    else
                    {
                        float3 missileForward = math.mul(rotation.Value, new float3(0, 0, 1) * missileSpeed * deltaTime);
                        position.Value += missileData.initialVector + missileForward;
                        rotation.Value = playerRotation;
                        missileData.currentLifeSpan += deltaTime;
                    }
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
