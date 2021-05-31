using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerShootingScript : JobComponentSystem
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
                      ref Rotation rotation,
                      ref Translation position) =>
            {
                // Check if player is shooting
                if (playerData.currentShootingCooldownTime >= playerData.shootingCooldownTime && shoot)
                {
                    // Reset timer
                    playerData.currentShootingCooldownTime = 0;

                    // Instantiate missile
                    InstantiateMissile(playerData, rotation, position, 0);
                    if (playerData.powerUp)
                    {
                        InstantiateMissile(playerData, rotation, position, -75);
                        InstantiateMissile(playerData, rotation, position, 75);
                    }
                }

                // Add delta time to shooting cooldown timer
                playerData.currentShootingCooldownTime += deltaTime;

            }).Run();

        return inputDeps;
    }

    private void InstantiateMissile(PlayerData playerData, Rotation rotation, Translation position, float angle)
    {
        Entity missileInstance = manager.Instantiate(playerData.missile);
        manager.SetComponentData(missileInstance, new Rotation
        {
            Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(0, 1, 0), angle))
        });
        manager.SetComponentData(missileInstance, new Translation { Value = position.Value });
        manager.SetComponentData(missileInstance, new MissileData
        {
            initialVector = playerData.currentVelocity,
            lifeSpan = playerData.missileLifeSpan,
            missileSpeed = playerData.missileSpeed
        });
    }
}
