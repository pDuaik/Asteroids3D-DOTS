using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class QuadrantSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        int quadrantSize = GameDataManager.singleton.asteroidRadius;

        JobHandle jobHandle = Entities
            .WithName("QuadrantSystem")
            .ForEach((
                ref Translation position,
                ref QuadrantData quadrantData) =>
            {
                int3 currentQuadrant = int3.zero;
                currentQuadrant = (int3)(position.Value / quadrantSize);
                quadrantData.quadrant = currentQuadrant;
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
