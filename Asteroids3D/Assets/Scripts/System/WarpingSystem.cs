using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class WarpingSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // get player position before warping.
        float3 playerPosition = EntityManager.GetComponentData<Translation>(GetSingletonEntity<PlayerData>()).Value;

        JobHandle jobHandle = Entities
            .WithName("WarpingSystem")
            .ForEach((ref Translation position,
                      ref WarpingData warpingData) =>
            {
                // Get canvas and game area.
                var canvasSize = warpingData.canvasHalfSize;
                var playerArea = canvasSize / 4;

                if (warpingData.isPlayer)
                {
                    // Warp player.
                    if (math.abs(playerPosition.x) > playerArea)
                        position.Value.x -= math.sign(position.Value.x) * canvasSize / 2;
                    if (math.abs(playerPosition.y) > playerArea)
                        position.Value.y -= math.sign(position.Value.y) * canvasSize / 2;
                    if (math.abs(playerPosition.z) > playerArea)
                        position.Value.z -= math.sign(position.Value.z) * canvasSize / 2;
                }
                else
                {
                    // Warp other entities.
                    if (math.abs(playerPosition.x) > playerArea)
                        position.Value.x -= math.sign(playerPosition.z) * canvasSize / 2;
                    if (math.abs(playerPosition.y) > playerArea)
                        position.Value.y -= math.sign(playerPosition.z) * canvasSize / 2;
                    if (math.abs(playerPosition.z) > playerArea)
                        position.Value.z -= math.sign(playerPosition.z) * canvasSize / 2;

                    // Put other entities inside canvas area.
                    if (math.abs(position.Value.x) > canvasSize)
                        position.Value.x -= math.sign(position.Value.x) * canvasSize * 2;
                    if (math.abs(position.Value.y) > canvasSize)
                        position.Value.y -= math.sign(position.Value.y) * canvasSize * 2;
                    if (math.abs(position.Value.z) > canvasSize)
                        position.Value.z -= math.sign(position.Value.z) * canvasSize * 2;
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
