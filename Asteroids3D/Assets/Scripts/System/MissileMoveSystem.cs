using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MissileMoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // Retrieve from Engine DeltaTime and Inputs.
        float deltaTime = UnityEngine.Time.deltaTime;
        float missileSpeed = GameDataManager.singleton.missileSpeed;

        JobHandle jobHandle = Entities
            .WithName("MissileMoveSystem")
            .ForEach((
                ref Translation position,
                ref Rotation rotation,
                ref MissileData missileData,
                ref CollisionData collisionData,
                ref HyperspaceJumpData hyperspaceJumpData) =>
            {
                float3 missileForward = math.mul(rotation.Value, new float3(0, 0, 1) * missileSpeed * deltaTime);
                position.Value += missileData.initialVector + missileForward;
                missileData.currentLifeSpan += deltaTime;
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
