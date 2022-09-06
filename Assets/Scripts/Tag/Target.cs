using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Target : IComponentData
{
    public bool isGo;
    public float3 Tpos;
    public float randomSpeed;
    public Entity targetTempentity;
    public float damage;
    //public Transform targetPos;
}