using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerCollisionSystem : JobComponentSystem
{
    protected override void OnStartRunning()
    {
        // Query for Power-Ups
        EntityQuery query = GetEntityQuery(typeof(CollectibleData));
        NativeArray<Entity> PowerUpEntities = query.ToEntityArray(Allocator.TempJob);

        var playerEntity = GetSingletonEntity<PlayerData>();

        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Populate Dynamic Buffer
        DynamicBuffer<EntityBufferData> entityBufferDatas = manager.GetBuffer<EntityBufferData>(playerEntity);
        DynamicBuffer<TranslationBufferData> float3BufferDatas = manager.GetBuffer<TranslationBufferData>(playerEntity);
        for (int i = 0; i < PowerUpEntities.Length; i++)
        {
            entityBufferDatas.Add(new EntityBufferData { entity = PowerUpEntities[i] });
            float3BufferDatas.Add(new TranslationBufferData { position = manager.GetComponentData<Translation>(PowerUpEntities[i]) });
        }

        PowerUpEntities.Dispose();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities
            .WithName("PlayerCollisionSystem")
            .ForEach((ref PlayerData playerData,
                      ref Translation position,
                      ref DynamicBuffer<EntityBufferData> entityBufferDatas,
                      ref DynamicBuffer<TranslationBufferData> translationBufferDatas) =>
            {
                for (int i = 0; i < entityBufferDatas.Length; i++)
                {
                    if (math.distancesq(translationBufferDatas[i].position.Value, position.Value) < 250 * 250)
                    {
                        playerData.hit = entityBufferDatas[i].entity;
                        break;
                    }
                }
            })
            .Schedule(inputDeps);

        jobHandle.Complete();

        return jobHandle;
    }
}
