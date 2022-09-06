using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class CharacterSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        float3 input;
        string h = "Horizontal";
        string v = "Vertical";
        bool isBroadcast = false;
        float hpTemp = 0;
        Entities.
            WithoutBurst().
            WithName("Player").
            ForEach((ref Entity entity,ref Translation translation, ref Rotation rotation, in Character character) =>
            {
                input.x = Input.GetAxis(h);
                input.y = 0;
                input.z = Input.GetAxis(v);
                var dir = character.speed * deltaTime * input;
                dir.y = 0;

                if (math.length(input) > 0.1f)
                {
                    //Debug.Log("Dir " + dir);
                    rotation.Value = quaternion.LookRotation(math.normalize(dir), math.up());

                }
                //if (GetComponent<Hp>(entity).HpValue > 0)
                //{
                //    DeleteTag deleteTag = new DeleteTag
                //    {
                //        lifeTime = 100,
                //    };
                //    SetComponent(entity, deleteTag);
                //}

                if (HasComponent<Hp>(entity))
                {
                    isBroadcast = true;
                    hpTemp = GetComponent<Hp>(entity).HpValue;
                }
                translation.Value += dir;

            }).Run();

        if (isBroadcast)
        {
            EventCenter.Broadcast(EventType.Event_PlayerHpDown, new TargetEventArgs { hp = hpTemp });
            //Debug.Log("广播角色血量: " + GetComponent<Hp>(entity).HpValue);
        }

    }
}