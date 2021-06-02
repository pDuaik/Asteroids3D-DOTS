using Unity.Entities;

[GenerateAuthoringComponent]
public struct EntityData : IComponentData
{
    public Entity entity;
}
