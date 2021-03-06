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
        bool stop = UnityEngine.Input.GetKeyDown("q");

        JobHandle jobHandle = Entities
            .WithName("PlayerMovementSystem")
            .ForEach((ref Translation position, ref Rotation rotation, ref PlayerData playerData) =>
            {
                // Set rotation
                rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(0, 0, -1), playerData.rotationSpeed * horizontal * deltaTime));
                rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(1, 0, 0), playerData.rotationSpeed * vertical * deltaTime));

                // Boost or Break
                if (thrust)
                {
                    playerData.currentThrust += math.mul(rotation.Value, new float3(0, 0, 1) * playerData.acceleration * deltaTime);
                }
                else if (stop)
                {
                    playerData.currentThrust = float3.zero;
                }

                // Update position and calculate current velocity
                playerData.currentVelocity = position.Value;
                position.Value += playerData.currentThrust;
                playerData.currentVelocity = position.Value - playerData.currentVelocity;
            })
            .Schedule(inputDeps);

        // trying to prevent some flickering.
        jobHandle.Complete();

        return jobHandle;
    }
}