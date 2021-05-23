using Unity.Entities;

[GenerateAuthoringComponent]
public struct HyperspaceJumpData : IComponentData
{
    public bool isPlayer;

    public bool isActive;
}
