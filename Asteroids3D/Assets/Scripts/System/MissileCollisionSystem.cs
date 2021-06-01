using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MissileCollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // TODO: Can I transform this query into a Buffer System?
        // Get the asteroids that are near the missile path and check distance only against them.
        // Maybe it will save not only distance calculation, but also query calculation.
        //EntityQuery query = GetEntityQuery(typeof(AsteroidData), typeof(Translation));
        //NativeArray<Entity> asteroidEntities = query.ToEntityArray(Allocator.TempJob);
        //NativeArray<Translation> asteroidPositions = query.ToComponentDataArray<Translation>(Allocator.TempJob);

        //.WithReadOnly(asteroidEntities)
        //.WithReadOnly(asteroidPositions)

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

        //asteroidEntities.Dispose(inputDeps);
        //asteroidPositions.Dispose(inputDeps);

        return inputDeps;
    }
}
