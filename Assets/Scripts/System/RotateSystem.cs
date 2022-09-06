using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class RotateSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
    }

    //常规旋转
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        float angel = 0.01f;
        float3 mainPos = float3.zero;

        float3 input = float3.zero;
        string h = "Horizontal";
        string v = "Vertical";

        Entities.
            WithoutBurst().
            WithName("Player").
            ForEach((ref Translation translation, ref Rotation rotation, in Character character) =>
            {
                mainPos = translation.Value;
                input.x = Input.GetAxis(h);
                input.y = 0;
                input.z = Input.GetAxis(v);

            }).Run();

        Entities.
            WithAll<RotateTag, TempEntityTag>().
            ForEach((ref Translation translation, ref Rotation rotation, ref Target target) =>
            {
                //target.Tpos = new float3(mainPos.x, mainPos.y + 2, mainPos.z + 2);

                float3 pos = translation.Value ;
                quaternion rot = quaternion.AxisAngle(math.forward(), angel);
                float3 dir = pos - target.Tpos;

                dir = math.mul(rot, dir);
                var dirX = 1.6f * deltaTime * target.Tpos * -input;
                dirX.y = 0;
                translation.Value = target.Tpos + dir;
                //translation.Value += dirX;
                //translation.Value = Vector3.Lerp(pos, input + pos, 10 * deltaTime);

            }).ScheduleParallel();
    }
}
