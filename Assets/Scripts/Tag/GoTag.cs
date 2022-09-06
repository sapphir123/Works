using Unity.Entities;
using Unity.Mathematics;

public struct GoTag : IComponentData
{
    public float3 targetPos;

    public Entity TempEntity;

    public float3 originPos;

    public bool isBack;

}