using Unity.Entities;
using Unity.Jobs;
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
        float missileLifeSpan = GameDataManager.singleton.missileLifeSpan;

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
                    Entity missileInstance = manager.Instantiate(playerData.missile);
                    manager.SetComponentData(missileInstance, new Rotation { Value = rotation.Value });
                    manager.SetComponentData(missileInstance, new Translation { Value = position.Value });
                    manager.SetComponentData(missileInstance, new MissileData
                    {
                        initialVector = playerData.currentVelocity,
                        lifeSpan = missileLifeSpan,
                    });
                }

                // Add delta time to shooting cooldown timer
                playerData.currentShootingCooldownTime += deltaTime;

            }).Run();

        return inputDeps;
    }
}
