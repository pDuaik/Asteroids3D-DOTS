using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //public GameObject followObject;
    private Entity EntityToTrack = Entity.Null;
    public Vector3 offset = float3.zero;
    //public float3 cameraEuler = float3.zero;

    private void LateUpdate()
    {
        if (EntityToTrack != Entity.Null)
        {
            try
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var pos = entityManager.GetComponentData<Translation>(EntityToTrack).Value;
                var rot = entityManager.GetComponentData<Rotation>(EntityToTrack).Value;
                transform.position = pos;
                transform.rotation = rot;

                // Position
                float3 forward = math.mul(rot, new float3(0, 0, 1));
                float3 up = math.mul(rot, new float3(0, 1, 0));
                transform.position = pos + forward * offset.z + up * offset.y;
            }
            catch
            {
                EntityToTrack = Entity.Null;
            }
        }

        /*
        var goTransform = followObject.GetComponent<Transform>();

        // Reset camera position
        transform.position = goTransform.position;

        // Rotation
        transform.rotation = goTransform.rotation;

        
        */
    }

    public void SetReceivedEntity(Entity entity)
    {
        EntityToTrack = entity;
    }
}
