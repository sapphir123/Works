using Unity.Entities;

[GenerateAuthoringComponent]
public struct Bullet: IComponentData
{
    public float flySpeed;
    public float damage;
    public bool isEnemyShoot;
}
