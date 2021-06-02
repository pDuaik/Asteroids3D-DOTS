using Unity.Entities;
using Unity.Jobs;

public class PlayerDestroySystem : JobComponentSystem
{
    EntityManager manager;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((ref PlayerData playerData) =>
            {
                if (playerData.hit != Entity.Null)
                {
                    playerData.shield = manager.GetComponentData<CollectibleData>(playerData.hit).shield;
                    playerData.power = manager.GetComponentData<CollectibleData>(playerData.hit).power;
                    //manager.DestroyEntity(playerData.hit);
                }
            })
            .Run();

        return inputDeps;
    }
}
