using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    // Gameplay variables
    public float rotationSpeed;
    public float acceleration;

    //public Entity Missile { get; set; }
}
