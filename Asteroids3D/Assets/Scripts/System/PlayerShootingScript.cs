using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class PlayerShootingScript : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        bool shoot = UnityEngine.Input.GetKeyDown("space");
        float deltaTime = UnityEngine.Time.deltaTime;

        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((ref PlayerData playerData,
                      ref Rotation rotation) =>
            {
                // Check if player is shooting
                if (playerData.currentShootingCooldownTime >= playerData.shootingCooldownTime && shoot)
                {
                    // Initialize manager using world default.
                    var manager = World.DefaultGameObjectInjectionWorld.EntityManager;

                    // Reset timer
                    playerData.currentShootingCooldownTime = 0;

                    // Instantiate missile
                    Entity missileInstance = manager.Instantiate(playerData.missile);
                    manager.SetComponentData(missileInstance, new Rotation { Value = rotation.Value });
                    manager.SetComponentData(missileInstance, new MissileData
                    {
                        awake = true,
                        lifeSpan = GameDataManager.singleton.missileLifeSpan,
                        currentLifeSpan = 0,
                    });
                }

                // Add delta time to shooting cooldown timer
                playerData.currentShootingCooldownTime += deltaTime;

            }).Run();

        return inputDeps;
    }
}
