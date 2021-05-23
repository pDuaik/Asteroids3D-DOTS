using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class AsteroidsMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = UnityEngine.Time.deltaTime;

        JobHandle jobHandle = Entities
            .WithName("AsteroidsMovementSystem")
            .ForEach((ref Translation position,
                      ref Rotation rotation,
                      ref AsteroidData asteroidData,
                      ref CollisionData collisionData,
                      ref HyperspaceJumpData hyperspaceJumpData) =>
            {
                if (collisionData.collision)
                {
                    collisionData.collision = false;
                    asteroidData.isActive = false;
                    position.Value = new float3(0, 800000, 0);
                    hyperspaceJumpData.isActive = false;
                }

                if (asteroidData.isActive)
                {
                    // Rotation
                    rotation.Value = math.mul(rotation.Value,
                                              quaternion.AxisAngle(asteroidData.asteroidAxisRotation,
                                                                   asteroidData.asteroidSpeedRotation * deltaTime)
                                              );
                    // Position
                    position.Value += asteroidData.asteroidDirection * asteroidData.asteroidSpeed * deltaTime;
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
