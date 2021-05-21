using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class PlayerMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // Retrieve from Engine DeltaTime and Inputs.
        float deltaTime = UnityEngine.Time.deltaTime;
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        bool thrust = UnityEngine.Input.GetKey("left shift");

        // GameDataManager variables
        float canvasSize = GameDataManager.singleton.canvasSize;

        // Check collision
        bool collision = false;
        foreach (var item in GameDataManager.singleton.asteroids)
        {
            if (math.distancesq(EntityManager.GetComponentData<Translation>(item).Value, GameDataManager.singleton.playerPosition) < math.pow(200, 2))
            {
                collision = true;
            }
        }

        JobHandle jobHandle = Entities
            .WithName("PlayerMovementSystem")
            .WithoutBurst()
            .ForEach((ref Translation position, ref Rotation rotation, ref PlayerData playerData) =>
            {
                if (collision)
                {
                    rotation.Value = quaternion.identity;
                    position.Value = float3.zero;
                    playerData.currentVelocity = float3.zero;
                }
                else
                {
                    // Set rotation
                    rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(0, 0, -1), playerData.rotationSpeed * horizontal * deltaTime));
                    rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(1, 0, 0), playerData.rotationSpeed * vertical * deltaTime));

                    // Set velocity
                    playerData.currentVelocity += thrust ? math.mul(rotation.Value, new float3(0, 0, 1) * playerData.acceleration * deltaTime) : float3.zero;
                    position.Value += playerData.currentVelocity;
                }

            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}