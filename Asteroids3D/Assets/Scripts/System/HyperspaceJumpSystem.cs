using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class HyperspaceJumpSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float3 cameraPosition = GameDataManager.singleton.mainCamera.position;
        float canvasSize = GameDataManager.singleton.canvasSize;
        float playerArea = canvasSize / 4;

        JobHandle jobHandle = Entities
            .WithName("HyperspaceJumpSystem")
            .ForEach((ref Translation position,
                      ref Rotation rotation,
                      ref HyperspaceJumpData hyperspaceJumpData) =>
            {
                // Player's Hyperspace Jump.
                if (hyperspaceJumpData.isPlayer)
                {
                    // Hyperspace Jump
                    if (math.abs(cameraPosition.x) > playerArea)
                        position.Value.x -= canvasSize / 2 * math.sign(position.Value.x);
                    if (math.abs(cameraPosition.y) >= playerArea)
                        position.Value.y -= canvasSize / 2 * math.sign(position.Value.y);
                    if (math.abs(cameraPosition.z) >= playerArea)
                        position.Value.z -= canvasSize / 2 * math.sign(position.Value.z);
                }
                // Asteroids Hyperspace Jump
                else
                {
                    // Move asteroids with player.
                    if (cameraPosition.x > playerArea)
                        position.Value.x -= canvasSize / 2;
                    else if (cameraPosition.x < -playerArea)
                        position.Value.x += canvasSize / 2;

                    if (cameraPosition.y > playerArea)
                        position.Value.y -= canvasSize / 2;
                    else if (cameraPosition.y < -playerArea)
                        position.Value.y += canvasSize / 2;

                    if (cameraPosition.z > playerArea)
                        position.Value.z -= canvasSize / 2;
                    else if (cameraPosition.z < -playerArea)
                        position.Value.z += canvasSize / 2;

                    // Check which asteroid is outside play area.
                    if (position.Value.x < -canvasSize)
                        position.Value.x += canvasSize * 2;
                    else if (position.Value.x > canvasSize)
                        position.Value.x -= canvasSize * 2;

                    if (position.Value.y < -canvasSize)
                        position.Value.y += canvasSize * 2;
                    else if (position.Value.y > canvasSize)
                        position.Value.y -= canvasSize * 2;

                    if (position.Value.z < -canvasSize)
                        position.Value.z += canvasSize * 2;
                    else if (position.Value.z > canvasSize)
                        position.Value.z -= canvasSize * 2;
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
