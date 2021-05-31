using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(PlayerMovementSystem))]
public class ShieldMovementSystem : JobComponentSystem
{
    Entity player;
    EntityManager manager;

    protected override void OnStartRunning()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        player = GetSingletonEntity<PlayerData>();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        bool shield = manager.GetComponentData<PlayerData>(player).shield;
        float3 playerPos = manager.GetComponentData<Translation>(player).Value;

        var jobHandle = Entities
            .WithName("ShieldMovementSystem")
            .ForEach((ref ShieldData shieldData, ref Translation position) =>
            {
                if (shield)
                {
                    position.Value = playerPos;
                }
                else
                {
                    position.Value = new float3(0, 100000, 0);
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
