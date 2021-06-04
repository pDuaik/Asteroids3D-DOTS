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
                      ref WarpingData warpingData,
                      ref Rotation rotation,
                      ref Translation position) =>
            {
                // Check if player is shooting
                if (playerData.currentShootingCooldownTime >= playerData.shootingCooldownTime && shoot)
                {
                    // Reset timer
                    playerData.currentShootingCooldownTime = 0;

                    // Instantiate missile
                    InstantiateMissile(playerData, warpingData, rotation, position, entityData.entity, 0);
                    if (playerData.power)
                    {
                        InstantiateMissile(playerData, warpingData, rotation, position, entityData.entity, -75);
                        InstantiateMissile(playerData, warpingData, rotation, position, entityData.entity, 75);
                    }
                }

                // Add delta time to shooting cooldown timer
                playerData.currentShootingCooldownTime += deltaTime;
            }).Run();

        return inputDeps;
    }

    private void InstantiateMissile(PlayerData playerData,
                                    WarpingData warpingData,
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
        // Set Warping Data
        manager.SetComponentData(missileInstance, new WarpingData
        {
            isPlayer = false,
            canvasHalfSize = warpingData.canvasHalfSize,
            update = true
        });
    }
}
