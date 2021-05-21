using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class CollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float3 playerPosition = GameDataManager.singleton.playerPosition;

        JobHandle jobHandle = Entities
            .WithName("CollisionSystem")
            .ForEach((ref Translation position,
                      ref Rotation rotation,
                      ref CollisionData collisionData) =>
            {
                
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
