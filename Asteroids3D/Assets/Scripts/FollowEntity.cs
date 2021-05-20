using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;
    public float3 offset = new float3(0, 0, 0);

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
        {
            return;
        }

        Translation entPos = manager.GetComponentData<Translation>(entityToFollow);
        transform.position = entPos.Value + offset;
    }
}
