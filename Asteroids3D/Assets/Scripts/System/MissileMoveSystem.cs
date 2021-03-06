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

        JobHandle jobHandle = Entities
            .WithName("MissileMoveSystem")
            .ForEach((
                ref Translation position,
                ref Rotation rotation,
                ref MissileData missileData) =>
            {
                float3 missileForward = math.mul(rotation.Value, new float3(0, 0, 1) * missileData.missileSpeed * deltaTime);
                position.Value += missileData.initialVector + missileForward;
                rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(5 *deltaTime));
                missileData.currentLifeSpan += deltaTime;
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
