using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MissileCollisionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var randomCheck = UnityEngine.Random.value;
        var isOdd = randomCheck > .5f ? true : false;

        var jobHandle = Entities
            .WithoutBurst()
            .WithName("MissileCollisionSystem")
            .ForEach((Entity missile,
                      ref MissileData missileData,
                      ref Translation position,
                      ref DynamicBuffer<CollisionEntityBufferData> collisionEntityBufferDatas,
                      ref DynamicBuffer<CollisionPositionBufferData> collisionPositionBufferDatas) =>
            {
                // Performing collision check for only half of the asteroids.
                for (int i = randomCheck > .5f ? 0 : 1 ; i < collisionEntityBufferDatas.Length; i += 2)
                {
                    try
                    {
                        if (math.distancesq(collisionPositionBufferDatas[i].position, position.Value) < 300 * 300)
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

        return jobHandle;
    }
}
