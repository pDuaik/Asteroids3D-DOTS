using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class MoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // Retrieve from Engine DeltaTime and Inputs.
        float deltaTime = UnityEngine.Time.deltaTime;
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        bool thrust = UnityEngine.Input.GetKey("left shift");

        JobHandle jobHandle = Entities
            .WithName("MoveSystem")
            .ForEach((ref Translation position, ref Rotation rotation, ref PlayerData playerData) =>
            {
                rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(0, 0, -1), playerData.rotationSpeed * horizontal * deltaTime));
                rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(1, 0, 0), playerData.rotationSpeed * vertical * deltaTime));
                position.Value += thrust ? math.mul(rotation.Value, new float3(0, 0, 1) * playerData.acceleration * deltaTime) : float3.zero;
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}