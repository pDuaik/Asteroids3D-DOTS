using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class AsteroidsMovementSystem : JobComponentSystem
{
    Entity player;
    protected override void OnStartRunning()
    {
        player = GetSingletonEntity<PlayerData>();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = UnityEngine.Time.deltaTime;
        float3 playerPosition = EntityManager.GetComponentData<Translation>(player).Value;
        float asteroidSizeSmall = GameDataManager.singleton.asteroidSize / 4;

        JobHandle jobHandle = Entities
            .WithName("AsteroidsMovementSystem")
            .ForEach((ref Translation position,
                      ref Rotation rotation,
                      ref AsteroidData asteroidData,
                      ref CollisionData collisionData,
                      ref ShatterData shatterData,
                      ref HyperspaceJumpData hyperspaceJumpData,
                      ref NonUniformScale nonUniformScale) =>
            {
                if (collisionData.collision)
                {
                    collisionData.collision = false;
                    if (shatterData.smallAsteroid)
                    {
                        asteroidData.isActive = false;
                        position.Value = new float3(0, 800000, 0);
                        hyperspaceJumpData.isActive = false;
                    }
                    else
                    {
                        position.Value += new float3(0, 1, 0) * 100;
                        asteroidData.asteroidDirection = math.normalize(position.Value - playerPosition);
                        nonUniformScale.Value = new float3(1, 1, 1) * asteroidSizeSmall;
                        shatterData.smallAsteroid = true;
                    }
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
