using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;
    public float3 offset = new float3(0, 0, 0);
    public float3 cameraEuler = new float3(0, 0, 0);

    private EntityManager manager;

    private void Start()
    {
        // Initialize manager using world default.
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {
        // Prevent update in case of missing entity.
        if (entityToFollow == null)
            return;

        // Get desired components.
        Translation entPos = manager.GetComponentData<Translation>(entityToFollow);
        Rotation entRot = manager.GetComponentData<Rotation>(entityToFollow);

        // Rotate camera.
        transform.rotation = entRot.Value;
        transform.Rotate(cameraEuler);

        // Position camera.
        float3 forward = math.mul(entRot.Value, new float3(0, 0, 1));
        float3 up = math.mul(entRot.Value, new float3(0, 1, 0));
        transform.position = entPos.Value + forward * offset.z + up * offset.y;
    }
}
