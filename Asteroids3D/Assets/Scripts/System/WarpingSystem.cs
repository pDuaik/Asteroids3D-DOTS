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
                    // Warp player data.
                    float3 warpingTemp = float3.zero;

                    if (math.abs(playerPosition.x) > playerArea)
                        warpingTemp.x -= math.sign(position.Value.x) * canvasSize / 2;
                    if (math.abs(playerPosition.y) > playerArea)
                        warpingTemp.y -= math.sign(position.Value.y) * canvasSize / 2;
                    if (math.abs(playerPosition.z) > playerArea)
                        warpingTemp.z -= math.sign(position.Value.z) * canvasSize / 2;

                    // Pass data to entity
                    warpingData.Value += warpingTemp;
                    position.Value += warpingTemp;
                }
                else
                {
                    // Warp other entities.
                    float3 warpingTemp = float3.zero;

                    if (math.abs(playerPosition.x) > playerArea)
                        warpingTemp.x -= math.sign(playerPosition.x) * canvasSize / 2;
                    if (math.abs(playerPosition.y) > playerArea)
                        warpingTemp.y -= math.sign(playerPosition.y) * canvasSize / 2;
                    if (math.abs(playerPosition.z) > playerArea)
                        warpingTemp.z -= math.sign(playerPosition.z) * canvasSize / 2;

                    // Pass data to entity
                    warpingData.Value += warpingTemp;
                    position.Value += warpingTemp;

                    // Put other entities inside canvas area.
                    warpingTemp = float3.zero;

                    if (math.abs(position.Value.x) > canvasSize)
                        warpingTemp.x -= math.sign(position.Value.x) * canvasSize * 2;
                    if (math.abs(position.Value.y) > canvasSize)
                        warpingTemp.y -= math.sign(position.Value.y) * canvasSize * 2;
                    if (math.abs(position.Value.z) > canvasSize)
                        warpingTemp.z -= math.sign(position.Value.z) * canvasSize * 2;

                    // Pass data to entity
                    warpingData.Value += warpingTemp;
                    position.Value += warpingTemp;
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
