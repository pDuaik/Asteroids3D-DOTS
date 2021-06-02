using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(PlayerMovementSystem))]
public class PlayerShootingSystem : JobComponentSystem
{
    EntityManager manager;

    protected override void OnCreate()
    {
        // Initialize manager using world default.
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        bool shoot = UnityEngine.Input.GetKeyDown("space");
        float deltaTime = UnityEngine.Time.deltaTime;

        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((ref PlayerData playerData,
                      ref EntityData entityData,
                      ref HyperspaceJumpData hyperspaceJumpData,
                      ref Rotation rotation,
                      ref Translation position) =>
            {
                // Check if player is shooting
                if (playerData.currentShootingCooldownTime >= playerData.shootingCooldownTime && shoot)
                {
                    // Query for Asteroids
                    EntityQuery query = GetEntityQuery(typeof(AsteroidData), typeof(Translation));
                    NativeArray<Entity> asteroidEntities = query.ToEntityArray(Allocator.TempJob);
                    NativeArray<Translation> asteroidPositions = query.ToComponentDataArray<Translation>(Allocator.TempJob);

                    // Reset timer
                    playerData.currentShootingCooldownTime = 0;

                    // Instantiate missile
                    InstantiateMissile(playerData, hyperspaceJumpData, asteroidEntities, asteroidPositions, rotation, position, entityData.entity, 0);
                    if (playerData.powerUp)
                    {
                        InstantiateMissile(playerData, hyperspaceJumpData, asteroidEntities, asteroidPositions, rotation, position, entityData.entity, -75);
                        InstantiateMissile(playerData, hyperspaceJumpData, asteroidEntities, asteroidPositions, rotation, position, entityData.entity, 75);
                    }

                    asteroidEntities.Dispose();
                    asteroidPositions.Dispose();
                }

                // Add delta time to shooting cooldown timer
                playerData.currentShootingCooldownTime += deltaTime;

            }).Run();

        return inputDeps;
    }

    private void InstantiateMissile(PlayerData playerData,
                                    HyperspaceJumpData hyperspaceJumpData,
                                    NativeArray<Entity> asteroidEntities,
                                    NativeArray<Translation> asteroidPositions,
                                    Rotation rotation,
                                    Translation position,
                                    Entity missile,
                                    float angle)
    {
        // Create missile
        Entity missileInstance = manager.Instantiate(missile);

        // Rotation
        manager.SetComponentData(missileInstance, new Rotation
        {
            Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(0, 1, 0), angle))
        });

        // Position
        manager.SetComponentData(missileInstance, new Translation { Value = position.Value });

        // Missile Data
        manager.SetComponentData(missileInstance, new MissileData
        {
            initialVector = playerData.currentVelocity,
            lifeSpan = playerData.missileLifeSpan,
            missileSpeed = playerData.missileSpeed
        });
        // Set Hyperspace Jump
        manager.SetComponentData(missileInstance, new HyperspaceJumpData { isPlayer = false, canvasHalfSize = hyperspaceJumpData.canvasHalfSize });

        // Populate Dynamic Buffer
        DynamicBuffer<EntityBufferData> entityBufferDatas = manager.GetBuffer<EntityBufferData>(missileInstance);
        DynamicBuffer<Float3BufferData> float3BufferDatas = manager.GetBuffer<Float3BufferData>(missileInstance);
        for (int i = 0; i < asteroidEntities.Length; i++)
        {
            entityBufferDatas.Add(new EntityBufferData { Value = asteroidEntities[i] });
            float3BufferDatas.Add(new Float3BufferData { Value = asteroidPositions[i].Value });
        }
    }
}
