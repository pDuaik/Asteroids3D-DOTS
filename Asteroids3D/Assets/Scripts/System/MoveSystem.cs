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
        float rotationSpeed = 1.0f;
        float speed = 10.0f;

        JobHandle jobHandle = Entities
            .WithName("MoveSystem")
            .ForEach((ref Translation position, ref Rotation rotation, ref PlayerData playerData) =>
            {
                rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(vertical, 0, -horizontal), rotationSpeed * deltaTime));
                position.Value += math.mul(rotation.Value, new float3(0, 0, 1) * speed * deltaTime);
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}