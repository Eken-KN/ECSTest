using UnityEngine;
using System;
using Unity.Entities;

//Entitycomponent containing speed of cube

[GenerateAuthoringComponent]
public struct MoveSpeed : IComponentData
{
    public float Value;
}
