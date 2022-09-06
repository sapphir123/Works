using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

[UpdateAfter(typeof(ArrayRotationSystem))]
public class GroupSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

    }

    protected override void OnUpdate()
    {
        var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("Group").
            ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, ref Rotation orientation, ref GoTag goTag, ref Target target) =>
            {
                var rotation = orientation;
                float3 targetPosition = goTag.targetPos;
                float distance = math.distance(targetPosition, translation.Value);
                LocalToWorld targetTransform = GetComponent<LocalToWorld>(goTag.TempEntity);

                //拒绝响应距离
                if (distance < 2f)
                {
                    if (goTag.TempEntity != null)
                    {
                        goTag.isBack = true;
                    }
                }

                if (goTag.isBack)
                {
                    float3 newPos = targetTransform.Position;
                    var a = newPos - translation.Value;

                    quaternion b = Quaternion.FromToRotation(Vector3.down, a);
                    orientation.Value = b;

                    float d1 = math.distance(translation.Value, newPos);
                    translation.Value += math.normalizesafe(a);
                    float d2 = math.distance(translation.Value, newPos);
                    float c = math.distance(newPos, float3.zero) / 100f;
                    float d = d1 - d2;

                    if (d1 > 10 + c)
                    {
                        int loop = (int)((10 + c) / d);
                        for (int i = 0; i < loop; i++)
                        {
                            translation.Value += math.normalizesafe(a) / 100;
                        }
                    }
                    else
                    {
                        target.Tpos = float3.zero;
                        translation.Value = targetTransform.Position;
                        float distance3 = math.distance(newPos, translation.Value);
                        ecb.RemoveComponent(entityInQueryIndex, entity, ComponentType.ReadWrite<GoTag>());
                    }

                    return;
                }

                #region 目标

                var targetDir = targetPosition - translation.Value;
                quaternion temp1 = Quaternion.FromToRotation(Vector3.down, targetDir);
                orientation.Value = temp1;
                float3 distancePos = goTag.targetPos - goTag.originPos;
                //目标速度
                translation.Value += distancePos * deltaTime * target.randomSpeed / 3f;
                #endregion

            }).ScheduleParallel();

        // 添加Job
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);

    }
}



