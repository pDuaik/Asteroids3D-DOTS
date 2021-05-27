using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EntityTracker : MonoBehaviour
{
    private Entity EntityToTrack = Entity.Null;

    /// <summary>
    /// Set the entity that will be used as target.
    /// </summary>
    public void SetReceivedEntity(Entity entity)
    {
        EntityToTrack = entity;
    }

    private void Update()
    {
        if (EntityToTrack != Entity.Null)
        {
            try
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                transform.position = entityManager.GetComponentData<Translation>(EntityToTrack).Value;
                transform.rotation = entityManager.GetComponentData<Rotation>(EntityToTrack).Value;
            }
            catch
            {
                EntityToTrack = Entity.Null;
            }
        }
    }
}
