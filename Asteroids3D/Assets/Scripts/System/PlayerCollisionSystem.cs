using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

public class PlayerCollisionSystem : JobComponentSystem
{
    protected override void OnStartRunning()
    {
        // Query for Power-Ups
        EntityQuery query = GetEntityQuery(typeof(CollectibleData), typeof(Translation));
        NativeArray<Entity> PowerUpEntities = query.ToEntityArray(Allocator.TempJob);
        NativeArray<Translation> PowerUpPositions = query.ToComponentDataArray<Translation>(Allocator.TempJob);

        var playerEntity = GetSingletonEntity<PlayerData>();

        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        // Populate Dynamic Buffer
        DynamicBuffer<EntityBufferData> entityBufferDatas = manager.GetBuffer<EntityBufferData>(playerEntity);
        DynamicBuffer<Float3BufferData> float3BufferDatas = manager.GetBuffer<Float3BufferData>(playerEntity);
        for (int i = 0; i < PowerUpEntities.Length; i++)
        {
            entityBufferDatas.Add(new EntityBufferData { Value = PowerUpEntities[i] });
            float3BufferDatas.Add(new Float3BufferData { Value = PowerUpPositions[i].Value });
        }

        PowerUpEntities.Dispose();
        PowerUpPositions.Dispose();
    }
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
