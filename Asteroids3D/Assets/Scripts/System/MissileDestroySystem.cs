using Unity.Entities;
using Unity.Jobs;

public class MissileDestroySystem : JobComponentSystem
{
    EntityManager manager;

    protected override void OnStartRunning()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity, ref MissileData missileData) =>
            {
                if (missileData.hit != Entity.Null)
                {
                    manager.DestroyEntity(missileData.hit);
                    manager.DestroyEntity(entity);
                }
                else if (missileData.currentLifeSpan >= missileData.lifeSpan)
                {
                    manager.DestroyEntity(entity);
                }
            })
            .Run();

        return inputDeps;
    }
}
