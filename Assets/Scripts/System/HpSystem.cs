using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class HpSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem endSimulationEcbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        //time = Time.DeltaTime;
    }
    protected override void OnUpdate()
    {
        var ecb = endSimulationEcbSystem.CreateCommandBuffer();
        bool isEnemyDeath = false;
        bool isCharacterDeath = false;
        Entities.
         ForEach((Entity entity, ref DeleteTag deleteTag, in Hp hp) =>
         {
             if (HasComponent<Character>(entity))
             {
                 if(hp.HpValue <= 0)
                 {
                     isCharacterDeath = true;
                 }
             }

             if (HasComponent<Enemy>(entity))
             {
                 if (hp.HpValue <= 0)
                 {
                     isEnemyDeath = true;
                 }
             }

             if (hp.HpValue <= 0)
             {
                 deleteTag.lifeTime = 0;
             }
         }).Run();

        if (isCharacterDeath)
        {
            EventCenter.Broadcast(EventType.Event_PlayerDeath);
            //Debug.Log("角色死亡");
        }

        if (isEnemyDeath)
        {
            EventCenter.Broadcast(EventType.Event_EnemyDeath);
            //Debug.Log("敌人死亡");
        }
        
    }
}
