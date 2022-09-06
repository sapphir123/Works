using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Enemy : IComponentData
{
    public float speed;
    public Entity targetEntity;
}
