using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.Jobs;

public class MoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = UnityEngine.Time.deltaTime;

        JobHandle jobHandle = Entities
            .WithName("MoveSystem")
            .ForEach((ref Translation position, ref Rotation rotation) =>
            {

            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}