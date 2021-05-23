using Unity.Entities;

[GenerateAuthoringComponent]
public struct CollisionData : IComponentData
{
    public bool collision;
}
