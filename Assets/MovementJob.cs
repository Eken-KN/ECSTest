using System;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

//Ijob that moves gameobjects. Not used anymore.
public struct MovementJob : IJobParallelForTransform
{
    public float speed;
    public float deltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        transform.position += transform.rotation * Vector2.right * speed * deltaTime;
    }
}
