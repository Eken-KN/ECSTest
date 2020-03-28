using UnityEngine;
using System.Collections;
using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

public class MovementSystem : SystemBase
{

    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        // Schedule job to move cubes
        Entities.WithName("Translate").ForEach((ref Unity.Transforms.Translation trans, in MoveSpeed speed) =>
            {
                //Debug.Log(trans.Value);
                trans.Value += new float3(Vector3.right) * speed.Value * deltaTime;
            }).ScheduleParallel();
    }
}
