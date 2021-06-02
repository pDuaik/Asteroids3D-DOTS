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
            .ForEach((ref MissileData missileData,
                      ref Translation position,
                      ref DynamicBuffer<EntityBufferData> entityBufferDatas,
                      ref DynamicBuffer<TranslationBufferData> translationBufferDatas) =>
            {
                for (int i = 0; i < entityBufferDatas.Length; i++)
                {
                    if (math.distancesq(translationBufferDatas[i].position.Value, position.Value) < 300 * 300)
                    {
                        missileData.hit = entityBufferDatas[i].entity;
                        break;
                    }
                }
            })
            .Schedule(inputDeps);

        // Guarantees the job has completed before schedulling another one.
        jobHandle.Complete();

        return jobHandle;
    }
}
