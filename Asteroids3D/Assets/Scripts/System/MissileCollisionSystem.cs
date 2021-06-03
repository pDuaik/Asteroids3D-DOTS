using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MissileCollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities
            .WithoutBurst()
            .WithName("MissileCollisionSystem")
            .ForEach((Entity missile,
                      ref MissileData missileData,
                      ref Translation position,
                      ref DynamicBuffer<CollisionEntityBufferData> collisionEntityBufferDatas,
                      ref DynamicBuffer<CollisionPositionBufferData> collisionPositionBufferDatas) =>
            {
                for (int i = 0; i < collisionEntityBufferDatas.Length; i++)
                {
                    try
                    {
                        if (math.distancesq(float3.zero, position.Value) < 300 * 300)
                        {
                            missileData.hit = collisionEntityBufferDatas[i].entity;
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            })
            .Schedule(inputDeps);

        // Guarantees the job has completed before schedulling another one.
        jobHandle.Complete();

        return jobHandle;
    }
}
