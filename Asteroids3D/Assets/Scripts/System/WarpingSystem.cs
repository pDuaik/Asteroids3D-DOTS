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
                    // Save initial position to campare with final result.
                    float3 latePosition = position.Value;

                    if (math.abs(playerPosition.x) > playerArea)
                        position.Value.x -= math.sign(position.Value.x) * canvasSize / 2;
                    if (math.abs(playerPosition.y) > playerArea)
                        position.Value.y -= math.sign(position.Value.y) * canvasSize / 2;
                    if (math.abs(playerPosition.z) > playerArea)
                        position.Value.z -= math.sign(position.Value.z) * canvasSize / 2;

                    // Check if something changed.
                    if (!position.Value.Equals(latePosition))
                    {
                        warpingData.update = true;
                    }
                }
                else
                {
                    // Save initial position to campare with final result.
                    float3 latePosition = position.Value;

                    // Move entities
                    if (math.abs(playerPosition.x) > playerArea)
                        position.Value.x -= math.sign(playerPosition.x) * canvasSize / 2;
                    if (math.abs(playerPosition.y) > playerArea)
                        position.Value.y -= math.sign(playerPosition.y) * canvasSize / 2;
                    if (math.abs(playerPosition.z) > playerArea)
                        position.Value.z -= math.sign(playerPosition.z) * canvasSize / 2;

                    // Put entities inside canvas area.
                    if (math.abs(position.Value.x) > canvasSize)
                        position.Value.x -= math.sign(position.Value.x) * canvasSize * 2;
                    if (math.abs(position.Value.y) > canvasSize)
                        position.Value.y -= math.sign(position.Value.y) * canvasSize * 2;
                    if (math.abs(position.Value.z) > canvasSize)
                        position.Value.z -= math.sign(position.Value.z) * canvasSize * 2;

                    // Check if something changed.
                    if (!position.Value.Equals(latePosition))
                    {
                        warpingData.update = true;
                    }
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
