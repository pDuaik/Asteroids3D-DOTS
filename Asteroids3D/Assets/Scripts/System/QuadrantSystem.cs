using Unity.Entities;
using Unity.Jobs;

public class QuadrantSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        return inputDeps;
    }
}
