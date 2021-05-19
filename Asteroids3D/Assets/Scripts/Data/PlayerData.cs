using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    public Entity Missile { get; set; }
}
