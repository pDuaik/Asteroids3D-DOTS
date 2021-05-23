using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ShatterData : IComponentData
{
    // If it has shattered once, next shattering will be the end of it.
    public bool smallAsteroid;
}
