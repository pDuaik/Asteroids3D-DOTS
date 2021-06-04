using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateAfter(typeof(WarpingSystem))]
public class MissileUpdateCollisionBufferSystem : JobComponentSystem
{
    EntityManager manager;

    protected override void OnCreate()
    {
        // Initialize manager using world default.
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((ref WarpingData warpingData,
                      ref MissileData missileData,
                      ref DynamicBuffer<CollisionEntityBufferData> collisionEntityBufferDatas,
                      ref DynamicBuffer<CollisionPositionBufferData> collisionPositionBufferDatas) =>
            {
                if (warpingData.update)
                {
                    // Query for Asteroids
                    EntityQuery query = GetEntityQuery(typeof(AsteroidData));
                    NativeArray<Entity> asteroidEntities = query.ToEntityArray(Allocator.TempJob);

                    // Populate Dynamic Buffer
                    for (int i = 0; i < asteroidEntities.Length; i++)
                    {
                        collisionEntityBufferDatas.Add(new CollisionEntityBufferData { entity = asteroidEntities[i] });
                        collisionPositionBufferDatas.Add(new CollisionPositionBufferData { position = manager.GetComponentData<Translation>(asteroidEntities[i]).Value });
                    }

                    warpingData.update = false;

                    asteroidEntities.Dispose();
                }
            })
            .Run();


        return inputDeps;
    }
}
