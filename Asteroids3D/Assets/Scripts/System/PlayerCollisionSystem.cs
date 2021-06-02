using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerCollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities
            .WithBurst()
            .WithName("PlayerCollisionSystem")
            .ForEach((Entity entity,
                      ref PlayerData playerData,
                      ref Translation position,
                      ref DynamicBuffer<EntityBufferData> entityBufferDatas,
                      ref DynamicBuffer<Float3BufferData> float3BufferDatas) =>
            {
                for (int i = 0; i < entityBufferDatas.Length; i++)
                {
                    if (math.distancesq(float3BufferDatas[i].Value, position.Value) < 300 * 300)
                    {
                        playerData.hit = entityBufferDatas[i].Value;
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
