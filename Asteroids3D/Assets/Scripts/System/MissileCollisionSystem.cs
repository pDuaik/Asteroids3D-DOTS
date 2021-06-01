using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MissileCollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities
            .WithBurst()
            .WithName("MissileCollisionSystem")
            .ForEach((Entity entity,
                      ref MissileData missileData,
                      ref Translation position,
                      ref DynamicBuffer<EntityBufferData> entityBufferDatas,
                      ref DynamicBuffer<Float3BufferData> float3BufferDatas) =>
            {
                for (int i = 0; i < entityBufferDatas.Length; i++)
                {
                    if (math.distancesq(float3BufferDatas[i].Value, position.Value) < 300 * 300)
                    {
                        missileData.hit = entityBufferDatas[i].Value;
                        break;
                    }
                }
            })
            .Schedule(inputDeps);

        // Guarantees the job has completed before schedulling another one.
        jobHandle.Complete();

        return inputDeps;
    }
}
