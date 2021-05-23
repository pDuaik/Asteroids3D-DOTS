using Unity.Entities;
using UnityEngine;

public class LeaderAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject followerObject;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        FollowEntity followEntity = followerObject.GetComponent<FollowEntity>();

        if (followEntity == null)
        {
            followEntity = followerObject.AddComponent<FollowEntity>();
        }

        followEntity.entityToFollow = entity;
    }
}
