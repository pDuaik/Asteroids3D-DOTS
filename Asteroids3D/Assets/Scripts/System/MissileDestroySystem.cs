using Unity.Entities;
using Unity.Jobs;

public class MissileDestroySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity, ref MissileData missileData) =>
            {
                if (missileData.hit != Entity.Null)
                {
                    EntityManager.DestroyEntity(missileData.hit);
                    EntityManager.DestroyEntity(entity);
                }
                else if (missileData.currentLifeSpan >= missileData.lifeSpan)
                {
                    EntityManager.DestroyEntity(entity);
                }
            })
            .Run();

        return inputDeps;
    }
}
