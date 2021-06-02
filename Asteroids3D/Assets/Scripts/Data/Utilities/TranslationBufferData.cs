using Unity.Entities;
using Unity.Transforms;

[GenerateAuthoringComponent]
public struct TranslationBufferData : IBufferElementData
{
    public Translation position;
}
